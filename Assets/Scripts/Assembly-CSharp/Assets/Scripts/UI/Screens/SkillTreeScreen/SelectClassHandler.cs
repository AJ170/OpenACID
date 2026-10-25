using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.UI.Helpers;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Utils;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.SkillTreeScreen
{
	public class SelectClassHandler : MonoBehaviour
	{
		public static SelectClassHandler CurrentActivated;

		public List<EventDelegate> onActivate = new List<EventDelegate>();

		[SerializeField]
		private UIExtendedLabel m_heading;

		[SerializeField]
		private UIExtendedLabel m_description1;

		[SerializeField]
		private UIExtendedLabel m_description2;

		[SerializeField]
		private UIExtendedLabel m_description3;

		[SerializeField]
		private UIExtendedLabel m_comingSoonLabel;

		[SerializeField]
		private UISprite m_classImage;

		[SerializeField]
		private Assassin.EClassTypeId m_type;

		[SerializeField]
		private TweenScale m_scaleTween;

		[SerializeField]
		private TweenAlpha m_highLight;

		[SerializeField]
		private BoxCollider m_boxCollider;

		[SerializeField]
		private UIEventTrigger m_activationTrigger;

		private UIExtendedLabel[] m_descriptions;

		private SkillTreeData m_data;

		private bool m_isLocked;

		public bool Visible
		{
			get
			{
				return base.gameObject.activeSelf;
			}
			set
			{
				base.gameObject.SetActive(value);
				if (value)
				{
					EnableListeners();
				}
				else
				{
					DisableListeners();
				}
			}
		}

		public Assassin.EClassTypeId Type
		{
			get
			{
				return m_type;
			}
		}

		public bool Activated { get; private set; }

		public void Init(SkillTreeData _data)
		{
			m_data = _data;
			OnDataUpdated();
			EventDelegate.Add(m_data.onDataChanged, OnDataUpdated);
		}

		public void Clear()
		{
			if (m_data != null)
			{
				EventDelegate.Remove(m_data.onDataChanged, OnDataUpdated);
			}
		}

		private void Awake()
		{
			m_descriptions = new UIExtendedLabel[3] { m_description1, m_description2, m_description3 };
		}

		private void Start()
		{
			m_classImage.spriteName = UbiCore.Utils.Singleton<UIDataMapper>.Instance.AssassinClassDescriptions[(int)m_type].AvatarCreation;
			m_heading.LocalizationKey = UbiCore.Utils.Singleton<UIDataMapper>.Instance.AssassinClassDescriptions[(int)m_type].NameId;
			SetDesc();
			ClassEntitlementDescription _desc;
			m_isLocked = ClassLockedStateHelper.GetClassTypeLocked(m_type, out _desc);
			UpdateLockedStatus();
		}

		private void OnEnable()
		{
			EnableListeners();
			OnDataUpdated();
		}

		private void OnDisable()
		{
			DisableListeners();
		}

		private void OnDestroy()
		{
			Clear();
			DisableListeners();
		}

		private void SetDesc()
		{
			for (int i = 0; i < m_descriptions.Length; i++)
			{
				m_descriptions[i].LocalizationKey = UbiCore.Utils.Singleton<UIDataMapper>.Instance.AssassinClassDescriptions[(int)m_type].DescriptionId[i];
			}
		}

		private void OnDataUpdated()
		{
			if (m_data != null)
			{
				Visible = m_data.PrimaryClass != m_type;
				ClassEntitlementDescription _desc;
				m_isLocked = ClassLockedStateHelper.GetClassTypeLocked(m_type, out _desc);
				m_boxCollider.enabled = !m_isLocked;
				m_comingSoonLabel.text = LocalizationManager.Instance.Get(_desc.DescriptionUnlocked);
				UpdateLockedStatus();
			}
		}

		private void EnableListeners()
		{
			EventDelegate.Add(m_activationTrigger.onClick, OnTabActivate);
		}

		private void DisableListeners()
		{
			EventDelegate.Remove(m_activationTrigger.onClick, OnTabActivate);
		}

		private void OnTabActivate()
		{
			CurrentActivated = this;
			EventDelegate.Execute(onActivate);
			CurrentActivated = null;
		}

		public void Activate(bool _animate)
		{
			m_highLight.from = 0f;
			m_highLight.to = 1f;
			if (_animate)
			{
				m_scaleTween.from = Vector3.one;
				m_scaleTween.to = new Vector3(1.1f, 1.1f, 1.1f);
				m_scaleTween.duration = 0.4f;
				m_scaleTween.ResetToBeginning();
				m_scaleTween.PlayForward();
				m_highLight.ResetToBeginning();
				m_highLight.PlayForward();
			}
			else
			{
				base.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
				m_highLight.value = 1f;
			}
			Activated = !m_isLocked;
		}

		public void Deactivate(bool _animate)
		{
			m_highLight.from = 1f;
			m_highLight.to = 0f;
			if (_animate)
			{
				m_scaleTween.from = new Vector3(1.1f, 1.1f, 1.1f);
				m_scaleTween.to = Vector3.one;
				m_scaleTween.duration = 0.4f;
				m_scaleTween.ResetToBeginning();
				m_scaleTween.PlayForward();
				m_highLight.ResetToBeginning();
				m_highLight.PlayForward();
			}
			else
			{
				base.transform.localScale = Vector3.one;
				m_highLight.value = 0f;
			}
			m_boxCollider.enabled = !m_isLocked;
			Activated = false;
		}

		public void UpdateLockedStatus()
		{
			m_comingSoonLabel.gameObject.SetActive(m_isLocked);
			m_description1.gameObject.SetActive(!m_isLocked);
			m_description2.gameObject.SetActive(!m_isLocked);
			m_description3.gameObject.SetActive(!m_isLocked);
		}
	}
}
