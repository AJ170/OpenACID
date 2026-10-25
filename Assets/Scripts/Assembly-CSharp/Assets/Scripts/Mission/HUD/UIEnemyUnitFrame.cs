using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.UI;
using Assets.Scripts.UI.Ingame.Behaviours;
using UbiCore.Utils;

namespace Assets.Scripts.Mission.HUD
{
	public class UIEnemyUnitFrame : UIUnitFrame<UIEnemyUnitFrameBehaviour>
	{
		public override bool InitiallyVisible
		{
			get
			{
				return false;
			}
		}

		public override UIScreenWidgetManager.EUiScreenWidget ScreenWidgetType
		{
			get
			{
				return UIScreenWidgetManager.EUiScreenWidget.EnemyUnitFrame;
			}
		}

		protected override void Show(EntityId _entityId)
		{
			base.Show(_entityId);
			IActiveBuff[] buffs = UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.GetBuffs(_entityId);
			string unitName = UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.GetUnitName(_entityId);
			float normalizedHealth = UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.GetNormalizedHealth(_entityId);
			UnitFrameBehaviour.Show(unitName, normalizedHealth, buffs);
			SetVisible(true);
		}

		public override void OnMissionLoaded()
		{
			base.OnMissionLoaded();
			if (IsActive)
			{
				UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.AddListener<CombatUiEvent>(OnCombatUiEvent);
			}
		}

		public override void OnMissionOutroStarted()
		{
			base.OnMissionOutroStarted();
			Hide();
		}

		public override void OnMissionUnload()
		{
			if (IsActive)
			{
				base.OnMissionUnload();
				UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.RemoveListener<CombatUiEvent>(OnCombatUiEvent);
			}
		}

		private void OnCombatUiEvent(CombatUiEvent _message)
		{
			switch (_message.Type)
			{
			case CombatUiEvent.EType.Select:
				if (_message.Target != null && HasValidPortraitData(_message.Target.Id))
				{
					Show(_message.Target.Id);
				}
				else
				{
					Hide();
				}
				break;
			case CombatUiEvent.EType.Exit:
				if (base.CurrentTargetId == _message.Target.Id)
				{
					Hide();
				}
				break;
			case CombatUiEvent.EType.Hit:
				UpdateHealth(_message.Target.Id);
				break;
			}
		}

		private void UpdateHealth(EntityId _entityId)
		{
			if (_entityId == base.CurrentTargetId)
			{
				UnitFrameBehaviour.Health = UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.GetNormalizedHealth(_entityId);
			}
		}
	}
}
