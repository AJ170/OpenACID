using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "SerializedNodeComponent")]
	public class SerializedNodeComponent : IExtensible
	{
		[ProtoContract(Name = "NodeType")]
		public enum NodeType
		{
			[ProtoEnum(Name = "NodeType_Invalid", Value = 0)]
			NodeType_Invalid = 0,
			[ProtoEnum(Name = "NodeType_MissionAreaNodeContent", Value = 1)]
			NodeType_MissionAreaNodeContent = 1,
			[ProtoEnum(Name = "NodeType_MissionAchievementEntryContent", Value = 2)]
			NodeType_MissionAchievementEntryContent = 2,
			[ProtoEnum(Name = "NodeType_MissionCodexEntryContent", Value = 3)]
			NodeType_MissionCodexEntryContent = 3,
			[ProtoEnum(Name = "NodeType_MissionHotspotsContent", Value = 4)]
			NodeType_MissionHotspotsContent = 4,
			[ProtoEnum(Name = "NodeType_MissionPlayerSpawnContent", Value = 5)]
			NodeType_MissionPlayerSpawnContent = 5,
			[ProtoEnum(Name = "NodeType_MissionRootContent", Value = 6)]
			NodeType_MissionRootContent = 6,
			[ProtoEnum(Name = "NodeType_MissionSectorsContent", Value = 7)]
			NodeType_MissionSectorsContent = 7,
			[ProtoEnum(Name = "NodeType_MissionWaypointContent", Value = 8)]
			NodeType_MissionWaypointContent = 8,
			[ProtoEnum(Name = "NodeType_MissionTextAnnotationContent", Value = 9)]
			NodeType_MissionTextAnnotationContent = 9,
			[ProtoEnum(Name = "NodeType_MissionAreaMovingContent", Value = 10)]
			NodeType_MissionAreaMovingContent = 10,
			[ProtoEnum(Name = "NodeType_MissionAreaContent", Value = 11)]
			NodeType_MissionAreaContent = 11,
			[ProtoEnum(Name = "NodeType_MissionNpcFormationContent", Value = 12)]
			NodeType_MissionNpcFormationContent = 12,
			[ProtoEnum(Name = "NodeType_MissionNpcSpawnContent", Value = 13)]
			NodeType_MissionNpcSpawnContent = 13,
			[ProtoEnum(Name = "NodeType_MissionSpawnEntityContent", Value = 14)]
			NodeType_MissionSpawnEntityContent = 14,
			[ProtoEnum(Name = "NodeType_MissionSpawnObjectContent", Value = 15)]
			NodeType_MissionSpawnObjectContent = 15,
			[ProtoEnum(Name = "NodeType_MissionSpawnPointContent", Value = 16)]
			NodeType_MissionSpawnPointContent = 16,
			[ProtoEnum(Name = "NodeType_MissionSpawnStaticObjectContent", Value = 17)]
			NodeType_MissionSpawnStaticObjectContent = 17,
			[ProtoEnum(Name = "NodeType_MissionTarget", Value = 41)]
			NodeType_MissionTarget = 41,
			[ProtoEnum(Name = "NodeType_ObjectiveEffectSpawnerContent", Value = 48)]
			NodeType_ObjectiveEffectSpawnerContent = 48,
			[ProtoEnum(Name = "NodeType_MissionGameLogicModContent", Value = 49)]
			NodeType_MissionGameLogicModContent = 49,
			[ProtoEnum(Name = "NodeType_MissionDesignTimeContent", Value = 50)]
			NodeType_MissionDesignTimeContent = 50,
			[ProtoEnum(Name = "NodeType_MissionPreventMovementContent", Value = 51)]
			NodeType_MissionPreventMovementContent = 51,
			[ProtoEnum(Name = "NodeType_TemplateSpawnPointContent", Value = 18)]
			NodeType_TemplateSpawnPointContent = 18,
			[ProtoEnum(Name = "NodeType_TemplateTargetContent", Value = 19)]
			NodeType_TemplateTargetContent = 19,
			[ProtoEnum(Name = "NodeType_ObjectiveBecomeAnonymousContent", Value = 20)]
			NodeType_ObjectiveBecomeAnonymousContent = 20,
			[ProtoEnum(Name = "NodeType_ObjectiveDeliverContent", Value = 21)]
			NodeType_ObjectiveDeliverContent = 21,
			[ProtoEnum(Name = "NodeType_ObjectiveDeliverNotorietyContent", Value = 22)]
			NodeType_ObjectiveDeliverNotorietyContent = 22,
			[ProtoEnum(Name = "NodeType_ObjectiveEscapeContent", Value = 23)]
			NodeType_ObjectiveEscapeContent = 23,
			[ProtoEnum(Name = "NodeType_ObjectiveEscortContent", Value = 24)]
			NodeType_ObjectiveEscortContent = 24,
			[ProtoEnum(Name = "NodeType_ObjectiveGetInRangeMobileContent", Value = 25)]
			NodeType_ObjectiveGetInRangeMobileContent = 25,
			[ProtoEnum(Name = "NodeType_ObjectiveGetInRangeStaticContent", Value = 26)]
			NodeType_ObjectiveGetInRangeStaticContent = 26,
			[ProtoEnum(Name = "NodeType_ObjectiveHotspotContent", Value = 27)]
			NodeType_ObjectiveHotspotContent = 27,
			[ProtoEnum(Name = "NodeType_ObjectiveIdentifyContent", Value = 28)]
			NodeType_ObjectiveIdentifyContent = 28,
			[ProtoEnum(Name = "NodeType_ObjectiveInteractionContent", Value = 29)]
			NodeType_ObjectiveInteractionContent = 29,
			[ProtoEnum(Name = "NodeType_ObjectiveKillContent", Value = 30)]
			NodeType_ObjectiveKillContent = 30,
			[ProtoEnum(Name = "NodeType_ObjectiveKillEnemyTypeContent", Value = 31)]
			NodeType_ObjectiveKillEnemyTypeContent = 31,
			[ProtoEnum(Name = "NodeType_ObjectiveLimitContent", Value = 32)]
			NodeType_ObjectiveLimitContent = 32,
			[ProtoEnum(Name = "NodeType_ObjectiveSkillLimitContent", Value = 33)]
			NodeType_ObjectiveSkillLimitContent = 33,
			[ProtoEnum(Name = "NodeType_ObjectiveNotorietyContent", Value = 34)]
			NodeType_ObjectiveNotorietyContent = 34,
			[ProtoEnum(Name = "NodeType_ObjectiveParkourContent", Value = 35)]
			NodeType_ObjectiveParkourContent = 35,
			[ProtoEnum(Name = "NodeType_ObjectiveStayInRangeMobileContent", Value = 36)]
			NodeType_ObjectiveStayInRangeMobileContent = 36,
			[ProtoEnum(Name = "NodeType_ObjectiveStayInRangeStaticContent", Value = 37)]
			NodeType_ObjectiveStayInRangeStaticContent = 37,
			[ProtoEnum(Name = "NodeType_ObjectiveStayAliveContent", Value = 38)]
			NodeType_ObjectiveStayAliveContent = 38,
			[ProtoEnum(Name = "NodeType_ObjectiveTimeLimitContent", Value = 39)]
			NodeType_ObjectiveTimeLimitContent = 39,
			[ProtoEnum(Name = "NodeType_ObjectiveUiContent", Value = 40)]
			NodeType_ObjectiveUiContent = 40,
			[ProtoEnum(Name = "NodeType_ObjectiveTimerContent", Value = 42)]
			NodeType_ObjectiveTimerContent = 42,
			[ProtoEnum(Name = "NodeType_ObjectiveVoidContent", Value = 43)]
			NodeType_ObjectiveVoidContent = 43,
			[ProtoEnum(Name = "NodeType_ObjectiveSpawnContent", Value = 44)]
			NodeType_ObjectiveSpawnContent = 44,
			[ProtoEnum(Name = "NodeType_ObjectiveLootContent", Value = 45)]
			NodeType_ObjectiveLootContent = 45,
			[ProtoEnum(Name = "NodeType_ObjectiveSelectTargetContent", Value = 46)]
			NodeType_ObjectiveSelectTargetContent = 46,
			[ProtoEnum(Name = "NodeType_ObjectiveCanTriggerSkillContent", Value = 47)]
			NodeType_ObjectiveCanTriggerSkillContent = 47,
			[ProtoEnum(Name = "NodeType_ObjectiveFreeCaptivesContent", Value = 52)]
			NodeType_ObjectiveFreeCaptivesContent = 52,
			[ProtoEnum(Name = "NodeType_ObjectiveTriggerAreaContent", Value = 53)]
			NodeType_ObjectiveTriggerAreaContent = 53
		}

		private NodeType _Type;

		private MissionAreaNodeContent _MissionAreaNodeContent;

		private MissionAchievementEntryContent _MissionAchievementEntryContent;

		private MissionCodexEntryContent _MissionCodexEntryContent;

		private MissionHotspotsContent _MissionHotspotsContent;

		private MissionPlayerSpawnContent _MissionPlayerSpawnContent;

		private MissionRootContent _MissionRootContent;

		private MissionSectorsContent _MissionSectorsContent;

		private MissionWaypointContent _MissionWaypointContent;

		private MissionTextAnnotationContent _MissionTextAnnotationContent;

		private MissionAreaMovingContent _MissionAreaMovingContent;

		private MissionAreaContent _MissionAreaContent;

		private MissionNpcFormationContent _MissionNpcFormationContent;

		private MissionNpcSpawnContent _MissionNpcSpawnContent;

		private MissionSpawnEntityContent _MissionSpawnEntityContent;

		private MissionSpawnObjectContent _MissionSpawnObjectContent;

		private MissionSpawnPointContent _MissionSpawnPointContent;

		private MissionSpawnStaticObjectContent _MissionSpawnStaticObjectContent;

		private MissionTargetContent _MissionTargetContent;

		private MissionGameLogicModContent _MissionGameLogicModContent;

		private MissionDesignTimeContent _MissionDesignTimeContent;

		private MissionPreventMovementContent _MissionPreventMovementContent;

		private TemplateSpawnPointContent _TemplateSpawnPointContent;

		private TemplateTargetContent _TemplateTargetContent;

		private ObjectiveBecomeAnonymousContent _ObjectiveBecomeAnonymousContent;

		private ObjectiveDeliverContent _ObjectiveDeliverContent;

		private ObjectiveDeliverNotorietyContent _ObjectiveDeliverNotorietyContent;

		private ObjectiveEscapeContent _ObjectiveEscapeContent;

		private ObjectiveEscortContent _ObjectiveEscortContent;

		private ObjectiveGetInRangeMobileContent _ObjectiveGetInRangeMobileContent;

		private ObjectiveGetInRangeStaticContent _ObjectiveGetInRangeStaticContent;

		private ObjectiveHotspotContent _ObjectiveHotspotContent;

		private ObjectiveIdentifyContent _ObjectiveIdentifyContent;

		private ObjectiveInteractionContent _ObjectiveInteractionContent;

		private ObjectiveKillContent _ObjectiveKillContent;

		private ObjectiveKillEnemyTypeContent _ObjectiveKillEnemyTypeContent;

		private ObjectiveLimitContent _ObjectiveLimitContent;

		private ObjectiveSkillLimitContent _ObjectiveSkillLimitContent;

		private ObjectiveNotorietyContent _ObjectiveNotorietyContent;

		private ObjectiveParkourContent _ObjectiveParkourContent;

		private ObjectiveStayInRangeMobileContent _ObjectiveStayInRangeMobileContent;

		private ObjectiveStayInRangeStaticContent _ObjectiveStayInRangeStaticContent;

		private ObjectiveStayAliveContent _ObjectiveStayAliveContent;

		private ObjectiveTimeLimitContent _ObjectiveTimeLimitContent;

		private ObjectiveUiContent _ObjectiveUiContent;

		private ObjectiveTimerContent _ObjectiveTimerContent;

		private ObjectiveVoidContent _ObjectiveVoidContent;

		private ObjectiveSpawnContent _ObjectiveSpawnContent;

		private ObjectiveLootContent _ObjectiveLootContent;

		private ObjectiveSelectTargetContent _ObjectiveSelectTargetContent;

		private ObjectiveCanTriggerSkillContent _ObjectiveCanTriggerSkillContent;

		private ObjectiveEffectSpawnerContent _ObjectiveEffectSpawnerContent;

		private ObjectiveFreeCaptivesContent _ObjectiveFreeCaptivesContent;

		private ObjectiveTriggerAreaContent _ObjectiveTriggerAreaContent;

		private IExtension extensionObject;

		[ProtoMember(1000, IsRequired = false, Name = "Type", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(NodeType.NodeType_Invalid)]
		public NodeType Type
		{
			get
			{
				return _Type;
			}
			set
			{
				_Type = value;
			}
		}

		[ProtoMember(1, IsRequired = false, Name = "MissionAreaNodeContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionAreaNodeContent MissionAreaNodeContent
		{
			get
			{
				return _MissionAreaNodeContent;
			}
			set
			{
				_MissionAreaNodeContent = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "MissionAchievementEntryContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionAchievementEntryContent MissionAchievementEntryContent
		{
			get
			{
				return _MissionAchievementEntryContent;
			}
			set
			{
				_MissionAchievementEntryContent = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "MissionCodexEntryContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionCodexEntryContent MissionCodexEntryContent
		{
			get
			{
				return _MissionCodexEntryContent;
			}
			set
			{
				_MissionCodexEntryContent = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "MissionHotspotsContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionHotspotsContent MissionHotspotsContent
		{
			get
			{
				return _MissionHotspotsContent;
			}
			set
			{
				_MissionHotspotsContent = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "MissionPlayerSpawnContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionPlayerSpawnContent MissionPlayerSpawnContent
		{
			get
			{
				return _MissionPlayerSpawnContent;
			}
			set
			{
				_MissionPlayerSpawnContent = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "MissionRootContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionRootContent MissionRootContent
		{
			get
			{
				return _MissionRootContent;
			}
			set
			{
				_MissionRootContent = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "MissionSectorsContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionSectorsContent MissionSectorsContent
		{
			get
			{
				return _MissionSectorsContent;
			}
			set
			{
				_MissionSectorsContent = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "MissionWaypointContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionWaypointContent MissionWaypointContent
		{
			get
			{
				return _MissionWaypointContent;
			}
			set
			{
				_MissionWaypointContent = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "MissionTextAnnotationContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionTextAnnotationContent MissionTextAnnotationContent
		{
			get
			{
				return _MissionTextAnnotationContent;
			}
			set
			{
				_MissionTextAnnotationContent = value;
			}
		}

		[ProtoMember(10, IsRequired = false, Name = "MissionAreaMovingContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionAreaMovingContent MissionAreaMovingContent
		{
			get
			{
				return _MissionAreaMovingContent;
			}
			set
			{
				_MissionAreaMovingContent = value;
			}
		}

		[ProtoMember(11, IsRequired = false, Name = "MissionAreaContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionAreaContent MissionAreaContent
		{
			get
			{
				return _MissionAreaContent;
			}
			set
			{
				_MissionAreaContent = value;
			}
		}

		[ProtoMember(12, IsRequired = false, Name = "MissionNpcFormationContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionNpcFormationContent MissionNpcFormationContent
		{
			get
			{
				return _MissionNpcFormationContent;
			}
			set
			{
				_MissionNpcFormationContent = value;
			}
		}

		[ProtoMember(13, IsRequired = false, Name = "MissionNpcSpawnContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionNpcSpawnContent MissionNpcSpawnContent
		{
			get
			{
				return _MissionNpcSpawnContent;
			}
			set
			{
				_MissionNpcSpawnContent = value;
			}
		}

		[ProtoMember(14, IsRequired = false, Name = "MissionSpawnEntityContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionSpawnEntityContent MissionSpawnEntityContent
		{
			get
			{
				return _MissionSpawnEntityContent;
			}
			set
			{
				_MissionSpawnEntityContent = value;
			}
		}

		[ProtoMember(15, IsRequired = false, Name = "MissionSpawnObjectContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionSpawnObjectContent MissionSpawnObjectContent
		{
			get
			{
				return _MissionSpawnObjectContent;
			}
			set
			{
				_MissionSpawnObjectContent = value;
			}
		}

		[ProtoMember(16, IsRequired = false, Name = "MissionSpawnPointContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionSpawnPointContent MissionSpawnPointContent
		{
			get
			{
				return _MissionSpawnPointContent;
			}
			set
			{
				_MissionSpawnPointContent = value;
			}
		}

		[ProtoMember(17, IsRequired = false, Name = "MissionSpawnStaticObjectContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionSpawnStaticObjectContent MissionSpawnStaticObjectContent
		{
			get
			{
				return _MissionSpawnStaticObjectContent;
			}
			set
			{
				_MissionSpawnStaticObjectContent = value;
			}
		}

		[ProtoMember(41, IsRequired = false, Name = "MissionTargetContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionTargetContent MissionTargetContent
		{
			get
			{
				return _MissionTargetContent;
			}
			set
			{
				_MissionTargetContent = value;
			}
		}

		[ProtoMember(49, IsRequired = false, Name = "MissionGameLogicModContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionGameLogicModContent MissionGameLogicModContent
		{
			get
			{
				return _MissionGameLogicModContent;
			}
			set
			{
				_MissionGameLogicModContent = value;
			}
		}

		[ProtoMember(50, IsRequired = false, Name = "MissionDesignTimeContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionDesignTimeContent MissionDesignTimeContent
		{
			get
			{
				return _MissionDesignTimeContent;
			}
			set
			{
				_MissionDesignTimeContent = value;
			}
		}

		[ProtoMember(51, IsRequired = false, Name = "MissionPreventMovementContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionPreventMovementContent MissionPreventMovementContent
		{
			get
			{
				return _MissionPreventMovementContent;
			}
			set
			{
				_MissionPreventMovementContent = value;
			}
		}

		[ProtoMember(18, IsRequired = false, Name = "TemplateSpawnPointContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public TemplateSpawnPointContent TemplateSpawnPointContent
		{
			get
			{
				return _TemplateSpawnPointContent;
			}
			set
			{
				_TemplateSpawnPointContent = value;
			}
		}

		[ProtoMember(19, IsRequired = false, Name = "TemplateTargetContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public TemplateTargetContent TemplateTargetContent
		{
			get
			{
				return _TemplateTargetContent;
			}
			set
			{
				_TemplateTargetContent = value;
			}
		}

		[ProtoMember(20, IsRequired = false, Name = "ObjectiveBecomeAnonymousContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public ObjectiveBecomeAnonymousContent ObjectiveBecomeAnonymousContent
		{
			get
			{
				return _ObjectiveBecomeAnonymousContent;
			}
			set
			{
				_ObjectiveBecomeAnonymousContent = value;
			}
		}

		[ProtoMember(21, IsRequired = false, Name = "ObjectiveDeliverContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public ObjectiveDeliverContent ObjectiveDeliverContent
		{
			get
			{
				return _ObjectiveDeliverContent;
			}
			set
			{
				_ObjectiveDeliverContent = value;
			}
		}

		[ProtoMember(22, IsRequired = false, Name = "ObjectiveDeliverNotorietyContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public ObjectiveDeliverNotorietyContent ObjectiveDeliverNotorietyContent
		{
			get
			{
				return _ObjectiveDeliverNotorietyContent;
			}
			set
			{
				_ObjectiveDeliverNotorietyContent = value;
			}
		}

		[ProtoMember(23, IsRequired = false, Name = "ObjectiveEscapeContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public ObjectiveEscapeContent ObjectiveEscapeContent
		{
			get
			{
				return _ObjectiveEscapeContent;
			}
			set
			{
				_ObjectiveEscapeContent = value;
			}
		}

		[ProtoMember(24, IsRequired = false, Name = "ObjectiveEscortContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public ObjectiveEscortContent ObjectiveEscortContent
		{
			get
			{
				return _ObjectiveEscortContent;
			}
			set
			{
				_ObjectiveEscortContent = value;
			}
		}

		[ProtoMember(25, IsRequired = false, Name = "ObjectiveGetInRangeMobileContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public ObjectiveGetInRangeMobileContent ObjectiveGetInRangeMobileContent
		{
			get
			{
				return _ObjectiveGetInRangeMobileContent;
			}
			set
			{
				_ObjectiveGetInRangeMobileContent = value;
			}
		}

		[ProtoMember(26, IsRequired = false, Name = "ObjectiveGetInRangeStaticContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public ObjectiveGetInRangeStaticContent ObjectiveGetInRangeStaticContent
		{
			get
			{
				return _ObjectiveGetInRangeStaticContent;
			}
			set
			{
				_ObjectiveGetInRangeStaticContent = value;
			}
		}

		[ProtoMember(27, IsRequired = false, Name = "ObjectiveHotspotContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public ObjectiveHotspotContent ObjectiveHotspotContent
		{
			get
			{
				return _ObjectiveHotspotContent;
			}
			set
			{
				_ObjectiveHotspotContent = value;
			}
		}

		[ProtoMember(28, IsRequired = false, Name = "ObjectiveIdentifyContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public ObjectiveIdentifyContent ObjectiveIdentifyContent
		{
			get
			{
				return _ObjectiveIdentifyContent;
			}
			set
			{
				_ObjectiveIdentifyContent = value;
			}
		}

		[ProtoMember(29, IsRequired = false, Name = "ObjectiveInteractionContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public ObjectiveInteractionContent ObjectiveInteractionContent
		{
			get
			{
				return _ObjectiveInteractionContent;
			}
			set
			{
				_ObjectiveInteractionContent = value;
			}
		}

		[ProtoMember(30, IsRequired = false, Name = "ObjectiveKillContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public ObjectiveKillContent ObjectiveKillContent
		{
			get
			{
				return _ObjectiveKillContent;
			}
			set
			{
				_ObjectiveKillContent = value;
			}
		}

		[ProtoMember(31, IsRequired = false, Name = "ObjectiveKillEnemyTypeContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public ObjectiveKillEnemyTypeContent ObjectiveKillEnemyTypeContent
		{
			get
			{
				return _ObjectiveKillEnemyTypeContent;
			}
			set
			{
				_ObjectiveKillEnemyTypeContent = value;
			}
		}

		[ProtoMember(32, IsRequired = false, Name = "ObjectiveLimitContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public ObjectiveLimitContent ObjectiveLimitContent
		{
			get
			{
				return _ObjectiveLimitContent;
			}
			set
			{
				_ObjectiveLimitContent = value;
			}
		}

		[ProtoMember(33, IsRequired = false, Name = "ObjectiveSkillLimitContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public ObjectiveSkillLimitContent ObjectiveSkillLimitContent
		{
			get
			{
				return _ObjectiveSkillLimitContent;
			}
			set
			{
				_ObjectiveSkillLimitContent = value;
			}
		}

		[ProtoMember(34, IsRequired = false, Name = "ObjectiveNotorietyContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public ObjectiveNotorietyContent ObjectiveNotorietyContent
		{
			get
			{
				return _ObjectiveNotorietyContent;
			}
			set
			{
				_ObjectiveNotorietyContent = value;
			}
		}

		[ProtoMember(35, IsRequired = false, Name = "ObjectiveParkourContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public ObjectiveParkourContent ObjectiveParkourContent
		{
			get
			{
				return _ObjectiveParkourContent;
			}
			set
			{
				_ObjectiveParkourContent = value;
			}
		}

		[ProtoMember(36, IsRequired = false, Name = "ObjectiveStayInRangeMobileContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public ObjectiveStayInRangeMobileContent ObjectiveStayInRangeMobileContent
		{
			get
			{
				return _ObjectiveStayInRangeMobileContent;
			}
			set
			{
				_ObjectiveStayInRangeMobileContent = value;
			}
		}

		[ProtoMember(37, IsRequired = false, Name = "ObjectiveStayInRangeStaticContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public ObjectiveStayInRangeStaticContent ObjectiveStayInRangeStaticContent
		{
			get
			{
				return _ObjectiveStayInRangeStaticContent;
			}
			set
			{
				_ObjectiveStayInRangeStaticContent = value;
			}
		}

		[ProtoMember(38, IsRequired = false, Name = "ObjectiveStayAliveContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public ObjectiveStayAliveContent ObjectiveStayAliveContent
		{
			get
			{
				return _ObjectiveStayAliveContent;
			}
			set
			{
				_ObjectiveStayAliveContent = value;
			}
		}

		[ProtoMember(39, IsRequired = false, Name = "ObjectiveTimeLimitContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public ObjectiveTimeLimitContent ObjectiveTimeLimitContent
		{
			get
			{
				return _ObjectiveTimeLimitContent;
			}
			set
			{
				_ObjectiveTimeLimitContent = value;
			}
		}

		[ProtoMember(40, IsRequired = false, Name = "ObjectiveUiContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public ObjectiveUiContent ObjectiveUiContent
		{
			get
			{
				return _ObjectiveUiContent;
			}
			set
			{
				_ObjectiveUiContent = value;
			}
		}

		[ProtoMember(42, IsRequired = false, Name = "ObjectiveTimerContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public ObjectiveTimerContent ObjectiveTimerContent
		{
			get
			{
				return _ObjectiveTimerContent;
			}
			set
			{
				_ObjectiveTimerContent = value;
			}
		}

		[ProtoMember(43, IsRequired = false, Name = "ObjectiveVoidContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public ObjectiveVoidContent ObjectiveVoidContent
		{
			get
			{
				return _ObjectiveVoidContent;
			}
			set
			{
				_ObjectiveVoidContent = value;
			}
		}

		[ProtoMember(44, IsRequired = false, Name = "ObjectiveSpawnContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public ObjectiveSpawnContent ObjectiveSpawnContent
		{
			get
			{
				return _ObjectiveSpawnContent;
			}
			set
			{
				_ObjectiveSpawnContent = value;
			}
		}

		[ProtoMember(45, IsRequired = false, Name = "ObjectiveLootContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public ObjectiveLootContent ObjectiveLootContent
		{
			get
			{
				return _ObjectiveLootContent;
			}
			set
			{
				_ObjectiveLootContent = value;
			}
		}

		[ProtoMember(46, IsRequired = false, Name = "ObjectiveSelectTargetContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public ObjectiveSelectTargetContent ObjectiveSelectTargetContent
		{
			get
			{
				return _ObjectiveSelectTargetContent;
			}
			set
			{
				_ObjectiveSelectTargetContent = value;
			}
		}

		[ProtoMember(47, IsRequired = false, Name = "ObjectiveCanTriggerSkillContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public ObjectiveCanTriggerSkillContent ObjectiveCanTriggerSkillContent
		{
			get
			{
				return _ObjectiveCanTriggerSkillContent;
			}
			set
			{
				_ObjectiveCanTriggerSkillContent = value;
			}
		}

		[ProtoMember(48, IsRequired = false, Name = "ObjectiveEffectSpawnerContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public ObjectiveEffectSpawnerContent ObjectiveEffectSpawnerContent
		{
			get
			{
				return _ObjectiveEffectSpawnerContent;
			}
			set
			{
				_ObjectiveEffectSpawnerContent = value;
			}
		}

		[ProtoMember(52, IsRequired = false, Name = "ObjectiveFreeCaptivesContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public ObjectiveFreeCaptivesContent ObjectiveFreeCaptivesContent
		{
			get
			{
				return _ObjectiveFreeCaptivesContent;
			}
			set
			{
				_ObjectiveFreeCaptivesContent = value;
			}
		}

		[ProtoMember(53, IsRequired = false, Name = "ObjectiveTriggerAreaContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public ObjectiveTriggerAreaContent ObjectiveTriggerAreaContent
		{
			get
			{
				return _ObjectiveTriggerAreaContent;
			}
			set
			{
				_ObjectiveTriggerAreaContent = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
