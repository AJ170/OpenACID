using System;
using AcierProtocol;
using Assets.Scripts.Database;
using Assets.Scripts.UI.Behaviours;
using Assets.Scripts.UI.MVC.Controllers.Misc;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.ScreenManager.SceenPlaceholder;
using Assets.Scripts.UI.Utils.Development;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.Controllers.Assassin
{
	public class UIAssassinEquipmentController : UIControllerBase
	{
		[CheckAssigned]
		[SerializeField]
		private UIAssassinModulePreviewController m_assassinModulePreview;

		private UIRewardItemController m_currentSelected;

		private UIAssassinEquipmentGearController[] m_equipmentSlots;

		public UIAssassinEquipmentGearController[] EquipmentSlots
		{
			get
			{
				if (m_equipmentSlots == null)
				{
					EquipmentSlots = GetComponentsInChildren<UIAssassinEquipmentGearController>(true);
				}
				return m_equipmentSlots;
			}
			set
			{
				if (m_equipmentSlots != null)
				{
					UpdateListeners(false);
				}
				m_equipmentSlots = value;
				UpdateListeners(true);
			}
		}

		public void InitializeController(AcierProtocol.Assassin _assassin)
		{
			m_currentSelected = null;
			for (int i = 0; i < EquipmentSlots.Length; i++)
			{
				EquipmentSlots[i].Initialize(_assassin, false);
			}
			CharacterAppearance appearance = new CharacterAppearance(_assassin);
			UIAssassinRankDisplay.AssassinHandle assassinHandle = new UIAssassinRankDisplay.AssassinHandle();
			assassinHandle.Appearance = appearance;
			assassinHandle.RawAssassin = _assassin;
			assassinHandle.IsPlaceHolder = false;
			UIAssassinRankDisplay.AssassinHandle assassinHandle2 = assassinHandle;
			UIPlaceholder placeholder = Singleton<UIScreenManager>.Instance.SceneData.Placeholders.GetPlaceholder(UIPlaceholders.EPlaceholderType.AssassinPreview);
			m_assassinModulePreview.UpdateController(assassinHandle2, false, placeholder);
			base.IsInitialized = true;
			Show();
		}

		public void OnClosePopup()
		{
			m_assassinModulePreview.FadeAvatar(false, 0.1f);
			if (m_currentSelected != null)
			{
				m_currentSelected.IsHighlighted = false;
				m_currentSelected.Close();
			}
		}

		protected override void OnDisableUnity()
		{
			base.OnDisableUnity();
			OnClosePopup();
		}

		protected override void OnDestroyUnity()
		{
			base.OnDestroyUnity();
			EquipmentSlots = null;
		}

		private void Clear()
		{
			EquipmentSlots = null;
			m_assassinModulePreview.FadeAvatar(true, 0.1f);
		}

		private void UpdateListeners(bool _addListeners)
		{
			if (EquipmentSlots == null)
			{
				return;
			}
			for (int i = 0; i < EquipmentSlots.Length; i++)
			{
				if (_addListeners)
				{
					UIAssassinEquipmentGearController obj = EquipmentSlots[i];
					obj.OnClicked = (Action<UIRewardItemController>)Delegate.Combine(obj.OnClicked, new Action<UIRewardItemController>(OnEquipmentClicked));
				}
				else
				{
					UIAssassinEquipmentGearController obj2 = EquipmentSlots[i];
					obj2.OnClicked = (Action<UIRewardItemController>)Delegate.Remove(obj2.OnClicked, new Action<UIRewardItemController>(OnEquipmentClicked));
				}
			}
		}

		private void OnEquipmentClicked(UIRewardItemController _controller)
		{
			if (m_currentSelected != null && m_currentSelected != _controller)
			{
				m_currentSelected.IsHighlighted = false;
			}
			m_currentSelected = _controller;
		}
	}
}
