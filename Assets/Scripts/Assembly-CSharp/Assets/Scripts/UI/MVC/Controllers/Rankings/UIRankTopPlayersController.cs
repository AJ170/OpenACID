using System;
using AcierProtocol;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.MVC.Controllers.Assassin;
using Assets.Scripts.UI.MVC.Controllers.Misc;
using Assets.Scripts.UI.MVC.Models.Assassin;
using Assets.Scripts.UI.Screens.Leaderboard;
using Assets.Scripts.UI.Utils.Development;
using Assets.Scripts.UI.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.Controllers.Rankings
{
	public class UIRankTopPlayersController : UIControllerBase
	{
		[SerializeField]
		[CheckAssigned]
		private UIHeaderTitleController m_titleController;

		[CheckAssigned]
		[SerializeField]
		private UIHeaderTitleController m_subTitleController;

		[CheckAssigned]
		[SerializeField]
		private UIAssassinPortraitSelectableListController m_assassinsListController;

		[SerializeField]
		[CheckAssigned]
		private UIControllerBase m_controller;

		public UIAssassinBaseModel CurrentSelectedModel
		{
			get
			{
				return m_assassinsListController.CurrentSelectedModel;
			}
		}

		public void UpdateTopRankPlayers(GetLeaderBoardsResponse _leaderboardResult, ELeaderboardType _type)
		{
			int num = 8556;
			if (_type == ELeaderboardType.EventsHeader)
			{
				num = 8556;
			}
			m_titleController.UpdateTitle(LocalizationManager.Instance.Get(num));
			UIAssassinBaseModel[] array = _leaderboardResult.CreateTopPlayerModels(_type);
			m_assassinsListController.UpdatePortraitList(array);
			SelectHigestAvailablePlayer(array);
		}

		protected override void OnStartUnity()
		{
			base.OnStartUnity();
			UIAssassinPortraitSelectableListController assassinsListController = m_assassinsListController;
			assassinsListController.OnSelectionChanged = (Action<UIAssassinBaseModel>)Delegate.Combine(assassinsListController.OnSelectionChanged, new Action<UIAssassinBaseModel>(OnSelectionChanged));
		}

		protected override void OnDestroyUnity()
		{
			base.OnDestroyUnity();
			if (m_assassinsListController != null)
			{
				UIAssassinPortraitSelectableListController assassinsListController = m_assassinsListController;
				assassinsListController.OnSelectionChanged = (Action<UIAssassinBaseModel>)Delegate.Remove(assassinsListController.OnSelectionChanged, new Action<UIAssassinBaseModel>(OnSelectionChanged));
			}
		}

		private void SelectHigestAvailablePlayer(UIAssassinBaseModel[] _topPlayers)
		{
			int index = 0;
			for (int i = 0; i < _topPlayers.Length; i++)
			{
				UIAssassinLeaderboardUserDetailsModel uIAssassinLeaderboardUserDetailsModel = (UIAssassinLeaderboardUserDetailsModel)_topPlayers[i];
				if (uIAssassinLeaderboardUserDetailsModel.LeaderBoardUser != null)
				{
					index = i;
					break;
				}
			}
			if (m_assassinsListController.CurrentSelectedModel == null)
			{
				m_assassinsListController.SelectPortraitInstantlyByIndex(index);
			}
			UpdateCurrentSelectedRank(CurrentSelectedModel);
		}

		private void UpdateCurrentSelectedRank(UIAssassinBaseModel _model)
		{
			UIAssassinLeaderboardUserDetailsModel uIAssassinLeaderboardUserDetailsModel = (UIAssassinLeaderboardUserDetailsModel)_model;
			if (uIAssassinLeaderboardUserDetailsModel != null)
			{
				if (uIAssassinLeaderboardUserDetailsModel.LeaderBoardUser != null)
				{
					m_subTitleController.UpdateTitle(uIAssassinLeaderboardUserDetailsModel.LeaderBoardUser.userName);
				}
				else
				{
					m_subTitleController.UpdateTitle(string.Empty);
				}
				m_controller.OnChanged(this);
			}
		}

		private void OnSelectionChanged(UIAssassinBaseModel _uiAssassinBaseModel)
		{
			UpdateCurrentSelectedRank(_uiAssassinBaseModel);
		}
	}
}
