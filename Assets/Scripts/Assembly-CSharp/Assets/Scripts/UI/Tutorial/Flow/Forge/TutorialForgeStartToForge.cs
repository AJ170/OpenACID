using AcierProtocol;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.InventoryScreen;
using Assets.Scripts.UI.Tutorial.Flow.Bases;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Tutorial.Flow.Forge
{
	internal class TutorialForgeStartToForge : TutorialButtonClickBase
	{
		private readonly STutorialFlowId m_index = EFlowId.GuideForgeStartToForge.ToTutorialFlowId();

		[SerializeField]
		private UIForgeHandler m_forgeHandler;

		public override string Name
		{
			get
			{
				return "TutorialForgeStartToForge";
			}
		}

		public override ETutorialCategory Category
		{
			get
			{
				return ETutorialCategory.TutCatMs25Forge;
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
			base.Activate();
			EventDelegate.Add(m_forgeHandler.OnOpenForgeItemPopup, OnConfirmShown);
		}

		protected override void OnClickInternal()
		{
		}

		protected override void OnDestroyBase()
		{
			base.OnDestroyBase();
			EventDelegate.Remove(m_forgeHandler.OnOpenForgeItemPopup, OnConfirmShown);
		}

		private void OnConfirmShown()
		{
			if (!GenericAndroidBackKey.IsExitPopupShown)
			{
				EventDelegate.Remove(m_forgeHandler.OnOpenForgeItemPopup, OnConfirmShown);
				UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.NextStep(Category);
			}
		}

		public override void Deactivate()
		{
			if (IsActive)
			{
				EventDelegate.Remove(Singleton<UIScreenManager>.Instance.onScreenPopped, OnConfirmShown);
				base.Deactivate();
			}
		}

		public override bool ConditionAllowShow()
		{
			return TutorialHelper.ConditionAllowShowGuideForge(this);
		}

		public override EScreenID GetScreenId()
		{
			return EScreenID.InventoryScreen;
		}

		public override int GetHeaderText()
		{
			return 6923;
		}

		public override int GetDescriptionText()
		{
			return (int)StringId.Invalid.Value;
		}

		public override string GetDescription()
		{
			return TutorialHelper.ExchangeCurrencyInString(6931, EResourceType.SoftCurrency);
		}
	}
}
