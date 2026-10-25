using System;
using AcierProtocol;
using Assets.Scripts.Audio;
using Assets.Scripts.UI.Animations.ValueObservers;
using Assets.Scripts.UI.Interfaces;
using Assets.Scripts.UI.NGUIExtensions.Tweens;
using Assets.Scripts.UI.Screens.AssassinInventoryScreen;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Development;
using Assets.Scripts.UI.Utils.Extensions;
using Database;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.InventoryScreen
{
	public class UIForgeItemHandler : MonoBehaviour, IInitialized<UIInventoryScreenData>
	{
		public const int OUTCOME_INDEX = -1;

		[SerializeField]
		[CheckAssigned]
		private int m_index;

		[SerializeField]
		[CheckAssigned]
		private UIPortraitTexture m_texture;

		[CheckAssigned]
		[SerializeField]
		private CompareItemHandler m_comparerHandler;

		[SerializeField]
		[CheckAssigned]
		private BBTweenAlpha m_highlightAlpha;

		[CheckAssigned]
		[SerializeField]
		private GameObject m_emptyBackground;

		[SerializeField]
		[CheckAssigned]
		private UIExtendedButton m_button;

		[SerializeField]
		[CheckAssigned]
		private BoxCollider m_collider;

		[SerializeField]
		[CheckAssigned]
		private UIBoolObserver m_updateItemObserver;

		[SerializeField]
		[CheckAssigned]
		private UIBoolObserver m_updateVisibilityObserver;

		[SerializeField]
		[CheckAssigned]
		private UIBoolObserver m_hideObserver;

		[SerializeField]
		[CheckAssigned]
		private UnityEngine.Animation m_animation;

		[CheckAssigned]
		[SerializeField]
		private GameObject m_swipeEffect;

		[SerializeField]
		[CheckAssigned]
		private UIWidget m_contentWidget;

		private bool m_selected;

		private UIInventoryScreenData m_data;

		private InventoryElement m_element;

		private UIWidget m_widget;

		public bool Selected
		{
			get
			{
				return m_selected;
			}
			set
			{
				if (m_selected != value)
				{
					m_selected = value;
					if (value)
					{
						m_highlightAlpha.StartTweenPlayForward();
					}
					else
					{
						m_highlightAlpha.StartTweenPlayReverse();
					}
				}
			}
		}

		public bool Empty
		{
			get
			{
				return m_element == null;
			}
		}

		public InventoryElement Element
		{
			get
			{
				return m_element;
			}
		}

		public bool Changed { get; private set; }

		public bool Visible
		{
			get
			{
				return m_data.IndexVisible(m_index);
			}
		}

		public bool ItemCurrentlyVisible
		{
			get
			{
				return m_texture.gameObject.activeSelf || m_emptyBackground.activeSelf;
			}
		}

		public event Action<UIForgeItemHandler> OnClick;

		public void PrepareVisibility()
		{
			m_emptyBackground.SetActive(false);
			m_comparerHandler.gameObject.SetActive(false);
			m_texture.gameObject.SetActive(false);
			m_animation.SampleAnimation(0f, string.Empty);
			m_widget.alpha = 0f;
			m_contentWidget.alpha = 1f;
		}

		public void Start()
		{
			m_swipeEffect.layer = 5;
		}

		public void Initialize(UIInventoryScreenData _data)
		{
			m_data = _data;
			m_data.OnAddedToForge += OnItemAddedToForge;
			m_data.OnRemovedFromForge += OnItemRemovedFromForge;
			m_data.OnForgeCleared += OnForgeCleared;
			m_data.OnStateChanged += OnStateChanged;
		}

		public void Clear()
		{
			m_data.OnAddedToForge -= OnItemAddedToForge;
			m_data.OnRemovedFromForge -= OnItemRemovedFromForge;
			m_data.OnForgeCleared -= OnForgeCleared;
			m_data.OnStateChanged -= OnStateChanged;
			m_element = null;
			m_data = null;
		}

		private void Awake()
		{
			m_widget = GetComponent<UIWidget>();
		}

		private void OnEnable()
		{
			EventDelegate.Add(m_button.onClick, OnButtonClick);
			m_updateVisibilityObserver.OnActive += UpdateVisibility;
			m_updateItemObserver.OnActive += ActivateItem;
			m_hideObserver.OnActive += OnHideItem;
			m_highlightAlpha.SetTweenToBeginning();
			UpdateDisplay();
		}

		private void OnDisable()
		{
			Changed = false;
			m_element = null;
			if (m_emptyBackground != null)
			{
				m_emptyBackground.SetActive(true);
			}
			if (m_comparerHandler != null)
			{
				m_comparerHandler.gameObject.SetActive(false);
			}
			if (m_texture != null)
			{
				m_texture.gameObject.SetActive(false);
			}
			EventDelegate.Remove(m_button.onClick, OnButtonClick);
			m_updateVisibilityObserver.OnActive -= UpdateVisibility;
			m_updateItemObserver.OnActive -= ActivateItem;
			m_hideObserver.OnActive -= OnHideItem;
			m_highlightAlpha.SetTweenToBeginning();
		}

		private void OnForgeCleared()
		{
			m_element = null;
			UpdateDisplay();
		}

		private void OnButtonClick()
		{
			if (this.OnClick != null)
			{
				this.OnClick(this);
			}
		}

		private void OnItemAddedToForge(InventoryElement _inventoryElement, int _i)
		{
			GameID equipSoundGameIDByItemType = _inventoryElement.GetEquipSoundGameIDByItemType();
			AudioManager.Instance.Play(equipSoundGameIDByItemType);
			if (_inventoryElement.IsBlueprint() && m_index == -1)
			{
				Changed = true;
			}
			if (m_index == -1 && m_data.ForgeSize == 1)
			{
				Changed = true;
			}
			if (_i == m_index)
			{
				m_element = _inventoryElement;
				Changed = true;
			}
		}

		private void OnItemRemovedFromForge(InventoryElement _inventoryElement, int _i)
		{
			GameID equipSoundGameIDByItemType = _inventoryElement.GetEquipSoundGameIDByItemType();
			AudioManager.Instance.Play(equipSoundGameIDByItemType);
			AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_generic_unequip);
			if (_inventoryElement.IsBlueprint() && m_index == -1)
			{
				Changed = true;
			}
			if (_i == m_index)
			{
				Selected = false;
				m_element = null;
				Changed = true;
			}
		}

		private void OnStateChanged(UIInventoryScreenData.StateChangeData _stateChangeData)
		{
			if (_stateChangeData.OldState == UIInventoryScreenData.EScreenState.Forge && _stateChangeData.NewState != UIInventoryScreenData.EScreenState.Forge)
			{
				m_element = null;
			}
		}

		public void UpdateVisibility()
		{
			m_emptyBackground.SetActive(m_data != null && m_data.IndexVisible(m_index));
			m_comparerHandler.gameObject.SetActive(false);
			m_texture.gameObject.SetActive(false);
		}

		private void OnHideItem()
		{
			m_contentWidget.alpha = 0f;
		}

		public void ActivateItem()
		{
			UpdateDisplay();
			Changed = false;
		}

		private void UpdateOutcome()
		{
			if (m_data == null)
			{
				return;
			}
			if (m_data.IsForgeValid)
			{
				m_emptyBackground.SetActive(false);
				m_texture.gameObject.SetActive(true);
				GameID gameID = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_unkownOutcomeGUID;
				if (m_data.IsOutcomeKnown)
				{
					gameID = UbiCore.Utils.Singleton<UIDataMapper>.Instance.ForgeDescriptions[(int)m_data.ForgeOutcomeType].GUID;
				}
				GameID colorGUID = UbiCore.Utils.Singleton<UIDataMapper>.Instance.ItemQualityDescriptions[(int)m_data.ForgeOutcomeQuality].ColorGUID;
				ColorDef colorDefForColorGuid = UIItemUtil.GetColorDefForColorGuid(colorGUID);
				bool isUnknownItem = !m_data.IsOutcomeKnown;
				UIItemUtil.UpdateItemByColorDef(gameID, colorDefForColorGuid.Color0, false, colorDefForColorGuid, m_texture, false, UIItemUtil.ItemUVCoordType.ENone, isUnknownItem, 0f);
			}
			else
			{
				m_emptyBackground.SetActive(true);
				m_texture.gameObject.SetActive(false);
			}
		}

		private void UpdateDisplay()
		{
			if (m_index == -1)
			{
				UpdateOutcome();
			}
			else if (m_element != null)
			{
				m_emptyBackground.SetActive(false);
				m_texture.gameObject.SetActive(true);
				UIItemUtil.UpdateItem(m_element, false, m_texture, false, string.Empty, 0f);
				m_collider.enabled = true;
			}
			else
			{
				m_emptyBackground.SetActive(m_data != null && m_data.IsForgeValid && m_data.IndexVisible(m_index));
				m_texture.gameObject.SetActive(false);
				m_collider.enabled = false;
			}
			CompareItems();
		}

		private void CompareItems()
		{
			if (m_element == null || m_element.IsBlueprint())
			{
				m_comparerHandler.State = CompareItemHandler.EState.Neutral;
				return;
			}
			InventoryElement equipedEquivalent = Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin.GetEquipedEquivalent(m_element);
			switch (m_element.CompareTo(equipedEquivalent))
			{
			case 0:
				m_comparerHandler.State = CompareItemHandler.EState.Neutral;
				break;
			case 1:
				m_comparerHandler.State = CompareItemHandler.EState.Better;
				break;
			case -1:
				m_comparerHandler.State = CompareItemHandler.EState.Worse;
				break;
			}
		}
	}
}
