using System;
using AcierProtocol;
using Assets.Scripts.Audio;
using Assets.Scripts.Mission.HUD;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Behaviours.Buttons;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Network;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.Popup;
using Assets.Scripts.UI.Screens.Popup.ErrorPopup;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Types;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Development;
using Assets.Scripts.UI.Utils.Extensions;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Behaviours
{
	public class UIDeleteAssassin : MonoBehaviour
	{
		[CheckAssigned]
		[SerializeField]
		private UIButtonProxy m_button;

		[SerializeField]
		[CheckAssigned]
		private UIEventTrigger m_eventTrigger;

		private bool CanDelete
		{
			get
			{
				return Singleton<StateManager>.Instance.ProfileManager.AssassinCount > 1;
			}
		}

		public static event Action<UIAssasinId> OnAssassinDeleted;

		private void Awake()
		{
			UIButtonSounds component = m_button.Button.GetComponent<UIButtonSounds>();
			if (component != null)
			{
				component.SetSuppressAudio(true);
			}
		}

		private void OnEnable()
		{
			EventDelegate.Add(m_button.onClick, OnClick);
			EventDelegate.Add(m_eventTrigger.onClick, OnClick);
			UpdateEnabledState();
		}

		private void UpdateEnabledState()
		{
			m_button.isEnabled = CanDelete;
		}

		private void OnDisable()
		{
			EventDelegate.Remove(m_button.onClick, OnClick);
			EventDelegate.Remove(m_eventTrigger.onClick, OnClick);
		}

		private void OnClick()
		{
			if (CanDelete)
			{
				AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_menu_global_accept);
				AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_menu_global_whoosh_small);
				DeleteAssassin();
			}
			else
			{
				AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_menu_global_deny);
				string message = LocalizationManager.Instance.Get(5549);
				UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UIOnScreenMessage>().ShowMessage(message);
			}
		}

		private void DeleteAssassin()
		{
			StringMapper stringMapper = new StringMapper();
			stringMapper.Add("assassin_name", Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin.GetAssassinName());
			StringMapper mapper = stringMapper;
			PopUpButtonConfiguration[] buttons = new PopUpButtonConfiguration[2]
			{
				new PopUpButtonConfiguration
				{
					ButtonType = PopUpButtonConfiguration.EButtonType.eSuggestedClick,
					Label = LocalizationManager.Instance.Get(1780),
					SoundIntent = EButtonIntent.Delete,
					OnButtonPressed = delegate
					{
						Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
						DoDelete();
					}
				},
				new PopUpButtonConfiguration
				{
					ButtonType = PopUpButtonConfiguration.EButtonType.eNormal,
					Label = LocalizationManager.Instance.Get(1781),
					OnButtonPressed = delegate
					{
						Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
					}
				}
			};
			PopUpConfiguration popUpConfiguration = new PopUpConfiguration();
			popUpConfiguration.ShowCloseButton = true;
			popUpConfiguration.OnCloseButtonClicked = delegate
			{
				Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
			};
			popUpConfiguration.Aligntarget = UIPopup.EContentAlignTarget.CloseButton;
			popUpConfiguration.Heading = LocalizationManager.Instance.Get(2341, mapper);
			popUpConfiguration.Buttons = buttons;
			popUpConfiguration.Data = new ErrorContent.ErrorData
			{
				Description = LocalizationManager.Instance.Get(5445)
			};
			PopUpConfiguration config = popUpConfiguration;
			Singleton<UIScreenManager>.Instance.ShowPopUpScreen(config);
		}

		private void DoDelete()
		{
			UIAssasinId deleted = Singleton<StateManager>.Instance.ProfileManager.CurrentAssassinId;
			int num = Singleton<StateManager>.Instance.ProfileManager.Profile.assassins.assassins.FindIndex((Assassin _x) => _x.id == deleted);
			num = ((num < Singleton<StateManager>.Instance.ProfileManager.AssassinCount - 1) ? (num + 1) : 0);
			string newId = Singleton<StateManager>.Instance.ProfileManager.Profile.assassins.assassins[num].id;
			AsyncCallback callback = delegate(IAsyncResult _response)
			{
				OnAssasssinDeleted(_response, newId, deleted);
			};
			UINetworkAdaptor.BlockedNetworkRequest(delegate(AsyncCallback _asyncResult)
			{
				Singleton<StateManager>.Instance.ProfileManager.DeleteAssassin(deleted, _asyncResult);
			}, null, callback);
		}

		private void OnAssasssinDeleted(IAsyncResult _result, UIAssasinId _nextId, UIAssasinId _oldId)
		{
			if (!_result.IsError())
			{
				int currentAssassinIndex = Singleton<StateManager>.Instance.ProfileManager.Profile.assassins.assassins.FindIndex((Assassin _x) => _x.id == _nextId);
				Singleton<StateManager>.Instance.ProfileManager.CurrentAssassinIndex = currentAssassinIndex;
				UpdateEnabledState();
				if (UIDeleteAssassin.OnAssassinDeleted != null)
				{
					UIDeleteAssassin.OnAssassinDeleted(_oldId);
				}
			}
		}
	}
}
