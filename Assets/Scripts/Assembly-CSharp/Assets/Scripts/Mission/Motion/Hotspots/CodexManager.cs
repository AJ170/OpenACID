using AcierProtocol;
using Assets.Scripts.Audio;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using UbiCore.Utils;
using UbiCore.Utils.Extensions;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Mission.Motion.Hotspots
{
	public class CodexManager : HotspotManager<Codex>
	{
		public CodexManager()
			: base("Codexes")
		{
		}

		protected override void CreateObject(GameObject _go)
		{
			if (UbiCore.Utils.Singleton<MissionManager>.Instance.MissionMain.Data.ObjectiveType != EObjectiveType.Story_EObjectiveType)
			{
				Vector3 position = _go.transform.position;
				Quaternion rotation = _go.transform.rotation;
				EntityWrapperComponent wrapper = _go.AddComponentIfNotExist<EntityWrapperComponent>();
				Codex codex = Assets.Scripts.Mission.Logic.Helpers.CreateRegisterAttachEntity<Codex>(UbiCore.Utils.Singleton<InteractableObjectData>.Instance.GetDefinitionByName("Codex"), wrapper);
				codex.Position = position;
				codex.Rotation = rotation;
				codex.OnActivate();
				ApproachEntityIcon approachEntityIcon = new ApproachEntityIcon();
				codex.HeadIcon.IconAttachment = approachEntityIcon;
				approachEntityIcon.Offset3D = new Vector3(0f, 0f, 0f);
				approachEntityIcon.SetIcon(IconMaterial.InteractCodex, 5);
				approachEntityIcon.SetInteractable(true, base.InteractAction);
			}
		}

		protected override bool Interact(AssassinCharacter _assassin, Codex _entity, NavMeshAgent _agent)
		{
			InteractionLocators.Locator _locator;
			NavMeshPath _path;
			if (_assassin.AssassinInteraction.CanInteractWithInteractableObject(_entity.Id) && _agent != null && _entity.InteractionLocators.ChooseClosestLocator(_agent.transform.position, _assassin.GetAutoPassableNavMeshLayerMask(), out _locator, out _path))
			{
				AudioManager.Instance.EventManager.OnEnterCodex();
				return _assassin.AssassinCharacterStateMachines.AssassinApproachingTarget.SetTarget(_entity.Id, EInteractionReason.InteractWithObject, _locator);
			}
			return false;
		}

		public override bool LeaveInteraction(AssassinCharacter _assassin, Vector3 _destination)
		{
			if (!base.LeaveInteraction(_assassin, _destination))
			{
				return false;
			}
			AudioManager.Instance.EventManager.OnLeaveCodex();
			return _assassin.AssassinInteraction.ExitCodex();
		}
	}
}
