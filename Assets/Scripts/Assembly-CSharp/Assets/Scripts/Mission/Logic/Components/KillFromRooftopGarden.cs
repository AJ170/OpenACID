using System.Collections.ObjectModel;
using Assets.Scripts.Animation.Tasks;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Motion.Hotspots;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class KillFromRooftopGarden : KillFromHideout
	{
		public override int Priority
		{
			get
			{
				return 24;
			}
		}

		public override bool IsIdling
		{
			get
			{
				return base.TaskManagerAssassin.GetCurrentTaskType(AnimationTaskManager.ELayer.Base) == AnimationTaskType.RooftopGardenIdle;
			}
		}

		public override bool IsExecuting
		{
			get
			{
				return base.TaskManagerAssassin.GetCurrentTaskType(AnimationTaskManager.ELayer.Base) == AnimationTaskType.RooftopGardenKillInstigator;
			}
		}

		public KillFromRooftopGarden(IEntity _entity)
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
				if (enemyCharacter != null && UbiCore.Utils.Singleton<HotspotService>.Instance.RooftopGardenManager.Current != null)
				{
					flag = UbiCore.Utils.Singleton<HotspotService>.Instance.RooftopGardenManager.Current.InteractionLocators.GetDistanceToTarget(enemyCharacter.GameObject) <= AnimationController.RooftopGardenTargetDistanceMax;
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
				RooftopGarden current = UbiCore.Utils.Singleton<HotspotService>.Instance.RooftopGardenManager.Current;
				InteractionLocators.Locator locatorByPosition = current.InteractionLocators.GetLocatorByPosition(selectedEnemy.GameObject.transform.position);
				Vector3 position = locatorByPosition.Position.transform.position;
				position.y = current.InteractionLocators.Base.transform.position.y;
				selectedEnemy.GameObject.transform.position = position;
				float distanceToTarget = current.InteractionLocators.GetDistanceToTarget(selectedEnemy.GameObject);
				base.TaskManagerAssassin.CreateAnimationTaskRooftopGardenKillInstigator(LogicMain.Player, selectedEnemy.GameObject, distanceToTarget, locatorByPosition);
				base.TaskManagerNPC.CreateAnimationTaskRooftopGardenKillTarget(LogicMain.Player, selectedEnemy.GameObject, distanceToTarget);
				selectedEnemy.KillCharacter(EKillType.AssassinationRooftopGarden);
				result = true;
			}
			return result;
		}

		private static bool TargetPositionIsValid(GameObject _target)
		{
			int num = 0;
			if (UbiCore.Utils.Singleton<HotspotService>.Instance.RooftopGardenManager.Current != null)
			{
				Vector3 position = _target.transform.position;
				ReadOnlyCollection<InteractionLocators.Locator> locators = UbiCore.Utils.Singleton<HotspotService>.Instance.RooftopGardenManager.Current.InteractionLocators.Locators;
				for (int i = 0; i < locators.Count; i++)
				{
					float distanceToPoint = locators[i].Plane.GetDistanceToPoint(position);
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
