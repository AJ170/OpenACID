using System;
using System.Collections;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Audio;
using Assets.Scripts.Mission;
using Assets.Scripts.Mission.Mission;
using Assets.Scripts.Mission.Mission.Interfaces;
using Assets.Scripts.Network.Missions.Mission.Data;
using Assets.Scripts.UI.Ingame.Implementation;
using Assets.Scripts.UI.NGUIExtensions;
using Assets.Scripts.UI.NGUIExtensions.Tweens;
using Assets.Scripts.UI.Utils;
using Mission;
using SharedBaseLib.Core;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Ingame.Behaviours
{
	public class UIObjectiveTrackerBehaviour : UIBaseBehaviour
	{
		public enum EObjectiveStatusChangeState
		{
			None = 0,
			StartStateChange = 1,
			StateChangeStarted = 2,
			StateChangeCompleted = 3
		}

		public enum ETrackerState
		{
			None = 0,
			Opening = 1,
			Open = 2,
			Closing = 3,
			Closed = 4
		}

		public delegate void TapTracker();

		private const float m_transissionTime = 2.5f;

		public TapTracker OnTapTracker;

		public float DisplayTime = 5f;

		public float SwitchingMissionDisplayTime = 1.5f;

		public UnityEngine.Animation TrackerAnimation;

		public UnityEngine.Animation CheckBoxAnimation;

		public UISprite ObjectiveIcon;

		public UIToggle ObjectivStatusBox;

		public UISprite BoxMarker;

		public UILabel MainObjectiveText;

		public UIAlignmentBox ModifiersChildContainer;

		public UIWidget ModifiersWidget;

		public UIWidget ModifierListWidget;

		public UIObjectiveModifier[] ObjectiveModifiers;

		public UIWidget QuickInfoContainer;

		public UILabel QuickInfoText;

		private ETrackerState m_currentState;

		private EObjectiveStatusChangeState m_objectiveStatusChangeState;

		private float m_elapsedWaitingTime;

		private bool m_isAnimating;

		private bool m_repositionModifierListOnOpen;

		private UITweener m_modifiersScaleTweener;

		private UITweener m_quickInfoAlphaTweener;

		private UITweener m_modifiersFadingTweener;

		private float m_transissionStartTime;

		private long m_currentPrimaryObjectiveID = -1L;

		private readonly BBQueue<long> m_primaryObjectivesToBeShownQueue = new BBQueue<long>();

		private bool m_hasMissionEnded;

		private bool m_isPaused;

		private IAudioHandle m_audioOpen;

		private IAudioHandle m_audioClose;

		private int m_modifierHash;

		private bool m_modifiersHaveChanged = true;

		private BBTweenAlpha m_additionalAlphaTweaner;

		public override UIScreenWidgetManager.EUiScreenWidget ScreenWidgetType
		{
			get
			{
				return UIScreenWidgetManager.EUiScreenWidget.ObjectiveTracker;
			}
		}

		public ETrackerState CurrentTrackerOpenState
		{
			get
			{
				return m_currentState;
			}
		}

		public bool IsChangingObjective { get; private set; }

		private bool HasNewObjectiveInQueue
		{
			get
			{
				return m_primaryObjectivesToBeShownQueue.Count != 0;
			}
		}

		private bool HasModifiers { get; set; }

		private bool HasMainObjectiveBeenFulfilled { get; set; }

		public int ModifierHash
		{
			get
			{
				return m_modifierHash;
			}
			set
			{
				m_modifiersHaveChanged = false;
				if (m_modifierHash != value)
				{
					m_modifierHash = value;
					m_modifiersHaveChanged = true;
				}
			}
		}

		public BBTweenAlpha AdditionalAlphaTweaner
		{
			get
			{
				if (m_additionalAlphaTweaner != null)
				{
					return m_additionalAlphaTweaner;
				}
				BBTweenAlpha bBTweenAlpha = (m_additionalAlphaTweaner = base.transform.parent.gameObject.AddComponent<BBTweenAlpha>());
				bBTweenAlpha.from = 0f;
				bBTweenAlpha.to = 1f;
				return bBTweenAlpha;
			}
		}

		public void ChangePhase(long _phaseID, int _modifierHash)
		{
			if (m_currentPrimaryObjectiveID != _phaseID && !m_primaryObjectivesToBeShownQueue.Contains(_phaseID))
			{
				AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_mission_update);
				ModifierHash = _modifierHash;
				m_primaryObjectivesToBeShownQueue.Enqueue(_phaseID);
				IsChangingObjective = true;
				HasMainObjectiveBeenFulfilled = false;
				UpdateTracker(false);
			}
		}

		public void Update()
		{
			if (UbiCore.Utils.Singleton<Timer>.Instance.timeScale > 0f && !m_isPaused)
			{
				foreach (AnimationState item in TrackerAnimation)
				{
					item.speed = 1f / UbiCore.Utils.Singleton<Timer>.Instance.timeScale;
				}
				foreach (AnimationState item2 in CheckBoxAnimation)
				{
					item2.speed = 1f / UbiCore.Utils.Singleton<Timer>.Instance.timeScale;
				}
				m_modifiersScaleTweener.ignoreTimeScale = true;
				m_quickInfoAlphaTweener.ignoreTimeScale = true;
				m_modifiersFadingTweener.ignoreTimeScale = true;
				return;
			}
			foreach (AnimationState item3 in TrackerAnimation)
			{
				item3.speed = 1f;
			}
			foreach (AnimationState item4 in CheckBoxAnimation)
			{
				item4.speed = 1f;
			}
			m_modifiersScaleTweener.ignoreTimeScale = false;
			m_quickInfoAlphaTweener.ignoreTimeScale = false;
			m_modifiersFadingTweener.ignoreTimeScale = false;
		}

		public void UpdateTracker(bool _missionEndedForceOpen)
		{
			if (!base.gameObject.activeInHierarchy)
			{
				return;
			}
			bool flag = false;
			if (_missionEndedForceOpen)
			{
				flag = true;
				m_elapsedWaitingTime = 0f;
				m_hasMissionEnded = true;
				m_objectiveStatusChangeState = EObjectiveStatusChangeState.StartStateChange;
			}
			else if (IsChangingObjective)
			{
				switch (m_currentState)
				{
				case ETrackerState.Open:
					switch (m_objectiveStatusChangeState)
					{
					case EObjectiveStatusChangeState.None:
					case EObjectiveStatusChangeState.StateChangeCompleted:
						StopAllCoroutines();
						StartCoroutine(AnimateClose(false));
						break;
					}
					break;
				case ETrackerState.Closed:
					switch (m_objectiveStatusChangeState)
					{
					case EObjectiveStatusChangeState.StateChangeCompleted:
						m_objectiveStatusChangeState = EObjectiveStatusChangeState.None;
						if (HasNewObjectiveInQueue)
						{
							m_currentPrimaryObjectiveID = m_primaryObjectivesToBeShownQueue.Dequeue();
							SetObjectiveMarkerImage(EProgressState.NotStarted);
							flag = true;
						}
						IsChangingObjective = HasNewObjectiveInQueue;
						break;
					}
					break;
				case ETrackerState.None:
					if (HasNewObjectiveInQueue)
					{
						m_currentPrimaryObjectiveID = m_primaryObjectivesToBeShownQueue.Dequeue();
						IsChangingObjective = HasNewObjectiveInQueue;
						flag = true;
					}
					break;
				}
			}
			MissionTracker.ObjectiveData objectiveData = UbiCore.Utils.Singleton<MissionManager>.Instance.MissionTracker.GetObjectiveData(m_currentPrimaryObjectiveID);
			if (objectiveData != null)
			{
				List<IObjectiveBase> visibleObjectives = objectiveData.GetVisibleObjectives();
				HasModifiers = visibleObjectives != null && (visibleObjectives.Count > 1 || !string.IsNullOrEmpty(visibleObjectives[0].SecondaryDescription));
				ModifiersWidget.alpha = (HasModifiers ? 1 : 0);
				if (flag)
				{
					Open(true);
				}
				UpdateData(visibleObjectives);
				SetObjectiveIcon(visibleObjectives[0].ObjectiveData.TrackerIcon);
			}
		}

		private void SetObjectiveIcon(EObjectiveType _objectiveType)
		{
			UIMissionUtil.UpdateMissionIconByMissionObjectiveType(ObjectiveIcon, _objectiveType, false);
		}

		public void Init()
		{
			m_hasMissionEnded = false;
			m_isAnimating = false;
			m_currentPrimaryObjectiveID = -1L;
			m_primaryObjectivesToBeShownQueue.Clear();
			m_currentState = ETrackerState.None;
			m_objectiveStatusChangeState = EObjectiveStatusChangeState.None;
			SetObjectiveMarkerImage(EProgressState.NotStarted);
			if (ModifiersWidget != null)
			{
				m_modifiersScaleTweener = ModifiersWidget.GetComponent<TweenScale>();
				m_modifiersFadingTweener = ModifierListWidget.GetComponent<TweenAlpha>();
			}
			if (QuickInfoContainer != null)
			{
				m_quickInfoAlphaTweener = QuickInfoContainer.GetComponent<TweenAlpha>();
			}
		}

		private void OnScaleChanged(Timer.EPauseState _pauseReason)
		{
			switch (_pauseReason)
			{
			case Timer.EPauseState.PauseDialog:
				SeekToEnd(false);
				break;
			case Timer.EPauseState.Running:
				SeekToEnd(true);
				break;
			case Timer.EPauseState.PauseGeneral:
			case Timer.EPauseState.PauseOptions:
				break;
			}
		}

		private void SeekToEnd(bool _show)
		{
			if (!_show)
			{
				AdditionalAlphaTweaner.PlayForward();
				AdditionalAlphaTweaner.ResetToBeginning();
				AdditionalAlphaTweaner.enabled = false;
			}
			else
			{
				AdditionalAlphaTweaner.PlayReverse();
				AdditionalAlphaTweaner.ResetToBeginning();
				AdditionalAlphaTweaner.enabled = false;
			}
			if (!_show)
			{
				ParticleEmitter[] componentsInChildren = base.gameObject.GetComponentsInChildren<ParticleEmitter>();
				foreach (ParticleEmitter particleEmitter in componentsInChildren)
				{
					particleEmitter.emit = false;
					particleEmitter.ClearParticles();
				}
				ParticleSystem[] componentsInChildren2 = base.gameObject.GetComponentsInChildren<ParticleSystem>();
				foreach (ParticleSystem particleSystem in componentsInChildren2)
				{
					particleSystem.Stop();
					particleSystem.Clear(true);
				}
			}
		}

		public bool IsObjectiveForceOpenType(IObjectiveBase _baseData)
		{
			return _baseData is ObjectiveKill || _baseData is ObjectiveKillEnemyType;
		}

		public void Show()
		{
			base.gameObject.SetActive(true);
		}

		public void Exclude()
		{
			CancelAnimation();
			base.gameObject.SetActive(false);
		}

		private void CancelAnimation()
		{
			StopAllCoroutines();
			TrackerAnimation.Stop();
			CheckBoxAnimation.Stop();
		}

		public void OnTapIcon()
		{
			if (!IsChangingObjective && !m_hasMissionEnded)
			{
				if (OnTapTracker != null)
				{
					OnTapTracker();
				}
				Open(false);
			}
		}

		public void SetPaused(bool _isPaused)
		{
			m_isPaused = _isPaused;
		}

		private void Awake()
		{
			Timer instance = UbiCore.Utils.Singleton<Timer>.Instance;
			instance.OnPauseChanged = (Timer.OnPauseChangeHandler)Delegate.Combine(instance.OnPauseChanged, new Timer.OnPauseChangeHandler(OnScaleChanged));
		}

		private void OnEnable()
		{
			if (m_audioOpen == null)
			{
				m_audioOpen = AudioManager.Instance.CreateHandle(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_mission_info_open, ELoadStrategy.LoadNext, EStoppedFinishedStrategy.UnloadCurrent);
				m_audioOpen.Load();
			}
			if (m_audioClose == null)
			{
				m_audioClose = AudioManager.Instance.CreateHandle(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_mission_info_close, ELoadStrategy.LoadNext, EStoppedFinishedStrategy.UnloadCurrent);
				m_audioClose.Load();
			}
		}

		private void OnDisable()
		{
			m_isAnimating = false;
			if (m_audioOpen != null)
			{
				m_audioOpen.UnloadAllNowOrOnDone();
				m_audioOpen = null;
			}
			if (m_audioClose != null)
			{
				m_audioClose.UnloadAllNowOrOnDone();
				m_audioClose = null;
			}
			ParticleEmitter[] componentsInChildren = base.gameObject.GetComponentsInChildren<ParticleEmitter>();
			foreach (ParticleEmitter particleEmitter in componentsInChildren)
			{
				particleEmitter.emit = false;
				particleEmitter.ClearParticles();
			}
			ParticleSystem[] componentsInChildren2 = base.gameObject.GetComponentsInChildren<ParticleSystem>();
			foreach (ParticleSystem particleSystem in componentsInChildren2)
			{
				particleSystem.Stop();
				particleSystem.Clear(true);
			}
		}

		private void OnDestroy()
		{
			Timer instance = UbiCore.Utils.Singleton<Timer>.Instance;
			instance.OnPauseChanged = (Timer.OnPauseChangeHandler)Delegate.Remove(instance.OnPauseChanged, new Timer.OnPauseChangeHandler(OnScaleChanged));
		}

		private void Open(bool _dataUpdate)
		{
			if (base.gameObject.activeInHierarchy)
			{
				m_elapsedWaitingTime = 0f;
				if (m_currentState == ETrackerState.Closing)
				{
					StopAllCoroutines();
				}
				else if (m_isAnimating)
				{
					return;
				}
				StartCoroutine(AnimateOpenAutoClose(_dataUpdate));
			}
		}

		private IEnumerator AnimateOpenAutoClose(bool _dataUpdate)
		{
			bool contentChanged = _dataUpdate && IsChangingObjective;
			yield return StartCoroutine(AnimateOpen(contentChanged));
			m_elapsedWaitingTime = 0f;
			while (m_elapsedWaitingTime < ((m_hasMissionEnded || IsChangingObjective) ? SwitchingMissionDisplayTime : DisplayTime))
			{
				m_elapsedWaitingTime += RealTime.deltaTime;
				yield return new WaitForEndOfFrame();
			}
			yield return StartCoroutine(AnimateClose(contentChanged));
		}

		private IEnumerator AnimateOpen(bool _contentChanged)
		{
			m_isAnimating = true;
			m_currentState = ETrackerState.Opening;
			if (m_audioOpen != null)
			{
				m_audioOpen.Load();
				m_audioOpen.Play();
			}
			m_quickInfoAlphaTweener.enabled = false;
			QuickInfoContainer.alpha = 0f;
			if (m_modifiersHaveChanged && !_contentChanged)
			{
				m_modifiersScaleTweener.PlayForward();
				m_modifiersScaleTweener.ResetToBeginning();
				m_modifiersScaleTweener.enabled = false;
				m_modifiersFadingTweener.PlayForward();
				m_modifiersFadingTweener.ResetToBeginning();
				m_modifiersFadingTweener.enabled = false;
			}
			TrackerAnimation.Play("misson_appear");
			while (TrackerAnimation.isPlaying)
			{
				yield return StartCoroutine(Timer.WaitForSecondsUnscaled(0.1f));
			}
			m_modifiersScaleTweener.enabled = true;
			m_modifiersScaleTweener.PlayForward();
			if (m_modifiersHaveChanged && HasModifiers)
			{
				m_modifiersFadingTweener.PlayForward();
				while (m_modifiersFadingTweener.enabled)
				{
					yield return StartCoroutine(Timer.WaitForSecondsUnscaled(0.1f));
				}
			}
			ModifiersChildContainer.RepositionNow = m_repositionModifierListOnOpen;
			m_repositionModifierListOnOpen = false;
			m_currentState = ETrackerState.Open;
		}

		private IEnumerator AnimateClose(bool _contentChanged)
		{
			m_currentState = ETrackerState.Closing;
			if (m_audioClose != null)
			{
				m_audioClose.Load();
				m_audioClose.Play();
			}
			if (m_modifiersHaveChanged && _contentChanged)
			{
				m_modifiersScaleTweener.PlayReverse();
				m_modifiersFadingTweener.PlayReverse();
				if (HasModifiers)
				{
					while (m_modifiersFadingTweener.enabled)
					{
						yield return StartCoroutine(Timer.WaitForSecondsUnscaled(0.1f));
					}
				}
			}
			TrackerAnimation.Play("mission_disappear");
			m_quickInfoAlphaTweener.ResetToBeginning();
			m_quickInfoAlphaTweener.PlayForward();
			while (TrackerAnimation.isPlaying)
			{
				yield return StartCoroutine(Timer.WaitForSecondsUnscaled(0.1f));
			}
			m_currentState = ETrackerState.Closed;
			m_isAnimating = false;
		}

		private void UpdateData(List<IObjectiveBase> _objectiveData)
		{
			if (_objectiveData != null && _objectiveData.Count > 0)
			{
				bool _hasTrackerChanged = false;
				bool _hasAnyModifierFailed = false;
				bool _hasAllMissionSucceeded = false;
				bool _hasModifiersVisibilityChanged = false;
				UpdateTrackerData(_objectiveData, ref _hasModifiersVisibilityChanged, ref _hasAllMissionSucceeded, ref _hasAnyModifierFailed, ref _hasTrackerChanged);
				UpdateTrackerBehaviour(_hasModifiersVisibilityChanged, _hasAllMissionSucceeded, _hasAnyModifierFailed, _hasTrackerChanged);
			}
			else
			{
				Exclude();
				m_objectiveStatusChangeState = EObjectiveStatusChangeState.StateChangeCompleted;
			}
		}

		private void UpdateTrackerData(List<IObjectiveBase> _objectiveData, ref bool _hasModifiersVisibilityChanged, ref bool _hasAllMissionSucceeded, ref bool _hasAnyModifierFailed, ref bool _hasTrackerChanged)
		{
			IObjectiveBase objectiveBase = _objectiveData[0];
			MainObjectiveText.text = objectiveBase.Description;
			QuickInfoText.text = objectiveBase.ShortDescription;
			_hasAllMissionSucceeded = objectiveBase.Result == global::Mission.EObjectiveResult.Fulfilled;
			_hasAnyModifierFailed = objectiveBase.Result == global::Mission.EObjectiveResult.Failed;
			if (!HasMainObjectiveBeenFulfilled && _hasAllMissionSucceeded)
			{
				_hasTrackerChanged = true;
			}
			HasMainObjectiveBeenFulfilled = _hasAllMissionSucceeded;
			if (m_hasMissionEnded)
			{
				_hasAnyModifierFailed |= UbiCore.Utils.Singleton<MissionManager>.Instance.MissionState == EMissionState.Failed;
			}
			int num = 0;
			if (!string.IsNullOrEmpty(objectiveBase.SecondaryDescription))
			{
				num++;
				UIObjectiveModifier uIObjectiveModifier = ObjectiveModifiers[0];
				uIObjectiveModifier.UpdateData(objectiveBase.SecondaryDescription, -1L, objectiveBase.Result == global::Mission.EObjectiveResult.Failed && m_currentState == ETrackerState.Open);
			}
			int num2 = 1;
			for (int i = num; i < ObjectiveModifiers.Length; i++)
			{
				UIObjectiveModifier uIObjectiveModifier2 = ObjectiveModifiers[i];
				if (_objectiveData.Count > num2)
				{
					IObjectiveBase objectiveBase2 = _objectiveData[num2];
					ObjectiveBaseData objectiveData = objectiveBase2.ObjectiveData;
					if (QuickInfoText.text.Length == 0)
					{
						QuickInfoText.text = objectiveBase2.ShortDescription;
					}
					else if (IsObjectiveForceOpenType(objectiveBase2) && objectiveData.ID == uIObjectiveModifier2.LastDataFromObjectiveId && uIObjectiveModifier2.ModifierText.text != objectiveBase2.Description)
					{
						_hasTrackerChanged = true;
					}
					bool flag = objectiveBase2.Result == global::Mission.EObjectiveResult.Failed;
					_hasModifiersVisibilityChanged |= !uIObjectiveModifier2.IsActive;
					if (!objectiveData.IsOptionalObjective)
					{
						_hasAllMissionSucceeded &= objectiveBase2.Result == global::Mission.EObjectiveResult.Fulfilled;
						_hasAnyModifierFailed |= flag;
					}
					_hasTrackerChanged |= uIObjectiveModifier2.UpdateData(objectiveBase2.Description, objectiveData.ID, flag && m_currentState == ETrackerState.Open);
				}
				else
				{
					_hasModifiersVisibilityChanged |= uIObjectiveModifier2.IsActive;
					uIObjectiveModifier2.Hide();
				}
				num2++;
			}
		}

		private void UpdateTrackerBehaviour(bool _hasModifiersVisibilityChanged, bool _hasAllMissionSucceeded, bool _hasAnyModifierFailed, bool _hasTrackerChanged)
		{
			if (QuickInfoText.text.Length == 0)
			{
				QuickInfoContainer.alpha = 0f;
			}
			ModifiersChildContainer.gameObject.SetActive(HasModifiers);
			m_repositionModifierListOnOpen |= _hasModifiersVisibilityChanged;
			if (m_objectiveStatusChangeState != EObjectiveStatusChangeState.StateChangeStarted)
			{
				if (_hasAllMissionSucceeded || _hasAnyModifierFailed)
				{
					IsChangingObjective = HasNewObjectiveInQueue;
					if (m_currentState == ETrackerState.Closed)
					{
						EProgressState objectiveMarkerImage = EProgressState.Fulfilled;
						if (_hasAnyModifierFailed)
						{
							objectiveMarkerImage = EProgressState.Failed;
						}
						SetObjectiveMarkerImage(objectiveMarkerImage);
						m_transissionStartTime = Time.time + 2.5f;
						m_objectiveStatusChangeState = EObjectiveStatusChangeState.StateChangeStarted;
					}
				}
				else if (_hasTrackerChanged && !IsChangingObjective)
				{
					Open(false);
				}
			}
			else if (m_objectiveStatusChangeState == EObjectiveStatusChangeState.StateChangeStarted && !CheckBoxAnimation.isPlaying && m_transissionStartTime <= Time.time)
			{
				m_objectiveStatusChangeState = EObjectiveStatusChangeState.StateChangeCompleted;
			}
		}

		private void SetObjectiveMarkerImage(EProgressState _progress)
		{
			bool flag = true;
			string spriteName = string.Empty;
			switch (_progress)
			{
			case EProgressState.Fulfilled:
				spriteName = "hud_Checkbox_true";
				AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_mission_objective_success);
				break;
			case EProgressState.Failed:
				spriteName = "hud_Checkbox_false";
				AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_mission_objective_fail);
				break;
			default:
				flag = false;
				break;
			}
			BoxMarker.enabled = flag;
			if (flag)
			{
				BoxMarker.spriteName = spriteName;
				CheckBoxAnimation.Play("set");
			}
			else
			{
				CheckBoxAnimation.Play("unset");
				BoxMarker.alpha = 0f;
			}
		}
	}
}
