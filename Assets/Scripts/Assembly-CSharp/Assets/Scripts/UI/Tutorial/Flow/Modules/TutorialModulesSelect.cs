using AcierProtocol;
using Assets.Scripts.UI.NGUIExtensions.Tweens;
using Assets.Scripts.UI.Tutorial.Flow.InventoryScreenBases;
using UnityEngine;

namespace Assets.Scripts.UI.Tutorial.Flow.Modules
{
	public class TutorialModulesSelect : TutorialInventorySelectBase
	{
		private readonly STutorialFlowId m_index = EFlowId.GuideLoadout.ToTutorialFlowId();

		[SerializeField]
		private BBTweenAlpha m_backgroundItemTween;

		public override string Name
		{
			get
			{
				return "TutorialModulesSelect";
			}
		}

		public override ETutorialCategory Category
		{
			get
			{
				return ETutorialCategory.TutCatMs26Modules;
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
			EventDelegate.Add(m_backgroundItemTween.onFinished, OnUnlockAnimFinsihed);
			base.OnScreenInit();
		}

		protected override void OnScreenDestroy()
		{
			if (m_backgroundItemTween != null)
			{
				EventDelegate.Remove(m_backgroundItemTween.onFinished, OnUnlockAnimFinsihed);
			}
			base.OnScreenDestroy();
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

		private void OnUnlockAnimFinsihed()
		{
			if (IsActive)
			{
				EventDelegate.Remove(m_backgroundItemTween.onFinished, OnUnlockAnimFinsihed);
				base.Activate();
			}
		}

		public override bool ConditionAllowShow()
		{
			return TutorialHelper.ConditionAllowShowGuideModules(this);
		}

		protected override string GetItemTechnicalName()
		{
			return TutorialHelper.TutorialEquipModule;
		}

		public override int GetHeaderText()
		{
			return 7317;
		}

		public override int GetDescriptionText()
		{
			return 7320;
		}
	}
}
