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
	internal class TutorialMysteryBoxCongratulations : TutorialButtonClickBase
	{
		[SerializeField]
		private ChainManager m_ChainManager;

		private readonly STutorialFlowId m_index = EFlowId.DEPRECATEDGuideMysteryBoxInfoContinue.ToTutorialFlowId();

		private MysteryBoxItemContainer itemContainer;

		public override string Name
		{
			get
			{
				return "Mystery Box Congratulations";
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

		protected override void OnScreenInit()
		{
			m_ChainManager.ItemContainersAdded.Add(OnItemContainersAdded);
			base.OnScreenInit();
		}

		protected override void OnScreenDestroy()
		{
			if (itemContainer != null)
			{
				itemContainer.OnAnimationFinished.Remove(FirstItemAnimationFinished);
				itemContainer = null;
			}
			if (m_ChainManager != null)
			{
				m_ChainManager.ItemContainersAdded.Remove(OnItemContainersAdded);
			}
			base.OnScreenDestroy();
		}

		public override void Activate()
		{
			if (!m_button && (bool)m_buttonWidget)
			{
				m_buttonWidget.gameObject.SetActive(true);
				m_button = m_buttonWidget.GetComponentInChildren<UIExtendedButton>();
			}
		}

		private void OnItemContainersAdded(object[] _params)
		{
			if (UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.IsTutorialCategoryActive(Category))
			{
				List<MysteryBoxItemContainer> list = _params[0] as List<MysteryBoxItemContainer>;
				if (list != null && list.Count > 1)
				{
					itemContainer = list[1];
					itemContainer.OnAnimationFinished.Add(FirstItemAnimationFinished);
				}
			}
		}

		private void FirstItemAnimationFinished(object[] _params)
		{
			itemContainer.OnAnimationFinished.Remove(FirstItemAnimationFinished);
			base.Activate();
		}

		public override bool ConditionAllowShow()
		{
			return TutorialHelper.ConditionAllowShowGuideMysteryBox(this);
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
			return 6901;
		}

		protected override int GetButtonText()
		{
			return 2162;
		}
	}
}
