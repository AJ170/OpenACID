using AcierProtocol;
using Assets.Scripts.Audio;
using Assets.Scripts.Engine.ExtensionMethods;
using Assets.Scripts.Mission;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.Mission;
using Assets.Scripts.Mission.Motion.Hotspots;
using Assets.Scripts.Network.Missions.Mission.Data;
using Database;
using SharedBaseLib.System;
using UbiCore.Utils;
using UbiCore.Utils.Extensions;
using UnityEngine;

public class MissionSpawnObject : MissionSpawnEntity<MissionSpawnObjectData>
{
	[ObjectTypeGuid]
	public int Type;

	private GameObject m_geometryNode;

	public InteractableObjectData.ObjectType ObjectType { get; private set; }

	public override void OnPostRead(MissionBaseData _nodeData)
	{
		base.OnPostRead(_nodeData);
		ObjectType = new InteractableObjectData.ObjectType(Main.Instance.GameDB.FindGameIdFromGuid(base.Data.TypeGuid));
		Type = ObjectType.GameID.Guid;
	}

	public override void PlaySuccessfullInteractionSound()
	{
		InteractableObjectData.IInteractableObjectDefinition interactableObjectDefinition = EntityID.Entity.Definition.EntityDefinition as InteractableObjectData.IInteractableObjectDefinition;
		if (interactableObjectDefinition != null)
		{
			switch (interactableObjectDefinition.SubCategory)
			{
			case EObjectSubCategory.Chest:
				AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_treasurechest_collectloot);
				break;
			}
		}
	}

	public override void OnDeactivate()
	{
		base.OnDeactivate();
		if (m_geometryNode != null)
		{
			UnityEngine.Object.Destroy(m_geometryNode);
			m_geometryNode = null;
		}
	}

	protected override GameEntity CreateEntity()
	{
		InteractableObjectData.IInteractableObjectDefinition definition = ObjectType.Definition;
		InteractableMissionObject interactableMissionObject = Assets.Scripts.Mission.Logic.Helpers.CreateEntity<InteractableMissionObject>(definition.EntityType);
		interactableMissionObject.Definition.EntityDefinition = definition;
		if (m_geometryNode == null)
		{
			return interactableMissionObject;
		}
		m_geometryNode.SetActive(true);
		GameObject gameObject = Assets.Scripts.Mission.Helpers.Instantiate("Prefab_Object");
		if (gameObject == null)
		{
			return interactableMissionObject;
		}
		gameObject.ParentTo(m_geometryNode);
		gameObject.SetActive(true);
		if (definition.Category == EObjectCategory.Mission_EObjectCategory && definition.SubCategory == EObjectSubCategory.Chest)
		{
			GameObject gameObject2 = ItemHelper.CreateItemOrEffectFromDB(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_effectChestHighlight);
			gameObject2.ParentTo(gameObject);
			gameObject2.SetActive(true);
		}
		GameObject gameObject3 = gameObject.FindChildByName("SelectionCollider");
		if (!gameObject3)
		{
			return interactableMissionObject;
		}
		gameObject3.tag = "Interactable";
		gameObject3.layer = Layers.InteractorsLayer;
		return interactableMissionObject;
	}

	protected override void PostSetupEntity(GameEntity _entity)
	{
		base.PostSetupEntity(_entity);
		InteractableMissionObject interactableMissionObject = _entity as InteractableMissionObject;
		if (interactableMissionObject != null)
		{
			EntityWrapperComponent entityWrapperComponent = m_geometryNode.AddComponent<EntityWrapperComponent>();
			entityWrapperComponent.Entity = interactableMissionObject;
			ApproachEntityIcon approachEntityIcon = new ApproachEntityIcon();
			interactableMissionObject.HeadIcon.IconAttachment = approachEntityIcon;
			approachEntityIcon.Visible = true;
			approachEntityIcon.Offset3D = new Vector3(0f, 0f, 0f);
			approachEntityIcon.SetIcon(IconMaterial.InteractObject, 5);
			approachEntityIcon.SetInteractable(true, Handle);
			interactableMissionObject.InteractableObjectStats.Set_bool(StatTypes.EStatType.Interaction_IsRelevant, !base.Data.IsMimic);
			interactableMissionObject.Highlight.SetMode(Highlight.EHighlightLayer.Base, Highlight.ESelectionMode.Off);
		}
	}

	private bool Handle(NavMeshAgent _agent, IconAttachment _attachment)
	{
		InteractableMissionObject interactableMissionObject = ((EntityIcon)_attachment).HeadIcon.GameEntity as InteractableMissionObject;
		if (interactableMissionObject != null)
		{
			return UbiCore.Utils.Singleton<HotspotService>.Instance.Interact(PlayerHelper.Entity, interactableMissionObject.GameObject, _agent);
		}
		return false;
	}

	public override bool OnDeserialized()
	{
		if (!base.OnDeserialized())
		{
			return false;
		}
		m_geometryNode = ItemHelper.CreateItemOrEffectFromDB(ObjectType.Definition.ResourceGameID);
		m_geometryNode.SetActive(false);
		return true;
	}
}
