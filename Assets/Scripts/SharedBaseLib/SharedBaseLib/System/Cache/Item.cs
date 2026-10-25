using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Threading;
using Ionic.Crc;
using SharedBaseLib.Core;
using SharedBaseLib.Serialization;
using UnityEngine;

namespace SharedBaseLib.System.Cache
{
	public abstract class Item
	{
		private const string TagDate = "d";

		private const string TagName = "name";

		private const string TagRevision = "rev";

		private const string TagSize = "s";

		private const string TagCurrentSize = "cs";

		private const string TagCRC = "crc";

		public string BaseName { get; private set; }

		public string Name { get; private set; }

		public uint Revision { get; private set; }

		public DateTime LastAccess { get; private set; }

		public string CacheFile { get; protected set; }

		public ulong Size { get; internal set; }

		public int CRC { get; protected set; }

		public ulong CurrentSize { get; internal set; }

		public abstract string Location { get; internal set; }

		public bool DownloadDone
		{
			get
			{
				return CurrentSize == Size;
			}
		}

		public void Write(DataSerializerBase _s)
		{
			_s.SerializeBeginNode("Item");
			_s.SerializeType("Type", GetType());
			Serialize(_s);
			_s.SerializeEndNode();
		}

		protected Item()
		{
		}

		protected Item(uint _revision, string _name)
		{
			CurrentSize = 0uL;
			Revision = _revision;
			Name = _name;
			BaseName = GetBaseName(_name);
			UpdateDate();
		}

		public static string GetBaseName(string _url)
		{
			return Path.GetFileNameWithoutExtension(_url);
		}

		public static CRC32 ConstructCRC32Generator()
		{
			return new CRC32(-306674912, false);
		}

		public virtual void Serialize(DataSerializerBase _s)
		{
			Name = _s.SerializeString("name", Name);
			Revision = _s.SerializeUInt32("rev", Revision);
			string s = _s.SerializeString("d", LastAccess.ToString("u", CultureInfo.InvariantCulture));
			Size = _s.SerializeUInt64("s", Size);
			CurrentSize = _s.SerializeUInt64("cs", CurrentSize);
			CRC = _s.SerializeInt32("crc", CRC);
			if (_s.Mode == EMode.EMode_Read)
			{
				BaseName = GetBaseName(Name);
				LastAccess = DateTime.Parse(s, CultureInfo.InvariantCulture);
			}
		}

		internal virtual void CleanUp()
		{
			CurrentSize = 0uL;
		}

		public void UpdateDate()
		{
			LastAccess = DateTime.UtcNow;
		}

		public IEnumerator UpdateCRCAsync()
		{
			bool done = false;
			WaitForEndOfFrame wait = new WaitForEndOfFrame();
			ThreadPool.QueueUserWorkItem(delegate
			{
				UpdateCRC();
				done = true;
			});
			while (!done)
			{
				yield return wait;
			}
		}

		public abstract IEnumerator Prepare(string _tempFile, bool _deleteCacheFile);

		public abstract bool UpdateCRC();

		public abstract bool Verify(ref BBList<string> _files);
	}
}
