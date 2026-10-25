using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.SkillTreeScreen
{
	public class ClassTabHandler : MonoBehaviour
	{
		private const string s_SiloutteSprite = "aUI_img_chrDual_empty";

		private static readonly StringId s_DefaultHeader = 1522;

		private static readonly StringId s_classAvailable = 2117;

		[SerializeField]
		private UISprite m_image;

		[SerializeField]
		private UIExtendedLabel m_classNameLabel;

		[SerializeField]
		private UIExtendedLabel m_pointsLabel;

		[SerializeField]
		private UIExtendedLabel m_unlockLabel;

		[SerializeField]
		private GameObject m_PointObject;

		[SerializeField]
		private UIExtendedLabel m_subHeader;

		public List<EventDelegate> onClick = new List<EventDelegate>();

		private SkillTreeData m_data;

		private bool m_primary;

		public Assassin.EClassTypeId ClassType
		{
			get
			{
				return (!m_primary) ? m_data.SecondaryClass : m_data.PrimaryClass;
			}
		}

		public void Init(SkillTreeData _data, bool _primary)
		{
			m_data = _data;
			m_primary = _primary;
			EventDelegate.Add(m_data.onDataChanged, OnDataUpdated);
			UpdateHandler();
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
		}

		private void OnDisable()
		{
			Clear();
		}

		private void OnClick()
		{
			EventDelegate.Execute(onClick);
		}

		private void DeactivateHandler()
		{
			m_image.gameObject.SetActive(false);
			m_classNameLabel.gameObject.SetActive(false);
			m_pointsLabel.gameObject.SetActive(false);
			m_PointObject.SetActive(false);
			m_subHeader.gameObject.SetActive(false);
			if (m_unlockLabel != null)
			{
				m_unlockLabel.gameObject.SetActive(false);
			}
		}

		private void OnDataUpdated()
		{
			UpdateHandler();
		}

		private void UpdateHandler()
		{
			DeactivateHandler();
			if (m_primary)
			{
				UpdatePrimaryClass();
			}
			else
			{
				UpdateSecondaryClass();
			}
		}

		private void UpdatePrimaryClass()
		{
			UpdateWithClass(m_data.PrimaryClass);
			m_pointsLabel.text = LocalizationManager.Instance.GetInteger(m_data.Assassin.PointSpent(true));
			m_pointsLabel.gameObject.SetActive(true);
			m_PointObject.SetActive(true);
			m_subHeader.gameObject.SetActive(true);
		}

		private void UpdateSecondaryClass()
		{
			if (m_data.SecondaryClass != Assassin.EClassTypeId.None)
			{
				UpdateWithClass(m_data.SecondaryClass);
				m_pointsLabel.text = LocalizationManager.Instance.GetInteger(m_data.Assassin.PointSpent(false));
				m_pointsLabel.gameObject.SetActive(true);
				m_PointObject.SetActive(true);
				m_subHeader.gameObject.SetActive(true);
				m_image.color = Color.white;
			}
			else
			{
				m_image.spriteName = "aUI_img_chrDual_empty";
				m_image.gameObject.SetActive(true);
				m_image.alpha = 0.5f;
				m_classNameLabel.text = LocalizationManager.Instance.Get(s_DefaultHeader);
				m_classNameLabel.gameObject.SetActive(true);
				m_unlockLabel.text = LocalizationManager.Instance.Get(s_classAvailable);
				m_unlockLabel.gameObject.SetActive(true);
			}
		}

		private void UpdateWithClass(Assassin.EClassTypeId _type)
		{
			m_image.spriteName = UbiCore.Utils.Singleton<UIDataMapper>.Instance.AssassinClassDescriptions[(int)_type].TrainingTab;
			m_classNameLabel.text = LocalizationManager.Instance.Get(UbiCore.Utils.Singleton<UIDataMapper>.Instance.AssassinClassDescriptions[(int)_type].NameId);
			m_image.gameObject.SetActive(true);
			m_classNameLabel.gameObject.SetActive(true);
		}
	}
}
