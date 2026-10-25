using System.Collections.ObjectModel;
using Assets.Scripts.Mission.Logic.StateMachines;
using SharedBaseLib.Core;
using UbiCore.Debug;

namespace Assets.Scripts.Mission.Logic.Components
{
	public abstract class StateMachines : TurnUpdateableComponent, IStateMachines, IUpdateable, ITurnUpdateable, IComponent, IUpdateableComponent, ITurnUpdateableComponent
	{
		private readonly BBList<IStateMachine> m_StateMachines = new BBList<IStateMachine>();

		private readonly IStateMachine[] m_StateMachinesByType = new IStateMachine[24];

		private readonly ReadOnlyCollection<IStateMachine> m_ReadOnlyStateMachines;

		public override int TurnUpdateIntervalTurns
		{
			get
			{
				return 0;
			}
		}

		public sealed override int Priority
		{
			get
			{
				return 7;
			}
		}

		public ReadOnlyCollection<IStateMachine> StateMachinesAccess
		{
			get
			{
				return m_ReadOnlyStateMachines;
			}
		}

		protected StateMachines(IEntity _entity)
			: base(_entity)
		{
			m_ReadOnlyStateMachines = new ReadOnlyCollection<IStateMachine>(m_StateMachines);
		}

		public override void Attach(EntityWrapperComponent _wrapperComponent)
		{
			base.Attach(_wrapperComponent);
			for (int i = 0; i < m_StateMachines.Count; i++)
			{
				m_StateMachines[i].Attach();
			}
		}

		public override void Detach()
		{
			base.Detach();
			for (int i = 0; i < m_StateMachines.Count; i++)
			{
				m_StateMachines[i].Detach();
			}
		}

		public override void OnRegistered()
		{
			base.OnRegistered();
			IsActive = true;
			for (int i = 0; i < m_StateMachines.Count; i++)
			{
				m_StateMachines[i].OnRegistered();
			}
		}

		public override void OnUnregistered()
		{
			for (int i = 0; i < m_StateMachines.Count; i++)
			{
				m_StateMachines[i].OnUnregistered();
			}
			base.OnUnregistered();
		}

		public sealed override void DoTurnUpdate(int _deltaTurns)
		{
			Profiling.Start("StateMachines::DoTurnUpdate()", float.MaxValue);
			for (int i = 0; i < m_StateMachines.Count; i++)
			{
				if (LogicMain.CurrentTurn >= m_StateMachines[i].NextTurnUpdateTurn && m_StateMachines[i].IsActive)
				{
					m_StateMachines[i].DoTurnUpdate(m_StateMachines[i].TurnsSinceTurnUpdate);
				}
			}
			base.DoTurnUpdate(_deltaTurns);
			Profiling.Stop();
		}

		public IStateMachine GetStateMachine(EStateMachineType _type)
		{
			return m_StateMachinesByType[(int)_type];
		}

		public static void InitStateMachines()
		{
			AssassinApproachingTarget.InitAssassinApproachingTarget();
			CharacterAwareness.InitCharacterAwareness();
			CharacterVitality.InitCharacterVitality();
			EnemyVitality.InitEnemyVitality();
			PlayerVitality.InitPlayerVitality();
			GameMain.InitGameMain();
			InteractionCharacterMain.InitInteractionCharacterMain();
			ObjectInteraction.InitObjectInteraction();
			PatrolSeduce.InitPatrolSeduce();
			PlayerAlertLevel.InitPlayerAlertLevel();
			PlayerDisguise.InitPlayerDisguise();
			PlayerProfile.InitPlayerProfile();
			PlayerStalkingZone.InitPlayerStalkingZone();
			PlayerVision.InitPlayerVision();
			Assets.Scripts.Mission.Logic.StateMachines.ProjectileFlight.InitProjectileFlight();
			CharacterCondition.InitCharacterCondition();
			HirelingMain.InitHirelingMain();
			CharacterDrawWeaponBase.InitCharacterDrawWeaponBase();
			CharacterInteraction.InitCharacterInteraction();
			EnemyMain.InitEnemyMain();
			PlayerMain.InitPlayerMain();
		}

		protected T AddStateMachine<T>(T _stateMachine) where T : IStateMachine
		{
			m_StateMachines.Add(_stateMachine);
			m_StateMachinesByType[(int)_stateMachine.Type] = _stateMachine;
			return _stateMachine;
		}
	}
}
