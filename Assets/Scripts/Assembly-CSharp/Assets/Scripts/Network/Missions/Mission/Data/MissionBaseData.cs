using System;
using AcierProtocol;
using ProtoBuf;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	public abstract class MissionBaseData
	{
		public const long c_DefaultInvalidId = -2L;

		public const string c_DefaultInvalidIdStr = "NONE";

		public long ActivateAtId = -2L;

		public long RemoveAtId = -2L;

		public readonly long ID = -2L;

		public EMapIconType MapIcon;

		public bool ClampMapIconOnBorder;

		public string IDStr
		{
			get
			{
				return ID.ToString("X");
			}
		}

		protected MissionBaseData()
		{
			ID = GenerateID();
		}

		protected MissionBaseData(MissionBaseContent _data)
		{
			ActivateAtId = _data.ActivateAtId;
			RemoveAtId = _data.RemoveAtId;
			MapIcon = _data.MapIcon;
			ClampMapIconOnBorder = _data.ClampMapIconOnBorder;
			ID = _data.ID;
		}

		private static long GenerateID()
		{
			long num;
			do
			{
				string text = Guid.NewGuid().ToString();
				string text2 = text.Substring(0, text.Length / 2);
				string text3 = text.Substring(text.Length / 2);
				num = (uint)(text2.GetHashCode() | text3.GetHashCode());
			}
			while (num == -2);
			return num;
		}

		public override string ToString()
		{
			return string.Format("{0}({1})", GetType().Name, ID.ToString("X16"));
		}

		public virtual IExtensible SerializeProto()
		{
			MissionBaseContent missionBaseContent = new MissionBaseContent();
			missionBaseContent.ActivateAtId = ActivateAtId;
			missionBaseContent.ClampMapIconOnBorder = ClampMapIconOnBorder;
			missionBaseContent.ID = ID;
			missionBaseContent.MapIcon = MapIcon;
			missionBaseContent.RemoveAtId = RemoveAtId;
			return missionBaseContent;
		}
	}
}
