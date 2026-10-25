using System.Collections;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.State;
using UnityEngine;

namespace Assets.Scripts.UI.SkillWheelImplementation
{
	public class SkillWheelPreview : UIScreenChild
	{
		[SerializeField]
		private List<SkillWheelPreviewModel> m_previewModels;

		protected override void OnScreenInit()
		{
			EventDelegate.Add(m_screen.onActivate, UpdateVisibility);
			EventDelegate.Add(m_screen.onActivate, AddEventDelegate);
			EventDelegate.Add(m_screen.onActivate, UpdatePreview);
			EventDelegate.Add(m_screen.onDeActivate, RemoveEventDelegate);
		}

		protected override void OnScreenDestroy()
		{
			if (m_screen != null)
			{
				EventDelegate.Remove(m_screen.onActivate, UpdateVisibility);
				EventDelegate.Remove(m_screen.onActivate, AddEventDelegate);
				EventDelegate.Remove(m_screen.onActivate, UpdatePreview);
				EventDelegate.Remove(m_screen.onDeActivate, RemoveEventDelegate);
			}
		}

		private void UpdateVisibility()
		{
			Assassin currentAssassin = Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin;
			bool active = UiMain.UIForceShowSkillWheelInAnimus || (currentAssassin != null && currentAssassin.extendedClassTypeId != Assassin.EClassTypeId.None);
			base.gameObject.SetActive(active);
		}

		private void AddEventDelegate()
		{
			Singleton<StateManager>.Instance.ProfileManager.OnAssassinChange.Add(OnProfileUpdate);
			EventDelegate.Add(Singleton<UIScreenManager>.Instance.GetScreen(EScreenID.AssassinSkillWheel).onDeActivate, UpdatePreview);
		}

		private void RemoveEventDelegate()
		{
			Singleton<StateManager>.Instance.ProfileManager.OnAssassinChange.Remove(OnProfileUpdate);
			UIScreen screen = Singleton<UIScreenManager>.Instance.GetScreen(EScreenID.AssassinSkillWheel);
			if (screen != null)
			{
				EventDelegate.Remove(screen.onDeActivate, UpdatePreview);
			}
		}

		private void OnProfileUpdate(params object[] _params)
		{
			UpdateVisibility();
			UpdatePreview();
		}

		private void UpdatePreview()
		{
			if (base.gameObject.activeInHierarchy)
			{
				StartCoroutine(UseData());
			}
		}

		private IEnumerator UseData()
		{
			if (Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin == null)
			{
				yield return null;
			}
			yield return new WaitForEndOfFrame();
			SkillWheel skillWheel = Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin.skillWheel;
			for (int j = 0; j < m_previewModels.Count; j++)
			{
				m_previewModels[j].SkillIcon.ClearTexture();
			}
			for (int i = 0; i < skillWheel.entries.Length; i++)
			{
				SkillWheelExtensions.SetIcon(m_previewModels[skillWheel.entries[i].position - 1].SkillIcon, skillWheel.entries[i].tierId, skillWheel.entries[i].classType);
			}
			m_previewModels[4].SkillIcon.mainTexture = "Call_Hireling".ToEnum<SkillTypes>().GetDefinition().IconUp.AccessObject<Texture2D>();
			m_previewModels[5].SkillIcon.mainTexture = "Eagle_Vision".ToEnum<SkillTypes>().GetDefinition().IconUp.AccessObject<Texture2D>();
		}
	}
}
