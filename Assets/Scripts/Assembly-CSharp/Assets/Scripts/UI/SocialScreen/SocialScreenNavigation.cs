using System.Collections;
using AcierProtocol;
using Assets.Scripts.UI.Behaviours;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.NGUIExtensions.Tweens;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.Popup;
using Assets.Scripts.UI.Screens.Popup.ErrorPopup;
using Assets.Scripts.UI.Utils.Development;
using Assets.Scripts.UbiPlayground;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI.SocialScreen
{
	public class SocialScreenNavigation : UIScreenChild
	{
		[CheckAssigned]
		[SerializeField]
		private TweenManager m_tweenManager;

		[SerializeField]
		[CheckAssigned]
		private UIWidget m_facebookAreaContainer;

		[SerializeField]
		[CheckAssigned]
		private UIWidget m_facebookNotLinkedContainer;

		[SerializeField]
		[CheckAssigned]
		private UIWidget m_facebookLinkedNotLoggedInContainer;

		[CheckAssigned]
		[SerializeField]
		private UIWidget m_facebookLoggedInContainer;

		[SerializeField]
		[CheckAssigned]
		private UIWidget m_facebookTab;

		[SerializeField]
		[CheckAssigned]
		private UIWidget m_googlePlayLogoutAreaContainer;

		[SerializeField]
		[CheckAssigned]
		private UIWidget m_googlePlayLoginAreaContainer;

		[SerializeField]
		[CheckAssigned]
		private UIWidget m_googlePlayNotLoggedInContainer;

		[CheckAssigned]
		[SerializeField]
		private UIWidget m_googlePlayLoggedInContainer;

		[CheckAssigned]
		[SerializeField]
		private UIWidget m_weiboAreaContainer;

		[SerializeField]
		[CheckAssigned]
		private UIWidget m_weiboNotLinkedContainer;

		[CheckAssigned]
		[SerializeField]
		private UIWidget m_weiboLinkedNotLoggedInContainer;

		[SerializeField]
		[CheckAssigned]
		private UIWidget m_weiboLoggedInContainer;

		[SerializeField]
		[CheckAssigned]
		private UIWidget m_uplayAreaContainer;

		[CheckAssigned]
		[SerializeField]
		private UIWidget m_uplayNotLinkedContainer;

		[CheckAssigned]
		[SerializeField]
		private UIWidget m_uplayLinkedNotLoggedInContainer;

		[CheckAssigned]
		[SerializeField]
		private UIWidget m_uplayLoggedInContainer;

		[CheckAssigned]
		[SerializeField]
		private BoxCollider m_uplayLoggedinCollider;

		[SerializeField]
		[CheckAssigned]
		private UIExtendedLabel m_facebookLinkedNotLoggedInUsername;

		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_facebookLoggedInUsername;

		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_weiboLinkedNotLoggedInUsername;

		[SerializeField]
		[CheckAssigned]
		private UIExtendedLabel m_weiboLoggedInUsername;

		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_uplayLinkedNotLoggedInUsername;

		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_uplayLoggedInUsername;

		[SerializeField]
		[CheckAssigned]
		private UITable m_scrollTable;

		private void Awake()
		{
		}

		protected override void OnScreenInit()
		{
			EventDelegate.Add(m_screen.onActivate, OnActivate);
			EventDelegate.Add(m_screen.onExit, OnExitScreen);
			m_facebookTab.gameObject.SetActive(Main.Instance.FacebookEnabled);
			m_scrollTable.Reposition();
		}

		protected override void OnScreenDestroy()
		{
			if (m_screen != null)
			{
				EventDelegate.Remove(m_screen.onActivate, OnActivate);
				EventDelegate.Remove(m_screen.onExit, OnExitScreen);
			}
		}

		private void OnActivate()
		{
			UISceneData sceneData = Singleton<UIScreenManager>.Instance.SceneData;
			UIRoot mainUIRoot = sceneData.MainUIRoot;
			Camera uI3DCamera = sceneData.UI3DCamera;
			BBTweenPosition tweenPos = uI3DCamera.gameObject.AddComponentIfNotExist<BBTweenPosition>();
			tweenPos.from = new Vector3(2000f, uI3DCamera.transform.position.y, -1000f);
			tweenPos.to = new Vector3(0f, uI3DCamera.transform.position.y, -1000f);
			tweenPos.duration = 0.4f;
			tweenPos.TweenType = BBTweenPosition.ETweenPosition.XAxisOnly;
			tweenPos.delay = 0.1f * (float)m_tweenManager.ScreenElements.Count + 0.1f;
			tweenPos.animationCurve = mainUIRoot.GetComponent<TweenPosition>().animationCurve;
			EventDelegate.Add(tweenPos.onFinished, delegate
			{
				NGUITools.Destroy(tweenPos);
			}, true);
			tweenPos.ResetToBeginning();
			tweenPos.PlayForward();
			UIScreenManager instance = Singleton<UIScreenManager>.Instance;
			if (instance != null && !(instance.SceneData == null))
			{
				UIPlayerCard playerCard = instance.SceneData.PlayerCard;
				if (playerCard != null)
				{
					playerCard.SetVisibility(true);
					playerCard.SetPosition(UIPlayerCard.EPositionType.Default);
					playerCard.SetRotation(UIPlayerCard.EPositionType.Default);
				}
				m_facebookAreaContainer.gameObject.SetActive(FacebookManager.IsAvailable);
				m_weiboAreaContainer.gameObject.SetActive(WeiboManager.IsWeiboAvailable);
				m_uplayAreaContainer.gameObject.SetActive(false);
				RefreshGooglePlayElements();
				bool flag = false;
				if (UbiPlaygroundManager.IsProfileLinkedToUplay && ((FacebookManager.IsAvailable && !FacebookManager.IsProfileLinkedToFacebook) || (WeiboManager.IsWeiboAvailable && !WeiboManager.IsProfileLinkedToWeibo)))
				{
					flag = true;
					ShowForceLinkProfileToOtherSocialNetwork();
				}
				if (!flag)
				{
					AutoLoginSocialNetworks();
				}
			}
		}

		private void AutoLoginSocialNetworks()
		{
			if (FacebookManager.IsAvailable && !FacebookManager.IsAutoLoginInProgress)
			{
				FacebookManager.AutoLogin(delegate
				{
					FacebookManager.IsAutoLoginInProgress = false;
					RefreshFacebookElements();
				});
			}
			if (WeiboManager.IsWeiboAvailable && !WeiboManager.IsAutoLoginInProgress)
			{
				Main.Instance.StartCoroutine(PrepForAutoLoginWeibo());
			}
			m_scrollTable.repositionNow = true;
		}

		private IEnumerator PrepForAutoLoginWeibo()
		{
			yield return new WaitForEndOfFrame();
			WeiboManager.AutoLogin(delegate
			{
				WeiboManager.IsAutoLoginInProgress = false;
				RefreshWeiboElements();
			});
			yield return null;
		}

		private void RefreshWeiboElements()
		{
			if (WeiboManager.IsWeiboAvailable)
			{
				if (!WeiboManager.IsProfileLinkedToWeibo)
				{
					m_weiboLinkedNotLoggedInContainer.gameObject.SetActive(false);
					m_weiboLoggedInContainer.gameObject.SetActive(false);
					m_weiboNotLinkedContainer.gameObject.SetActive(true);
				}
				else if (!WeiboManager.HasLoggedIntoWeiboKey)
				{
					m_weiboNotLinkedContainer.gameObject.SetActive(false);
					m_weiboLoggedInContainer.gameObject.SetActive(false);
					m_weiboLinkedNotLoggedInContainer.gameObject.SetActive(true);
					m_weiboLinkedNotLoggedInUsername.text = WeiboManager.NameWeiboProfile;
				}
				else
				{
					m_weiboNotLinkedContainer.gameObject.SetActive(false);
					m_weiboLinkedNotLoggedInContainer.gameObject.SetActive(false);
					m_weiboLoggedInContainer.gameObject.SetActive(true);
					m_weiboLoggedInUsername.text = WeiboManager.NameWeiboProfile;
				}
			}
		}

		private void RefreshFacebookElements()
		{
			if (FacebookManager.IsAvailable)
			{
				if (!FacebookManager.IsProfileLinkedToFacebook)
				{
					m_facebookLinkedNotLoggedInContainer.gameObject.SetActive(false);
					m_facebookLoggedInContainer.gameObject.SetActive(false);
					m_facebookNotLinkedContainer.gameObject.SetActive(true);
				}
				else if (!FacebookManager.IsLoggedIn)
				{
					m_facebookNotLinkedContainer.gameObject.SetActive(false);
					m_facebookLoggedInContainer.gameObject.SetActive(false);
					m_facebookLinkedNotLoggedInContainer.gameObject.SetActive(true);
					m_facebookLinkedNotLoggedInUsername.text = FacebookManager.LinkedFacebookUserName;
				}
				else
				{
					m_facebookNotLinkedContainer.gameObject.SetActive(false);
					m_facebookLinkedNotLoggedInContainer.gameObject.SetActive(false);
					m_facebookLoggedInContainer.gameObject.SetActive(true);
					m_facebookLoggedInUsername.text = FacebookManager.LinkedFacebookUserName;
				}
			}
		}

		private void RefreshGooglePlayElements()
		{
			m_googlePlayLoginAreaContainer.gameObject.SetActive(ClientAchievementsManager.IsGooglePlayAvailable());
			m_googlePlayLogoutAreaContainer.gameObject.SetActive(ClientAchievementsManager.IsGooglePlayAvailable());
			if (ClientAchievementsManager.IsGooglePlayAvailable())
			{
				bool flag = ClientAchievementsManager.IsSignedIn();
				m_googlePlayLoggedInContainer.gameObject.SetActive(flag);
				m_googlePlayNotLoggedInContainer.gameObject.SetActive(!flag);
				m_googlePlayLogoutAreaContainer.gameObject.SetActive(flag);
			}
		}

		private void OnExitScreen()
		{
			UIScreenManager instance = Singleton<UIScreenManager>.Instance;
			if (instance != null && !(instance.SceneData == null))
			{
				UIPlayerCard playerCard = instance.SceneData.PlayerCard;
				if (playerCard != null)
				{
					playerCard.SetVisibilityByScreen();
				}
			}
		}

		public void OnFacebookLoginClicked()
		{
			FacebookManager.Login(delegate
			{
				RefreshFacebookElements();
			});
		}

		public void OnUplayLoginClicked()
		{
		}

		public void OnWeiboLoginClicked()
		{
			WeiboManager.Login(delegate
			{
				RefreshWeiboElements();
			});
		}

		public void OnGooglePlayLoginClicked()
		{
			ClientAchievementsManager.ClientAchievementSignInCallback += GooglePlaySignInCallback;
			ClientAchievementsManager.SignIn();
		}

		public void OnGooglePlayLogoutClicked()
		{
			ShowConfirmSignOutFromGooglePlayPopup();
		}

		private void GooglePlaySignInCallback()
		{
			RefreshGooglePlayElements();
			if (ClientAchievementsManager.IsSignedIn())
			{
				ClientAchievementsManager.ClientAchievementSignInCallback -= GooglePlaySignInCallback;
			}
		}

		private void GooglePlaySignOutCallback()
		{
			RefreshGooglePlayElements();
			if (!ClientAchievementsManager.IsSignedIn())
			{
				ClientAchievementsManager.ClientAchievementSignInCallback -= GooglePlaySignOutCallback;
			}
		}

		public void OnGooglePlayShowAchievements()
		{
			ClientAchievementsManager.ShowAchievements();
		}

		public void OnFacebookChangeUserClicked()
		{
			ChangeUserClicked(ESocialNetwork.Facebook, LocalizationManager.Instance.Get(5839));
		}

		public void OnUplayChangeUserClicked()
		{
			ChangeUserClicked(ESocialNetwork.UPlay, LocalizationManager.Instance.Get(8331));
		}

		public void OnWeiboChangeUserClicked()
		{
			ChangeUserClicked(ESocialNetwork.Weibo, LocalizationManager.Instance.Get(8384));
		}

		public void ChangeUserClicked(ESocialNetwork socialNetwork, string description)
		{
			PopUpConfiguration popUpConfiguration = new PopUpConfiguration();
			popUpConfiguration.ShowCloseButton = true;
			popUpConfiguration.OnCloseButtonClicked = delegate
			{
				Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
			};
			popUpConfiguration.Aligntarget = UIPopup.EContentAlignTarget.CloseButton;
			popUpConfiguration.Heading = LocalizationManager.Instance.Get(5838);
			popUpConfiguration.Buttons = new PopUpButtonConfiguration[2]
			{
				new PopUpButtonConfiguration
				{
					Label = LocalizationManager.Instance.Get(1780),
					OnButtonPressed = delegate
					{
						Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
						if (socialNetwork == ESocialNetwork.Facebook)
						{
							string oldUser2 = FacebookManager.UserId;
							FacebookManager.ShowLoadingInitOverlay();
							FacebookManager.Logout(delegate(bool logoutSuccess, FacebookManager.EFBLogoutFlowResult logoutResult)
							{
								if (!logoutSuccess)
								{
									FacebookManager.HideLoadingOverlay();
								}
								else
								{
									FacebookManager.Login(delegate(bool loginSuccess, FacebookManager.EFBLoginFlowResult loginResult)
									{
										FacebookManager.HideLoadingOverlay();
										RefreshFacebookElements();
										if (loginSuccess && FacebookManager.UserId == oldUser2)
										{
											WarnSameAccount(LocalizationManager.Instance.Get(6455));
										}
									});
								}
							});
						}
						else if (socialNetwork == ESocialNetwork.Weibo)
						{
							string oldUser = WeiboManager.LoggedInWeiboUserId;
							WeiboManager.Logout(delegate(bool logoutSuccess, WeiboManager.EWeiboLogoutResult logoutResult)
							{
								if (logoutSuccess)
								{
									WeiboManager.Login(delegate(bool success, WeiboManager.EWeiboLoginResult result)
									{
										RefreshWeiboElements();
										if (success && WeiboManager.LoggedInWeiboUserId == oldUser)
										{
											WarnSameAccount(LocalizationManager.Instance.Get(8383));
										}
									});
								}
							});
						}
					}
				},
				new PopUpButtonConfiguration
				{
					Label = LocalizationManager.Instance.Get(1781),
					OnButtonPressed = delegate
					{
						Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
					}
				}
			};
			popUpConfiguration.Data = new ErrorContent.ErrorData
			{
				Description = description
			};
			PopUpConfiguration config = popUpConfiguration;
			Singleton<UIScreenManager>.Instance.ShowPopUpScreen(config);
		}

		private void WarnSameAccount(string description)
		{
			PopUpConfiguration popUpConfiguration = new PopUpConfiguration();
			popUpConfiguration.ShowCloseButton = false;
			popUpConfiguration.OnCloseButtonClicked = delegate
			{
			};
			popUpConfiguration.Aligntarget = UIPopup.EContentAlignTarget.CloseButton;
			popUpConfiguration.Heading = LocalizationManager.Instance.Get(5838);
			popUpConfiguration.Buttons = new PopUpButtonConfiguration[1]
			{
				new PopUpButtonConfiguration
				{
					Label = LocalizationManager.Instance.Get(1780),
					OnButtonPressed = delegate
					{
						Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
					}
				}
			};
			popUpConfiguration.Data = new ErrorContent.ErrorData
			{
				Description = description
			};
			PopUpConfiguration config = popUpConfiguration;
			Singleton<UIScreenManager>.Instance.ShowPopUpScreen(config);
		}

		private void ShowConfirmSignOutFromGooglePlayPopup()
		{
			PopUpConfiguration popUpConfiguration = new PopUpConfiguration();
			popUpConfiguration.ShowCloseButton = true;
			popUpConfiguration.OnCloseButtonClicked = delegate
			{
				Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
			};
			popUpConfiguration.Aligntarget = UIPopup.EContentAlignTarget.CloseButton;
			popUpConfiguration.Heading = LocalizationManager.Instance.Get(2162);
			popUpConfiguration.Buttons = new PopUpButtonConfiguration[2]
			{
				new PopUpButtonConfiguration
				{
					Label = LocalizationManager.Instance.Get(9698),
					OnButtonPressed = delegate
					{
						ClientAchievementsManager.ClientAchievementSignOutCallback += GooglePlaySignOutCallback;
						ClientAchievementsManager.SignOut();
						Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
					}
				},
				new PopUpButtonConfiguration
				{
					Label = LocalizationManager.Instance.Get(1781),
					OnButtonPressed = delegate
					{
						Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
					}
				}
			};
			popUpConfiguration.Data = new ErrorContent.ErrorData
			{
				Description = LocalizationManager.Instance.Get(10708)
			};
			PopUpConfiguration config = popUpConfiguration;
			Singleton<UIScreenManager>.Instance.ShowPopUpScreen(config);
		}

		private void ShowForceLinkProfileToOtherSocialNetwork()
		{
			string heading = LocalizationManager.Instance.Get((!FacebookManager.IsAvailable) ? 8385 : 4315);
			PopUpConfiguration popUpConfiguration = new PopUpConfiguration();
			popUpConfiguration.ShowCloseButton = true;
			popUpConfiguration.OnCloseButtonClicked = delegate
			{
				Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
			};
			popUpConfiguration.Aligntarget = UIPopup.EContentAlignTarget.CloseButton;
			popUpConfiguration.Heading = heading;
			popUpConfiguration.Buttons = new PopUpButtonConfiguration[1]
			{
				new PopUpButtonConfiguration
				{
					Label = LocalizationManager.Instance.Get(10312),
					OnButtonPressed = delegate
					{
						Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
						if (WeiboManager.IsWeiboAvailable)
						{
							OnWeiboLoginClicked();
						}
						else
						{
							OnFacebookLoginClicked();
						}
					}
				}
			};
			popUpConfiguration.Data = new ErrorContent.ErrorData
			{
				Description = LocalizationManager.Instance.Get(10760)
			};
			PopUpConfiguration config = popUpConfiguration;
			Singleton<UIScreenManager>.Instance.ShowPopUpScreen(config);
		}
	}
}
