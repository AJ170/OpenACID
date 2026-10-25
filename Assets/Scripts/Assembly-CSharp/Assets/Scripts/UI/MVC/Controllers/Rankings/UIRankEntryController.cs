using System;
using AcierProtocol;
using Assets.Scripts.Network;
using Assets.Scripts.UI.MVC.Models.Rankings;
using Assets.Scripts.UI.MVC.UIView.Rankings;
using Assets.Scripts.UI.Network;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Development;
using Assets.Scripts.UI.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.Controllers.Rankings
{
	public class UIRankEntryController : UIControllerBase
	{
		[CheckAssigned]
		[SerializeField]
		private UIRankEntryView m_rankEntryView;

		private UIRankEntryBaseModel m_rankModel;

		public int EntryIndex { get; set; }

		public override void Show()
		{
			base.Show();
			m_rankEntryView.Show();
		}

		public override void Hide()
		{
			base.Hide();
			m_rankEntryView.Hide();
		}

		public void UpdateRankController(UIRankEntryBaseModel _rankModel, int _index)
		{
			m_rankModel = _rankModel;
			EntryIndex = _index;
			m_rankEntryView.SetPosition(m_rankModel.Position);
			m_rankEntryView.SetPlayerName(m_rankModel.Name);
			m_rankEntryView.SetScore(m_rankModel.Score);
			m_rankEntryView.SetRelationIcon(m_rankModel.RelationIconName);
			m_rankEntryView.SetColor(m_rankModel.Color);
		}

		public void UpdateItemAnchor(UIRect _anchorTarget)
		{
			UIUtil.UpdateLeftAndRightAnchorsForTarget(m_rankEntryView.ViewRoot, _anchorTarget);
		}

		public override void OnControllerClicked()
		{
			base.OnControllerClicked();
			LeaderBoardUser user = m_rankModel.User;
			AsyncCallback callback = delegate(IAsyncResult _response)
			{
				if (!_response.IsError())
				{
					GetForeignAssassinResponse getForeignAssassinResponse = ((AsyncDataResult<GetForeignAssassinResponse>)_response).Result();
					Singleton<UIScreenManager>.Instance.SceneData.AssassinAndEquipmentPreview.OpenPreview(m_rankModel.Name, m_rankModel.HeroicRank, -1, getForeignAssassinResponse.assassin);
				}
			};
			UINetworkAdaptor.BlockedNetworkRequest(delegate(AsyncCallback _asyncResult)
			{
				Singleton<StateManager>.Instance.ProfileManager.GetForeignAssassinByUserId(user.userId, _asyncResult);
			}, null, callback);
		}
	}
}
