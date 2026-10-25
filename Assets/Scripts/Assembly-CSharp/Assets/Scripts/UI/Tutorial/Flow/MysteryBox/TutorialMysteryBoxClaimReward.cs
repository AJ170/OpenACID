using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.MysteryBoxScreen;
using Assets.Scripts.UI.Tutorial.Flow.Bases;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Tutorial.Flow.MysteryBox
{
	internal class TutorialMysteryBoxClaimReward : TutorialButtonClickBase
	{
		[SerializeField]
		private ChainManager m_ChainManager;

		private MysteryBoxItemContainer itemContainer;

		private readonly STutorialFlowId m_index = EFlowId.GuideMysteryBoxClaim.ToTutorialFlowId();

		public override string Name
		{
			get
			{
				return "Mystery Box Claim Reward";
			}
		}

		public override ETutorialCategory Category
		{
			get
			{
				return ETutorialCategory.TutCatMs25MysteryBox;
			}
		}

		public override STutorialFlowId Index
		{
			get
			{
				return m_index;
			}
		}

		public override void Activate()
		{
			base.IsActive = true;
			base.gameObject.SetActive(true);
			CollectBlockers();
			InitializeBlockers();
			for (int i = 0; i < m_blockers.Count; i++)
			{
				FullscreenBlocker blocker = m_blockers[i];
				ActivateBlocker(blocker);
			}
		}

		protected override void OnScreenInit()
		{
			if (m_ChainManager != null)
			{
				m_ChainManager.ItemContainersAdded.Add(OnItemContainersAdded);
			}
			base.OnScreenInit();
		}

		protected override void OnScreenDestroy()
		{
			if (itemContainer != null)
			{
				itemContainer.OnAnimationFinished.Remove(OnItemAnimationFinished);
				itemContainer = null;
			}
			if (m_ChainManager != null)
			{
				if (m_ChainManager.GetChainDataClaimedEvent() != null)
				{
					m_ChainManager.GetChainDataClaimedEvent().Remove(OnClaimed);
				}
				m_ChainManager.ItemContainersAdded.Remove(OnItemContainersAdded);
			}
			base.OnScreenDestroy();
		}

		private void OnItemContainersAdded(object[] _params)
		{
			if (!UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.IsTutorialCategoryActive(Category))
			{
				return;
			}
			List<MysteryBoxItemContainer> list = _params[0] as List<MysteryBoxItemContainer>;
			if (list != null && list.Count > 1)
			{
				itemContainer = list[1];
				itemContainer.OnAnimationFinished.Add(OnItemAnimationFinished);
				if (m_ChainManager.GetChainDataClaimedEvent() != null)
				{
					m_ChainManager.GetChainDataClaimedEvent().Add(OnClaimed);
				}
			}
		}

		private void OnItemAnimationFinished(object[] _params)
		{
			itemContainer.OnAnimationFinished.Remove(OnItemAnimationFinished);
			base.Activate();
		}

		protected override void OnClickInternal()
		{
		}

		private void OnClaimed(params object[] _params)
		{
			if (m_ChainManager != null && m_ChainManager.GetChainDataClaimedEvent() != null)
			{
				m_ChainManager.GetChainDataClaimedEvent().Remove(OnClaimed);
			}
			base.OnClickInternal();
		}

		public override bool ConditionAllowShow()
		{
			return TutorialHelper.ConditionDefaultCheck(this, true);
		}

		public override EScreenID GetScreenId()
		{
			return EScreenID.MysteryBox;
		}

		public override int GetHeaderText()
		{
			return 6956;
		}

		public override int GetDescriptionText()
		{
			return 6902;
		}
	}
}
