using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.Stats;
using SharedBaseLib.System;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.Entities
{
	public class CrowdCharacter : NpcCharacter, ICrowdCharacterStatsCarrier, IDefinitionCarrier, ICharacterStateMachinesCarrier, ICrowdCharacterStateMachinesCarrier, IEntityStatsCarrier, IGameObjectProxyCarrier, IHighlightCarrier, ISpacialEntityCarrier, IStateMachinesCarrier, IUnityComponentCacheCarrier, IEntity, IUpdateableEntity, IPositionProvider, IPositionEntity, IGameObjectEntity, IGameEntity, ICharacterConditionCarrierEntity
	{
		private readonly CharacterStats m_charStats;

		public override EntityTypes.EType Type
		{
			get
			{
				return EntityTypes.EType.CrowdCharacter;
			}
		}

		public override CharacterStats CharacterStats
		{
			get
			{
				return m_charStats;
			}
		}

		public CrowdCharacterStats CrowdCharacterStats
		{
			get
			{
				return (CrowdCharacterStats)m_charStats;
			}
		}

		public ICharacterConditionCarrier CharacterConditionCarrier
		{
			get
			{
				return CrowdCharacterStateMachines;
			}
		}

		public CrowdCharacterStateMachines CrowdCharacterStateMachines
		{
			get
			{
				return (CrowdCharacterStateMachines)CharacterStateMachines;
			}
		}

		public override bool SkipLogicUpdateAllowed
		{
			get
			{
				return base.SkipLogicUpdateAllowed && (base.Patrol == null || base.Patrol.VictimComponent == null);
			}
		}

		public CrowdCharacter()
			: base((IEntity entity) => new CrowdCharacterStateMachines(entity))
		{
			m_charStats = AddComponent<CrowdCharacterStats>();
		}

		public override void AttachGameObject(EntityWrapperComponent _wrapperComponent)
		{
			base.AttachGameObject(_wrapperComponent);
			GameObject gameObject = GameObject.FindChildByName("SelectionCollider");
			EntityId entityId = CharacterStats.Get_EntityId(StatTypes.EStatType.Npc_PatrolId);
			if (entityId != EntityId.InvalidId && entityId.GetEntity<Patrol>().PatrolDefinition.Definition.InteractionReason != EInteractionReason.Invalid)
			{
				gameObject.tag = "Interactable";
				gameObject.layer = Layers.InteractorsLayer;
			}
			else
			{
				gameObject.tag = "Untagged";
				gameObject.layer = 0;
			}
		}
	}
}
