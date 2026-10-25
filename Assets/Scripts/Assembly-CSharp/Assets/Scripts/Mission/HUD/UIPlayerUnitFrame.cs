using AcierProtocol;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.UI;
using Assets.Scripts.UI.Ingame.Behaviours;
using UbiCore.Utils;

namespace Assets.Scripts.Mission.HUD
{
	public class UIPlayerUnitFrame : UIUnitFrame<UIPlayerUnitFrameBehaviour>
	{
		public override UIScreenWidgetManager.EUiScreenWidget ScreenWidgetType
		{
			get
			{
				return UIScreenWidgetManager.EUiScreenWidget.PlayerUnitFrame;
			}
		}

		public override void OnMissionLoaded()
		{
			base.OnMissionLoaded();
			if (IsActive)
			{
				Show(UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.PlayerId);
			}
		}

		public override void OnMissionOutroStarted()
		{
			base.OnMissionOutroStarted();
			Hide();
		}

		public override void UpdateFrame(float _deltaTime)
		{
			if (!base.HasMissionEnded)
			{
				base.UpdateFrame(_deltaTime);
				UnitFrameBehaviour.Health = GetPlayerHealth();
				int playerNotoriety = UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.PlayerNotoriety;
				EAlertLevel playerAlertLevel = UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.PlayerAlertLevel;
				UnitFrameBehaviour.SetNotoriety(playerNotoriety, playerAlertLevel);
			}
		}

		protected override void Show(EntityId _entityId)
		{
			base.Show(_entityId);
			IActiveBuff[] buffs = UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.GetBuffs(_entityId);
			int playerNotoriety = UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.PlayerNotoriety;
			EAlertLevel playerAlertLevel = UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.PlayerAlertLevel;
			float playerHealth = GetPlayerHealth();
			string playerName = UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.PlayerName;
			UnitFrameBehaviour.Show(playerName, playerHealth, buffs, playerNotoriety, playerAlertLevel);
		}

		private static float GetPlayerHealth()
		{
			PlayerStats playerStats = UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.PlayerId.GetEntity<PlayerCharacter>().PlayerStats;
			float num = playerStats.Get_float(StatTypes.EStatType.Character_Conflict_Health);
			float num2 = playerStats.Get_float(StatTypes.EStatType.Character_Conflict_ActualMaxHealth);
			return num / num2;
		}
	}
}
