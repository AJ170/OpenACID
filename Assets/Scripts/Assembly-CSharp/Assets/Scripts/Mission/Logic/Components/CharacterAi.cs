using Assets.Scripts.Mission.AI.NpcBehavior;
using Assets.Scripts.Mission.AI.System;
using Assets.Scripts.Mission.Logic.Entities;
using UbiCore.Debug;
using UbiCore.Utils;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class CharacterAi : UnityComponentListeningTurnUpdateableComponent
	{
		private const int DefaultIntervalTurns = 1;

		private const int FarIntervalTurns = 11;

		private const int NotAttachedIntervalTurns = 15;

		private int m_TurnUpdateIntervalTurns;

		public override bool IsActive
		{
			get
			{
				return base.IsActive;
			}
			set
			{
				if (!value && base.IsActive)
				{
					CancelRunning();
				}
				base.IsActive = value;
			}
		}

		private NpcCharacter NpcCharacter
		{
			get
			{
				return Entity as NpcCharacter;
			}
		}

		public NpcTreeHolder BehaviorTreeHolder { get; private set; }

		public NpcRuntimeData TreeRuntimeData { get; private set; }

		public sealed override int Priority
		{
			get
			{
				return 9;
			}
		}

		public override int TurnUpdateIntervalTurns
		{
			get
			{
				return m_TurnUpdateIntervalTurns;
			}
		}

		public CharacterAi(IEntity _entity)
			: base(_entity)
		{
		}

		public override void DoTurnUpdate(int _deltaTurns)
		{
			Profiling.Start("CharacterAI::DoUpdate()", float.MaxValue);
			BehaviorTreeHolder.UpdateTurn(TreeRuntimeData);
			CalcNumberTurnsBetweenUpdates();
			base.DoTurnUpdate(_deltaTurns);
			Profiling.Stop();
		}

		public override void Attach(EntityWrapperComponent _wrapperComponent)
		{
			base.Attach(_wrapperComponent);
			ScheduleTurnUpdate(false);
		}

		public override void OnAttachedAndInitialized()
		{
			base.OnAttachedAndInitialized();
			NpcCharacter.UnityComponentCache.AnimationController.TaskManager.AnimationTaskExited += OnAnimationTaskExited;
			BehaviorTreeHolder.OnAttachedAndInitialized(TreeRuntimeData);
			ScheduleTurnUpdate(false);
		}

		public override void Detach()
		{
			NpcCharacter.UnityComponentCache.AnimationController.TaskManager.AnimationTaskExited -= OnAnimationTaskExited;
			base.Detach();
		}

		public override void OnRegistered()
		{
			base.OnRegistered();
			string behaviorTreePath = NpcCharacter.NpcDefinition.Definition.BehaviorTreePath;
			BehaviorTreeHolder = UbiCore.Utils.Singleton<BehaviorTreeService>.Instance.RequestBehaviorTreeHolder(behaviorTreePath, BehaviorTreeCategory.Npc) as NpcTreeHolder;
			TreeRuntimeData = new NpcRuntimeData();
			TreeRuntimeData.Init(NpcCharacter);
			IsActive = true;
		}

		public override void OnUnregistered()
		{
			TreeRuntimeData.Dispose();
		}

		public void CancelRunning()
		{
			BehaviorTreeHolder.CancelRunning(TreeRuntimeData);
		}

		private void OnAnimationTaskExited(object _sender, AnimationTaskManager.AnimationTaskEventArgs _eventArgs)
		{
			if (NpcCharacter.UnityComponentCache.AnimationController.TaskManager.GetTaskCount(AnimationTaskManager.ELayer.Base) == 0)
			{
				ScheduleTurnUpdate(false);
			}
		}

		private void CalcNumberTurnsBetweenUpdates()
		{
			if (GameEntity == null || !GameEntity.IsAttached)
			{
				m_TurnUpdateIntervalTurns = 15;
			}
			else
			{
				m_TurnUpdateIntervalTurns = 1 + (int)(GameEntity.DistanceBucket * 11f);
			}
		}
	}
}
