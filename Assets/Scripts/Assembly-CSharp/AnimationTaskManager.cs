using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Assets.Scripts.Animation.Tasks;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Definitions;
using SharedBaseLib.Core;
using UbiCore.Debug;

public class AnimationTaskManager
{
	public interface IAnimationTaskEventListener : IDisposable
	{
		bool OnAnimationTaskEvent(AnimationTaskEventArgs _eventArgs);
	}

	public interface IAnimationStateEventListener : IDisposable
	{
		bool OnAnimationStateEvent(AnimationStateEventArgs _eventArgs);
	}

	public enum ELayer
	{
		Base = 0,
		UpperBody = 1,
		Special = 2
	}

	public enum EChangeType
	{
		Entered = 0,
		Exited = 1,
		Canceled = 2
	}

	public class AnimationTaskEventArgs : EventArgs
	{
		public AnimationTaskBase AnimationTask { get; set; }

		public EChangeType ChangeType { get; set; }

		public bool Canceled { get; set; }
	}

	public class AnimationStateEventArgs : EventArgs
	{
		public AnimationStateBase AnimationState { get; set; }

		public EChangeType ChangeType { get; set; }

		public AnimationTaskBase AnimationTask { get; set; }
	}

	private AnimationController m_AnimationController;

	private readonly BBList<AnimationTaskBase>[] m_CurrentTasks;

	private readonly ReadOnlyCollection<AnimationTaskBase>[] m_CurrentTasksAccessors;

	private readonly BBList<IAnimationTaskEventListener>[] m_taskEventListeners;

	private readonly BBList<IAnimationStateEventListener>[] m_stateEventListeners;

	private AnimationStateEventArgs m_AnimationStateEvent = new AnimationStateEventArgs();

	private AnimationTaskEventArgs m_AnimationTaskEventArgs = new AnimationTaskEventArgs();

	private static readonly AnimationTaskProperties[] s_AnimationTaskProperties;

	public int LayerCount { get; private set; }

	public CharacterData.ICharacterDefinition CharacterDefinition
	{
		get
		{
			return m_AnimationController.CharacterDefinition;
		}
	}

	public AnimationController AnimationController
	{
		get
		{
			return m_AnimationController;
		}
		private set
		{
			m_AnimationController = value;
		}
	}

	public event EventHandler<AnimationStateEventArgs> AnimationStateEntered;

	public event EventHandler<AnimationStateEventArgs> AnimationStateExited;

	public event EventHandler<AnimationTaskEventArgs> AnimationTaskEntered;

	public event EventHandler<AnimationTaskEventArgs> AnimationTaskExited;

	public AnimationTaskManager(AnimationController _controller)
	{
		AnimationController = _controller;
		if (AnimationController.Animator.layerCount == 0)
		{
			AnimationController.Animator.Rebind();
		}
		LayerCount = AnimationController.Animator.layerCount;
		m_CurrentTasks = new BBList<AnimationTaskBase>[AnimationController.Animator.layerCount];
		m_CurrentTasksAccessors = new ReadOnlyCollection<AnimationTaskBase>[AnimationController.Animator.layerCount];
		m_taskEventListeners = new BBList<IAnimationTaskEventListener>[AnimationController.Animator.layerCount];
		m_stateEventListeners = new BBList<IAnimationStateEventListener>[AnimationController.Animator.layerCount];
		for (int i = 0; i < m_CurrentTasks.Length; i++)
		{
			m_CurrentTasks[i] = new BBList<AnimationTaskBase>();
			m_CurrentTasksAccessors[i] = new ReadOnlyCollection<AnimationTaskBase>(m_CurrentTasks[i]);
			m_stateEventListeners[i] = new BBList<IAnimationStateEventListener>();
			m_taskEventListeners[i] = new BBList<IAnimationTaskEventListener>();
		}
	}

	static AnimationTaskManager()
	{
		s_AnimationTaskProperties = new AnimationTaskProperties[90];
		s_AnimationTaskProperties[0] = AnimationTaskAssassinateLeapInstigator.Properties;
		s_AnimationTaskProperties[1] = AnimationTaskAssassinateLeapTarget.Properties;
		s_AnimationTaskProperties[2] = AnimationTaskAssassinateStealthInstigator.Properties;
		s_AnimationTaskProperties[3] = AnimationTaskAssassinateStealthTarget.Properties;
		s_AnimationTaskProperties[4] = AnimationTaskAssassinateAirInstigator.Properties;
		s_AnimationTaskProperties[5] = AnimationTaskAssassinateAirTarget.Properties;
		s_AnimationTaskProperties[6] = AnimationTaskBalance.Properties;
		s_AnimationTaskProperties[7] = AnimationTaskBenchEnter.Properties;
		s_AnimationTaskProperties[8] = AnimationTaskBenchExit.Properties;
		s_AnimationTaskProperties[9] = AnimationTaskBenchIdle.Properties;
		s_AnimationTaskProperties[10] = AnimationTaskBenchKillInstigator.Properties;
		s_AnimationTaskProperties[11] = AnimationTaskBenchKillTarget.Properties;
		s_AnimationTaskProperties[13] = AnimationTaskClimb.Properties;
		s_AnimationTaskProperties[14] = AnimationTaskClimbRebound.Properties;
		s_AnimationTaskProperties[17] = AnimationTaskCodex.Properties;
		s_AnimationTaskProperties[18] = AnimationTaskCombat.Properties;
		s_AnimationTaskProperties[63] = AnimationTaskGetHit.Properties;
		s_AnimationTaskProperties[19] = AnimationTaskPlayerMoveTo.Properties;
		s_AnimationTaskProperties[20] = AnimationTaskDead.Properties;
		s_AnimationTaskProperties[21] = AnimationTaskHayStackEnter.Properties;
		s_AnimationTaskProperties[22] = AnimationTaskHayStackExit.Properties;
		s_AnimationTaskProperties[23] = AnimationTaskHayStackIdle.Properties;
		s_AnimationTaskProperties[24] = AnimationTaskHayStackKillInstigator.Properties;
		s_AnimationTaskProperties[25] = AnimationTaskHayStackKillTarget.Properties;
		s_AnimationTaskProperties[26] = AnimationTaskIdle.Properties;
		s_AnimationTaskProperties[27] = AnimationTaskJumpDown.Properties;
		s_AnimationTaskProperties[28] = AnimationTaskJumpDownAnywhere.Properties;
		s_AnimationTaskProperties[29] = AnimationTaskJumpHorizontal.Properties;
		s_AnimationTaskProperties[31] = AnimationTaskLeapOfFaith.Properties;
		s_AnimationTaskProperties[15] = AnimationTaskRunPassOver.Properties;
		s_AnimationTaskProperties[16] = AnimationTaskRunPassOverDown.Properties;
		s_AnimationTaskProperties[30] = AnimationTaskParkourSequence.Properties;
		s_AnimationTaskProperties[32] = AnimationTaskMoveTo.Properties;
		s_AnimationTaskProperties[36] = AnimationTaskDoctor.Properties;
		s_AnimationTaskProperties[37] = AnimationTaskHealAtDoctor.Properties;
		s_AnimationTaskProperties[78] = AnimationTaskDeployBomb.Properties;
		s_AnimationTaskProperties[79] = AnimationTaskThrowSand.Properties;
		s_AnimationTaskProperties[80] = AnimationTaskThrowSandReaction.Properties;
		s_AnimationTaskProperties[38] = AnimationTaskUnsheathWeapon.Properties;
		s_AnimationTaskProperties[39] = AnimationTaskSheathWeapon.Properties;
		s_AnimationTaskProperties[40] = AnimationTaskUnsheathBow.Properties;
		s_AnimationTaskProperties[41] = AnimationTaskSheathBow.Properties;
		s_AnimationTaskProperties[44] = AnimationTaskAimWithBow.Properties;
		s_AnimationTaskProperties[45] = AnimationTaskAimWithCrossbow.Properties;
		s_AnimationTaskProperties[49] = AnimationTaskInteractWithObject.Properties;
		s_AnimationTaskProperties[42] = AnimationTaskUnsheathCrossbow.Properties;
		s_AnimationTaskProperties[43] = AnimationTaskSheathCrossbow.Properties;
		s_AnimationTaskProperties[50] = AnimationTaskStunned.Properties;
		s_AnimationTaskProperties[51] = AnimationTaskStunningStrikeReaction.Properties;
		s_AnimationTaskProperties[52] = AnimationTaskKnockdowned.Properties;
		s_AnimationTaskProperties[53] = AnimationTaskGreedy.Properties;
		s_AnimationTaskProperties[54] = AnimationTaskStickyBombPanic.Properties;
		s_AnimationTaskProperties[60] = AnimationTaskSmokeBombReaction.Properties;
		s_AnimationTaskProperties[61] = AnimationTaskDagger.Properties;
		s_AnimationTaskProperties[62] = AnimationTaskSurgeOfFury.Properties;
		s_AnimationTaskProperties[33] = AnimationTaskNpcBenchIdle.Properties;
		s_AnimationTaskProperties[34] = AnimationTaskNpcTurning.Properties;
		s_AnimationTaskProperties[35] = AnimationTaskPlayerTurning.Properties;
		s_AnimationTaskProperties[55] = AnimationTaskLureReaction.Properties;
		s_AnimationTaskProperties[57] = AnimationTaskDismiss.Properties;
		s_AnimationTaskProperties[58] = AnimationTaskPushInstigator.Properties;
		s_AnimationTaskProperties[59] = AnimationTaskPushTarget.Properties;
		s_AnimationTaskProperties[66] = AnimationTaskThrowSmokeBomb.Properties;
		s_AnimationTaskProperties[70] = AnimationTaskThrowCoins.Properties;
		s_AnimationTaskProperties[67] = AnimationTaskMissionDeliverInstigator.Properties;
		s_AnimationTaskProperties[68] = AnimationTaskMissionDeliverTarget.Properties;
		s_AnimationTaskProperties[69] = AnimationTaskSplinterBomb.Properties;
		s_AnimationTaskProperties[71] = AnimationTaskFanOfKnives.Properties;
		s_AnimationTaskProperties[46] = AnimationTaskCourtesanSeduce.Properties;
		s_AnimationTaskProperties[47] = AnimationTaskSpecialSeduce.Properties;
		s_AnimationTaskProperties[48] = AnimationTaskEnemySeduced.Properties;
		s_AnimationTaskProperties[72] = AnimationTaskRooftopGardenEnter.Properties;
		s_AnimationTaskProperties[73] = AnimationTaskRooftopGardenIdle.Properties;
		s_AnimationTaskProperties[74] = AnimationTaskRooftopGardenExit.Properties;
		s_AnimationTaskProperties[75] = AnimationTaskRooftopGardenKillInstigator.Properties;
		s_AnimationTaskProperties[76] = AnimationTaskRooftopGardenKillTarget.Properties;
		s_AnimationTaskProperties[77] = AnimationTaskCapturedNpc.Properties;
		s_AnimationTaskProperties[56] = AnimationTaskPokeIntoHaystack.Properties;
		s_AnimationTaskProperties[81] = AnimationTaskHitEscortee.Properties;
		s_AnimationTaskProperties[82] = AnimationTaskThreat.Properties;
		s_AnimationTaskProperties[83] = AnimationTaskPigeonDelivery.Properties;
		s_AnimationTaskProperties[84] = AnimationTaskDieFromFall.Properties;
		s_AnimationTaskProperties[85] = AnimationTaskBlackjackInstigator.Properties;
		s_AnimationTaskProperties[86] = AnimationTaskBlackjackTarget.Properties;
		s_AnimationTaskProperties[87] = AnimationTaskStrafe.Properties;
		s_AnimationTaskProperties[88] = AnimationTaskHoldCombatIdle.Properties;
		s_AnimationTaskProperties[64] = AnimationTaskExecuteOnGround.Properties;
		s_AnimationTaskProperties[65] = AnimationTaskGetExecutedOnGround.Properties;
	}

	internal bool CreateAnimationTask<TTask>(AnimationTaskType _type) where TTask : AnimationTaskBase, new()
	{
		return CreateAnimationTask<TTask>(_type, true);
	}

	internal bool CreateAnimationTask<TTask>(AnimationTaskType _type, bool _cancelSameType) where TTask : AnimationTaskBase, new()
	{
		ELayer layer = GetLayer(_type);
		bool flag = GetCurrentTaskType(layer) == _type;
		if ((_cancelSameType || !flag) && CancelCurrentTask(layer))
		{
			AddAnimationTask(new TTask());
			flag = true;
		}
		return flag;
	}

	public static AnimationTaskProperties GetProperties(AnimationTaskType _type)
	{
		return s_AnimationTaskProperties[(int)_type];
	}

	public static ELayer GetLayer(AnimationTaskType _type)
	{
		return GetProperties(_type).Layer;
	}

	public static T GetParameters<T>(AnimationTaskType _type) where T : AnimationTaskParameters
	{
		return (T)GetProperties(_type).ParametersTemplate;
	}

	public void RegisterTaskEventListener(IAnimationTaskEventListener _listener, ELayer _layer)
	{
		m_taskEventListeners[(int)_layer].Add(_listener);
	}

	public void UnregisterTaskEventListener(IAnimationTaskEventListener _listener, ELayer _layer)
	{
		m_taskEventListeners[(int)_layer].Remove(_listener);
	}

	public void RegisterStateEventListener(IAnimationStateEventListener _listener, ELayer _layer)
	{
		m_stateEventListeners[(int)_layer].Add(_listener);
	}

	public void UnregisterStateEventListener(IAnimationStateEventListener _listener, ELayer _layer)
	{
		m_stateEventListeners[(int)_layer].Remove(_listener);
	}

	public void Detach()
	{
		Reset();
	}

	public void AddAnimationTask(AnimationTaskBase _task)
	{
		int layer = (int)GetLayer(_task.TaskType);
		m_CurrentTasks[layer].Add(_task);
	}

	public ReadOnlyCollection<AnimationTaskBase> GetCurrentTasks(ELayer _layer)
	{
		return m_CurrentTasksAccessors[(int)_layer];
	}

	public AnimationTaskBase GetCurrentTask(ELayer _layer)
	{
		AnimationTaskBase result = null;
		if (m_CurrentTasks[(int)_layer].Count > 0)
		{
			result = m_CurrentTasks[(int)_layer][0];
		}
		return result;
	}

	public void OnAnimationStateEnter(AnimationStateBase _animationState, AnimationTaskBase _animationTask)
	{
		if (this.AnimationStateEntered == null && m_stateEventListeners[(int)_animationTask.Layer].Count == 0)
		{
			return;
		}
		m_AnimationStateEvent.AnimationState = _animationState;
		m_AnimationStateEvent.ChangeType = EChangeType.Entered;
		m_AnimationStateEvent.AnimationTask = _animationTask;
		if (this.AnimationStateEntered != null)
		{
			this.AnimationStateEntered(this, m_AnimationStateEvent);
		}
		for (int i = 0; i < m_stateEventListeners[(int)_animationTask.Layer].Count; i++)
		{
			if (m_stateEventListeners[(int)_animationTask.Layer][i].OnAnimationStateEvent(m_AnimationStateEvent))
			{
				m_stateEventListeners[(int)_animationTask.Layer][i].Dispose();
				m_stateEventListeners[(int)_animationTask.Layer].RemoveAt(i);
				i--;
			}
		}
		m_AnimationStateEvent.AnimationState = null;
	}

	public void OnAnimationStateExit(AnimationStateBase _animationState, AnimationTaskBase _animationTask, bool _canceled)
	{
		if (this.AnimationStateExited == null && m_stateEventListeners[(int)_animationTask.Layer].Count == 0)
		{
			return;
		}
		m_AnimationStateEvent.AnimationState = _animationState;
		m_AnimationStateEvent.ChangeType = ((!_canceled) ? EChangeType.Exited : EChangeType.Canceled);
		m_AnimationStateEvent.AnimationTask = _animationTask;
		if (this.AnimationStateExited != null)
		{
			this.AnimationStateExited(this, m_AnimationStateEvent);
		}
		for (int i = 0; i < m_stateEventListeners[(int)_animationTask.Layer].Count; i++)
		{
			if (m_stateEventListeners[(int)_animationTask.Layer][i].OnAnimationStateEvent(m_AnimationStateEvent))
			{
				m_stateEventListeners[(int)_animationTask.Layer][i].Dispose();
				m_stateEventListeners[(int)_animationTask.Layer].RemoveAt(i);
				i--;
			}
		}
		m_AnimationStateEvent.AnimationState = null;
	}

	public void OnAnimationTaskEnter(AnimationTaskBase _animationTask)
	{
		if (this.AnimationTaskEntered == null && m_taskEventListeners[(int)_animationTask.Layer].Count == 0)
		{
			return;
		}
		m_AnimationTaskEventArgs.AnimationTask = _animationTask;
		m_AnimationTaskEventArgs.ChangeType = EChangeType.Entered;
		m_AnimationTaskEventArgs.Canceled = false;
		if (this.AnimationTaskEntered != null)
		{
			this.AnimationTaskEntered(this, m_AnimationTaskEventArgs);
		}
		for (int i = 0; i < m_taskEventListeners[(int)_animationTask.Layer].Count; i++)
		{
			if (m_taskEventListeners[(int)_animationTask.Layer][i].OnAnimationTaskEvent(m_AnimationTaskEventArgs))
			{
				m_taskEventListeners[(int)_animationTask.Layer][i].Dispose();
				m_taskEventListeners[(int)_animationTask.Layer].RemoveAt(i);
				i--;
			}
		}
		m_AnimationTaskEventArgs.AnimationTask = null;
	}

	public void OnAnimationTaskExit(AnimationTaskBase _animationTask, bool _canceled)
	{
		int layer = (int)_animationTask.Layer;
		if (m_CurrentTasks[layer].Count > 0)
		{
			m_CurrentTasks[layer].Remove(_animationTask);
		}
		List<IAnimationTaskEventListener> list = m_taskEventListeners[layer];
		int count = list.Count;
		if (this.AnimationTaskExited == null && count == 0)
		{
			return;
		}
		m_AnimationTaskEventArgs.AnimationTask = _animationTask;
		m_AnimationTaskEventArgs.ChangeType = ((!_canceled) ? EChangeType.Exited : EChangeType.Canceled);
		m_AnimationTaskEventArgs.Canceled = _canceled;
		if (this.AnimationTaskExited != null)
		{
			this.AnimationTaskExited(this, m_AnimationTaskEventArgs);
		}
		for (int i = 0; i < count; i++)
		{
			IAnimationTaskEventListener animationTaskEventListener = list[i];
			if (animationTaskEventListener.OnAnimationTaskEvent(m_AnimationTaskEventArgs))
			{
				animationTaskEventListener.Dispose();
				list.RemoveAt(i);
				i--;
			}
		}
		_animationTask.OnDispose();
		m_AnimationTaskEventArgs.AnimationTask = null;
	}

	public bool CancelCurrentTask(ELayer _layer, bool _startIdle = false)
	{
		return CancelCurrentTaskInternal(_layer, _startIdle, false);
	}

	public bool CancelCurrentTaskForced(ELayer _layer)
	{
		return CancelCurrentTaskInternal(_layer, false, true);
	}

	public void OnUpdate()
	{
		Profiling.Start("AnimationTaskManager::OnUpdate", float.MaxValue);
		List<AnimationTaskBase> list = null;
		for (int i = 0; i < LayerCount; i++)
		{
			list = m_CurrentTasks[i];
			if (list.Count > 0)
			{
				list[0].OnUpdate();
			}
		}
		Profiling.Stop();
	}

	public void OnDrawGizmos()
	{
		for (int i = 0; i < LayerCount; i++)
		{
			for (int j = 0; j < GetTaskCount((ELayer)i); j++)
			{
				m_CurrentTasks[i][j].OnDrawGizmos();
			}
		}
	}

	public AnimationTaskType GetCurrentTaskType(ELayer _layer)
	{
		AnimationTaskType result = AnimationTaskType.Unknown;
		if (GetTaskCount(_layer) > 0)
		{
			result = m_CurrentTasks[(int)_layer][0].TaskType;
		}
		return result;
	}

	public AnimationTaskType GetLastTaskType(ELayer _layer)
	{
		AnimationTaskType result = AnimationTaskType.Unknown;
		if (GetTaskCount(_layer) > 0)
		{
			result = m_CurrentTasks[(int)_layer][GetTaskCount(_layer) - 1].TaskType;
		}
		return result;
	}

	public int GetTaskCount(ELayer _layer)
	{
		return m_CurrentTasks[(int)_layer].Count;
	}

	public AnimationTaskType GetTaskType(int _idx, ELayer _layer)
	{
		return m_CurrentTasks[(int)_layer][_idx].TaskType;
	}

	public bool IsCurrentTaskCancelable(ELayer _layer)
	{
		bool result = true;
		if (GetTaskCount(_layer) > 0)
		{
			result = m_CurrentTasks[(int)_layer][0].CanCancel();
		}
		return result;
	}

	private bool CancelCurrentTaskInternal(ELayer _layer, bool _startIdle, bool _forceSuccess)
	{
		bool flag = true;
		if (GetTaskCount(_layer) > 0)
		{
			AnimationTaskBase animationTaskBase = m_CurrentTasks[(int)_layer][0];
			if (animationTaskBase.CanCancel())
			{
				m_CurrentTasks[(int)_layer].RemoveAt(0);
				Reset(_layer);
				animationTaskBase.OnExit(true);
			}
			else if (!Helpers.Verify(!_forceSuccess, "Forced canceling of current task {0}: Task is not allowed to be canceled. It will not be exited but just disposed. ({1})", GetCurrentTaskType(_layer), AnimationController.EntityId))
			{
				Reset(_layer);
			}
			else
			{
				flag = false;
			}
		}
		if (flag && _startIdle)
		{
			this.CreateAnimationTaskIdle();
		}
		return flag;
	}

	private void Reset(ELayer _layer)
	{
		for (int i = 0; i < m_CurrentTasks[(int)_layer].Count; i++)
		{
			m_CurrentTasks[(int)_layer][i].OnDispose();
		}
		m_CurrentTasks[(int)_layer].ClearIfNotEmpty();
		for (int j = 0; j < m_taskEventListeners[(int)_layer].Count; j++)
		{
			m_taskEventListeners[(int)_layer][j].Dispose();
		}
		m_taskEventListeners[(int)_layer].ClearIfNotEmpty();
		for (int k = 0; k < m_stateEventListeners[(int)_layer].Count; k++)
		{
			m_stateEventListeners[(int)_layer][k].Dispose();
		}
		m_stateEventListeners[(int)_layer].ClearIfNotEmpty();
	}

	private void Reset()
	{
		for (int i = 0; i < LayerCount; i++)
		{
			Reset((ELayer)i);
		}
	}
}
