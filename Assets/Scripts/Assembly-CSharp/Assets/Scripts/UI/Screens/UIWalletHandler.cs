using System;
using AcierProtocol;
using Assets.Scripts.Audio;
using Assets.Scripts.UI.Behaviours.Buttons.Headers;
using Assets.Scripts.UI.NGUIExtensions;
using Assets.Scripts.UI.NGUIExtensions.Tweens;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.ProfileManager;
using Assets.Scripts.UI.Utils.Extensions;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens
{
	public class UIWalletHandler : MonoBehaviour
	{
		[SerializeField]
		private UIExtendedLabel m_hardCurrencyLabel;

		[SerializeField]
		private UIExtendedLabel m_softCurrencyLabel;

		[SerializeField]
		private UIExtendedLabel m_socialCurrencyLabel;

		[SerializeField]
		private UISprite m_hardIcon;

		[SerializeField]
		private UISprite m_softIcon;

		[SerializeField]
		private UISprite m_socialIcon;

		[SerializeField]
		private GameObject m_hardCurrencyContainer;

		[SerializeField]
		private UIExtendedButton m_toShopButton;

		[SerializeField]
		private UIButtonHeaderGoToScreen m_goToScreenHeaderButtonHandler;

		[SerializeField]
		private UIAlignmentBox m_mainContainer;

		private bool m_profileSyncEnabled;

		private long m_lastHardCurrencyCount;

		private long m_lastSoftCurrencyCount;

		private long m_lastSocialCurrencyCount;

		private long[] m_virtualAmounts = new long[3];

		private bool m_isDirectPurchase;

		public UISprite HardIcon
		{
			get
			{
				return m_hardIcon;
			}
		}

		public UISprite SoftIcon
		{
			get
			{
				return m_softIcon;
			}
		}

		public UISprite SocialIcon
		{
			get
			{
				return m_socialIcon;
			}
		}

		public int TweenWaitForTweensCounter { get; private set; }

		public bool ProfileSyncEnabled
		{
			get
			{
				return m_profileSyncEnabled;
			}
			set
			{
				if (m_profileSyncEnabled != value)
				{
					m_profileSyncEnabled = value;
					if (m_profileSyncEnabled)
					{
						Singleton<StateManager>.Instance.ProfileManager.OnProfileUpdate.Add(OnProfileUpdate);
						ProfileManager profileManager = Singleton<StateManager>.Instance.ProfileManager;
						profileManager.onDirectPurchasedVerifyed = (Action<string>)Delegate.Combine(profileManager.onDirectPurchasedVerifyed, new Action<string>(OnDirectPurchasedVerifyed));
						OnProfileUpdate();
					}
					else if (Singleton<StateManager>.Instance.IsProfileManagerInitialized)
					{
						Singleton<StateManager>.Instance.ProfileManager.OnProfileUpdate.Remove(OnProfileUpdate);
						ProfileManager profileManager2 = Singleton<StateManager>.Instance.ProfileManager;
						profileManager2.onDirectPurchasedVerifyed = (Action<string>)Delegate.Remove(profileManager2.onDirectPurchasedVerifyed, new Action<string>(OnDirectPurchasedVerifyed));
					}
				}
			}
		}

		public bool ShowEventButton
		{
			get
			{
				return m_goToScreenHeaderButtonHandler.gameObject.activeSelf;
			}
			set
			{
				m_goToScreenHeaderButtonHandler.gameObject.SetActive(value);
				m_goToScreenHeaderButtonHandler.IsEnabled = value;
				m_mainContainer.RepositionNow = true;
			}
		}

		private void OnDirectPurchasedVerifyed(string _obj)
		{
			m_isDirectPurchase = true;
		}

		public void ForceUpdateResourceType(EResourceType _type)
		{
			UserProfile profile = Singleton<StateManager>.Instance.ProfileManager.Profile;
			SetCurrencyText(_type, profile.GetResourceAmountByType(_type));
		}

		public void SetupTweenCurrencyCallback(EResourceType _type, long _amount, EventDelegate.Callback _animationFinished)
		{
			TweenWaitForTweensCounter++;
			TweenSetCurrency(_type, _amount, _animationFinished);
		}

		public long GetVirtualAddedAmount(EResourceType _type)
		{
			return m_virtualAmounts[GetVirtualAmountIndex(_type)];
		}

		public void SetVirtualAddedAmount(EResourceType _type, long _amount)
		{
			m_virtualAmounts[GetVirtualAmountIndex(_type)] = _amount;
		}

		public void ResetVirtualAddedAmounts()
		{
			for (int i = 0; i < m_virtualAmounts.Length; i++)
			{
				m_virtualAmounts[i] = 0L;
			}
		}

		public void SubtractAmount(EResourceType _type, long _amount)
		{
			switch (_type)
			{
			case EResourceType.HardCurrency:
			{
				long abbreviatedNumberIfNotFit3 = m_lastHardCurrencyCount - _amount;
				m_hardCurrencyLabel.SetAbbreviatedNumberIfNotFit(abbreviatedNumberIfNotFit3);
				break;
			}
			case EResourceType.SoftCurrency:
			{
				long abbreviatedNumberIfNotFit2 = m_lastSoftCurrencyCount - _amount;
				m_softCurrencyLabel.SetAbbreviatedNumberIfNotFit(abbreviatedNumberIfNotFit2);
				break;
			}
			case EResourceType.SocialCurrency:
			{
				long abbreviatedNumberIfNotFit = m_lastSocialCurrencyCount - _amount;
				m_socialCurrencyLabel.SetAbbreviatedNumberIfNotFit(abbreviatedNumberIfNotFit);
				break;
			}
			}
		}

		private void Start()
		{
			TweenWaitForTweensCounter = 0;
			m_hardCurrencyContainer.SetActive(Singleton<StateManager>.Instance.TemplateManager.ShowHCInWallet);
			ProfileSyncEnabled = true;
		}

		private void OnDestroy()
		{
			ProfileSyncEnabled = false;
		}

		private void OnProfileUpdate(params object[] _params)
		{
			UserProfile profile = Singleton<StateManager>.Instance.ProfileManager.Profile;
			if (profile != null)
			{
				m_lastHardCurrencyCount = profile.GetResourceAmountByType(EResourceType.HardCurrency);
				m_lastSoftCurrencyCount = profile.GetResourceAmountByType(EResourceType.SoftCurrency);
				m_lastSocialCurrencyCount = profile.GetResourceAmountByType(EResourceType.SocialCurrency);
				SetCurrencyText(EResourceType.HardCurrency, m_lastHardCurrencyCount);
				SetCurrencyText(EResourceType.SoftCurrency, m_lastSoftCurrencyCount);
				SetCurrencyText(EResourceType.SocialCurrency, m_lastSocialCurrencyCount);
				m_toShopButton.isEnabled = Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlement(StaticEntitlementIds.ShopTabEntitlementGUID);
			}
			m_mainContainer.RepositionNow = true;
		}

		private int GetVirtualAmountIndex(EResourceType _type)
		{
			return (int)(_type - 1);
		}

		private void SetCurrencyText(EResourceType _type, long _amount)
		{
			long abbreviatedNumberIfNotFit = _amount + m_virtualAmounts[GetVirtualAmountIndex(_type)];
			switch (_type)
			{
			case EResourceType.HardCurrency:
				m_hardCurrencyLabel.SetAbbreviatedNumberIfNotFit(abbreviatedNumberIfNotFit);
				break;
			case EResourceType.SoftCurrency:
				m_softCurrencyLabel.SetAbbreviatedNumberIfNotFit(abbreviatedNumberIfNotFit);
				break;
			case EResourceType.SocialCurrency:
				m_socialCurrencyLabel.SetAbbreviatedNumberIfNotFit(abbreviatedNumberIfNotFit);
				break;
			}
		}

		private void TweenSetCurrency(EResourceType _resource, long _increaseAmount, EventDelegate.Callback _onFinished = null)
		{
			UIExtendedLabel uIExtendedLabel = null;
			long num = 0L;
			switch (_resource)
			{
			case EResourceType.HardCurrency:
				uIExtendedLabel = m_hardCurrencyLabel;
				num = m_lastHardCurrencyCount;
				m_lastHardCurrencyCount += _increaseAmount;
				if (_increaseAmount > 0)
				{
					AudioManager.Instance.Play((!m_isDirectPurchase) ? UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_generic_claim : UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_currency_purchase);
				}
				m_isDirectPurchase = false;
				break;
			case EResourceType.SoftCurrency:
				uIExtendedLabel = m_softCurrencyLabel;
				num = m_lastSoftCurrencyCount;
				m_lastSoftCurrencyCount += _increaseAmount;
				break;
			case EResourceType.SocialCurrency:
				uIExtendedLabel = m_socialCurrencyLabel;
				num = m_lastSocialCurrencyCount;
				m_lastSocialCurrencyCount += _increaseAmount;
				break;
			}
			if (uIExtendedLabel != null)
			{
				BBTweenTickingCounterText bBTweenTickingCounterText = uIExtendedLabel.gameObject.GetComponent<BBTweenTickingCounterText>();
				if (bBTweenTickingCounterText == null)
				{
					bBTweenTickingCounterText = uIExtendedLabel.gameObject.AddComponent<BBTweenTickingCounterText>();
					bBTweenTickingCounterText.enabled = false;
				}
				if (bBTweenTickingCounterText.enabled)
				{
					bBTweenTickingCounterText.FinishTween();
					bBTweenTickingCounterText.ExecuteOnFinishCallbacks();
				}
				bBTweenTickingCounterText.enabled = false;
				float num2 = 1.2f;
				float num3 = Mathf.Abs(num + _increaseAmount - num);
				if (num3 < 200f)
				{
					num2 *= num3 / 200f;
					num2 = Mathf.Max(0.1f, num2);
				}
				bBTweenTickingCounterText.duration = num2;
				bBTweenTickingCounterText.method = UITweener.Method.EaseInOut;
				bBTweenTickingCounterText.from = num;
				bBTweenTickingCounterText.to = num + _increaseAmount;
				if (_onFinished != null)
				{
					EventDelegate.Add(bBTweenTickingCounterText.onFinished, _onFinished, true);
				}
				EventDelegate.Add(bBTweenTickingCounterText.onFinished, OnCurrencyTweenFinished, true);
				bBTweenTickingCounterText.StartTweenPlayForward();
			}
		}

		private void OnCurrencyTweenFinished()
		{
			TweenWaitForTweensCounter--;
			if (TweenWaitForTweensCounter <= 0)
			{
				ProfileSyncEnabled = true;
			}
		}

		public void RegisterWaitForExternalAnimation()
		{
			TweenWaitForTweensCounter++;
		}

		public void UnregisterWaitForExternalAnimation()
		{
			TweenWaitForTweensCounter--;
		}
	}
}
