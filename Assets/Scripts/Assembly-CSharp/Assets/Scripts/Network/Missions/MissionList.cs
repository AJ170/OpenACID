using AcierNetwork.Communication;
using AssetBundleData;
using SharedBaseLib.Core;

namespace Assets.Scripts.Network.Missions
{
	public class MissionList : AcierNetwork.Communication.Model
	{
		public BBList<MissionDescExt> Entries { get; private set; }

		public BBHashSet<AssetBundleInfo> KnownBundleInfos { get; set; }

		public MissionList()
		{
			Entries = new BBList<MissionDescExt>();
		}

		public MissionDescExt FindDescription(string _missionURL)
		{
			if (string.IsNullOrEmpty(_missionURL))
			{
				return null;
			}
			return Entries.Find((MissionDescExt _desc) => string.Equals(_desc.MissionUrl, _missionURL));
		}
	}
}
