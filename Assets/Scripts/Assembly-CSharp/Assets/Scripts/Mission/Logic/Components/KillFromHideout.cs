using AcierProtocol;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.Selection;
using Assets.Scripts.UI.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.Components
{
	public abstract class KillFromHideout : Component
	{
		protected bool m_isExecutable;

		public bool AutoSelectionEnabled { get; set; }

		public AssassinCharacter AssassinCharacter
		{
			get
			{
				return (AssassinCharacter)Entity;
			}
		}

		public bool Executable
		{
			get
			{
				return IsExecutable && SelectedEnemy != null;
			}
		}

		public EnemyCharacter SelectedEnemy { get; set; }

		public EnemyCharacter ClosestEnemy { get; set; }

		protected AnimationTaskManager TaskManagerNPC
		{
			get
			{
				EnemyCharacter selectedEnemy = SelectedEnemy;
				if (selectedEnemy == null)
				{
					return null;
				}
				return selectedEnemy.UnityComponentCache.AnimationController.TaskManager;
			}
		}

		protected AnimationTaskManager TaskManagerAssassin
		{
			get
			{
				return Entity.Cast<AssassinCharacter>().UnityComponentCache.AnimationController.TaskManager;
			}
		}

		public bool IsExecutable
		{
			get
			{
				return m_isExecutable;
			}
		}

		public abstract bool IsIdling { get; }

		public abstract bool IsExecuting { get; }

		public EntityId KillTarget { get; private set; }

		protected KillFromHideout(IEntity _entity)
			: base(_entity)
		{
			SelectedEnemy = null;
			AutoSelectionEnabled = true;
			KillTarget = EntityId.InvalidId;
		}

		private void UpdateClosestEnemy()
		{
			EnemyCharacter closestEnemy = null;
			Vector3 position = AssassinCharacter.Position;
			float num = float.MaxValue;
			foreach (EnemyCharacter item in LogicMain.EntityManager.CollectEntitiesInRadius<EnemyCharacter>(position, 4f))
			{
				if (item.IsAttached && !item.GetIsDead())
				{
					float num2 = Vector3.SqrMagnitude(item.Position - position);
					if (num2 < num)
					{
						closestEnemy = item;
						num = num2;
					}
				}
			}
			ClosestEnemy = closestEnemy;
		}

		public virtual void Update()
		{
			if (!EntitlementExtensions.AreHidingSpotKillsUnlocked())
			{
				SelectedEnemy = null;
				ClosestEnemy = null;
				KillTarget = EntityId.InvalidId;
				return;
			}
			if (SelectedEnemy != null && (!SelectedEnemy.IsAttached || !SelectedEnemy.IsSelectable()))
			{
				SelectedEnemy = null;
			}
			if (!AssassinCharacter.GetIsMovementAllowed())
			{
				PreventMovementExceptionTypes preventMovementExceptionTypes = (PreventMovementExceptionTypes)AssassinCharacter.EntityStats.Get_int(StatTypes.EStatType.Player_MovementPreventedException);
				if (preventMovementExceptionTypes != PreventMovementExceptionTypes.PreventMovementException_AssassinateFromHideout)
				{
					SelectedEnemy = null;
					AutoSelectionEnabled = false;
				}
			}
			if (AutoSelectionEnabled)
			{
				UpdateClosestEnemy();
				SelectedEnemy = ((!m_isExecutable) ? null : ClosestEnemy);
			}
			else
			{
				ClosestEnemy = null;
			}
			if (IsExecuting)
			{
				AssassinCharacter entity = Entity.Cast<AssassinCharacter>();
				if (entity.GetIsExposed())
				{
					PlayerMain.HidingInObjectState.HidingInObjectPayload hidingInObjectPayload = (PlayerMain.HidingInObjectState.HidingInObjectPayload)entity.GetMainStateMachine().CurrentStatePayload;
					hidingInObjectPayload.ShouldExit = true;
				}
			}
			else
			{
				KillTarget = EntityId.InvalidId;
			}
		}

		public bool OnKill(EntityId _targetId)
		{
			bool flag = OnKillInternal(_targetId);
			if (flag)
			{
				KillTarget = _targetId;
			}
			return flag;
		}

		protected abstract bool OnKillInternal(EntityId _targetId);

		public void OnExitHideout()
		{
			AutoSelectionEnabled = true;
			SelectedEnemy = null;
			KillTarget = EntityId.InvalidId;
		}
	}
}
