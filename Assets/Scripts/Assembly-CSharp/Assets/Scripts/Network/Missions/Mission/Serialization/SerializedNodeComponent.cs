using System;
using AcierProtocol;
using Assets.Scripts.Network.Missions.Mission.Data;

namespace Assets.Scripts.Network.Missions.Mission.Serialization
{
	public class SerializedNodeComponent
	{
		public MissionBaseData NodeData { get; private set; }

		public Type DataType { get; private set; }

		public string ComponentType { get; private set; }

		public SerializedNode Node { get; private set; }

		public SerializedNodeComponent(SerializedNode _ownerNode, MissionBaseData _data, string _componentType)
		{
			DataType = ((_data == null) ? typeof(void) : _data.GetType());
			NodeData = _data;
			ComponentType = _componentType;
			Node = _ownerNode;
		}

		public SerializedNodeComponent(SerializedNode _ownerNode, AcierProtocol.SerializedNodeComponent _component)
		{
			Node = _ownerNode;
			switch (_component.Type)
			{
			case AcierProtocol.SerializedNodeComponent.NodeType.NodeType_MissionAreaNodeContent:
				ComponentType = "AreaNode";
				NodeData = new AreaNodeData(_component.MissionAreaNodeContent);
				break;
			case AcierProtocol.SerializedNodeComponent.NodeType.NodeType_MissionAchievementEntryContent:
				ComponentType = "MissionAchievementEntry";
				NodeData = new MissionAchievementEntryData(_component.MissionAchievementEntryContent);
				break;
			case AcierProtocol.SerializedNodeComponent.NodeType.NodeType_MissionCodexEntryContent:
				ComponentType = "MissionCodexEntry";
				NodeData = new MissionCodexEntryData(_component.MissionCodexEntryContent);
				break;
			case AcierProtocol.SerializedNodeComponent.NodeType.NodeType_MissionHotspotsContent:
				ComponentType = "MissionHotspots";
				NodeData = new MissionHotspotsData(_component.MissionHotspotsContent);
				break;
			case AcierProtocol.SerializedNodeComponent.NodeType.NodeType_MissionPlayerSpawnContent:
				ComponentType = "MissionPlayerSpawn";
				NodeData = new MissionPlayerSpawnData(_component.MissionPlayerSpawnContent);
				break;
			case AcierProtocol.SerializedNodeComponent.NodeType.NodeType_MissionRootContent:
				ComponentType = "MissionRoot";
				NodeData = new MissionRootData(_component.MissionRootContent);
				break;
			case AcierProtocol.SerializedNodeComponent.NodeType.NodeType_MissionSectorsContent:
				ComponentType = "MissionSectors";
				NodeData = new MissionSectorsData(_component.MissionSectorsContent);
				break;
			case AcierProtocol.SerializedNodeComponent.NodeType.NodeType_MissionWaypointContent:
				ComponentType = "MissionWaypoint";
				NodeData = new MissionWaypointData(_component.MissionWaypointContent);
				break;
			case AcierProtocol.SerializedNodeComponent.NodeType.NodeType_MissionTextAnnotationContent:
				ComponentType = "MissionTextAnnotation";
				NodeData = new MissionTextAnnotationData(_component.MissionTextAnnotationContent);
				break;
			case AcierProtocol.SerializedNodeComponent.NodeType.NodeType_MissionGameLogicModContent:
				ComponentType = "MissionGameLogicMod";
				NodeData = new MissionGameLogicModData(_component.MissionGameLogicModContent);
				break;
			case AcierProtocol.SerializedNodeComponent.NodeType.NodeType_MissionAreaMovingContent:
				ComponentType = "MissionAreaMoving";
				NodeData = new MissionAreaMovingData(_component.MissionAreaMovingContent);
				break;
			case AcierProtocol.SerializedNodeComponent.NodeType.NodeType_MissionAreaContent:
				ComponentType = "MissionArea";
				NodeData = new MissionAreaData(_component.MissionAreaContent);
				break;
			case AcierProtocol.SerializedNodeComponent.NodeType.NodeType_MissionNpcFormationContent:
				ComponentType = "MissionNpcFormation";
				NodeData = new MissionNpcFormationData(_component.MissionNpcFormationContent);
				break;
			case AcierProtocol.SerializedNodeComponent.NodeType.NodeType_MissionNpcSpawnContent:
				ComponentType = "MissionNpcSpawn";
				NodeData = new MissionNpcSpawnData(_component.MissionNpcSpawnContent);
				break;
			case AcierProtocol.SerializedNodeComponent.NodeType.NodeType_MissionSpawnObjectContent:
				ComponentType = "MissionSpawnObject";
				NodeData = new MissionSpawnObjectData(_component.MissionSpawnObjectContent);
				break;
			case AcierProtocol.SerializedNodeComponent.NodeType.NodeType_MissionSpawnPointContent:
				ComponentType = "MissionSpawnPoint";
				NodeData = new MissionSpawnPointData(_component.MissionSpawnPointContent);
				break;
			case AcierProtocol.SerializedNodeComponent.NodeType.NodeType_MissionSpawnStaticObjectContent:
				ComponentType = "MissionSpawnStaticObject";
				NodeData = new MissionSpawnStaticObjectData(_component.MissionSpawnStaticObjectContent);
				break;
			case AcierProtocol.SerializedNodeComponent.NodeType.NodeType_TemplateSpawnPointContent:
				ComponentType = "TemplateSpawnPoint";
				NodeData = new TemplateSpawnPointData(_component.TemplateSpawnPointContent);
				break;
			case AcierProtocol.SerializedNodeComponent.NodeType.NodeType_TemplateTargetContent:
				ComponentType = "TemplateTarget";
				NodeData = new TemplateTargetData(_component.TemplateTargetContent);
				break;
			case AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveBecomeAnonymousContent:
				ComponentType = "ObjectiveBecomeAnonymous";
				NodeData = new ObjectiveBecomeAnonymousData(_component.ObjectiveBecomeAnonymousContent);
				break;
			case AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveDeliverContent:
				ComponentType = "ObjectiveDeliver";
				NodeData = new ObjectiveDeliverData(_component.ObjectiveDeliverContent);
				break;
			case AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveDeliverNotorietyContent:
				ComponentType = "ObjectiveDeliverNotoriety";
				NodeData = new ObjectiveDeliverNotorietyData(_component.ObjectiveDeliverNotorietyContent);
				break;
			case AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveEscapeContent:
				ComponentType = "ObjectiveEscape";
				NodeData = new ObjectiveEscapeData(_component.ObjectiveEscapeContent);
				break;
			case AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveEscortContent:
				ComponentType = "ObjectiveEscort";
				NodeData = new ObjectiveEscortData(_component.ObjectiveEscortContent);
				break;
			case AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveGetInRangeMobileContent:
				ComponentType = "ObjectiveGetInRangeMobile";
				NodeData = new ObjectiveGetInRangeMobileData(_component.ObjectiveGetInRangeMobileContent);
				break;
			case AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveGetInRangeStaticContent:
				ComponentType = "ObjectiveGetInRangeStatic";
				NodeData = new ObjectiveGetInRangeStaticData(_component.ObjectiveGetInRangeStaticContent);
				break;
			case AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveHotspotContent:
				ComponentType = "ObjectiveHotspot";
				NodeData = new ObjectiveHotspotData(_component.ObjectiveHotspotContent);
				break;
			case AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveIdentifyContent:
				ComponentType = "ObjectiveIdentify";
				NodeData = new ObjectiveIdentifyData(_component.ObjectiveIdentifyContent);
				break;
			case AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveInteractionContent:
				ComponentType = "ObjectiveInteraction";
				NodeData = new ObjectiveInteractionData(_component.ObjectiveInteractionContent);
				break;
			case AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveKillContent:
				ComponentType = "ObjectiveKill";
				NodeData = new ObjectiveKillData(_component.ObjectiveKillContent);
				break;
			case AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveKillEnemyTypeContent:
				ComponentType = "ObjectiveKillEnemyType";
				NodeData = new ObjectiveKillEnemyTypeData(_component.ObjectiveKillEnemyTypeContent);
				break;
			case AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveLimitContent:
				ComponentType = "ObjectiveLimit";
				NodeData = new ObjectiveLimitData(_component.ObjectiveLimitContent);
				break;
			case AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveSkillLimitContent:
				ComponentType = "ObjectiveSkillLimit";
				NodeData = new ObjectiveSkillLimitData(_component.ObjectiveSkillLimitContent);
				break;
			case AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveNotorietyContent:
				ComponentType = "ObjectiveNotoriety";
				NodeData = new ObjectiveNotorietyData(_component.ObjectiveNotorietyContent);
				break;
			case AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveParkourContent:
				ComponentType = "ObjectiveParkour";
				NodeData = new ObjectiveParkourData(_component.ObjectiveParkourContent);
				break;
			case AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveStayInRangeMobileContent:
				ComponentType = "ObjectiveStayInRangeMobile";
				NodeData = new ObjectiveStayInRangeMobileData(_component.ObjectiveStayInRangeMobileContent);
				break;
			case AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveStayInRangeStaticContent:
				ComponentType = "ObjectiveStayInRangeStatic";
				NodeData = new ObjectiveStayInRangeStaticData(_component.ObjectiveStayInRangeStaticContent);
				break;
			case AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveStayAliveContent:
				ComponentType = "ObjectiveStayAlive";
				NodeData = new ObjectiveStayAliveData(_component.ObjectiveStayAliveContent);
				break;
			case AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveTimeLimitContent:
				ComponentType = "ObjectiveTimeLimit";
				NodeData = new ObjectiveTimeLimitData(_component.ObjectiveTimeLimitContent);
				break;
			case AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveUiContent:
				ComponentType = "ObjectiveUi";
				NodeData = new ObjectiveUiData(_component.ObjectiveUiContent);
				break;
			case AcierProtocol.SerializedNodeComponent.NodeType.NodeType_MissionTarget:
				ComponentType = "MissionTarget";
				NodeData = new MissionTargetData(_component.MissionTargetContent);
				break;
			case AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveTimerContent:
				ComponentType = "ObjectiveTimer";
				NodeData = new ObjectiveTimerData(_component.ObjectiveTimerContent);
				break;
			case AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveVoidContent:
				ComponentType = "ObjectiveVoid";
				NodeData = new ObjectiveVoidData(_component.ObjectiveVoidContent);
				break;
			case AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveSpawnContent:
				ComponentType = "ObjectiveSpawn";
				NodeData = new ObjectiveSpawnData(_component.ObjectiveSpawnContent);
				break;
			case AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveLootContent:
				ComponentType = "ObjectiveLoot";
				NodeData = new ObjectiveLootData(_component.ObjectiveLootContent);
				break;
			case AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveSelectTargetContent:
				ComponentType = "ObjectiveSelectTarget";
				NodeData = new ObjectiveSelectTargetData(_component.ObjectiveSelectTargetContent);
				break;
			case AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveCanTriggerSkillContent:
				ComponentType = "ObjectiveCanTriggerSkill";
				NodeData = new ObjectiveCanTriggerSkillData(_component.ObjectiveCanTriggerSkillContent);
				break;
			case AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveEffectSpawnerContent:
				ComponentType = "ObjectiveEffectSpawner";
				NodeData = new ObjectiveEffectSpawnerData(_component.ObjectiveEffectSpawnerContent);
				break;
			case AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveFreeCaptivesContent:
				ComponentType = "ObjectiveFreeCaptives";
				NodeData = new ObjectiveFreeCaptivesData(_component.ObjectiveFreeCaptivesContent);
				break;
			case AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveTriggerAreaContent:
				ComponentType = "ObjectiveTriggerAreaManager";
				NodeData = new ObjectiveTriggerAreaManagerData(_component.ObjectiveTriggerAreaContent);
				break;
			case AcierProtocol.SerializedNodeComponent.NodeType.NodeType_MissionDesignTimeContent:
				ComponentType = "MissionDesignTime";
				NodeData = new MissionDesignTimeData(_component.MissionDesignTimeContent);
				break;
			case AcierProtocol.SerializedNodeComponent.NodeType.NodeType_MissionPreventMovementContent:
				ComponentType = "MissionPreventMovement";
				NodeData = new MissionPreventMovementData(_component.MissionPreventMovementContent);
				break;
			default:
				throw new ArgumentException("No implementation found for " + _component.Type);
			}
		}

		public override string ToString()
		{
			return string.Format("Component: Node({0}), {1}", Node, DataType);
		}

		public AcierProtocol.SerializedNodeComponent SerializeProto()
		{
			AcierProtocol.SerializedNodeComponent serializedNodeComponent = new AcierProtocol.SerializedNodeComponent();
			switch (ComponentType)
			{
			case "AreaNode":
				serializedNodeComponent.Type = AcierProtocol.SerializedNodeComponent.NodeType.NodeType_MissionAreaNodeContent;
				serializedNodeComponent.MissionAreaNodeContent = (MissionAreaNodeContent)NodeData.SerializeProto();
				break;
			case "MissionAchievementEntry":
				serializedNodeComponent.Type = AcierProtocol.SerializedNodeComponent.NodeType.NodeType_MissionAchievementEntryContent;
				serializedNodeComponent.MissionAchievementEntryContent = (MissionAchievementEntryContent)NodeData.SerializeProto();
				break;
			case "MissionCodexEntry":
				serializedNodeComponent.Type = AcierProtocol.SerializedNodeComponent.NodeType.NodeType_MissionCodexEntryContent;
				serializedNodeComponent.MissionCodexEntryContent = (MissionCodexEntryContent)NodeData.SerializeProto();
				break;
			case "MissionHotspots":
				serializedNodeComponent.Type = AcierProtocol.SerializedNodeComponent.NodeType.NodeType_MissionHotspotsContent;
				serializedNodeComponent.MissionHotspotsContent = (MissionHotspotsContent)NodeData.SerializeProto();
				break;
			case "MissionPlayerSpawn":
				serializedNodeComponent.Type = AcierProtocol.SerializedNodeComponent.NodeType.NodeType_MissionPlayerSpawnContent;
				serializedNodeComponent.MissionPlayerSpawnContent = (MissionPlayerSpawnContent)NodeData.SerializeProto();
				break;
			case "MissionRoot":
				serializedNodeComponent.Type = AcierProtocol.SerializedNodeComponent.NodeType.NodeType_MissionRootContent;
				serializedNodeComponent.MissionRootContent = (MissionRootContent)NodeData.SerializeProto();
				break;
			case "MissionSectors":
				serializedNodeComponent.Type = AcierProtocol.SerializedNodeComponent.NodeType.NodeType_MissionSectorsContent;
				serializedNodeComponent.MissionSectorsContent = (MissionSectorsContent)NodeData.SerializeProto();
				break;
			case "MissionWaypoint":
				serializedNodeComponent.Type = AcierProtocol.SerializedNodeComponent.NodeType.NodeType_MissionWaypointContent;
				serializedNodeComponent.MissionWaypointContent = (MissionWaypointContent)NodeData.SerializeProto();
				break;
			case "MissionTextAnnotation":
				serializedNodeComponent.Type = AcierProtocol.SerializedNodeComponent.NodeType.NodeType_MissionTextAnnotationContent;
				serializedNodeComponent.MissionTextAnnotationContent = (MissionTextAnnotationContent)NodeData.SerializeProto();
				break;
			case "MissionGameLogicMod":
				serializedNodeComponent.Type = AcierProtocol.SerializedNodeComponent.NodeType.NodeType_MissionGameLogicModContent;
				serializedNodeComponent.MissionGameLogicModContent = (MissionGameLogicModContent)NodeData.SerializeProto();
				break;
			case "ObjectiveEffectSpawner":
				serializedNodeComponent.Type = AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveEffectSpawnerContent;
				serializedNodeComponent.ObjectiveEffectSpawnerContent = (ObjectiveEffectSpawnerContent)NodeData.SerializeProto();
				break;
			case "MissionAreaMoving":
				serializedNodeComponent.Type = AcierProtocol.SerializedNodeComponent.NodeType.NodeType_MissionAreaMovingContent;
				serializedNodeComponent.MissionAreaMovingContent = (MissionAreaMovingContent)NodeData.SerializeProto();
				break;
			case "MissionArea":
				serializedNodeComponent.Type = AcierProtocol.SerializedNodeComponent.NodeType.NodeType_MissionAreaContent;
				serializedNodeComponent.MissionAreaContent = (MissionAreaContent)NodeData.SerializeProto();
				break;
			case "MissionNpcFormation":
				serializedNodeComponent.Type = AcierProtocol.SerializedNodeComponent.NodeType.NodeType_MissionNpcFormationContent;
				serializedNodeComponent.MissionNpcFormationContent = (MissionNpcFormationContent)NodeData.SerializeProto();
				break;
			case "MissionNpcSpawn":
				serializedNodeComponent.Type = AcierProtocol.SerializedNodeComponent.NodeType.NodeType_MissionNpcSpawnContent;
				serializedNodeComponent.MissionNpcSpawnContent = (MissionNpcSpawnContent)NodeData.SerializeProto();
				break;
			case "MissionSpawnObject":
				serializedNodeComponent.Type = AcierProtocol.SerializedNodeComponent.NodeType.NodeType_MissionSpawnObjectContent;
				serializedNodeComponent.MissionSpawnObjectContent = (MissionSpawnObjectContent)NodeData.SerializeProto();
				break;
			case "MissionSpawnPoint":
				serializedNodeComponent.Type = AcierProtocol.SerializedNodeComponent.NodeType.NodeType_MissionSpawnPointContent;
				serializedNodeComponent.MissionSpawnPointContent = (MissionSpawnPointContent)NodeData.SerializeProto();
				break;
			case "MissionSpawnStaticObject":
				serializedNodeComponent.Type = AcierProtocol.SerializedNodeComponent.NodeType.NodeType_MissionSpawnStaticObjectContent;
				serializedNodeComponent.MissionSpawnStaticObjectContent = (MissionSpawnStaticObjectContent)NodeData.SerializeProto();
				break;
			case "TemplateSpawnPoint":
				serializedNodeComponent.Type = AcierProtocol.SerializedNodeComponent.NodeType.NodeType_TemplateSpawnPointContent;
				serializedNodeComponent.TemplateSpawnPointContent = (TemplateSpawnPointContent)NodeData.SerializeProto();
				break;
			case "TemplateTarget":
				serializedNodeComponent.Type = AcierProtocol.SerializedNodeComponent.NodeType.NodeType_TemplateTargetContent;
				serializedNodeComponent.TemplateTargetContent = (TemplateTargetContent)NodeData.SerializeProto();
				break;
			case "ObjectiveBecomeAnonymous":
				serializedNodeComponent.Type = AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveBecomeAnonymousContent;
				serializedNodeComponent.ObjectiveBecomeAnonymousContent = (ObjectiveBecomeAnonymousContent)NodeData.SerializeProto();
				break;
			case "ObjectiveDeliver":
				serializedNodeComponent.Type = AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveDeliverContent;
				serializedNodeComponent.ObjectiveDeliverContent = (ObjectiveDeliverContent)NodeData.SerializeProto();
				break;
			case "ObjectiveDeliverNotoriety":
				serializedNodeComponent.Type = AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveDeliverNotorietyContent;
				serializedNodeComponent.ObjectiveDeliverNotorietyContent = (ObjectiveDeliverNotorietyContent)NodeData.SerializeProto();
				break;
			case "ObjectiveEscape":
				serializedNodeComponent.Type = AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveEscapeContent;
				serializedNodeComponent.ObjectiveEscapeContent = (ObjectiveEscapeContent)NodeData.SerializeProto();
				break;
			case "ObjectiveEscort":
				serializedNodeComponent.Type = AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveEscortContent;
				serializedNodeComponent.ObjectiveEscortContent = (ObjectiveEscortContent)NodeData.SerializeProto();
				break;
			case "ObjectiveGetInRangeMobile":
				serializedNodeComponent.Type = AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveGetInRangeMobileContent;
				serializedNodeComponent.ObjectiveGetInRangeMobileContent = (ObjectiveGetInRangeMobileContent)NodeData.SerializeProto();
				break;
			case "ObjectiveGetInRangeStatic":
				serializedNodeComponent.Type = AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveGetInRangeStaticContent;
				serializedNodeComponent.ObjectiveGetInRangeStaticContent = (ObjectiveGetInRangeStaticContent)NodeData.SerializeProto();
				break;
			case "ObjectiveHotspot":
				serializedNodeComponent.Type = AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveHotspotContent;
				serializedNodeComponent.ObjectiveHotspotContent = (ObjectiveHotspotContent)NodeData.SerializeProto();
				break;
			case "ObjectiveIdentify":
				serializedNodeComponent.Type = AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveIdentifyContent;
				serializedNodeComponent.ObjectiveIdentifyContent = (ObjectiveIdentifyContent)NodeData.SerializeProto();
				break;
			case "ObjectiveInteraction":
				serializedNodeComponent.Type = AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveInteractionContent;
				serializedNodeComponent.ObjectiveInteractionContent = (ObjectiveInteractionContent)NodeData.SerializeProto();
				break;
			case "ObjectiveKill":
				serializedNodeComponent.Type = AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveKillContent;
				serializedNodeComponent.ObjectiveKillContent = (ObjectiveKillContent)NodeData.SerializeProto();
				break;
			case "ObjectiveKillEnemyType":
				serializedNodeComponent.Type = AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveKillEnemyTypeContent;
				serializedNodeComponent.ObjectiveKillEnemyTypeContent = (ObjectiveKillEnemyTypeContent)NodeData.SerializeProto();
				break;
			case "ObjectiveLimit":
				serializedNodeComponent.Type = AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveLimitContent;
				serializedNodeComponent.ObjectiveLimitContent = (ObjectiveLimitContent)NodeData.SerializeProto();
				break;
			case "ObjectiveSkillLimit":
				serializedNodeComponent.Type = AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveSkillLimitContent;
				serializedNodeComponent.ObjectiveSkillLimitContent = (ObjectiveSkillLimitContent)NodeData.SerializeProto();
				break;
			case "ObjectiveNotoriety":
				serializedNodeComponent.Type = AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveNotorietyContent;
				serializedNodeComponent.ObjectiveNotorietyContent = (ObjectiveNotorietyContent)NodeData.SerializeProto();
				break;
			case "ObjectiveParkour":
				serializedNodeComponent.Type = AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveParkourContent;
				serializedNodeComponent.ObjectiveParkourContent = (ObjectiveParkourContent)NodeData.SerializeProto();
				break;
			case "ObjectiveStayInRangeMobile":
				serializedNodeComponent.Type = AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveStayInRangeMobileContent;
				serializedNodeComponent.ObjectiveStayInRangeMobileContent = (ObjectiveStayInRangeMobileContent)NodeData.SerializeProto();
				break;
			case "ObjectiveStayInRangeStatic":
				serializedNodeComponent.Type = AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveStayInRangeStaticContent;
				serializedNodeComponent.ObjectiveStayInRangeStaticContent = (ObjectiveStayInRangeStaticContent)NodeData.SerializeProto();
				break;
			case "ObjectiveStayAlive":
				serializedNodeComponent.Type = AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveStayAliveContent;
				serializedNodeComponent.ObjectiveStayAliveContent = (ObjectiveStayAliveContent)NodeData.SerializeProto();
				break;
			case "ObjectiveTimeLimit":
				serializedNodeComponent.Type = AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveTimeLimitContent;
				serializedNodeComponent.ObjectiveTimeLimitContent = (ObjectiveTimeLimitContent)NodeData.SerializeProto();
				break;
			case "ObjectiveUi":
				serializedNodeComponent.Type = AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveUiContent;
				serializedNodeComponent.ObjectiveUiContent = (ObjectiveUiContent)NodeData.SerializeProto();
				break;
			case "MissionTarget":
				serializedNodeComponent.Type = AcierProtocol.SerializedNodeComponent.NodeType.NodeType_MissionTarget;
				serializedNodeComponent.MissionTargetContent = (MissionTargetContent)NodeData.SerializeProto();
				break;
			case "ObjectiveTimer":
				serializedNodeComponent.Type = AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveTimerContent;
				serializedNodeComponent.ObjectiveTimerContent = (ObjectiveTimerContent)NodeData.SerializeProto();
				break;
			case "ObjectiveVoid":
				serializedNodeComponent.Type = AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveVoidContent;
				serializedNodeComponent.ObjectiveVoidContent = (ObjectiveVoidContent)NodeData.SerializeProto();
				break;
			case "ObjectiveSpawn":
				serializedNodeComponent.Type = AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveSpawnContent;
				serializedNodeComponent.ObjectiveSpawnContent = (ObjectiveSpawnContent)NodeData.SerializeProto();
				break;
			case "ObjectiveLoot":
				serializedNodeComponent.Type = AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveLootContent;
				serializedNodeComponent.ObjectiveLootContent = (ObjectiveLootContent)NodeData.SerializeProto();
				break;
			case "ObjectiveSelectTarget":
				serializedNodeComponent.Type = AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveSelectTargetContent;
				serializedNodeComponent.ObjectiveSelectTargetContent = (ObjectiveSelectTargetContent)NodeData.SerializeProto();
				break;
			case "ObjectiveCanTriggerSkill":
				serializedNodeComponent.Type = AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveCanTriggerSkillContent;
				serializedNodeComponent.ObjectiveCanTriggerSkillContent = (ObjectiveCanTriggerSkillContent)NodeData.SerializeProto();
				break;
			case "ObjectiveFreeCaptives":
				serializedNodeComponent.Type = AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveFreeCaptivesContent;
				serializedNodeComponent.ObjectiveFreeCaptivesContent = (ObjectiveFreeCaptivesContent)NodeData.SerializeProto();
				break;
			case "ObjectiveTriggerAreaManager":
				serializedNodeComponent.Type = AcierProtocol.SerializedNodeComponent.NodeType.NodeType_ObjectiveTriggerAreaContent;
				serializedNodeComponent.ObjectiveTriggerAreaContent = (ObjectiveTriggerAreaContent)NodeData.SerializeProto();
				break;
			case "MissionDesignTime":
				serializedNodeComponent.Type = AcierProtocol.SerializedNodeComponent.NodeType.NodeType_MissionDesignTimeContent;
				serializedNodeComponent.MissionDesignTimeContent = (MissionDesignTimeContent)NodeData.SerializeProto();
				break;
			case "MissionPreventMovement":
				serializedNodeComponent.Type = AcierProtocol.SerializedNodeComponent.NodeType.NodeType_MissionPreventMovementContent;
				serializedNodeComponent.MissionPreventMovementContent = (MissionPreventMovementContent)NodeData.SerializeProto();
				break;
			}
			return serializedNodeComponent;
		}
	}
}
