using System;
using System.IO;
using AcierProtocol;
using Assets.Scripts.UI.Interfaces;

namespace Assets.Scripts.UI.Mail
{
	public sealed class CachedMail : IComparable, IValidated, IComparable<CachedMail>
	{
		private string m_bodyPath;

		private string m_headerPath;

		public MailHeader Header { get; private set; }

		public MailBody Body { get; set; }

		private string BodyPath
		{
			get
			{
				if (string.IsNullOrEmpty(m_bodyPath))
				{
					m_bodyPath = string.Format("{0}/{1}", MailCache.BodyPath, Header.headerId);
				}
				return m_bodyPath;
			}
		}

		private string HeaderPath
		{
			get
			{
				if (string.IsNullOrEmpty(m_headerPath))
				{
					m_headerPath = string.Format("{0}/{1}", MailCache.HeaderPath, Header.headerId);
				}
				return m_headerPath;
			}
		}

		public bool Valid
		{
			get
			{
				return Header != null && !string.IsNullOrEmpty(Header.headerId);
			}
		}

		public CachedMail(MailHeader _header)
		{
			Header = _header;
			Body = null;
		}

		public void Save()
		{
			if (!Valid)
			{
				return;
			}
			if (Body != null)
			{
				try
				{
					using (FileStream dest = File.Create(BodyPath))
					{
						MailCache.Serializer.Serialize(dest, Body);
					}
				}
				catch (IOException)
				{
				}
			}
			try
			{
				using (FileStream dest2 = File.Create(HeaderPath))
				{
					MailCache.Serializer.Serialize(dest2, Header);
				}
			}
			catch (IOException)
			{
			}
		}

		public void Delete()
		{
			if (Valid)
			{
				FileInfo fileInfo = new FileInfo(HeaderPath);
				if (fileInfo.Exists)
				{
					fileInfo.Delete();
				}
				FileInfo fileInfo2 = new FileInfo(BodyPath);
				if (fileInfo2.Exists)
				{
					fileInfo2.Delete();
				}
				Header = null;
				Body = null;
			}
		}

		public override int GetHashCode()
		{
			return Header.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format("CachedMail({0})", Header.headerId);
		}

		public bool Equals(CachedMail _other)
		{
			if (object.ReferenceEquals(_other, null))
			{
				return false;
			}
			if (object.ReferenceEquals(this, _other))
			{
				return true;
			}
			if (Body != null && _other.Body == null)
			{
				return false;
			}
			if (Body == null && _other.Body != null)
			{
				return false;
			}
			if (Body != null && _other.Body != null)
			{
				return Header.headerId.Equals(_other.Header.headerId, StringComparison.InvariantCultureIgnoreCase) && Body.bodyId.Equals(_other.Body.bodyId, StringComparison.InvariantCultureIgnoreCase);
			}
			return Header.headerId.Equals(_other.Header.headerId, StringComparison.InvariantCultureIgnoreCase);
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as CachedMail);
		}

		public int CompareTo(CachedMail other)
		{
			if (other == null)
			{
				return 1;
			}
			return string.Compare(Header.headerId, other.Header.headerId, StringComparison.Ordinal);
		}

		public int CompareTo(object obj)
		{
			return CompareTo(obj as CachedMail);
		}

		public static bool operator ==(CachedMail _lhs, CachedMail _rhs)
		{
			if (object.ReferenceEquals(_lhs, null))
			{
				if (object.ReferenceEquals(_rhs, null))
				{
					return true;
				}
				return false;
			}
			return _lhs.Equals(_rhs);
		}

		public static bool operator !=(CachedMail _lhs, CachedMail _rhs)
		{
			return !(_lhs == _rhs);
		}
	}
}
