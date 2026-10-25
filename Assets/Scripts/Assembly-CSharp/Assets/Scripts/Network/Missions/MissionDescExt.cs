using System;
using System.Collections.Generic;
using System.IO;
using AcierProtocol;
using AssetBundleData;
using Assets.Scripts.Network.Missions.Mission.Data;
using Assets.Scripts.Network.Missions.Mission.Serialization;
using SharedBaseLib.Core;
using UbiCore.Helpers;

namespace Assets.Scripts.Network.Missions
{
	public class MissionDescExt : MissionRootData
	{
		public AssetBundleInfo AssetBundle { get; set; }

		public string FullMissionUrl { get; set; }

		public MissionDescExt(MissionRootData _original)
			: base(_original)
		{
		}

		public MissionDescExt(MissionRootContent _original)
			: base(_original)
		{
		}

		public static BBList<MissionDescExt> CreateMissionListLocal(Action<int, int, float> onUpdate, List<string> files)
		{
			BBList<MissionDescExt> bBList = new BBList<MissionDescExt>();
			for (int i = 0; i < files.Count; i++)
			{
				float arg = (float)i / (float)files.Count;
				if (onUpdate != null)
				{
					onUpdate(i, files.Count, arg);
				}
				string text = files[i];
				try
				{
					Assets.Scripts.Network.Missions.Mission.Serialization.SerializedNode serializedNode = MissionSerializer.ReadBinaryFileWrapped(text);
					MissionRootData original = (MissionRootData)serializedNode.GetComponent("MissionRoot").NodeData;
					MissionDescExt missionDescExt = new MissionDescExt(original);
					FileInfo fileInfo = new FileInfo(text);
					string missionUrl = (missionDescExt.FullMissionUrl = GameFolders.FixSlashes(Path.Combine(fileInfo.Directory.Name, fileInfo.Name), true));
					missionDescExt.MissionUrl = missionUrl;
					bBList.Add(missionDescExt);
				}
				catch (Exception)
				{
				}
				arg = (float)(i + 1) / (float)files.Count;
				if (onUpdate != null)
				{
					onUpdate(i + 1, files.Count, arg);
				}
			}
			return bBList;
		}

		public static BBList<MissionDescExt> CreateMissionListUrl(string _url)
		{
			return new BBList<MissionDescExt>();
		}
	}
}
