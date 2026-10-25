using Assets.Scripts.Database;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.MissionScreen
{
	public class UIMissionMessage : UIMissionMessageBase
	{
		[SerializeField]
		private UIExtendedLabel m_titleLabel;

		[SerializeField]
		private UIExtendedLabel m_textLabel;

		[SerializeField]
		private UITexture m_itemTexture;

		[SerializeField]
		private UnityEngine.Animation m_completedAnimation;

		[SerializeField]
		private BBTweenAnimation m_completedAnimationTween;

		private bool m_shouldInvokeOpenBox;

		private bool m_updateTextureOnUpdate;

		private EntitlementDefinition m_entitlementDef;

		public UnityEngine.Animation CompleteMissionAnimation
		{
			get
			{
				return m_completedAnimation;
			}
		}

		protected override void Awake()
		{
			base.Awake();
		}

		public void SetupAnimation(bool _hasCurrentMissionStatusChanged)
		{
			if (_hasCurrentMissionStatusChanged)
			{
				m_completedAnimationTween.SetAnimationList(new UnityEngine.Animation[1] { m_completedAnimation }, string.Empty);
			}
			else
			{
				m_completedAnimationTween.SetAnimationList(new UnityEngine.Animation[0], string.Empty);
			}
			m_shouldInvokeOpenBox = _hasCurrentMissionStatusChanged;
		}

		public void StopAnimationIfPlaying()
		{
			m_completedAnimationTween.StopAnimationIfPlaying(true);
		}

		public void UpdateDateByEntitlement(EntitlementHighlightDescription _entitlementDesc, bool _wasCompletedAnimationShown, bool _isUnlocked)
		{
			m_entitlementDef = EntitlementExtensions.GetEntitlementByGUID(_entitlementDesc.EntitlementGUID);
			string text = LocalizationManager.Instance.Get(m_entitlementDef.NameLocaKey.Key);
			int num = ((!_isUnlocked) ? 6744 : 7200);
			m_titleLabel.SetLocalizationKey(num, new StringMapper { 
			{
				"unlocked_feature_name",
				new UINGUIFormattedText(text)
			} });
			StringId key = ((!_isUnlocked) ? _entitlementDesc.DescriptionLocked : _entitlementDesc.DescriptionUnlocked);
			m_textLabel.text = LocalizationManager.Instance.Get(key);
			m_updateTextureOnUpdate = true;
			m_itemTexture.gameObject.SetActive(true);
			if (_isUnlocked && _wasCompletedAnimationShown)
			{
				m_completedAnimation.SampleAnimation(1f, string.Empty);
				if (m_shouldInvokeOpenBox)
				{
					OnToggleOpenState();
					m_shouldInvokeOpenBox = false;
				}
			}
			else
			{
				m_completedAnimation.SampleAnimation(0f, string.Empty);
			}
			base.IsVisible = true;
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			UpdateDisplay();
		}

		private void OnDisable()
		{
			m_updateTextureOnUpdate = false;
		}

		private void UpdateDisplay()
		{
			base.IsActivated = true;
		}

		private void Update()
		{
			if (m_updateTextureOnUpdate)
			{
				m_updateTextureOnUpdate = false;
				Updatetexture();
			}
		}

		private void Updatetexture()
		{
			if (m_entitlementDef != null)
			{
				UIItemUtil.UpdateUITexture(m_entitlementDef.Icon, m_itemTexture);
			}
		}
	}
}
