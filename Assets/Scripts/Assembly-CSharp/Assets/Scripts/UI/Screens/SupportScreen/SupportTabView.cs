using Assets.Scripts.Native;
using Assets.Scripts.Network;
using Assets.Scripts.Network.Support;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.SupportScreen
{
	public class SupportTabView : MonoBehaviour
	{
		[SerializeField]
		private UIExtendedLabel m_headingLabel;

		[SerializeField]
		private UIExtendedLabel m_descriptionLabel;

		[SerializeField]
		private UIButtonProxy m_supportButton;

		private SupportDescription m_supportDesc;

		private ESupportQueue m_queue;

		public ESupportQueue Queue
		{
			get
			{
				return m_queue;
			}
			set
			{
				m_queue = value;
				UpdateData();
			}
		}

		private void Awake()
		{
		}

		private void OnEnable()
		{
			EventDelegate.Add(m_supportButton.onClick, OnButtonClicked);
		}

		private void OnDisable()
		{
			EventDelegate.Remove(m_supportButton.onClick, OnButtonClicked);
		}

		private void OnDestroy()
		{
			NetworkDataStore dataStore = Singleton<StateManager>.Instance.ProfileManager.DataStore;
			dataStore.RestoreShopResponseCallback -= OnRestoreSuccessfulCallback;
		}

		private void UpdateData()
		{
			m_supportDesc = null;
			if (UbiCore.Utils.Singleton<UIDataMapper>.Instance.SupportDescription.TryGetValue((int)Queue, out m_supportDesc))
			{
				m_headingLabel.text = LocalizationManager.Instance.Get(m_supportDesc.HeadingId);
				m_descriptionLabel.text = LocalizationManager.Instance.Get(m_supportDesc.DescriptionId);
				m_supportButton.Button.SetLabel(m_supportDesc.ButtonTextId);
			}
		}

		private void OnButtonClicked()
		{
			if (m_queue != ESupportQueue.Refound)
			{
				NativeTools.SendMail(Queue);
			}
			else if (m_supportDesc != null)
			{
				Singleton<UIScreenManager>.Instance.ShowLoadingOverlay(LoadingOverlayConfiguration.Default);
				NetworkDataStore dataStore = Singleton<StateManager>.Instance.ProfileManager.DataStore;
				dataStore.RestoreShopResponseCallback += OnRestoreSuccessfulCallback;
				dataStore.RestoreCompletedTransactions();
			}
		}

		private void OnRestoreSuccessfulCallback(bool _successful)
		{
			NetworkDataStore dataStore = Singleton<StateManager>.Instance.ProfileManager.DataStore;
			dataStore.RestoreShopResponseCallback -= OnRestoreSuccessfulCallback;
			Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
			if (_successful)
			{
				UIUtil.PopUpGeneric(m_supportDesc.PopupHeadingId, m_supportDesc.PopupDescriptionId);
			}
		}
	}
}
