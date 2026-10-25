using System;
using System.Collections.Generic;
using AcierProtocol;
using ProtoBuf;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	[Serializable]
	public sealed class MissionSectorsData : MissionBaseData
	{
		public List<MissionSector> Sectors = new List<MissionSector>();

		public MissionSectorsData()
		{
		}

		public MissionSectorsData(MissionSectorsContent _missionSectorsContent)
			: base(_missionSectorsContent.MissionBaseContent)
		{
			Sectors = new List<MissionSector>();
			MissionSectorsContent.MissionSector[] sectors = _missionSectorsContent.Sectors;
			foreach (MissionSectorsContent.MissionSector missionSector in sectors)
			{
				Sectors.Add(new MissionSector(missionSector));
			}
		}

		public override IExtensible SerializeProto()
		{
			MissionSectorsContent missionSectorsContent = new MissionSectorsContent();
			missionSectorsContent.MissionBaseContent = (MissionBaseContent)base.SerializeProto();
			MissionSectorsContent missionSectorsContent2 = missionSectorsContent;
			foreach (MissionSector sector in Sectors)
			{
				missionSectorsContent2.Sectors = missionSectorsContent2.Sectors.Add(sector.SerializeProto());
			}
			return missionSectorsContent2;
		}
	}
}
