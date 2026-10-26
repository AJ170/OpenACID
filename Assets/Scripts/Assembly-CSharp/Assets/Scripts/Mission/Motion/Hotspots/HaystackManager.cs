using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Entities;
using UbiCore.Utils;
using UbiCore.Utils.Extensions;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Mission.Motion.Hotspots
{
	public class HaystackManager : HideoutManager<Haystack>
	{
		public HaystackManager()
			: base("Haystacks")
		{
		}

		protected override bool Interact(AssassinCharacter _assassin, Haystack _entity, NavMeshAgent _agent)
		{
			bool result = false;
			if (m_currentHideout == EntityId.InvalidId && _assassin.AssassinInteraction.CanInteractWithInteractableObject(_entity.Id) && _agent != null)
			{
				Vector3 _edgePosition;
				if (ParcourHelper.CanLeapOfFaith(_assassin, _entity, out _edgePosition) && _assassin.AssassinCharacterStateMachines.AssassinApproachingTarget.SetTarget(_entity.Id, EInteractionReason.InteractWithObject, _edgePosition, false))
				{
					return true;
				}
				result = ParcourHelper.TryEnterHideout(_assassin, _entity, _agent);
			}
			return result;
		}

		protected override void CreateObject(GameObject _go)
		{
			MissionHotspots component = UbiCore.Utils.Singleton<MissionManager>.Instance.MissionMain.GetComponent<MissionHotspots>();
			if (component == null || component.HotspotIsActive(_go))
			{
				Vector3 position = _go.transform.position;
				Quaternion rotation = _go.transform.rotation;
				EntityWrapperComponent wrapper = _go.AddComponentIfNotExist<EntityWrapperComponent>();
				Haystack haystack = Assets.Scripts.Mission.Logic.Helpers.CreateRegisterAttachEntity<Haystack>(UbiCore.Utils.Singleton<InteractableObjectData>.Instance.GetDefinitionByName("Haystack"), wrapper);
				haystack.Position = position;
				haystack.Rotation = rotation;
				haystack.OnActivate();
				HaystackIcon haystackIcon = new HaystackIcon();
				haystack.HeadIcon.IconAttachment = haystackIcon;
				haystackIcon.Offset3D = new Vector3(0f, 2f, 0f);
				haystackIcon.SetIcon(IconMaterial.InteractHaystack, 5);
				haystackIcon.SetInteractable(true, base.InteractAction);
				GameObject gameObject = CreatePrefabIfNotExists(_go, UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_envInHaystackDynamic);
				Rendering.ConfigureLightProbes(gameObject);
				haystack.Animator = gameObject.GetComponent<Animator>();
				haystack.Highlight.RescanGameObjects();
			}
		}

		protected override bool OnExitHideout(AssassinCharacter _assassin, Vector3 _destination)
		{
			LogicMain.PlayerEntity.KillFromHaystack.OnExitHideout();
			return _assassin.ExitHaystack(_destination);
		}

		protected override bool OnKill(EntityId _id)
		{
			return LogicMain.PlayerEntity.KillFromHaystack.OnKill(_id);
		}

		protected override bool IsIdlingInHideout()
		{
			return LogicMain.PlayerEntity.UnityComponentCache.AnimationController.TaskManager.GetCurrentTaskType(AnimationTaskManager.ELayer.Base) == AnimationTaskType.HayStackIdle;
		}
	}
}
