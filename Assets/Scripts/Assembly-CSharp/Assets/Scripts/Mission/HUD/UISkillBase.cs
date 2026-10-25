using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;
using Assets.Scripts.UI;
using Assets.Scripts.UI.Ingame.Behaviours.Skills;
using Assets.Scripts.UI.Localization;
using UbiCore.Utils;

namespace Assets.Scripts.Mission.HUD
{
	public abstract class UISkillBase : UIBase
	{
		public override UIScreenWidgetManager.EUiScreenWidget ScreenWidgetType
		{
			get
			{
				return UIScreenWidgetManager.EUiScreenWidget.SkillWheel;
			}
		}

		public override void OnSceneLoaded()
		{
			base.OnSceneLoaded();
			if (base.IsActiveOnSceneLoaded)
			{
				Init();
			}
		}

		public override void OnMissionLoaded()
		{
			base.OnMissionLoaded();
			if (IsActive)
			{
				InitLogic();
				UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.AddListener<SkillEvent>(OnSkillUpdate);
			}
		}

		public override void OnMissionUnloaded()
		{
			if (IsActive)
			{
				base.OnMissionUnloaded();
				UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.RemoveListener<SkillEvent>(OnSkillUpdate);
			}
		}

		protected virtual bool CanBeTriggered(Enum<SkillTypes> _skill, out ESkillAvailabilityFlags _availabilityFlags)
		{
			return UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.CanBeTriggeredOnSelectedEntity(_skill, out _availabilityFlags);
		}

		protected virtual bool CanBeAborted(Enum<SkillTypes> _skill)
		{
			return UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.CanBeAborted(_skill);
		}

		protected bool IsSkillReady(Enum<SkillTypes> _skillType, out ESkillAvailabilityFlags _availabilityFlags)
		{
			return CanBeTriggered(_skillType, out _availabilityFlags);
		}

		protected ESkillState GetSkillState(Enum<SkillTypes> _skill)
		{
			return UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.GetState(_skill);
		}

		protected abstract void Init();

		protected abstract void InitLogic();

		protected abstract void OnSkillUpdate(SkillEvent _message);

		protected virtual void OnSkillUpdateButton(SkillEvent _message, UISkillButtonBaseBehaviour _baseButton)
		{
			if (!(_baseButton == null))
			{
				ILogicInterface logicInterface = UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface;
				bool isSkillEnabled = true;
				int startGameTurn = 0;
				int gameTurnDuration = 0;
				UISkillButtonBaseBehaviour.ESkillStatus status = UISkillButtonBaseBehaviour.ESkillStatus.None;
				ESkillAvailabilityFlags _availabilityFlags;
				switch (_message.SubType)
				{
				default:
					return;
				case ESkillEventType.Triggered:
					isSkillEnabled = CanBeAborted(_message.SkillType);
					status = UISkillButtonBaseBehaviour.ESkillStatus.Activated;
					break;
				case ESkillEventType.ActivityStart:
					isSkillEnabled = CanBeAborted(_message.SkillType);
					startGameTurn = logicInterface.GetActivityStart(_message.SkillType);
					gameTurnDuration = logicInterface.GetActivityDuration(_message.SkillType);
					status = UISkillButtonBaseBehaviour.ESkillStatus.ActivatedCooldown;
					break;
				case ESkillEventType.ActivityEnd:
					startGameTurn = logicInterface.GetCoolDownStart(_message.SkillType);
					gameTurnDuration = logicInterface.GetCoolDownDuration(_message.SkillType);
					status = UISkillButtonBaseBehaviour.ESkillStatus.CoolDown;
					break;
				case ESkillEventType.CooldownFinished:
					isSkillEnabled = IsSkillReady(_message.SkillType, out _availabilityFlags);
					break;
				case ESkillEventType.Abort:
					isSkillEnabled = IsSkillReady(_message.SkillType, out _availabilityFlags);
					break;
				case ESkillEventType.Remove:
					isSkillEnabled = false;
					status = UISkillButtonBaseBehaviour.ESkillStatus.Removed;
					break;
				}
				_baseButton.LastLogicSubType = _message.SubType;
				_baseButton.IsSkillEnabled = isSkillEnabled;
				_baseButton.SetSkillState(status, gameTurnDuration, startGameTurn);
			}
		}

		protected void UpdateSkillButtonState(UISkillButtonBaseBehaviour _baseButton)
		{
			ILogicInterface logicInterface = UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface;
			Enum<SkillTypes> skillType = _baseButton.SkillType;
			if (skillType != SkillTypes.Invalid)
			{
				bool cachedSkillAvailable = _baseButton.CachedSkillAvailable;
				ESkillAvailabilityFlags _availabilityFlags;
				_baseButton.CachedSkillAvailable = IsSkillReady(skillType, out _availabilityFlags);
				bool flag = CanBeAborted(skillType);
				_baseButton.IsSkillEnabled = _baseButton.CachedSkillAvailable || flag;
				_baseButton.UpdateButtonState(logicInterface.GameTurn, logicInterface.GameTurnProgress, flag);
				if (cachedSkillAvailable != _baseButton.CachedSkillAvailable && GetSkillState(skillType) == ESkillState.Idle)
				{
					LogicMain.LogicInterface.InvokeSkillAvailabilityChanged(LogicMain.PlayerEntity, skillType, _baseButton.CachedSkillAvailable, _availabilityFlags);
				}
			}
		}

		protected bool TriggerSkill(Enum<SkillTypes> _skillType)
		{
			ESkillAvailabilityFlags _availabilityFlags;
			if (CanBeTriggered(_skillType, out _availabilityFlags))
			{
				return UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.TriggerOnSelectedEntity(_skillType);
			}
			if (CanBeAborted(_skillType))
			{
				return UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.Abort(_skillType);
			}
			int num = 0;
			if ((_availabilityFlags & ESkillAvailabilityFlags.NotInCombat) == ESkillAvailabilityFlags.NotInCombat)
			{
				num = 5451;
			}
			else if ((_availabilityFlags & ESkillAvailabilityFlags.NotOutOfCombat) == ESkillAvailabilityFlags.NotOutOfCombat)
			{
				num = 5450;
			}
			else if ((_availabilityFlags & ESkillAvailabilityFlags.IsCoolingDown) == ESkillAvailabilityFlags.IsCoolingDown)
			{
				num = 5449;
			}
			else if ((_availabilityFlags & ESkillAvailabilityFlags.NotAnonymous) == ESkillAvailabilityFlags.NotAnonymous)
			{
				num = 5447;
			}
			else if ((_availabilityFlags & ESkillAvailabilityFlags.NoTargetSelected) == ESkillAvailabilityFlags.NoTargetSelected)
			{
				num = 5448;
			}
			else if ((_availabilityFlags & ESkillAvailabilityFlags.TargetNotInRange) == ESkillAvailabilityFlags.TargetNotInRange)
			{
				num = 5544;
			}
			if (num != 0)
			{
				UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UIOnScreenMessage>().ShowMessage(LocalizationManager.Instance.Get(num), true);
			}
			return false;
		}
	}
}
