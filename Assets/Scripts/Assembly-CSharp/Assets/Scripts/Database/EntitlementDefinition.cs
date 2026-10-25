using System.Collections.Generic;
using AssetBundleData;
using Database;
using SharedBaseLib.Serialization;

namespace Assets.Scripts.Database
{
	[Database("EntitlementDefinition")]
	public class EntitlementDefinition : DBEntry
	{
		public LocaKey NameLocaKey { get; set; }

		public LocaKey DescriptionLocaKey { get; set; }

		public GameID Icon { get; set; }

		public bool Hidden { get; set; }

		public List<GameID> RequiredEntitlements { get; set; }

		public ushort RequiredProfileLevel { get; set; }

		public EntitlementDefinition(AssetBundleInfo _assetBundleInfo)
			: base(_assetBundleInfo)
		{
			NameLocaKey = new LocaKey();
			DescriptionLocaKey = new LocaKey();
			Icon = GameID.Invalid;
			Hidden = false;
			RequiredEntitlements = new List<GameID>();
			RequiredProfileLevel = 0;
		}

		public override void Serialize(DataSerializerBase _s)
		{
			base.Serialize(_s);
			NameLocaKey.Key = _s.SerializeInt("NameLocaKey", NameLocaKey.Key);
			DescriptionLocaKey.Key = _s.SerializeInt("DescriptionLocaKey", DescriptionLocaKey.Key);
			Icon = SerializeGameID("Icon", _s, this, Icon);
			Hidden = _s.SerializeBool("Hidden", Hidden);
			RequiredProfileLevel = _s.SerializeUInt16("RequiredProfileLevel", RequiredProfileLevel);
			_s.SerializeBeginNode("RequiredEntitlements");
			int num = _s.SerializeItemCount(RequiredEntitlements.Count);
			if (_s.Mode == EMode.EMode_Read && num > 0)
			{
				RequiredEntitlements.Capacity = num;
			}
			for (int i = 0; i < num; i++)
			{
				_s.SerializeBeginNode("GameID");
				if (_s.Mode == EMode.EMode_Read)
				{
					RequiredEntitlements.Add(SerializeGameID("Value", _s, this));
				}
				else
				{
					SerializeGameID("Value", _s, this, RequiredEntitlements[i]);
				}
				_s.SerializeEndNode();
			}
			_s.SerializeEndNode();
		}

		public override void ClearCache()
		{
		}
	}
}
