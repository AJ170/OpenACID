using AcierProtocol;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using SharedBaseLib.Core;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class Vision : Component
	{
		private readonly BBList<EntityId> m_relevantEntities = new BBList<EntityId>();

		private bool m_active;

		public PlayerCharacter PlayerCharacter
		{
			get
			{
				return (PlayerCharacter)Entity;
			}
		}

		public override int Priority
		{
			get
			{
				return 17;
			}
		}

		public Vision(IEntity _entity)
			: base(_entity)
		{
		}

		public override void OnRegistered()
		{
			base.OnRegistered();
			LogicMain.EntityManager.OnEntityRegistered += SetEntityHighlight;
		}

		public override void OnUnregistered()
		{
			m_relevantEntities.Clear();
			m_active = false;
			LogicMain.EntityManager.OnEntityRegistered -= SetEntityHighlight;
			base.OnUnregistered();
		}

		public void Activate()
		{
			Activate(PlayerCharacter.PlayerCharacterStateMachines.PlayerVision.CurrentStateType == EStateType.PlayerVision_EagleEye);
		}

		public void AddEntity(EntityId _id)
		{
			m_relevantEntities.Add(_id);
			SetEntityHighlight(m_active, _id.GameEntity);
		}

		public void RemoveEntity(EntityId _id)
		{
			if (_id.IsValid)
			{
				m_relevantEntities.Remove(_id);
				SetEntityHighlight(m_active, _id.GameEntity);
			}
		}

		public bool IsRelevant(EntityId _id)
		{
			return m_relevantEntities.Contains(_id);
		}

		public void Activate(bool _activate)
		{
			if (m_active == _activate)
			{
				return;
			}
			m_active = _activate;
			if (CombinedPostEffect.IsSupported())
			{
				IEffectsCarrier playerCharacter = PlayerCharacter;
				if (playerCharacter != null)
				{
					SetEffectEnabled(playerCharacter, "EagleVision", _activate);
				}
			}
			int entityCount = LogicMain.EntityManager.GetEntityCount(EntityTypes.EType.GameEntity);
			for (int i = 0; i < entityCount; i++)
			{
				GameEntity entity = LogicMain.EntityManager.GetEntity<GameEntity>(EntityTypes.EType.GameEntity, i);
				SetEntityHighlight(_activate, entity);
			}
		}

		private static void SetEffectEnabled(IEffectsCarrier _entity, string _effect, bool _enabled)
		{
			if (_enabled)
			{
				_entity.Effects.OnEffectOn(_effect);
			}
			else
			{
				_entity.Effects.OnEffectOff(_effect);
			}
		}

		private void SetEntityHighlight(IEntity _entity)
		{
			SetEntityHighlight(PlayerCharacter.PlayerCharacterStateMachines.PlayerVision.CurrentStateType == EStateType.PlayerVision_EagleEye, _entity as IHighlightCarrier);
		}

		private Highlight.ESelectionMode NpcMode(NpcCharacter _npc)
		{
			if (_npc != null)
			{
				switch (_npc.NpcDefinition.Definition.Category)
				{
				case ENpcCategory.Monk_ENpcCategory:
				case ENpcCategory.Courtesan_ENpcCategory:
				case ENpcCategory.Doctor_ENpcCategory:
					return Highlight.ESelectionMode.Friendly;
				case ENpcCategory.Enemy_ENpcCategory:
					return Highlight.ESelectionMode.Enemy;
				}
			}
			return Highlight.ESelectionMode.Off;
		}

		private Highlight.ESelectionMode IoMode(InteractableObject _io)
		{
			if (_io != null)
			{
				switch (_io.InteractableObjectDefinition.Definition.Category)
				{
				case EObjectCategory.Codex_EObjectCategory:
					return Highlight.ESelectionMode.Friendly;
				case EObjectCategory.HidingSpot_EObjectCategory:
					return Highlight.ESelectionMode.Hiding;
				case EObjectCategory.Mission_EObjectCategory:
					switch (_io.InteractableObjectDefinition.Definition.SubCategory)
					{
					case EObjectSubCategory.Chest:
					case EObjectSubCategory.PigeonCoup:
					case EObjectSubCategory.Elevated:
						return Highlight.ESelectionMode.Friendly;
					}
					break;
				}
			}
			return Highlight.ESelectionMode.Off;
		}

		private void SetEntityHighlight(bool _activate, IHighlightCarrier _entity)
		{
			if (_entity == null)
			{
				return;
			}
			if (_activate)
			{
				Highlight.ESelectionMode mode = Highlight.ESelectionMode.Off;
				if (m_relevantEntities.Contains(_entity.Id))
				{
					mode = Highlight.ESelectionMode.Objective;
				}
				else
				{
					InteractableObject interactableObject = _entity as InteractableObject;
					if (interactableObject != null)
					{
						mode = IoMode(interactableObject);
					}
					else
					{
						NpcCharacter npcCharacter = _entity as NpcCharacter;
						if (npcCharacter != null)
						{
							mode = NpcMode(npcCharacter);
						}
						else
						{
							StaticObject staticObject = _entity as StaticObject;
							if (staticObject != null)
							{
								mode = ((_entity.Highlight.HightlightEffectType == typeof(GlyphBehaviour)) ? Highlight.ESelectionMode.Objective : Highlight.ESelectionMode.Off);
							}
						}
					}
				}
				if (_entity is IAvatarDefinitionCarrier)
				{
					PlayerCharacter playerCharacter = _entity as PlayerCharacter;
					if (playerCharacter != null)
					{
						mode = Highlight.ESelectionMode.Friendly;
					}
				}
				if (_entity is ICrowdCharacterStatsCarrier)
				{
					CrowdCharacter crowdCharacter = _entity as CrowdCharacter;
					CharacterCondition characterCondition = crowdCharacter.CharacterStateMachines.Cast<ICharacterConditionCarrier>().CharacterCondition;
					if (characterCondition.IsCurrentOrScheduled(EStateType.CharacterCondition_OnBench))
					{
						mode = Highlight.ESelectionMode.Friendly;
					}
				}
				_entity.Highlight.SetMode(Highlight.EHighlightLayer.Base, mode);
				return;
			}
			Highlight.ESelectionMode mode2 = Highlight.ESelectionMode.Off;
			InteractableObject interactableObject2 = _entity as InteractableObject;
			if (interactableObject2 != null)
			{
				switch (interactableObject2.InteractableObjectDefinition.Definition.Category)
				{
				case EObjectCategory.Codex_EObjectCategory:
				case EObjectCategory.Portal_EObjectCategory:
					mode2 = Highlight.ESelectionMode.On;
					break;
				}
			}
			_entity.Highlight.SetMode(Highlight.EHighlightLayer.Base, mode2);
		}
	}
}
