using System;
using AcierProtocol;
using Assets.Scripts.Audio;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.UI.Interfaces;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.NGUIExtensions.Tweens;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Types;
using Assets.Scripts.UI.Utils.Development;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Widget
{
	public class AssassinStatsTab : MonoBehaviour, IInitialized<AssassinStatsTab.AssassinStatData>
	{
		public sealed class AssassinStatValue
		{
			public float NormalValue;

			public float BuffedValue;
		}

		public sealed class AssassinStatData
		{
			public delegate AssassinStatValue Getvalue(AvatarData.IAvatarDefinition _avatarDefinition, Assassin _assassin);

			public EAssassinStat Stat;

			public StringId Name;

			public StringId Description;

			public bool Primary;

			public string IconName;

			public bool Factor;

			public Getvalue GetStatsValue;
		}

		private const string k_openTrack = "Open";

		private const string k_closeTrack = "Close";

		private const float k_alphaClosed = 0f;

		private const float k_alphaOpened = 1f;

		private static readonly StringId s_valueId = new StringId(2059);

		private static readonly Vector3 s_scaleClosed = new Vector3(1f, 0f, 1f);

		private static readonly Vector3 s_scaleOpened = new Vector3(1f, 1f, 1f);

		[CheckAssigned]
		[SerializeField]
		private UIExtendedButton m_button;

		[SerializeField]
		[CheckAssigned]
		private UnityEngine.Animation m_animation;

		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_nameLabel;

		[SerializeField]
		[CheckAssigned]
		private UIExtendedLabel m_descriptionLabel;

		[SerializeField]
		[CheckAssigned]
		private UIExtendedLabel m_valueLabel;

		[CheckAssigned]
		[SerializeField]
		private UISprite m_iconSprite;

		[CheckAssigned]
		[SerializeField]
		private BBTweenAlpha m_alpha;

		[SerializeField]
		[CheckAssigned]
		private BBTweenScale m_scale;

		private AssassinStatData m_data;

		public event Action<AssassinStatsTab> OnTabClick;

		public void Initialize(AssassinStatData _data)
		{
			m_data = _data;
			UpdateData(_data);
		}

		public void Clear()
		{
			m_data = null;
		}

		public void Select(bool _selected, bool _instant = false)
		{
			m_alpha.from = ((!_selected) ? 1f : 0f);
			m_alpha.to = ((!_selected) ? 0f : 1f);
			m_scale.from = ((!_selected) ? s_scaleOpened : s_scaleClosed);
			m_scale.to = ((!_selected) ? s_scaleClosed : s_scaleOpened);
			if (_instant)
			{
				m_alpha.SetTweenToEnd();
				m_scale.SetTweenToEnd();
			}
			else
			{
				AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_menu_global_whoosh_small);
				m_animation.Play((!_selected) ? "Close" : "Open");
			}
		}

		public void Refresh(AvatarData.IAvatarDefinition _avatarDefinition)
		{
			UpdateValue(_avatarDefinition);
		}

		private void Awake()
		{
			m_iconSprite.atlas = Singleton<UIScreenManager>.Instance.GetAtlasByType(UIScreenManager.EAtlasType.Animus);
		}

		private void OnEnable()
		{
			EventDelegate.Add(m_button.onClick, OnButtonClicked);
		}

		private void OnDisable()
		{
			EventDelegate.Remove(m_button.onClick, OnButtonClicked);
		}

		private void OnButtonClicked()
		{
			if (this.OnTabClick != null)
			{
				this.OnTabClick(this);
			}
		}

		private void UpdateData(AssassinStatData _data)
		{
			m_nameLabel.LocalizationKey = _data.Name;
			m_descriptionLabel.LocalizationKey = _data.Description;
			UpdateIcon(_data);
		}

		private void UpdateIcon(AssassinStatData _data)
		{
			m_iconSprite.enabled = !string.IsNullOrEmpty(_data.IconName);
			m_iconSprite.spriteName = _data.IconName;
			m_iconSprite.MakePixelPerfect();
		}

		private void UpdateValue(AvatarData.IAvatarDefinition _avatarDefinition)
		{
			if (m_data.GetStatsValue != null)
			{
				AssassinStatValue assassinStatValue = m_data.GetStatsValue(_avatarDefinition, Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin);
				m_valueLabel.SetLocalizationKey(s_valueId, new StringMapper { 
				{
					"value",
					new UIStatValue(m_data.Factor, assassinStatValue.NormalValue, assassinStatValue.BuffedValue)
				} });
			}
			else
			{
				m_valueLabel.text = string.Empty;
			}
		}
	}
}
