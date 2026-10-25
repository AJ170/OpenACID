using System;
using AcierProtocol;
using ProtoBuf;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	[Serializable]
	public sealed class MissionWaypointData : MissionBaseData
	{
		public const EMovementSpeed MinSpeedDefault = EMovementSpeed.Walk;

		public const EMovementSpeed MaxSpeedDefault = EMovementSpeed.WalkFast;

		private const EFormationIterationType ActivationIterationTypeDefault = EFormationIterationType.BackAndForth;

		private const ERouteIterationStart ActivationIterationStartTypeDefault = ERouteIterationStart.Up_ERouteIterationStart;

		public ENpcIdleCategory IdleCategory = ENpcIdleCategory.Invalid_ENpcIdleCategory;

		public float IdleTime;

		public ERotationAtWaypoint RotationType;

		public EMovementSpeed MinSpeed = EMovementSpeed.Walk;

		public EMovementSpeed MaxSpeed = EMovementSpeed.WalkFast;

		public bool WarpTarget;

		public EFormationIterationType ActivationIterationType;

		public ERouteIterationStart ActivationIterationStartType = ERouteIterationStart.Up_ERouteIterationStart;

		public MissionWaypointData()
		{
		}

		public MissionWaypointData(MissionWaypointContent _missionWaypointContent)
			: base(_missionWaypointContent.MissionBaseContent)
		{
			IdleCategory = _missionWaypointContent.IdleCategory;
			IdleTime = _missionWaypointContent.IdleTime;
			RotationType = _missionWaypointContent.RotationType;
			MinSpeed = _missionWaypointContent.MinSpeed;
			MaxSpeed = _missionWaypointContent.MaxSpeed;
			WarpTarget = _missionWaypointContent.WarpTarget;
			ActivationIterationStartType = _missionWaypointContent.ActivationIterationStartType;
			ActivationIterationType = _missionWaypointContent.ActivationIterationType;
		}

		public override IExtensible SerializeProto()
		{
			MissionWaypointContent missionWaypointContent = new MissionWaypointContent();
			missionWaypointContent.ActivationIterationStartType = ActivationIterationStartType;
			missionWaypointContent.ActivationIterationType = ActivationIterationType;
			missionWaypointContent.IdleCategory = IdleCategory;
			missionWaypointContent.IdleTime = IdleTime;
			missionWaypointContent.MaxSpeed = MaxSpeed;
			missionWaypointContent.MinSpeed = MinSpeed;
			missionWaypointContent.MissionBaseContent = (MissionBaseContent)base.SerializeProto();
			missionWaypointContent.RotationType = RotationType;
			missionWaypointContent.WarpTarget = WarpTarget;
			return missionWaypointContent;
		}
	}
}
