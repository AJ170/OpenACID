using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using AcierProtocol;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils.Extensions;
using SharedBaseLib.Core;
using SharedBaseLib.System.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI.Mail
{
	public static class MailCache
	{
		private sealed class PriorityList
		{
			private List<CachedMail> m_list = new List<CachedMail>(32);

			public void Add(CachedMail _item)
			{
				HashSet<CachedMail> hashSet = new HashSet<CachedMail>(m_list);
				hashSet.Add(_item);
				HashSet<CachedMail> set = hashSet;
				m_list = set.ToList().OrderBy((CachedMail _x) => _x.Header.creationDate, (long d1, long d2) => d1.CompareTo(d2));
			}

			public void Add(IEnumerable<CachedMail> _items)
			{
				HashSet<CachedMail> hashSet = new HashSet<CachedMail>(m_list);
				hashSet.SymmetricExceptWith(_items);
				m_list = hashSet.ToList().OrderBy((CachedMail _x) => _x.Header.creationDate, (long d1, long d2) => d1.CompareTo(d2));
			}

			public CachedMail Pop()
			{
				if (m_list.Count == 0)
				{
					return null;
				}
				CachedMail result = m_list[m_list.Count - 1];
				m_list.RemoveAt(m_list.Count - 1);
				return result;
			}
		}

		public delegate void HandleCacheUpdate();

		public delegate void HandleMailRead(MailHeader _header, bool _read);

		private const string s_HeaderDirectory = "/headers/";

		private const string s_BodyDirectory = "/bodies/";

		private static readonly AcierProtocolSerializer s_serializer = new AcierProtocolSerializer();

		private static readonly string s_StoragePath = Application.temporaryCachePath + "/mail/";

		private static readonly BBDictionary<string, CachedMail> s_mails = new BBDictionary<string, CachedMail>(128);

		private static readonly PriorityList s_saveBuffer = new PriorityList();

		private static bool s_dirty = true;

		private static readonly List<MailHeader> s_headers = new List<MailHeader>();

		private static string s_currentUser;

		private static string s_basePath;

		private static string s_bodyPath;

		private static string s_headerPath;

		public static AcierProtocolSerializer Serializer
		{
			get
			{
				return s_serializer;
			}
		}

		public static int NumberOfHeaders
		{
			get
			{
				return Headers.Count;
			}
		}

		public static int NumberOfBodies
		{
			get
			{
				return s_mails.Values.ToList().Count((CachedMail _x) => _x.Body != null);
			}
		}

		public static List<MailHeader> Headers
		{
			get
			{
				if (s_dirty)
				{
					s_headers.Clear();
					foreach (KeyValuePair<string, CachedMail> s_mail in s_mails)
					{
						if (s_mail.Value.Header != null)
						{
							s_headers.Add(s_mail.Value.Header);
						}
					}
					s_headers.Sort((MailHeader d1, MailHeader d2) => d2.creationDate.CompareTo(d1.creationDate));
					s_dirty = false;
				}
				return s_headers;
			}
		}

		public static long LastSyncTime
		{
			get
			{
				return MaxCreationDate();
			}
		}

		private static string BasePath
		{
			get
			{
				if (string.IsNullOrEmpty(s_basePath) || s_currentUser != Singleton<StateManager>.Instance.ProfileManager.Profile.id)
				{
					s_currentUser = Singleton<StateManager>.Instance.ProfileManager.Profile.id;
					long num = Convert.ToInt64(AcierProtocolVersion.ACIER_PROTOCOL_VERSION_CURRENT);
					s_basePath = string.Format("{0}/{1}/{2}", s_StoragePath, num, s_currentUser.GetHashCode());
				}
				return s_basePath;
			}
		}

		public static string BodyPath
		{
			get
			{
				if (string.IsNullOrEmpty(s_bodyPath))
				{
					s_bodyPath = string.Format("{0}/{1}", BasePath, "/bodies/");
				}
				return s_bodyPath;
			}
		}

		public static string HeaderPath
		{
			get
			{
				if (string.IsNullOrEmpty(s_headerPath))
				{
					s_headerPath = string.Format("{0}/{1}", BasePath, "/headers/");
				}
				return s_headerPath;
			}
		}

		public static event HandleCacheUpdate OnMailRemoved;

		public static event HandleCacheUpdate OnHeaderAdded;

		public static event HandleCacheUpdate OnBodyAdded;

		public static event HandleMailRead OnMailRead;

		public static void Init()
		{
			CreateStructure();
			LoadMail();
			Main.Instance.StartCoroutine(co_saveMonitor());
		}

		public static void Clear()
		{
			ClearDirectory(BasePath);
			s_mails.Clear();
			CreateStructure();
			s_dirty = true;
		}

		public static bool HasMail(MailHeader _mail)
		{
			return HasMail(_mail.headerId);
		}

		public static bool HasMail(string _id)
		{
			return s_mails.ContainsKey(_id);
		}

		public static void AddHeaders(MailHeader[] _headers)
		{
			List<CachedMail> list = new List<CachedMail>(_headers.Length);
			for (int i = 0; i < _headers.Length; i++)
			{
				list.Add(AddHeader(_headers[i], false, false));
			}
			s_saveBuffer.Add(list);
			Trigger(MailCache.OnHeaderAdded);
		}

		public static void AddHeader(MailHeader _header)
		{
			AddHeader(_header, true, true);
		}

		private static CachedMail AddHeader(MailHeader _header, bool _triggerEvent, bool _save)
		{
			if (!s_mails.ContainsKey(_header.headerId))
			{
				CachedMail cachedMail = new CachedMail(_header);
				s_mails.Add(_header.headerId, cachedMail);
				if (_save)
				{
					s_saveBuffer.Add(cachedMail);
				}
			}
			s_dirty = true;
			if (_triggerEvent)
			{
				Trigger(MailCache.OnHeaderAdded);
			}
			return s_mails[_header.headerId];
		}

		public static void RemoveHeader(MailHeader _header)
		{
			RemoveMail(_header.headerId);
		}

		public static void RemoveMails(List<string> _ids)
		{
			for (int i = 0; i < _ids.Count; i++)
			{
				RemoveMail(_ids[i], false);
			}
			Trigger(MailCache.OnMailRemoved);
		}

		public static void RemoveMail(string _id)
		{
			RemoveMail(_id, true);
		}

		private static void RemoveMail(string _id, bool _triggerEvent)
		{
			CachedMail value;
			if (s_mails.TryGetValue(_id, out value))
			{
				value.Delete();
				s_mails.Remove(_id);
			}
			s_dirty = true;
			if (_triggerEvent)
			{
				Trigger(MailCache.OnMailRemoved);
			}
		}

		public static MailBody GetMailBody(MailHeader _header)
		{
			CachedMail value;
			if (s_mails.TryGetValue(_header.headerId, out value))
			{
				return value.Body;
			}
			return null;
		}

		public static void AddBody(MailHeader _header, MailBody _body)
		{
			CachedMail value;
			if (!s_mails.TryGetValue(_header.headerId, out value))
			{
				value = new CachedMail(_header);
				s_mails.Add(_header.headerId, value);
			}
			value.Body = _body;
			s_saveBuffer.Add(value);
			s_dirty = true;
			Trigger(MailCache.OnBodyAdded);
		}

		public static void MarkAsRead(MailHeader _header, bool _read)
		{
			CachedMail value;
			if (s_mails.TryGetValue(_header.headerId, out value))
			{
				value.Header.isRead = true;
				s_saveBuffer.Add(value);
				if (MailCache.OnMailRead != null)
				{
					MailCache.OnMailRead(_header, _read);
				}
			}
		}

		private static void Trigger(HandleCacheUpdate _callback)
		{
			if (_callback != null)
			{
				_callback();
			}
		}

		private static void LoadMail()
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(HeaderPath);
			if (directoryInfo.Exists)
			{
				FileInfo[] files = directoryInfo.GetFiles();
				foreach (FileInfo fileInfo in files)
				{
					using (FileStream source = fileInfo.OpenRead())
					{
						try
						{
							MailHeader mailHeader = (MailHeader)s_serializer.Deserialize(source, null, typeof(MailHeader));
							s_mails.Add(mailHeader.headerId, new CachedMail(mailHeader));
						}
						catch (Exception)
						{
						}
					}
				}
			}
			DirectoryInfo directoryInfo2 = new DirectoryInfo(BodyPath);
			if (directoryInfo2.Exists)
			{
				FileInfo[] files2 = directoryInfo2.GetFiles();
				foreach (FileInfo fileInfo2 in files2)
				{
					using (FileStream fileStream = fileInfo2.OpenRead())
					{
						try
						{
							MailBody body = (MailBody)s_serializer.Deserialize(fileStream, null, typeof(MailBody));
							CachedMail value;
							if (s_mails.TryGetValue(fileInfo2.Name, out value))
							{
								value.Body = body;
								continue;
							}
							fileStream.Dispose();
							fileInfo2.Delete();
						}
						catch (Exception)
						{
						}
					}
				}
			}
			s_dirty = true;
		}

		private static void RemoveExpiredMails()
		{
			List<string> list = new List<string>(s_mails.Count);
			foreach (KeyValuePair<string, CachedMail> s_mail in s_mails)
			{
				if (s_mail.Value.IsExpired())
				{
					list.Add(s_mail.Key);
				}
			}
			for (int i = 0; i < list.Count; i++)
			{
				RemoveMail(list[i]);
			}
			s_dirty = list.Count > 0;
		}

		private static void ClearDirectory(string _path)
		{
			DirectoryExtensions.Clean(_path, false);
		}

		private static void CreateStructure()
		{
			CreateDirectory(BasePath);
			CreateDirectory(HeaderPath);
			CreateDirectory(BodyPath);
		}

		private static void CreateDirectory(string _path)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(_path);
			if (!directoryInfo.Exists)
			{
				directoryInfo.Create();
			}
		}

		private static long MaxCreationDate()
		{
			long num = 0L;
			foreach (KeyValuePair<string, CachedMail> s_mail in s_mails)
			{
				if (s_mail.Value.Header.creationDate > num)
				{
					num = s_mail.Value.Header.creationDate;
				}
			}
			return num;
		}

		private static IEnumerator co_saveMonitor()
		{
			WaitForEndOfFrame wait = new WaitForEndOfFrame();
			while (true)
			{
				CachedMail mail = s_saveBuffer.Pop();
				if (mail != null && s_mails.ContainsKey(mail.Header.headerId))
				{
					mail.Save();
				}
				yield return wait;
			}
		}
	}
}
