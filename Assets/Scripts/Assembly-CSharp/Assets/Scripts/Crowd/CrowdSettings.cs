using Assets.Scripts.Animation;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Crowd
{
	public class CrowdSettings
	{
		public static float MinSpeed = 1.5f;

		public static float MaxSpeed = 2.5f;

		public static float FleeSpeed = 5.12f;

		public static float StopSpeed = 1.2f;

		public static float IdleThreshold = 0.1f;

		public static float FleeResponse = 0.5f;

		public static float FleeDamping = 0.7f;

		public static float EdgeAnticipationDistance = 2f;

		public static float VelocityDamping = 0f;

		public static float RotationDamping = 0.2f;

		public static float MinRotationSpeed = 90f;

		public static float AnimatorSpeedDamping = 0.1f;

		public static float MovementStateVelocityDamping = 0.25f;

		public static int MinAvoidancePriority = 80;

		public static int MaxAvoidancePriority = 100;

		public static bool AnimatorLOD = false;

		public static bool AvoidanceLOD = true;

		public static bool RandomizeOnWarp = true;

		public static float DynamicWeightResponse = 100000f;

		public static float DirectionResponse = 100f;

		public static float AgentAcceleration = 500f;

		public static float AgentAngularSpeed = 1000f;

		public static float MaxSpeedToIdle = 0.15f;

		public static float MinSpeedToMove = MaxSpeedToIdle + 0.05f;

		public static float MaxSpeedToTurn = 1f;

		public static float UpdatePeriod = 0f;

		public static float CorrectPositionPeriod = 0f;

		public static float AgentRadius = 0.25f;

		public static bool IgnoreRootMotion = false;

		public static bool FadeCharacters = true;

		public static float StayAtStationAddition = 0.25f;

		public static float StationSpotOccupiedAddition = 0.25f;

		public static ObstacleAvoidanceType AvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;

		public static IdleType[] MaleIdleTypes = new IdleType[7]
		{
			IdleType.CountingMale,
			IdleType.StandActArrogantMale,
			IdleType.StandAndArgumentMale,
			IdleType.StandAndTalkMale,
			IdleType.AdvertisingGoodsMale,
			IdleType.StandAndWaitMale,
			IdleType.Searching
		};

		public static IdleType[] FemaleIdleTypes = new IdleType[3]
		{
			IdleType.StandAndListenFemale,
			IdleType.StandAndTalkFemale,
			IdleType.StandAndWaitFemale
		};

		public static string[] GuidList = new string[10] { "15c667c9-4ec9-4044-84c7-965cf31ba7d7", "3ab3d8c0-3aa3-4675-a66a-28594a527d41", "4d0a5004-9f63-487b-bb3f-67ac58e7285b", "7ee7ed83-5975-4137-97ea-b15683283dd1", "86d505ad-c3ba-43ac-a5b9-101fff12be84", "9f772dc7-1c87-494c-a5eb-f96793122663", "ddac503d-625f-486e-a56d-7c552f579cbf", "b1e84e84-7424-4a9f-bfb9-d749385cc12c", "ef0eb7e6-310d-41c5-b17b-5eebe95c6293", "ba47c436-c31b-4792-9a71-3e7cd7d9e0de" };

		public static string MalePrefabGuid = "ee34f189-3b65-4618-ae57-f9d16499f3a2";

		public static string FemalePrefabGuid = "1ba1c2cf-babd-4445-9487-f45214038538";
	}
}
