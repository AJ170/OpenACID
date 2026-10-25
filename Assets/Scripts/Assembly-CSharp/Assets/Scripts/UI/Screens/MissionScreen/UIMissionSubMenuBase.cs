using System;
using Assets.Scripts.UI.Behaviours;
using Assets.Scripts.UI.EnvironmentBridge;
using Assets.Scripts.UI.Screens.MissionScreen.EarthPins;
using Assets.Scripts.UI.State.MissionManager;
using Assets.Scripts.UI.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.MissionScreen
{
	public abstract class UIMissionSubMenuBase : MonoBehaviour
	{
		[SerializeField]
		protected UIMissionActionButtonBar m_actionButtonBar;

		public abstract MissionSelectionManager.EMissionMenu ListType { get; }

		public virtual bool ShowBackButton
		{
			get
			{
				return false;
			}
		}

		protected EarthPinManager EarthPinManager { get; set; }

		public virtual void OnInit(EarthPinManager _earthPinManager)
		{
			EarthPinManager = _earthPinManager;
			base.gameObject.SetActive(false);
		}

		public virtual void OnActivate()
		{
			base.gameObject.SetActive(true);
			if (BehaviourSingleton<EnvironmentBridgeMain>.IsInitialized && BehaviourSingleton<EnvironmentBridgeMain>.Instance.EarthTransitionManager != null)
			{
				EarthTransitionManager earthTransitionManager = BehaviourSingleton<EnvironmentBridgeMain>.Instance.EarthTransitionManager;
				earthTransitionManager.OnEarthTransitionDone = (Action)Delegate.Combine(earthTransitionManager.OnEarthTransitionDone, new Action(OnEarthTransitionDone));
			}
			m_actionButtonBar.GetButtonByType(UIMissionActionButtonBar.EButtonType.Primary).isEnabled = true;
			EventDelegate.Add(m_actionButtonBar.GetButtonByType(UIMissionActionButtonBar.EButtonType.Primary).onClick, OnPrimaryActionButtonClicked);
			EventDelegate.Add(m_actionButtonBar.GetButtonByType(UIMissionActionButtonBar.EButtonType.Secondary).onClick, OnSecondaryActionButtonClicked);
		}

		public virtual void OnDeactivate()
		{
			if (BehaviourSingleton<EnvironmentBridgeMain>.IsInitialized && BehaviourSingleton<EnvironmentBridgeMain>.Instance.EarthTransitionManager != null)
			{
				EarthTransitionManager earthTransitionManager = BehaviourSingleton<EnvironmentBridgeMain>.Instance.EarthTransitionManager;
				earthTransitionManager.OnEarthTransitionDone = (Action)Delegate.Remove(earthTransitionManager.OnEarthTransitionDone, new Action(OnEarthTransitionDone));
			}
			EventDelegate.Remove(m_actionButtonBar.GetButtonByType(UIMissionActionButtonBar.EButtonType.Primary).onClick, OnPrimaryActionButtonClicked);
			EventDelegate.Remove(m_actionButtonBar.GetButtonByType(UIMissionActionButtonBar.EButtonType.Secondary).onClick, OnSecondaryActionButtonClicked);
			m_actionButtonBar.GetButtonByType(UIMissionActionButtonBar.EButtonType.Primary).isEnabled = false;
			base.gameObject.SetActive(false);
		}

		public virtual bool OnTapBack()
		{
			return false;
		}

		public virtual void Refresh()
		{
		}

		protected abstract void SetupEarthPinConnection();

		protected abstract void OnActionButtonClicked(UIMissionActionButtonBar.EButtonType _buttontype);

		private void OnPrimaryActionButtonClicked()
		{
			OnActionButtonClicked(UIMissionActionButtonBar.EButtonType.Primary);
		}

		private void OnSecondaryActionButtonClicked()
		{
			OnActionButtonClicked(UIMissionActionButtonBar.EButtonType.Secondary);
		}

		private void OnEarthTransitionDone()
		{
			SetupEarthPinConnection();
		}
	}
}
