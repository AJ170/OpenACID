using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EObjectiveParameter")]
	public enum EObjectiveParameter
	{
		[ProtoEnum(Name = "DoNotKill", Value = 0)]
		DoNotKill = 0,
		[ProtoEnum(Name = "TimerSeconds", Value = 1)]
		TimerSeconds = 1,
		[ProtoEnum(Name = "Invert", Value = 2)]
		Invert = 2,
		[ProtoEnum(Name = "Targets", Value = 3)]
		Targets = 3,
		[ProtoEnum(Name = "Entourage", Value = 4)]
		Entourage = 4,
		[ProtoEnum(Name = "Range", Value = 5)]
		Range = 5,
		[ProtoEnum(Name = "ObjectivePopulationFormationSize", Value = 6)]
		ObjectivePopulationFormationSize = 6,
		[ProtoEnum(Name = "ObjectivePopulationHotspotCountForFormations", Value = 7)]
		ObjectivePopulationHotspotCountForFormations = 7,
		[ProtoEnum(Name = "ObjectiveHotspotCount", Value = 8)]
		ObjectiveHotspotCount = 8,
		[ProtoEnum(Name = "StaticObjectTypeGuid", Value = 9)]
		StaticObjectTypeGuid = 9,
		[ProtoEnum(Name = "ObjectiveMaxDistance", Value = 10)]
		ObjectiveMaxDistance = 10,
		[ProtoEnum(Name = "ObjectiveMinDistance", Value = 11)]
		ObjectiveMinDistance = 11,
		[ProtoEnum(Name = "UnitCountMax", Value = 12)]
		UnitCountMax = 12,
		[ProtoEnum(Name = "SpawnGroupLeader", Value = 13)]
		SpawnGroupLeader = 13,
		[ProtoEnum(Name = "SpawnGroupFiller", Value = 14)]
		SpawnGroupFiller = 14,
		[ProtoEnum(Name = "UseMissionArea", Value = 15)]
		UseMissionArea = 15,
		[ProtoEnum(Name = "NotorietyObjectiveParameter", Value = 16)]
		NotorietyObjectiveParameter = 16,
		[ProtoEnum(Name = "Comparer", Value = 17)]
		Comparer = 17,
		[ProtoEnum(Name = "FakeCount", Value = 18)]
		FakeCount = 18,
		[ProtoEnum(Name = "AreaInfluence", Value = 19)]
		AreaInfluence = 19,
		[ProtoEnum(Name = "FleeOnTransgression", Value = 20)]
		FleeOnTransgression = 20,
		[ProtoEnum(Name = "UseMovingArea", Value = 21)]
		UseMovingArea = 21,
		[ProtoEnum(Name = "MovingRestrictedRadius", Value = 22)]
		MovingRestrictedRadius = 22,
		[ProtoEnum(Name = "ObjectiveTargetMaxDistance", Value = 23)]
		ObjectiveTargetMaxDistance = 23,
		[ProtoEnum(Name = "ObjectiveTargetMinDistance", Value = 24)]
		ObjectiveTargetMinDistance = 24,
		[ProtoEnum(Name = "TargetKind", Value = 25)]
		TargetKind = 25,
		[ProtoEnum(Name = "MissionAreaType", Value = 26)]
		MissionAreaType = 26,
		[ProtoEnum(Name = "UnitCount", Value = 27)]
		UnitCount = 27,
		[ProtoEnum(Name = "MinDistance", Value = 28)]
		MinDistance = 28,
		[ProtoEnum(Name = "SpawnObjectCategory", Value = 29)]
		SpawnObjectCategory = 29,
		[ProtoEnum(Name = "Limit", Value = 30)]
		Limit = 30,
		[ProtoEnum(Name = "LimitType", Value = 31)]
		LimitType = 31,
		[ProtoEnum(Name = "SkillName", Value = 32)]
		SkillName = 32,
		[ProtoEnum(Name = "Radius", Value = 33)]
		Radius = 33,
		[ProtoEnum(Name = "SecondaryTimerSeconds", Value = 34)]
		SecondaryTimerSeconds = 34,
		[ProtoEnum(Name = "NpcInteractionType", Value = 35)]
		NpcInteractionType = 35,
		[ProtoEnum(Name = "TravelDistanceToTurn", Value = 36)]
		TravelDistanceToTurn = 36,
		[ProtoEnum(Name = "MaintainLineOfSight", Value = 37)]
		MaintainLineOfSight = 37,
		[ProtoEnum(Name = "TargetNearDistance", Value = 38)]
		TargetNearDistance = 38,
		[ProtoEnum(Name = "TargetNearFormations", Value = 39)]
		TargetNearFormations = 39,
		[ProtoEnum(Name = "WaitForTimerEnd", Value = 40)]
		WaitForTimerEnd = 40,
		[ProtoEnum(Name = "PathLength", Value = 41)]
		PathLength = 41,
		[ProtoEnum(Name = "RunProbability", Value = 42)]
		RunProbability = 42,
		[ProtoEnum(Name = "TravelDistanceToGuards", Value = 43)]
		TravelDistanceToGuards = 43,
		[ProtoEnum(Name = "DeviationAngle", Value = 44)]
		DeviationAngle = 44,
		[ProtoEnum(Name = "IdentifyActivatesMapIcon", Value = 45)]
		IdentifyActivatesMapIcon = 45,
		[ProtoEnum(Name = "TargetNearFormationUnitCount", Value = 46)]
		TargetNearFormationUnitCount = 46,
		[ProtoEnum(Name = "InvertedObjectiveResult", Value = 47)]
		InvertedObjectiveResult = 47,
		[ProtoEnum(Name = "PreventFailingOnIncrease", Value = 48)]
		PreventFailingOnIncrease = 48,
		[ProtoEnum(Name = "FailOnTransgression", Value = 49)]
		FailOnTransgression = 49,
		[ProtoEnum(Name = "HidingRequired", Value = 50)]
		HidingRequired = 50,
		[ProtoEnum(Name = "Minimum", Value = 51)]
		Minimum = 51,
		[ProtoEnum(Name = "IsMissionAreaCoveringWholeMap", Value = 52)]
		IsMissionAreaCoveringWholeMap = 52
	}
}
