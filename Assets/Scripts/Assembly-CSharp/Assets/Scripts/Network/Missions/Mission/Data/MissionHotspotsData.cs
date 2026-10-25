using System;
using AcierProtocol;
using Assets.Scripts.Network.Missions.Math;
using ProtoBuf;
using SharedBaseLib.Core;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	[Serializable]
	public sealed class MissionHotspotsData : MissionBaseData
	{
		public class Hotspot : IHotspot
		{
			public bool Active;

			public int UniqueId;

			public Point3D Position { get; private set; }

			public Hotspot()
			{
				UniqueId = 0;
				Active = false;
			}

			public Hotspot(int _id, bool _active, Point3D _posArr)
			{
				UniqueId = _id;
				Active = _active;
				Position = _posArr;
			}

			public Hotspot(MissionHotspotsContent.Hotspot _hotspot)
			{
				UniqueId = _hotspot.UniqueId;
				Active = _hotspot.Active;
				Position = new Point3D(_hotspot.PositionX, _hotspot.PositionY, _hotspot.PositionZ);
			}

			public MissionHotspotsContent.Hotspot SerializeProto()
			{
				MissionHotspotsContent.Hotspot hotspot = new MissionHotspotsContent.Hotspot();
				hotspot.Active = Active;
				hotspot.PositionX = Position.x;
				hotspot.PositionY = Position.y;
				hotspot.PositionZ = Position.z;
				hotspot.UniqueId = UniqueId;
				return hotspot;
			}
		}

		private class Deserialized
		{
			internal BBSimpleList<Hotspot> Benches = new BBSimpleList<Hotspot>();

			internal BBSimpleList<Hotspot> Haystacks = new BBSimpleList<Hotspot>();

			internal BBSimpleList<Hotspot> RoofGardens = new BBSimpleList<Hotspot>();
		}

		public BBSimpleList<Hotspot> Benches = new BBSimpleList<Hotspot>();

		public BBSimpleList<Hotspot> Haystacks = new BBSimpleList<Hotspot>();

		public BBSimpleList<Hotspot> RoofGardens = new BBSimpleList<Hotspot>();

		private static Deserialized sContent = new Deserialized();

		public MissionHotspotsData()
		{
		}

		public MissionHotspotsData(MissionHotspotsContent _missionHotspotsContent)
			: base(_missionHotspotsContent.MissionBaseContent)
		{
			Benches = new BBSimpleList<Hotspot>(_missionHotspotsContent.Benches.Length);
			for (int i = 0; i < _missionHotspotsContent.Benches.Length; i++)
			{
				Benches.Add(new Hotspot(_missionHotspotsContent.Benches[i]));
			}
			Haystacks = new BBSimpleList<Hotspot>(_missionHotspotsContent.Haystacks.Length);
			for (int j = 0; j < _missionHotspotsContent.Haystacks.Length; j++)
			{
				Haystacks.Add(new Hotspot(_missionHotspotsContent.Haystacks[j]));
			}
			RoofGardens = new BBSimpleList<Hotspot>(_missionHotspotsContent.RoofGardens.Length);
			for (int k = 0; k < _missionHotspotsContent.RoofGardens.Length; k++)
			{
				RoofGardens.Add(new Hotspot(_missionHotspotsContent.RoofGardens[k]));
			}
		}

		public override IExtensible SerializeProto()
		{
			MissionHotspotsContent missionHotspotsContent = new MissionHotspotsContent();
			missionHotspotsContent.MissionBaseContent = (MissionBaseContent)base.SerializeProto();
			MissionHotspotsContent missionHotspotsContent2 = missionHotspotsContent;
			missionHotspotsContent2.Benches = new MissionHotspotsContent.Hotspot[Benches.Count];
			for (int i = 0; i < Benches.Count; i++)
			{
				Hotspot hotspot = Benches[i];
				missionHotspotsContent2.Benches[i] = hotspot.SerializeProto();
			}
			missionHotspotsContent2.Haystacks = new MissionHotspotsContent.Hotspot[Haystacks.Count];
			for (int j = 0; j < Haystacks.Count; j++)
			{
				Hotspot hotspot2 = Haystacks[j];
				missionHotspotsContent2.Haystacks[j] = hotspot2.SerializeProto();
			}
			missionHotspotsContent2.RoofGardens = new MissionHotspotsContent.Hotspot[RoofGardens.Count];
			for (int k = 0; k < RoofGardens.Count; k++)
			{
				Hotspot hotspot3 = RoofGardens[k];
				missionHotspotsContent2.RoofGardens[k] = hotspot3.SerializeProto();
			}
			return missionHotspotsContent2;
		}
	}
}
