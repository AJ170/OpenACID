using AcierProtocol;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.SkillTreeScreen
{
	public class SecondClassHandler : MonoBehaviour
	{
		[SerializeField]
		private SelectClassHandler[] m_classes;

		[SerializeField]
		private UITable m_table;

		[SerializeField]
		private UISprite m_previewImage;

		private SkillTreeData m_data;

		private SelectClassHandler m_prevSelected;

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
					UpdateItems();
				}
			}
		}

		public Assassin.EClassTypeId SelectedType { get; private set; }

		public void Init(SkillTreeData _data)
		{
			Visible = false;
			m_data = _data;
			for (int i = 0; i < m_classes.Length; i++)
			{
				m_classes[i].Init(_data);
			}
			EventDelegate.Add(m_data.onDataChanged, OnDataChanged);
			m_table.Reposition();
		}

		public void Clear()
		{
			if (m_data != null)
			{
				EventDelegate.Remove(m_data.onDataChanged, OnDataChanged);
				SetPreviewImage(m_data.SecondaryClass);
			}
			else
			{
				SetPreviewImage(Assassin.EClassTypeId.None);
			}
			DisableListeners();
		}

		private void Awake()
		{
		}

		private void OnDestroy()
		{
			m_prevSelected = null;
		}

		private void OnEnable()
		{
			EnableListeners();
		}

		private void OnDisable()
		{
			SetPreviewImage((m_data == null) ? Assassin.EClassTypeId.None : m_data.SecondaryClass);
			DisableListeners();
		}

		private void UpdateItems(bool _dataUpdate = false)
		{
			if (!Visible)
			{
				return;
			}
			Assassin.EClassTypeId primaryClass = m_data.PrimaryClass;
			Assassin.EClassTypeId eClassTypeId = m_data.PreviewClass;
			if (eClassTypeId == primaryClass)
			{
				eClassTypeId = Assassin.EClassTypeId.None;
			}
			if (!_dataUpdate && m_prevSelected != null)
			{
				m_prevSelected.Deactivate(false);
				m_prevSelected = null;
			}
			for (int i = 0; i < m_classes.Length; i++)
			{
				Assassin.EClassTypeId type = m_classes[i].Type;
				if (type != primaryClass)
				{
					if (type == eClassTypeId || eClassTypeId == Assassin.EClassTypeId.None)
					{
						OnActivateInternal(m_classes[i]);
						eClassTypeId = type;
					}
					else
					{
						m_classes[i].Deactivate(false);
					}
				}
			}
			for (int j = 0; j < m_classes.Length; j++)
			{
				m_classes[j].Visible = m_classes[j].Type != primaryClass;
			}
			m_table.Reposition();
			for (int k = 0; k < m_classes.Length; k++)
			{
				if (m_classes[k].Type != primaryClass)
				{
					GameObject gameObject = m_classes[k].gameObject;
					gameObject.SetActive(false);
					gameObject.SetActive(true);
				}
			}
		}

		private void EnableListeners()
		{
			for (int i = 0; i < m_classes.Length; i++)
			{
				EventDelegate.Add(m_classes[i].onActivate, OnActivate);
			}
		}

		private void DisableListeners()
		{
			for (int i = 0; i < m_classes.Length; i++)
			{
				EventDelegate.Remove(m_classes[i].onActivate, OnActivate);
			}
		}

		private void OnActivate()
		{
			bool flag = m_prevSelected.Type != m_data.PreviewClass;
			OnActivateInternal(SelectClassHandler.CurrentActivated);
			if (flag)
			{
				m_table.Reposition();
			}
		}

		private void OnActivateInternal(SelectClassHandler _selectClass)
		{
			if (!_selectClass.Activated)
			{
				if (m_prevSelected != null && m_prevSelected != _selectClass)
				{
					m_prevSelected.Deactivate(true);
				}
				_selectClass.Activate(m_prevSelected != _selectClass);
				SetPreviewImage(_selectClass.Type);
				m_previewImage.alpha = 0.5f;
				m_data.PreviewClass = _selectClass.Type;
				m_prevSelected = _selectClass;
			}
		}

		private void OnDataChanged()
		{
			UpdateItems(true);
		}

		private void SetPreviewImage(Assassin.EClassTypeId _classTypeId)
		{
			switch (_classTypeId)
			{
			default:
				m_previewImage.spriteName = "aUI_img_chrDual_empty";
				break;
			case Assassin.EClassTypeId.Berserker:
				m_previewImage.spriteName = "aUI_img_chrDual_baserker";
				break;
			case Assassin.EClassTypeId.Shadowblade:
				m_previewImage.spriteName = "aUI_img_chrDual_shadowblade";
				break;
			case Assassin.EClassTypeId.Trickster:
				m_previewImage.spriteName = "aUI_img_chrDual_trickster";
				break;
			case Assassin.EClassTypeId.Thief:
				m_previewImage.spriteName = "aUI_img_chrDual_thief";
				break;
			}
		}
	}
}
