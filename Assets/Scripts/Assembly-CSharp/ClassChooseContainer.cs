using AcierProtocol;
using Assets.Scripts.UI.Helpers;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Utils;
using UbiCore.Utils;
using UnityEngine;

public class ClassChooseContainer : MonoBehaviour
{
	private const int k_lockAnchorValue = 41;

	[SerializeField]
	private TweenScale m_tweenScale;

	[SerializeField]
	private UISprite m_highLight;

	[SerializeField]
	private BoxCollider m_activateBoxCollider;

	[SerializeField]
	private GameObject m_chooseButton;

	[SerializeField]
	private GameObject m_bigChooseButton;

	[SerializeField]
	private Assassin.EClassTypeId m_classType;

	[SerializeField]
	private UISprite m_darkTint;

	[SerializeField]
	private UISprite m_lockGraphic;

	[SerializeField]
	private UIExtendedLabel m_className;

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

	private bool m_isLocked;

	public UIEventTrigger ActivateTabTrigger;

	public bool Active { get; private set; }

	public void SetBlocked(bool _value)
	{
		m_chooseButton.SetActive(false);
		m_bigChooseButton.SetActive(false);
		if (!_value)
		{
			m_darkTint.gameObject.SetActive(m_isLocked);
			m_activateBoxCollider.enabled = true;
		}
		else
		{
			m_darkTint.gameObject.SetActive(true);
			m_activateBoxCollider.enabled = false;
		}
	}

	public void SetStaticViewing()
	{
		m_darkTint.gameObject.SetActive(false);
		m_activateBoxCollider.enabled = false;
		m_chooseButton.SetActive(false);
		m_bigChooseButton.SetActive(false);
	}

	private void Awake()
	{
	}

	private void Start()
	{
		UpdateLockedStat();
	}

	public void Init()
	{
		Init(m_classType);
	}

	public void Init(Assassin.EClassTypeId _classType)
	{
		m_classType = _classType;
		ClassDescription classDescription = UbiCore.Utils.Singleton<UIDataMapper>.Instance.AssassinClassDescriptions[(int)m_classType];
		m_className.text = LocalizationManager.Instance.Get(classDescription.NameId);
		m_description1.text = LocalizationManager.Instance.Get(classDescription.DescriptionId[0]);
		m_description2.text = LocalizationManager.Instance.Get(classDescription.DescriptionId[1]);
		m_description3.text = LocalizationManager.Instance.Get(classDescription.DescriptionId[2]);
		m_classImage.spriteName = classDescription.AvatarCreation;
		UpdateLockedStat();
	}

	public void GetActivated()
	{
		m_chooseButton.SetActive(!m_isLocked);
		m_highLight.alpha = 1f;
		m_tweenScale.from = Vector3.one;
		m_tweenScale.to = new Vector3(1.1f, 1.1f, 0f);
		m_tweenScale.ResetToBeginning();
		m_tweenScale.PlayForward();
		m_activateBoxCollider.enabled = false;
		Active = true;
	}

	public void GetDeActivated(bool _playAnimation = true)
	{
		m_chooseButton.SetActive(false);
		m_highLight.alpha = 0f;
		if (_playAnimation)
		{
			if (Active)
			{
				m_tweenScale.from = new Vector3(1.1f, 1.1f, 1f);
				m_tweenScale.to = Vector3.one;
				m_tweenScale.ResetToBeginning();
				m_tweenScale.PlayForward();
			}
		}
		else
		{
			m_tweenScale.from = Vector3.one;
			if (!Active)
			{
				m_tweenScale.value = Vector3.one;
			}
		}
		Active = false;
		m_activateBoxCollider.enabled = true;
	}

	public void UpdateLockedStat()
	{
		ClassEntitlementDescription _desc;
		m_isLocked = ClassLockedStateHelper.GetClassTypeLocked(m_classType, out _desc);
		if (m_isLocked)
		{
			string text = LocalizationManager.Instance.Get(_desc.DescriptionUnlocked);
			m_lockGraphic.rightAnchor.absolute = 41;
			m_lockGraphic.gameObject.SetActive(true);
			m_darkTint.gameObject.SetActive(true);
			m_chooseButton.SetActive(false);
			m_comingSoonLabel.gameObject.SetActive(true);
			m_comingSoonLabel.text = text;
			m_description1.gameObject.SetActive(false);
			m_description2.gameObject.SetActive(false);
			m_description3.gameObject.SetActive(false);
		}
		else
		{
			m_lockGraphic.rightAnchor.absolute = 0;
			m_lockGraphic.gameObject.SetActive(false);
			m_darkTint.gameObject.SetActive(false);
			m_comingSoonLabel.gameObject.SetActive(false);
			m_description1.gameObject.SetActive(true);
			m_description2.gameObject.SetActive(true);
			m_description3.gameObject.SetActive(true);
		}
	}
}
