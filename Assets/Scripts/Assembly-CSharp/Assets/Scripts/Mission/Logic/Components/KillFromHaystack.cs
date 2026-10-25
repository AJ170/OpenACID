using Assets.Scripts.Animation.Tasks;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Motion.Hotspots;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class KillFromHaystack : KillFromHideout
	{
		public override int Priority
		{
			get
			{
				return 22;
			}
		}

		public override bool IsIdling
		{
			get
			{
				return base.TaskManagerAssassin.GetCurrentTaskType(AnimationTaskManager.ELayer.Base) == AnimationTaskType.HayStackIdle;
			}
		}

		public override bool IsExecuting
		{
			get
			{
				return base.TaskManagerAssassin.GetCurrentTaskType(AnimationTaskManager.ELayer.Base) == AnimationTaskType.HayStackKillInstigator;
			}
		}

		public KillFromHaystack(IEntity _entity)
			: base(_entity)
		{
		}

		public override void Update()
		{
			base.Update();
			m_isExecutable = IsIdling;
			if (base.IsExecutable)
			{
				bool flag = false;
				bool flag2 = false;
				EnemyCharacter enemyCharacter = ((!base.AutoSelectionEnabled) ? base.SelectedEnemy : base.ClosestEnemy);
				if (enemyCharacter != null && UbiCore.Utils.Singleton<HotspotService>.Instance.HaystackManager.Current != null)
				{
					flag = UbiCore.Utils.Singleton<HotspotService>.Instance.HaystackManager.Current.InteractionLocators.GetDistanceToTarget(enemyCharacter.GameObject) <= AnimationController.HaystackTargetDistanceMax;
					flag2 = TargetPositionIsValid(enemyCharacter.GameObject);
				}
				m_isExecutable = flag && flag2;
			}
		}

		protected override bool OnKillInternal(EntityId _targetId)
		{
			bool result = false;
			if (base.IsExecutable && base.SelectedEnemy != null && _targetId == base.SelectedEnemy.Id && base.TaskManagerAssassin != null && base.TaskManagerNPC != null)
			{
				EnemyCharacter selectedEnemy = base.SelectedEnemy;
				float distanceToTarget = UbiCore.Utils.Singleton<HotspotService>.Instance.HaystackManager.Current.InteractionLocators.GetDistanceToTarget(selectedEnemy.GameObject);
				base.TaskManagerAssassin.CreateAnimationTaskHayStackKillInstigator(LogicMain.Player, selectedEnemy.GameObject, distanceToTarget);
				base.TaskManagerNPC.CreateAnimationTaskHayStackKillTarget(LogicMain.Player, selectedEnemy.GameObject, distanceToTarget);
				selectedEnemy.KillCharacter(EKillType.AssassinationHaystack);
				result = true;
			}
			return result;
		}

		private static bool TargetPositionIsValid(GameObject _target)
		{
			int num = 0;
			if (UbiCore.Utils.Singleton<HotspotService>.Instance.HaystackManager.Current != null)
			{
				Vector3 position = _target.transform.position;
				foreach (InteractionLocators.Locator locator in UbiCore.Utils.Singleton<HotspotService>.Instance.HaystackManager.Current.InteractionLocators.Locators)
				{
					float distanceToPoint = locator.Plane.GetDistanceToPoint(position);
					if (distanceToPoint > 0f)
					{
						num++;
					}
				}
			}
			return num == 1;
		}
	}
}
