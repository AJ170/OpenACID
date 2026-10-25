using System;
using System.Collections.Generic;
using Assets.Scripts.Audio;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.Combat;
using SharedBaseLib.Core;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.StateMachines
{
	public class ProjectileFlight : StateMachineBase<Projectile, ProjectileFlight>, IAnimatorEventListener
	{
		private readonly BBList<ICombatAnnotation> m_activeAnnotations = new BBList<ICombatAnnotation>(2);

		public override EStateType InitialState
		{
			get
			{
				return EStateType.Projectile_Flying;
			}
		}

		public override EStateMachineType Type
		{
			get
			{
				return EStateMachineType.ProjectileFlight;
			}
		}

		public Action<Projectile> HitAction { get; set; }

		public Action<Projectile> ExplodeAction { get; set; }

		public int ExpireTurn { get; private set; }

		public float TriggerRadius { get; private set; }

		public int TriggerTurn { get; private set; }

		public ProjectileFlight(Projectile _entity)
			: base(_entity)
		{
		}

		public static void InitProjectileFlight()
		{
			StateMachineBase<Projectile, ProjectileFlight>.InitStateMachineBase();
			StateMachineBase<Projectile, ProjectileFlight>.AddState<State<Projectile>>(EStateType.Projectile_Flying, null, null, null);
			StateMachineBase<Projectile, ProjectileFlight>.AddState<State<Projectile>>(EStateType.Projectile_Arrived, OnEnterArrived, null, OnUpdateArrived);
			StateMachineBase<Projectile, ProjectileFlight>.AddState<State<Projectile>>(EStateType.Projectile_Expire, OnEnterExpire, null, null);
			StateMachineBase<Projectile, ProjectileFlight>.AddTransition(EStateType.Projectile_Flying, EStateType.Projectile_Arrived);
			StateMachineBase<Projectile, ProjectileFlight>.AddTransition(EStateType.Projectile_Flying, EStateType.Projectile_Expire);
			StateMachineBase<Projectile, ProjectileFlight>.AddTransition(EStateType.Projectile_Arrived, EStateType.Projectile_Expire);
		}

		public override void OnUnregistered()
		{
			while (m_activeAnnotations.Count > 0)
			{
				m_activeAnnotations[0].Release();
			}
			base.OnUnregistered();
		}

		public override void Detach()
		{
			GameObject gameObject = Entity.GameObject;
			base.Detach();
			UnityEngine.Object.Destroy(gameObject);
		}

		private static void OnEnterArrived(IStateMachine<Projectile> _stateMachine, IPayload _payload, IState<Projectile> _prevState, IState<Projectile> _currentState)
		{
			ProjectileFlight projectileFlight = _stateMachine.Cast<ProjectileFlight>();
			projectileFlight.Hit();
			EntityStats entityStats = projectileFlight.Entity.EntityStats;
			int num = entityStats.Get_int(StatTypes.EStatType.Projectile_Delay);
			int num2 = entityStats.Get_int(StatTypes.EStatType.Projectile_TriggerDelay);
			float triggerRadius = entityStats.Get_float(StatTypes.EStatType.Projectile_TriggerRadius);
			if (num > 0 && num % 10 == 0)
			{
				num++;
			}
			projectileFlight.ExpireTurn = LogicMain.CurrentTurn + num;
			projectileFlight.TriggerTurn = LogicMain.CurrentTurn + num2;
			projectileFlight.TriggerRadius = triggerRadius;
		}

		private static void OnUpdateArrived(IStateMachine<Projectile> _stateMachine, IPayload _payload, IState<Projectile> _currentState)
		{
			ProjectileFlight projectileFlight = _stateMachine.Cast<ProjectileFlight>();
			EProjectileFlightType eProjectileFlightType = (EProjectileFlightType)projectileFlight.Entity.ProjectileStats.Get_int(StatTypes.EStatType.Projectile_FlightType);
			int num = projectileFlight.ExpireTurn - LogicMain.CurrentTurn;
			int num2 = projectileFlight.TriggerTurn - LogicMain.CurrentTurn;
			EProjectileFlightType eProjectileFlightType2 = eProjectileFlightType;
			if (eProjectileFlightType2 == EProjectileFlightType.DeployTrigger)
			{
				if (num2 <= 0)
				{
					IEnumerable<EnemyCharacter> enumerable = LogicMain.EntityManager.CollectEntitiesInRadius<EnemyCharacter>(projectileFlight.Entity.Position, projectileFlight.TriggerRadius, CharacterHelper.FilterEnemyAlivePredicate);
					if (enumerable.GetEnumerator().MoveNext())
					{
						projectileFlight.Explode();
						return;
					}
				}
				if (num <= 0)
				{
					projectileFlight.Expire();
				}
				else if (num % 10 == 0)
				{
					AudioSource component = projectileFlight.Entity.GameObject.GetComponent<AudioSource>();
					IAudioHandle audioHandle = AudioManager.Instance.CreateHandle(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_wpn_bomb_tripwire_ticktock, ELoadStrategy.LoadNext, EStoppedFinishedStrategy.UnloadAll);
					audioHandle.Load();
					audioHandle.Play(false, component);
				}
			}
			else if (num <= 0)
			{
				projectileFlight.Explode();
			}
			else if (num % 10 == 0)
			{
				projectileFlight.AddAnnotation(CombatAnnotation.GetNumber(projectileFlight.Entity, num / 10, CombatAnnotation.Type.Timer));
				AudioSource component2 = projectileFlight.Entity.GameObject.GetComponent<AudioSource>();
				IAudioHandle audioHandle2 = AudioManager.Instance.CreateHandle(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_wpn_bomb_tripwire_ticktock, ELoadStrategy.LoadNext, EStoppedFinishedStrategy.UnloadAll);
				audioHandle2.Load();
				audioHandle2.Play(false, component2);
			}
		}

		private static void OnEnterExpire(IStateMachine<Projectile> _stateMachine, IPayload _payload, IState<Projectile> _prevState, IState<Projectile> _currentState)
		{
			ProjectileFlight projectileFlight = _stateMachine.Cast<ProjectileFlight>();
			projectileFlight.Vanish();
		}

		private void Hit()
		{
			if (HitAction != null)
			{
				HitAction(Entity);
				HitAction = null;
			}
		}

		private void Explode()
		{
			if (ExplodeAction != null)
			{
				ExplodeAction(Entity);
				ExplodeAction = null;
			}
			ExpireTurn = int.MaxValue;
			TriggerRadius = 0f;
			PlaceCameraShake component = Entity.GameObject.GetComponent<PlaceCameraShake>();
			if (component != null)
			{
				component.OnExplode();
			}
			Vanish();
		}

		public void Expire()
		{
			if (CurrentStateType != EStateType.Projectile_Expire)
			{
				ChangeState(EStateType.Projectile_Expire);
			}
		}

		private void Vanish()
		{
			LogicMain.EntityManager.UnregisterEntity(Entity);
		}

		public void RegisterTarget(Character _targetEntity)
		{
			AnimationController animationController = _targetEntity.UnityComponentCache.AnimationController;
			animationController.DataController.RegisterEventListener(this);
			animationController.TaskManager.AnimationTaskExited += OnAnimationTaskExited;
		}

		public bool OnAnimatorEvent(AnimatorEvent _event)
		{
			if (_event.Type != 0)
			{
				return false;
			}
			GetAnimationController().TaskManager.AnimationTaskExited -= OnAnimationTaskExited;
			ChangeState(EStateType.Projectile_Arrived);
			return true;
		}

		private void OnAnimationTaskExited(object _sender, AnimationTaskManager.AnimationTaskEventArgs _args)
		{
			if (_args.AnimationTask.TaskType == AnimationTaskType.DeployBomb)
			{
				AnimationController animationController = GetAnimationController();
				animationController.DataController.UnregisterEventListener(this);
				animationController.TaskManager.AnimationTaskExited -= OnAnimationTaskExited;
				if (CurrentStateType != EStateType.Projectile_Arrived)
				{
					ChangeState(EStateType.Projectile_Arrived);
				}
			}
		}

		private AnimationController GetAnimationController()
		{
			ProjectileStats projectileStats = Entity.ProjectileStats;
			Character entity = projectileStats.Get_EntityId(StatTypes.EStatType.Projectile_TargetId).GetEntity<Character>();
			return entity.UnityComponentCache.AnimationController;
		}

		private void AddAnnotation(ICombatAnnotation _annotation)
		{
			_annotation.OnRelease += OnAnnotationRelease;
			m_activeAnnotations.Add(_annotation);
		}

		private void OnAnnotationRelease(ICombatAnnotation _o)
		{
			m_activeAnnotations.Remove(_o);
			_o.OnRelease -= OnAnnotationRelease;
		}
	}
}
