using System;
using Assets.Scripts.UI.MVC.Models.Assassin;
using Assets.Scripts.UI.Utils.Development;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.Controllers.Assassin
{
	public class UIAssassinPortraitSelectableListController : UIControllerBase
	{
		[CheckAssigned]
		[SerializeField]
		private UIAssassinPortraitSelectableController[] m_assassinPortraitController;

		public Action<UIAssassinBaseModel> OnSelectionChanged;

		private UIAssassinPortraitSelectableController m_currentSelectedPortrait;

		public UIAssassinBaseModel CurrentSelectedModel
		{
			get
			{
				return (!(m_currentSelectedPortrait != null)) ? null : m_currentSelectedPortrait.Model;
			}
		}

		public void UpdatePortraitList(UIAssassinBaseModel[] _assassinModels)
		{
			for (int i = 0; i < m_assassinPortraitController.Length; i++)
			{
				UIAssassinPortraitSelectableController uIAssassinPortraitSelectableController = m_assassinPortraitController[i];
				bool isSelected = m_currentSelectedPortrait == uIAssassinPortraitSelectableController;
				uIAssassinPortraitSelectableController.UpdatePortrait(_assassinModels[i], isSelected);
			}
		}

		public void SelectPortraitInstantlyByIndex(int _index)
		{
			if (m_assassinPortraitController.Length > _index)
			{
				UIAssassinPortraitSelectableController uIAssassinPortraitSelectableController = m_assassinPortraitController[_index];
				uIAssassinPortraitSelectableController.SetSelectedIntantly(true);
				OnPortraitSelectionChanged(uIAssassinPortraitSelectableController);
			}
		}

		protected override void OnStartUnity()
		{
			base.OnStartUnity();
			for (int i = 0; i < m_assassinPortraitController.Length; i++)
			{
				UIAssassinPortraitSelectableController obj = m_assassinPortraitController[i];
				obj.OnPortraitClicked = (Action<UIAssassinPortraitSelectableController>)Delegate.Combine(obj.OnPortraitClicked, new Action<UIAssassinPortraitSelectableController>(OnPortraitSelectionChanged));
			}
		}

		protected override void OnDestroyUnity()
		{
			base.OnDestroyUnity();
			if (m_assassinPortraitController == null)
			{
				return;
			}
			for (int i = 0; i < m_assassinPortraitController.Length; i++)
			{
				if (m_assassinPortraitController[i] != null)
				{
					UIAssassinPortraitSelectableController obj = m_assassinPortraitController[i];
					obj.OnPortraitClicked = (Action<UIAssassinPortraitSelectableController>)Delegate.Remove(obj.OnPortraitClicked, new Action<UIAssassinPortraitSelectableController>(OnPortraitSelectionChanged));
				}
			}
		}

		protected override void OnUpdateUnity()
		{
			base.OnUpdateUnity();
			AlignElements();
		}

		private void OnPortraitSelectionChanged(UIAssassinPortraitSelectableController _selected)
		{
			if (m_currentSelectedPortrait != _selected)
			{
				if (m_currentSelectedPortrait != null)
				{
					m_currentSelectedPortrait.IsSelected = false;
				}
				m_currentSelectedPortrait = _selected;
			}
			Action<UIAssassinBaseModel> onSelectionChanged = OnSelectionChanged;
			if (onSelectionChanged != null)
			{
				onSelectionChanged(_selected.Model);
			}
		}

		private void AlignElements()
		{
			for (int i = 0; i < base.transform.childCount; i++)
			{
				Transform child = base.transform.GetChild(i);
				child.localPosition = new Vector3(Mathf.Round(child.localPosition.x), Mathf.Round(child.localPosition.y), Mathf.Round(child.localPosition.z));
			}
		}
	}
}
