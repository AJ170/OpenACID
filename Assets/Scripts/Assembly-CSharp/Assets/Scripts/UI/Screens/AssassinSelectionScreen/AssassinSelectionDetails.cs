using Assets.Scripts.UI.Behaviours;
using Assets.Scripts.UI.ScreenManager;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.AssassinSelectionScreen
{
	public class AssassinSelectionDetails : UIScreenChild
	{
		[SerializeField]
		private GameObject ActivateSkillWheelBtn;

		[SerializeField]
		private GameObject m_assassinDetails;

		[SerializeField]
		private GameObject m_assassinsOptions;

		[SerializeField]
		private AssassinCreationHandler AssassinCreation;

		[SerializeField]
		private TweenManager m_tweenManager;

		private int m_nextFreeSlotIndex;

		public int NextFreeSlotIndex
		{
			get
			{
				return m_nextFreeSlotIndex;
			}
		}

		protected override void OnScreenInit()
		{
			EventDelegate.Add(m_screen.onActivate, OnScreenActivate);
			EventDelegate.Add(m_screen.onDeActivate, OnScreenDeactivate);
			DisableScreen();
		}

		protected override void OnScreenDestroy()
		{
			if ((bool)m_screen)
			{
				EventDelegate.Remove(m_screen.onActivate, OnScreenActivate);
				EventDelegate.Remove(m_screen.onDeActivate, OnScreenDeactivate);
			}
		}

		public void OnEnable()
		{
			EnableScreen();
		}

		private static void EnableScreen()
		{
			Singleton<UIScreenManager>.Instance.SceneData.AssassinContainer.Display.CanCreateAssassins = true;
			Singleton<UIScreenManager>.Instance.SceneData.AssassinContainer.Display.ShowSwitchAssassins = true;
			Singleton<UIScreenManager>.Instance.SceneData.AssassinContainer.Display.CanSwitchRank = false;
		}

		public void OnDisable()
		{
			DisableScreen();
		}

		private void DisableScreen()
		{
			DisableEventHandlers();
			if ((bool)AssassinCreation)
			{
				AssassinCreation.EnableHandler(false, NextFreeSlotIndex);
				AssassinCreation.HideHandler(true);
			}
			m_assassinDetails.SetActive(false);
			m_assassinsOptions.SetActive(false);
		}

		private void SetupScreen()
		{
			UpdatePanels();
			EnableEventHandlers();
		}

		private void UpdatePanels()
		{
			UIAssassinRankDisplay.AssassinHandle current = Singleton<UIScreenManager>.Instance.SceneData.AssassinContainer.Display.Current;
			if (current != null)
			{
				AssassinCreation.EnableHandler(current.IsPlaceHolder, Singleton<UIScreenManager>.Instance.SceneData.AssassinContainer.Display.Index);
				AssassinCreation.HideHandler(!current.IsPlaceHolder);
				m_assassinDetails.SetActive(!current.IsPlaceHolder);
				m_assassinsOptions.SetActive(!current.IsPlaceHolder);
			}
		}

		private void EnableEventHandlers()
		{
			Singleton<UIScreenManager>.Instance.SceneData.AssassinContainer.Display.OnSelectionChanged += OnSelectionChange;
		}

		private void DisableEventHandlers()
		{
			if (!(Singleton<UIScreenManager>.Instance.SceneData == null) && !(Singleton<UIScreenManager>.Instance.SceneData.AssassinContainer == null) && !(Singleton<UIScreenManager>.Instance.SceneData.AssassinContainer.Display == null))
			{
				Singleton<UIScreenManager>.Instance.SceneData.AssassinContainer.Display.OnSelectionChanged -= OnSelectionChange;
			}
		}

		private void OnSelectionChange(UIAssassinRankDisplay.EDirection _direction)
		{
			UpdatePanels();
			if (Singleton<UIScreenManager>.Instance.SceneData.AssassinContainer.Display.Next == null || !Singleton<UIScreenManager>.Instance.SceneData.AssassinContainer.Display.Next.IsPlaceHolder || _direction != 0)
			{
				return;
			}
			for (int i = 0; i < m_tweenManager.m_tweenedComponents.Count; i++)
			{
				UITweenedScreenComponent uITweenedScreenComponent = m_tweenManager.m_tweenedComponents[i];
				if (!(uITweenedScreenComponent == null))
				{
					UITweener tweener = uITweenedScreenComponent.Tweener;
					if (!(tweener == null))
					{
						tweener.ResetToBeginning();
						tweener.PlayForward();
					}
				}
			}
		}

		private void OnScreenActivate()
		{
			DisableScreen();
			SetupScreen();
		}

		private void OnScreenDeactivate()
		{
			DisableScreen();
			StopAllCoroutines();
		}
	}
}
