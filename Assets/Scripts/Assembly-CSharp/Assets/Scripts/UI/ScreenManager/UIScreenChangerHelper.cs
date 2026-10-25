using System;
using AcierProtocol;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Network;
using Assets.Scripts.UI.Network;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils.Extensions;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.ScreenManager
{
	public class UIScreenChangerHelper : MonoBehaviour
	{
		public static EScreenID IsScreenChangeValid(EScreenID _ScreenID)
		{
			EScreenID eScreenID = _ScreenID;
			if (Singleton<StateManager>.Instance.ProfileManager.Profile.IsMysteryBoxOpen() && eScreenID == EScreenID.AssassinSelection)
			{
				if (Singleton<StateManager>.Instance.CurrentChain == null)
				{
					UINetworkAdaptor.BlockedNetworkRequest(delegate(AsyncCallback _asyncRequest)
					{
						Singleton<StateManager>.Instance.ProfileManager.GetMysteryBoxInfo(_asyncRequest);
					}, null, delegate(IAsyncResult _result)
					{
						if (!_result.IsError())
						{
							Singleton<StateManager>.Instance.CurrentChain = ((AsyncDataResult<GetMysteryBoxInfoResponse>)_result).Result().mysteryBoxChain;
							if (Singleton<StateManager>.Instance.CurrentChain.mysteryBoxType != EMysteryBoxType.MysteryBoxItemType_Tutorial && !UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.IsTutorialActive)
							{
								Singleton<UIScreenManager>.Instance.ChangeScreen(EScreenID.MysteryBox, new UITweenHandle(null, null));
							}
						}
					});
				}
				else if (!UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.IsTutorialActive)
				{
					eScreenID = EScreenID.MysteryBox;
				}
			}
			return eScreenID;
		}
	}
}
