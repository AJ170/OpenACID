using System.Collections.ObjectModel;
using AcierProtocol;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Utilities;
using Database;
using SharedBaseLib.Core;
using SharedBaseLib.System;
using UbiCore.Utils;
using UbiCore.Utils.Extensions;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Mission.Motion.Hotspots
{
	public class BenchManager : HideoutManager<Bench>
	{
		public enum ELeaveReason
		{
			KillFromBench = 0,
			StandUpFromBench = 1,
			None = 2
		}

		private BBDictionary<EntityId, Bench.Seat> m_EntitiesOnBenches = new BBDictionary<EntityId, Bench.Seat>();

		private BBList<NpcData.NpcDefinition> m_npcTypes = new BBList<NpcData.NpcDefinition>();

		public BenchManager()
			: base("Benches")
		{
			base.LeaveHideoutAfterKill = true;
		}

		protected override void CreateObject(GameObject _go)
		{
			MissionHotspots component = UbiCore.Utils.Singleton<MissionManager>.Instance.MissionMain.GetComponent<MissionHotspots>();
			if (!component.HotspotIsActive(_go))
			{
				return;
			}
			CollectExistingCrowdCharacterTypes();
			Vector3 position = _go.transform.position;
			Quaternion rotation = _go.transform.rotation;
			EntityWrapperComponent wrapper = _go.AddComponentIfNotExist<EntityWrapperComponent>();
			Bench bench = Assets.Scripts.Mission.Logic.Helpers.CreateRegisterAttachEntity<Bench>(UbiCore.Utils.Singleton<InteractableObjectData>.Instance.GetDefinitionByName("Bench"), wrapper);
			bench.Position = position;
			bench.Rotation = rotation;
			GameObject gameObject = _go.FindChildByName("PlayerSeat");
			if (gameObject != null)
			{
				bench.SetSeat(Bench.ESeats.Player, gameObject.transform.position, gameObject.transform.rotation);
			}
			gameObject = _go.FindChildByName("LeftNPCSeat");
			if (gameObject != null)
			{
				bench.SetSeat(Bench.ESeats.LeftNPC, gameObject.transform.position, gameObject.transform.rotation);
				SpawnRandomNpcOnSeat(bench.GetSeat(Bench.ESeats.LeftNPC));
			}
			gameObject = _go.FindChildByName("RightNPCSeat");
			if (gameObject != null)
			{
				bench.SetSeat(Bench.ESeats.RightNPC, gameObject.transform.position, gameObject.transform.rotation);
				SpawnRandomNpcOnSeat(bench.GetSeat(Bench.ESeats.RightNPC));
			}
			bench.OnActivate();
			ReadOnlyCollection<InteractionLocators.Locator> locators = bench.InteractionLocators.Locators;
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
			bench.HeadIcon.IconAttachment = approachEntityIcon;
			approachEntityIcon.Offset3D = new Vector3(0f, 2f, 0f);
			approachEntityIcon.SetIcon(IconMaterial.InteractHaystack, 5);
			approachEntityIcon.SetInteractable(true, base.InteractAction);
		}

		protected override bool IsIdlingInHideout()
		{
			return LogicMain.PlayerEntity.UnityComponentCache.AnimationController.TaskManager.GetCurrentTaskType(AnimationTaskManager.ELayer.Base) == AnimationTaskType.BenchIdle;
		}

		protected override bool OnExitHideout(AssassinCharacter _assassin, Vector3 _destination)
		{
			LogicMain.PlayerEntity.KillFromBench.OnExitHideout();
			return _assassin.ExitBench(_destination, ELeaveReason.StandUpFromBench);
		}

		protected override bool OnKill(EntityId _id)
		{
			return LogicMain.PlayerEntity.KillFromBench.OnKill(_id);
		}

		public bool KillFromBench(AssassinCharacter _assassin)
		{
			LogicMain.PlayerEntity.KillFromBench.OnExitHideout();
			return _assassin.ExitBench(Vector3.zero, ELeaveReason.KillFromBench);
		}

		private void SpawnRandomNpcOnSeat(Bench.Seat _seat)
		{
			if (m_npcTypes.Count > 0)
			{
				GameEntity gameEntity = Assets.Scripts.Mission.Logic.Helpers.CreateEntity<CrowdCharacter>(GetRandomCrowdCharacterType());
				LogicMain.EntityManager.RegisterEntity(gameEntity);
				CrowdCharacter crowdCharacter = (CrowdCharacter)gameEntity;
				CharacterCondition characterCondition = crowdCharacter.CharacterStateMachines.Cast<ICharacterConditionCarrier>().CharacterCondition;
				characterCondition.ExternalChange(EStateType.CharacterCondition_OnBench);
				gameEntity.Position = _seat.Position;
				gameEntity.Rotation = _seat.Orientation;
				_seat.EntityId = gameEntity.Id;
				m_EntitiesOnBenches.Add(gameEntity.Id, _seat);
				crowdCharacter.OnActivate();
			}
		}

		public Bench.Seat GetBenchSeatByEntityId(EntityId _id)
		{
			Bench.Seat value = null;
			m_EntitiesOnBenches.TryGetValue(_id, out value);
			return value;
		}

		private NpcData.NpcDefinition GetRandomCrowdCharacterType()
		{
			int index = LogicMain.Random.Rand(0, m_npcTypes.Count - 1);
			return m_npcTypes[index];
		}

		private void CollectExistingCrowdCharacterTypes()
		{
			if (m_npcTypes.Count != 0)
			{
				return;
			}
			GameDB safeGameDb = Main.SafeGameDb;
			EEra flagSet = Main.Instance.SceneLoader.CurrentMissionDescription.Age.Fix();
			BBDictionary<GameID, DBEntry> data = safeGameDb.GetSubDB(typeof(NpcData.NpcDefinition)).Data;
			foreach (GameID key in data.Keys)
			{
				NpcData.NpcDefinition npcDefinition = data[key] as NpcData.NpcDefinition;
				HumanoidDef item = safeGameDb.GetItem<HumanoidDef>(npcDefinition.HumanoidGameID);
				if (item != null && item.AtlasPage != 0 && item.Age.Contains(flagSet) && item.MaySitOnBench && npcDefinition.EntityType == EntityTypes.EType.CrowdCharacter && (item.CharType == HumanoidDef.ECharType.Male || item.CharType == HumanoidDef.ECharType.Female))
				{
					m_npcTypes.Add(npcDefinition);
				}
			}
		}
	}
}
