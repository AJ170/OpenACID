using System;
using AcierProtocol;
using AssetBundleData;
using Assets.Scripts.Mission.Utilities;
using SharedBaseLib.Serialization;

namespace Database
{
	public class DBEntry
	{
		public enum EStatus
		{
			Production = 0,
			Obsolete = 1,
			WorkInProgress = 2
		}

		private GameID m_GameID = GameID.Invalid;

		private EEra m_Age;

		public static bool LightWeightMode { get; set; }

		public GameID GameID
		{
			get
			{
				return m_GameID;
			}
			protected set
			{
				m_GameID = value;
			}
		}

		public EEra Age
		{
			get
			{
				return m_Age;
			}
			set
			{
				EEra eEra = value.Fix();
				if (eEra != m_Age)
				{
					m_Age = eEra;
				}
			}
		}

		public AssetBundleInfo AssetBundleInfo { get; private set; }

		public short[] DepAssetBundlesInfos { get; set; }

		public string Name { get; set; }

		public virtual bool Export
		{
			get
			{
				return true;
			}
		}

		public virtual Type SerializedType
		{
			get
			{
				return GetType();
			}
		}

		public EStatus Status { get; set; }

		public DBEntry(AssetBundleInfo _assetBundleInfo)
		{
			AssetBundleInfo = _assetBundleInfo;
		}

		public DBEntry()
		{
			Name = "default";
			Age = EEra.Common;
			GameID = GameID.Invalid;
		}

		protected DBEntry(AssetBundleInfo _assetBundleInfo, DBEntry _entry)
		{
			GameID = _entry.GameID;
			Name = _entry.Name;
			AssetBundleInfo = _assetBundleInfo;
		}

		public bool Equals(DBEntry _other)
		{
			return GameID == _other.GameID;
		}

		public override bool Equals(object _obj)
		{
			if (object.ReferenceEquals(null, _obj))
			{
				return false;
			}
			return _obj is DBEntry && Equals((DBEntry)_obj);
		}

		public override int GetHashCode()
		{
			return GameID.GetHashCode();
		}

		public virtual int CompareTo(DBEntry _other)
		{
			return GameID.Guid.CompareTo(_other.GameID.Guid);
		}

		public virtual void ClearCache()
		{
		}

		protected GameID SerializeGameID(string _tag, DataSerializerBase _s, DBEntry _parent, GameID _value = null, GameID _default = null, GameID _template = null)
		{
			if (_s.Mode == EMode.EMode_Read)
			{
				string text = _s.SerializeString(_tag);
				if (!string.IsNullOrEmpty(text))
				{
					return new GameID(text, _parent);
				}
				if (_default != null)
				{
					return new GameID(_default, _parent);
				}
				return GameID.Invalid;
			}
			string value = string.Empty;
			GameID result = GameID.Invalid;
			if (_value != _template)
			{
				if (GameID.IsValid(_value) && !string.IsNullOrEmpty(_value.GuidString))
				{
					value = _value.GuidString;
					result = _value;
				}
				else if (GameID.IsValid(_template))
				{
					value = _template.GuidString;
					result = _template;
				}
			}
			_s.SerializeString(_tag, value);
			return result;
		}

		public virtual void Serialize(DataSerializerBase _s)
		{
			GameID = SerializeGameID("GameID", _s, this, GameID, GameID.Invalid);
			Name = _s.SerializeString("Name", Name);
			Status = (EStatus)_s.SerializeEnum("Status", typeof(EStatus), (int)Status);
			Age = (EEra)_s.SerializeEnum("Age", Age.GetType(), (int)Age);
		}
	}
}
