using System;
using AcierProtocol;
using Assets.Scripts.Network;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.MVC.Controllers.Assassin;
using Assets.Scripts.UI.MVC.Controllers.Misc;
using Assets.Scripts.UI.MVC.Models.Assassin;
using Assets.Scripts.UI.Network;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils.Development;
using Assets.Scripts.UI.Utils.Extensions;
using Database;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.Controllers.Rankings
{
	public class UIRankAssassinDetailsController : UIControllerBase
	{
		[SerializeField]
		[CheckAssigned]
		private UIHeaderTitleController m_headerController;

		[CheckAssigned]
		[SerializeField]
		private UIAssassinSkillSummeryController m_assassinSummeryController;

		[CheckAssigned]
		[SerializeField]
		private UIAssassinPortraitWithHeroicRankController m_assassnPortraitController;

		[CheckAssigned]
		[SerializeField]
		private UIRankEntryDetailsController m_rankDetailsController;

		private UIAssassinLeaderboardUserDetailsModel m_leaderboardUserModel;

		private bool m_updateDataOnNextUpdate;

		public void UpdateRankAssassinDetails(UIAssassinLeaderboardUserDetailsModel _leaderboardUserModel)
		{
			m_updateDataOnNextUpdate = true;
			m_leaderboardUserModel = _leaderboardUserModel;
			m_headerController.UpdateTitle(LocalizationManager.Instance.Get(8554, new StringMapper { { "type", _leaderboardUserModel.Title } }));
			m_rankDetailsController.UpdateRankEntryDetails(m_leaderboardUserModel);
		}

		public override void OnControllerClicked()
		{
			base.OnControllerClicked();
			LeaderBoardUser user = m_leaderboardUserModel.LeaderBoardUser;
			AsyncCallback callback = delegate(IAsyncResult _response)
			{
				if (!_response.IsError())
				{
					GetForeignAssassinResponse getForeignAssassinResponse = ((AsyncDataResult<GetForeignAssassinResponse>)_response).Result();
					Singleton<UIScreenManager>.Instance.SceneData.AssassinAndEquipmentPreview.OpenPreview(m_leaderboardUserModel.UserName, m_leaderboardUserModel.HeroicRank, -1, getForeignAssassinResponse.assassin);
				}
			};
			UINetworkAdaptor.BlockedNetworkRequest(delegate(AsyncCallback _asyncResult)
			{
				Singleton<StateManager>.Instance.ProfileManager.GetForeignAssassinByUserId(user.userId, _asyncResult);
			}, null, callback);
		}

		protected override void OnUpdateUnity()
		{
			base.OnUpdateUnity();
			if (m_updateDataOnNextUpdate)
			{
				m_updateDataOnNextUpdate = false;
				m_assassinSummeryController.UpdateSummery(m_leaderboardUserModel.Summery);
				if (m_leaderboardUserModel.Assassin != null)
				{
					GameID _headId = GameID.Invalid;
					GameID _humanoidId = GameID.Invalid;
					GameID _colorId = GameID.Invalid;
					GameID _itemId = GameID.Invalid;
					m_leaderboardUserModel.GetAssasssinPortraitData(ref _headId, ref _humanoidId, ref _colorId, ref _itemId);
					m_assassnPortraitController.UpdateAssassinPortrait(_headId, _humanoidId, _colorId, _itemId, false, m_leaderboardUserModel.HeroicRank);
				}
			}
		}
	}
}
