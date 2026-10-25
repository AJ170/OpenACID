using System.IO;
using AcierProtocol;

namespace Assets.Scripts.Network.Missions.Mission.Serialization
{
	public static class MissionSerializer
	{
		private static readonly AcierProtocolSerializer s_Serializer = new AcierProtocolSerializer();

		public static SerializedNode Convert(MissionFile _missionFile)
		{
			return new SerializedNode(_missionFile.RootNode);
		}

		public static MissionData ReadBinaryFileAsMissionData(string _filePath, string _metaFilePath, LangKey _objectiveKey, LangKey _missionFlavorTextLoca, LangKey _missionDescriptionLoca, LangKey _missionShortDescriptionLoca, string _campaignId, int _sortIndex, string[] _requiredEntitlements, string _regionAppearanceOption)
		{
			MissionFile missionFile = ReadBinaryFileProto<MissionFile>(_filePath);
			MissionGoals missionGoals = ((_metaFilePath == null) ? new MissionGoals() : ReadBinaryFileProto<MissionGoals>(_metaFilePath));
			MissionRootContent missionRootContent = missionFile.RootNode.Components.First((AcierProtocol.SerializedNodeComponent x) => x.Type == AcierProtocol.SerializedNodeComponent.NodeType.NodeType_MissionRootContent).MissionRootContent;
			MissionData missionData = new MissionData();
			missionData.mission = missionFile;
			missionData.baseObjectiveType = missionRootContent.ObjectiveType;
			missionData.objectiveLangKey = _objectiveKey;
			missionData.missionFlavorTextLoca = _missionFlavorTextLoca;
			missionData.missionDescription = _missionDescriptionLoca;
			missionData.missionShortDescription = _missionShortDescriptionLoca;
			missionData.era = missionRootContent.Age;
			missionData.fullName = missionFile.RootNode.Name;
			missionData.baseName = missionFile.RootNode.Name;
			missionData.mood = missionRootContent.Mood;
			missionData.region = missionRootContent.Region;
			missionData.missionGoals = missionGoals;
			missionData.campaignId = _campaignId;
			missionData.storyMissionSortIndex = _sortIndex;
			missionData.reqEntitlements = _requiredEntitlements;
			missionData.regionAppearanceOption = _regionAppearanceOption;
			missionData.difficulty = missionRootContent.DifficultyLevel;
			return missionData;
		}

		public static T ReadBinaryFileProto<T>(string _filePath)
		{
			using (FileStream stream = File.OpenRead(_filePath))
			{
				return ReadBinaryStream<T>(stream);
			}
		}

		public static SerializedNode ReadBinaryFileWrapped(string _filePath)
		{
			MissionFile missionFile = ReadBinaryFileProto<MissionFile>(_filePath);
			return Convert(missionFile);
		}

		public static T ReadBinaryStream<T>(Stream _stream)
		{
			_stream.Position = 0L;
			return (T)s_Serializer.Deserialize(_stream, null, typeof(T));
		}

		public static void WriteBinaryFile(string _filename, SerializedNode _rootNode)
		{
			if (File.Exists(_filename))
			{
				File.SetAttributes(_filename, FileAttributes.Normal);
			}
			AcierProtocol.SerializedNode rootNode = _rootNode.SerializeProto();
			MissionFile missionFile = new MissionFile();
			missionFile.RootNode = rootNode;
			MissionFile content = missionFile;
			WriteBinaryFileProto(_filename, content);
		}

		public static void WriteBinaryFileProto<T>(string _filename, T _content)
		{
			FileStream fileStream = File.Create(_filename);
			s_Serializer.Serialize(fileStream, _content);
			fileStream.Close();
		}
	}
}
