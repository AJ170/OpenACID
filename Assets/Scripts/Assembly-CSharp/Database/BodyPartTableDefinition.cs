using System.Collections.Generic;
using AssetBundleData;
using SharedBaseLib.Serialization;

namespace Database
{
	[Database("BodyPartTableDefinition")]
	public class BodyPartTableDefinition : DBEntry
	{
		public enum EBodyPartTypes
		{
			Unknown = 0,
			Body = 1,
			Head = 2,
			Accessory = 3
		}

		public EBodyPartTypes BodyPartType;

		public List<GameID> BodyParts;

		public BodyPartTableDefinition(AssetBundleInfo _assetBundleInfo)
			: base(_assetBundleInfo)
		{
			BodyParts = new List<GameID>();
		}

		public override void Serialize(DataSerializerBase _s)
		{
			base.Serialize(_s);
			BodyPartType = (EBodyPartTypes)_s.SerializeEnum("BodyPartType", BodyPartType.GetType(), (int)BodyPartType);
			_s.SerializeBeginNode("BodyParts");
			int num = _s.SerializeItemCount(BodyParts.Count);
			if (_s.Mode == EMode.EMode_Read)
			{
				BodyParts = new List<GameID>(num);
			}
			for (int i = 0; i < num; i++)
			{
				_s.SerializeBeginNode("BodyParts");
				if (_s.Mode == EMode.EMode_Read)
				{
					BodyParts.Add(SerializeGameID("GameID", _s, this));
				}
				else
				{
					SerializeGameID("GameID", _s, this, BodyParts[i]);
				}
				_s.SerializeEndNode();
			}
			_s.SerializeEndNode();
		}
	}
}
