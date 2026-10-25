using Assets.Scripts.UI.MVC.Models.Rankings;
using Assets.Scripts.UI.MVC.UIView.Rankings;
using Assets.Scripts.UI.Utils.Development;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.Controllers.Rankings
{
	public class UIRankEntriesWrapperController : UIControllerBase
	{
		[CheckAssigned]
		[SerializeField]
		private UIRankEntriesWrapperView m_rankEntriesWrapperView;

		private UIRankEntryBaseModel[] m_rankEntries;

		public long CurrentSelectedId { get; set; }

		public UIRankEntryController CurrentSelected { get; set; }

		public void Initialize()
		{
			m_rankEntriesWrapperView.Wrapper.OnUpdateItem += OnWraperItemUpdate;
			m_rankEntriesWrapperView.Wrapper.OnItemCreated += OnWrapperItemCreated;
			m_rankEntriesWrapperView.Wrapper.Init();
		}

		public void DestroyController()
		{
			m_rankEntriesWrapperView.Wrapper.OnUpdateItem -= OnWraperItemUpdate;
			m_rankEntriesWrapperView.Wrapper.OnItemCreated -= OnWrapperItemCreated;
		}

		public void UpdateRankEntries(UIRankEntryBaseModel[] _rankEntries)
		{
			m_rankEntries = _rankEntries;
			m_rankEntriesWrapperView.SetItemCount(m_rankEntries.Length);
			m_rankEntriesWrapperView.ResetPosition();
		}

		private void OnWrapperItemCreated(Transform _transform)
		{
			UIRankEntryController component = _transform.GetComponent<UIRankEntryController>();
			if (component != null)
			{
				component.UpdateItemAnchor(m_rankEntriesWrapperView.AnchorTarget);
			}
		}

		private void OnWraperItemUpdate(Transform _transform, int _index)
		{
			UIRankEntryBaseModel uIRankEntryBaseModel = m_rankEntries[_index];
			UIRankEntryController component = _transform.GetComponent<UIRankEntryController>();
			component.UpdateRankController(uIRankEntryBaseModel, _index);
			if (CurrentSelectedId == uIRankEntryBaseModel.Position)
			{
				CurrentSelected = component;
			}
		}
	}
}
