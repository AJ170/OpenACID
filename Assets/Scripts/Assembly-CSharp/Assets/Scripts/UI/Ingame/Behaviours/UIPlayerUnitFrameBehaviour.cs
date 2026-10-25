using AcierProtocol;
using Assets.Scripts.Mission.UI.LogicInterface;

namespace Assets.Scripts.UI.Ingame.Behaviours
{
	public class UIPlayerUnitFrameBehaviour : UIUnitFrameBehaviour
	{
		public UINotorietyBarBehaviour NotorietyBar;

		public override UIScreenWidgetManager.EUiScreenWidget ScreenWidgetType
		{
			get
			{
				return UIScreenWidgetManager.EUiScreenWidget.PlayerUnitFrame;
			}
		}

		public override void Init()
		{
			if (!IsInitialized && !(NotorietyBar == null))
			{
				base.Init();
			}
		}

		public override void SetVisibility(bool _visible)
		{
			base.SetVisibility(_visible);
			NotorietyBar.SetVisibility(_visible);
		}

		public override void Exclude()
		{
			base.Exclude();
			NotorietyBar.Exclude();
		}

		public void SetNotoriety(int _notoriety100, EAlertLevel _currentAlertLevel)
		{
			if (IsInitialized)
			{
				NotorietyBar.SetNotoriety(_notoriety100, _currentAlertLevel);
			}
		}

		public void Show(string _name, float _healthNormalized, IActiveBuff[] _currentBuffs, int _notoriety100, EAlertLevel _currentAlertLevel)
		{
			SetNotoriety(_notoriety100, _currentAlertLevel);
			Show(_name, _healthNormalized, _currentBuffs);
		}
	}
}
