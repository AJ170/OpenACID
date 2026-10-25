using System;
using AcierProtocol;
using Assets.Scripts.Mission.Utilities;
using ProtoBuf;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	[Serializable]
	public class MissionRootData : MissionBaseData
	{
		public EEra Age;

		public string BriefingText;

		public int CrowdDensity;

		public string DebriefingText;

		public int DifficultyLevel;

		public int Rank;

		public bool Hidden;

		public int MaxCrowdEntities;

		public EMissionType MissionType;

		public EMood Mood;

		public string Name;

		public EObjectiveType ObjectiveType;

		public ERegion Region;

		public string Scene;

		public string PlayerAssassinGUID;

		public string PlayerHirelingGUID;

		public EMissionKind MissionKind;

		public string RegionAppearanceOption = string.Empty;

		public string BaseName { get; set; }

		public string MissionUrl { get; set; }

		public string GUID { get; set; }

		public DateTime GenerationTime { get; private set; }

		public MissionRootData()
		{
			Hidden = true;
			ResetGUID();
		}

		public MissionRootData(MissionRootContent _missionRootContent)
			: base(_missionRootContent.MissionBaseContent)
		{
			Age = _missionRootContent.Age.Fix();
			DifficultyLevel = _missionRootContent.DifficultyLevel;
			Rank = _missionRootContent.Rank;
			GUID = _missionRootContent.GUID;
			BaseName = _missionRootContent.BaseName;
			MissionType = _missionRootContent.MissionType;
			Mood = _missionRootContent.Mood;
			Name = _missionRootContent.Name;
			ObjectiveType = _missionRootContent.ObjectiveType;
			Region = _missionRootContent.Region;
			Hidden = _missionRootContent.Hidden;
			MaxCrowdEntities = _missionRootContent.MaxCrowdEntities;
			CrowdDensity = _missionRootContent.CrowdDensity;
			Scene = _missionRootContent.Scene;
			BriefingText = _missionRootContent.BriefingText;
			DebriefingText = _missionRootContent.DebriefingText;
			GenerationTime = XmlReaderHelper.ParseDateTime(_missionRootContent.GenerationTime);
			MissionUrl = _missionRootContent.MissionUrl;
			PlayerAssassinGUID = _missionRootContent.PlayerAssassinGUID;
			PlayerHirelingGUID = _missionRootContent.PlayerHirelingGUID;
			MissionKind = _missionRootContent.MissionKind;
			if (string.IsNullOrEmpty(GUID))
			{
				ResetGUID();
			}
		}

		public MissionRootData(MissionRootData _original)
		{
			Age = _original.Age.Fix();
			DifficultyLevel = _original.DifficultyLevel;
			Rank = _original.Rank;
			GUID = _original.GUID;
			BaseName = _original.BaseName;
			MissionType = _original.MissionType;
			Mood = _original.Mood;
			Name = _original.Name;
			ObjectiveType = _original.ObjectiveType;
			Region = _original.Region;
			Hidden = _original.Hidden;
			MaxCrowdEntities = _original.MaxCrowdEntities;
			CrowdDensity = _original.CrowdDensity;
			Scene = _original.Scene;
			BriefingText = _original.BriefingText;
			DebriefingText = _original.DebriefingText;
			GenerationTime = _original.GenerationTime;
			MissionUrl = _original.MissionUrl;
			PlayerAssassinGUID = _original.PlayerAssassinGUID;
			PlayerHirelingGUID = _original.PlayerHirelingGUID;
			MissionKind = _original.MissionKind;
			if (string.IsNullOrEmpty(GUID))
			{
				ResetGUID();
			}
		}

		public void ResetGUID()
		{
			GUID = Guid.NewGuid().ToString();
		}

		public override IExtensible SerializeProto()
		{
			MissionRootContent missionRootContent = new MissionRootContent();
			missionRootContent.Age = Age.Fix();
			missionRootContent.BaseName = BaseName;
			missionRootContent.BriefingText = BriefingText;
			missionRootContent.CrowdDensity = CrowdDensity;
			missionRootContent.DebriefingText = DebriefingText;
			missionRootContent.DifficultyLevel = DifficultyLevel;
			missionRootContent.Rank = Rank;
			missionRootContent.GUID = GUID;
			missionRootContent.GenerationTime = XmlReaderHelper.FormatDateTime(DateTime.Now);
			missionRootContent.Hidden = Hidden;
			missionRootContent.MaxCrowdEntities = MaxCrowdEntities;
			missionRootContent.MissionBaseContent = (MissionBaseContent)base.SerializeProto();
			missionRootContent.MissionType = MissionType;
			missionRootContent.Mood = Mood;
			missionRootContent.Name = Name;
			missionRootContent.ObjectiveType = ObjectiveType;
			missionRootContent.Region = Region;
			missionRootContent.Scene = Scene;
			missionRootContent.MissionUrl = MissionUrl;
			missionRootContent.PlayerAssassinGUID = PlayerAssassinGUID;
			missionRootContent.PlayerHirelingGUID = PlayerHirelingGUID;
			missionRootContent.MissionKind = MissionKind;
			return missionRootContent;
		}
	}
}
