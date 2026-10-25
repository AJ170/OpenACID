using Assets.Scripts.Animation.Tasks;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Motion.Hotspots;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class KillFromBench : KillFromHideout
	{
		public override int Priority
		{
			get
			{
				return 23;
			}
		}

		public override bool IsIdling
		{
			get
			{
				return base.TaskManagerAssassin.GetCurrentTaskType(AnimationTaskManager.ELayer.Base) == AnimationTaskType.BenchIdle;
			}
		}

		public override bool IsExecuting
		{
			get
			{
				return base.TaskManagerAssassin.GetCurrentTaskType(AnimationTaskManager.ELayer.Base) == AnimationTaskType.BenchKillInstigator;
			}
		}

		public KillFromBench(IEntity _entity)
			: base(_entity)
		{
		}

		public override void Update()
		{
			base.Update();
			m_isExecutable = IsIdling;
			if (base.IsExecutable)
			{
				bool isExecutable = false;
				EnemyCharacter enemyCharacter = ((!base.AutoSelectionEnabled) ? base.SelectedEnemy : base.ClosestEnemy);
				Bench current = UbiCore.Utils.Singleton<HotspotService>.Instance.BenchManager.Current;
				if (enemyCharacter != null && current != null)
				{
					isExecutable = current.InteractionLocators.GetSimpleDistanceToTarget(enemyCharacter.GameObject) <= AnimationController.BenchTargetDistanceMax;
				}
				m_isExecutable = isExecutable;
			}
		}

		protected override bool OnKillInternal(EntityId _targetId)
		{
			bool result = false;
			if (base.IsExecutable && base.SelectedEnemy != null && _targetId == base.SelectedEnemy.Id && base.TaskManagerAssassin != null && base.TaskManagerNPC != null)
			{
				EnemyCharacter selectedEnemy = base.SelectedEnemy;
				float num = Vector3.Distance(LogicMain.PlayerEntity.UnityComponentCache.AnimationController.Animator.GetBoneTransform(HumanBodyBones.LeftFoot).position, selectedEnemy.GameObject.transform.position);
				float benchTargetDistance = AnimationController.BenchTargetDistance;
				float benchTargetDistanceRelative = AnimationController.BenchTargetDistanceRelative;
				AnimationController.Direction direction = AnimationHelper.DetermineDirectionFrontOrBack(LogicMain.Player.gameObject, selectedEnemy.GameObject);
				base.TaskManagerAssassin.CreateAnimationTaskBenchKillInstigator(LogicMain.Player, selectedEnemy.GameObject, benchTargetDistance, benchTargetDistanceRelative, direction);
				base.TaskManagerNPC.CreateAnimationTaskBenchKillTarget(selectedEnemy.GameObject, UbiCore.Utils.Singleton<HotspotService>.Instance.BenchManager.Current, benchTargetDistance, benchTargetDistanceRelative, AnimationController.Direction.Front);
				selectedEnemy.KillCharacter(EKillType.AssassinationBench);
				UbiCore.Utils.Singleton<HotspotService>.Instance.BenchManager.Current.AssassinationStartTurn = LogicMain.CurrentTurn;
				UbiCore.Utils.Singleton<HotspotService>.Instance.BenchManager.KillFromBench(PlayerHelper.Entity);
				result = true;
			}
			return result;
		}
	}
}
