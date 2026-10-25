using AcierProtocol;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.NGUIExtensions;
using Assets.Scripts.UI.SocialScreen;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.Popup.ThirdPartyAccountPopup
{
	public class ThirdPartyAccountContent : PopUpContentView<ThirdPartyAccountContent.ThirdPartyAccountData>
	{
		public class ThirdPartyAccountData : PopUpContentData
		{
			public enum EThirdPartyType
			{
				Facebook = 0,
				Uplay = 1,
				Weibo = 2
			}

			public bool ShowOnlyLoadProfile;

			public EThirdPartyType ThirdPartyType;

			public string ThirdPartyName;

			public UserProfile ThirdPartyProfile;

			public UserProfile CurrentProfile;

			public override UIPopup.EContentType Type
			{
				get
				{
					return UIPopup.EContentType.ThirdPartyAccount;
				}
			}
		}

		[SerializeField]
		private GameObject m_facebookHeader;

		[SerializeField]
		private GameObject m_uplayHeader;

		[SerializeField]
		private GameObject m_weiboHeader;

		[SerializeField]
		private GameObject m_padding3GameObject;

		[SerializeField]
		private GameObject m_currentProfilePromptGameObject;

		[SerializeField]
		private GameObject m_currentProfileDataGameObject;

		[SerializeField]
		private UIExtendedLabel m_nameFacebookLabel;

		[SerializeField]
		private UIExtendedLabel m_nameUplayLabel;

		[SerializeField]
		private UIExtendedLabel m_nameWeiboLabel;

		[SerializeField]
		private UIExtendedLabel m_newlyLoggedInProfilePromptLabel;

		[SerializeField]
		private UIExtendedLabel m_newlyLoggedInProfileDataLabel;

		[SerializeField]
		private UIExtendedLabel m_currentProfileDataLabel;

		[SerializeField]
		private UIExtendedLabel m_questionPromptLabel;

		[SerializeField]
		private UIAlignmentBox m_vBox;

		private void Awake()
		{
		}

		protected override void Render(ThirdPartyAccountData _data)
		{
			if (_data.ThirdPartyType == ThirdPartyAccountData.EThirdPartyType.Weibo)
			{
				m_newlyLoggedInProfilePromptLabel.text = LocalizationManager.Instance.Get(8381);
				m_nameWeiboLabel.text = _data.ThirdPartyName;
				m_facebookHeader.gameObject.SetActive(false);
				m_uplayHeader.gameObject.SetActive(false);
				m_weiboHeader.gameObject.SetActive(true);
			}
			else if (_data.ThirdPartyType == ThirdPartyAccountData.EThirdPartyType.Uplay)
			{
				m_newlyLoggedInProfilePromptLabel.text = LocalizationManager.Instance.Get(8391);
				m_nameUplayLabel.text = _data.ThirdPartyName;
				m_facebookHeader.gameObject.SetActive(false);
				m_uplayHeader.gameObject.SetActive(true);
				m_weiboHeader.gameObject.SetActive(false);
			}
			else
			{
				m_newlyLoggedInProfilePromptLabel.text = LocalizationManager.Instance.Get(6133);
				m_nameFacebookLabel.text = _data.ThirdPartyName;
				m_facebookHeader.gameObject.SetActive(true);
				m_uplayHeader.gameObject.SetActive(false);
				m_weiboHeader.gameObject.SetActive(false);
			}
			m_newlyLoggedInProfileDataLabel.text = LocalizationManager.Instance.Get(6134, new StringMapper
			{
				{
					"name",
					_data.ThirdPartyProfile.userName
				},
				{
					"id",
					FriendUtil.ToHumanReadableUserHash(_data.ThirdPartyProfile.userIdHash)
				},
				{
					"level",
					_data.ThirdPartyProfile.level
				}
			});
			if (_data.ShowOnlyLoadProfile)
			{
				m_questionPromptLabel.text = LocalizationManager.Instance.Get(8392);
				m_padding3GameObject.gameObject.SetActive(false);
				m_currentProfilePromptGameObject.gameObject.SetActive(false);
				m_currentProfileDataGameObject.gameObject.SetActive(false);
			}
			else
			{
				m_questionPromptLabel.text = LocalizationManager.Instance.Get(6135);
				m_currentProfileDataLabel.text = LocalizationManager.Instance.Get(6134, new StringMapper
				{
					{
						"name",
						_data.CurrentProfile.userName
					},
					{
						"id",
						FriendUtil.ToHumanReadableUserHash(_data.CurrentProfile.userIdHash)
					},
					{
						"level",
						_data.CurrentProfile.level
					}
				});
			}
		}

		public override void UpdateLayout()
		{
			EventDelegate.Add(m_vBox.OnReposition, delegate
			{
				EventDelegate.Execute(onLayoutFinished);
			}, true);
			m_vBox.RepositionNow = true;
		}
	}
}
