using System.Collections.ObjectModel;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using SharedBaseLib.System;
using UbiCore.Utils;
using UbiCore.Utils.Extensions;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Mission.Motion.Hotspots
{
	public class RooftopGardenManager : HideoutManager<RooftopGarden>
	{
		public RooftopGardenManager()
			: base("Roof Gardens")
		{
		}

		protected override bool Interact(AssassinCharacter _assassin, RooftopGarden _entity, NavMeshAgent _agent)
		{
			bool result = false;
			if (m_currentHideout == EntityId.InvalidId && _assassin.AssassinInteraction.CanInteractWithInteractableObject(_entity.Id) && _agent != null)
			{
				result = ParcourHelper.TryEnterHideout(_assassin, _entity, _agent);
			}
			return result;
		}

		protected override void CreateObject(GameObject _go)
		{
			MissionHotspots component = UbiCore.Utils.Singleton<MissionManager>.Instance.MissionMain.GetComponent<MissionHotspots>();
			// no hotspot data (hand-authored offline missions): every hotspot is active, the game's own default
			if (component != null && !component.HotspotIsActive(_go))
			{
				return;
			}
			Vector3 position = _go.transform.position;
			Quaternion rotation = _go.transform.rotation;
			EntityWrapperComponent wrapper = _go.AddComponentIfNotExist<EntityWrapperComponent>();
			RooftopGarden rooftopGarden = Assets.Scripts.Mission.Logic.Helpers.CreateRegisterAttachEntity<RooftopGarden>(UbiCore.Utils.Singleton<InteractableObjectData>.Instance.GetDefinitionByName("RooftopGarden"), wrapper);
			rooftopGarden.Position = position;
			rooftopGarden.Rotation = rotation;
			rooftopGarden.OnActivate();
			ReadOnlyCollection<InteractionLocators.Locator> locators = rooftopGarden.InteractionLocators.Locators;
			for (int i = 0; i < locators.Count; i++)
			{
				InteractionLocators.Locator locator = locators[i];
				NavMeshHit hit;
				if (NavMesh.SamplePosition(locator.Position.transform.position, out hit, 2f, NavMeshLayers.WalkableLayerMaskPreset))
				{
					Vector3 position2 = locator.Step.transform.position;
					locator.Position.transform.position = hit.position;
					locator.Step.transform.position = position2;
				}
				else
				{
					locator.Position.SetActive(false);
				}
			}
			ApproachEntityIcon approachEntityIcon = new ApproachEntityIcon();
			rooftopGarden.HeadIcon.IconAttachment = approachEntityIcon;
			approachEntityIcon.Offset3D = new Vector3(0f, 2f, 0f);
			approachEntityIcon.SetIcon(IconMaterial.InteractHaystack, 5);
			approachEntityIcon.SetInteractable(true, base.InteractAction);
			GameObject gameObject = CreatePrefabIfNotExists(_go, UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_envInRoofGardenDynamic);
			rooftopGarden.RooftopGardenClothes.Initialize(gameObject);
			rooftopGarden.Highlight.RescanGameObjects();
			Rendering.ConfigureLightProbes(gameObject);
		}

		protected override bool OnExitHideout(AssassinCharacter _assassin, Vector3 _destination)
		{
			LogicMain.PlayerEntity.KillFromRooftopGarden.OnExitHideout();
			return _assassin.ExitRooftopGarden(_destination);
		}

		protected override bool OnKill(EntityId _id)
		{
			return LogicMain.PlayerEntity.KillFromRooftopGarden.OnKill(_id);
		}

		protected override bool IsIdlingInHideout()
		{
			return LogicMain.PlayerEntity.UnityComponentCache.AnimationController.TaskManager.GetCurrentTaskType(AnimationTaskManager.ELayer.Base) == AnimationTaskType.RooftopGardenIdle;
		}
	}
}
