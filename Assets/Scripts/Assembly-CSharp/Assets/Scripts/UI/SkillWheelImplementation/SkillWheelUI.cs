using System;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Audio;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Network;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.SkillWheelImplementation
{
	public class SkillWheelUI : UIScreenChild
	{
		public List<SkillModel> m_skillWheelModels;

		public List<SkillWheelButton> m_skillWheelButtons;

		[SerializeField]
		private GameObject m_SkillWheelContainer;

		[SerializeField]
		private UIExtendedButton m_DeactivationButton;

		[SerializeField]
		private UIExtendedButton m_undoChangesButton;

		[SerializeField]
		private UIExtendedLabel SkillNameMiddle;

		[SerializeField]
		private UIExtendedLabel SkillNameBody;

		[SerializeField]
		private UIExtendedLabel SkillDescriptionBody;

		[SerializeField]
		private UITexture SkillIconBody;

		[SerializeField]
		private UIExtendedLabel SkillLevelBody;

		[SerializeField]
		private UISprite m_baseClassImage;

		[SerializeField]
		private UIExtendedLabel m_baseClassName;

		[SerializeField]
		private UISprite m_extendedClassImage;

		[SerializeField]
		private UIExtendedLabel m_extendedClassName;

		[SerializeField]
		private SkillWheelAnimationHandler m_animationHandler;

		private bool m_extendedClassUnlocked;

		private SkillModel m_lastClickedSkillModel;

		private SkillWheelButton m_lastClickedSkillWheelButton;

		private SkillModel m_currentSkillModel;

		private SkillWheelButton m_currentSkillWheelButton;

		private bool m_changesDone;

		protected override void OnScreenInit()
		{
			EventDelegate.Add(m_DeactivationButton.onClick, DeactivateSkillWheel);
			EventDelegate.Add(m_screen.onActivate, ActivateSkillWheel);
			EventDelegate.Add(m_screen.onDeActivate, OnScreenDeactivate);
			EventDelegate.Add(m_undoChangesButton.onClick, UndoChanges);
			for (int i = 0; i < m_skillWheelModels.Count; i++)
			{
				EventDelegate.Add(m_skillWheelModels[i].EventTrigger.onClick, ClickedSkillModel);
			}
			for (int j = 0; j < m_skillWheelButtons.Count; j++)
			{
				EventDelegate.Add(m_skillWheelButtons[j].EventTrigger.onClick, ClickedSkillWheelButton);
			}
		}

		protected override void OnScreenDestroy()
		{
			if ((bool)m_screen)
			{
				EventDelegate.Remove(m_screen.onActivate, ActivateSkillWheel);
				EventDelegate.Remove(m_screen.onDeActivate, OnScreenDeactivate);
			}
			EventDelegate.Remove(m_DeactivationButton.onClick, DeactivateSkillWheel);
			EventDelegate.Remove(m_undoChangesButton.onClick, UndoChanges);
			for (int i = 0; i < m_skillWheelModels.Count; i++)
			{
				EventDelegate.Remove(m_skillWheelModels[i].EventTrigger.onClick, ClickedSkillModel);
			}
			m_skillWheelModels.Clear();
			for (int j = 0; j < m_skillWheelButtons.Count; j++)
			{
				EventDelegate.Remove(m_skillWheelButtons[j].EventTrigger.onClick, ClickedSkillWheelButton);
			}
			m_skillWheelButtons.Clear();
		}

		private void ActivateSkillWheel()
		{
			m_changesDone = false;
			SkillNameMiddle.text = string.Empty;
			SkillNameBody.text = string.Empty;
			SkillDescriptionBody.text = LocalizationManager.Instance.Get(1829);
			SkillIconBody.ClearTexture();
			SkillLevelBody.text = string.Empty;
			ResetAndPlayWheelAnimation();
			m_animationHandler.GetElementsAndPlayAnimations();
			UpdateDataAndRepresentation();
			Singleton<StateManager>.Instance.ProfileManager.OnProfileUpdate.Add(OnProfileUpdated);
		}

		private void OnScreenDeactivate()
		{
			Singleton<StateManager>.Instance.ProfileManager.OnProfileUpdate.Remove(OnProfileUpdated);
		}

		private void DeactivateSkillWheel()
		{
			Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin.skillWheel.entries.ClearIfNotEmpty();
			for (int i = 0; i < m_skillWheelButtons.Count; i++)
			{
				if (!m_skillWheelButtons[i].empty && !m_skillWheelButtons[i].locked)
				{
					Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin.skillWheel.entries = Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin.skillWheel.entries.Add(m_skillWheelButtons[i].entry);
				}
				m_skillWheelButtons[i].Highlight.alpha = 0f;
			}
			m_currentSkillModel = null;
			m_currentSkillWheelButton = null;
			SendDataAsynchronous();
		}

		private void ResetAndPlayWheelAnimation()
		{
			m_SkillWheelContainer.gameObject.SetActive(true);
		}

		public override void OnBackKey()
		{
			UndoChanges();
		}

		private void UndoChanges()
		{
			if (m_changesDone)
			{
				DeactivateAllHighLights();
				m_currentSkillModel = null;
				m_currentSkillWheelButton = null;
				UpdateDataAndRepresentation();
				UpdateDisplay(0, true, Assassin.EClassTypeId.None);
				m_changesDone = false;
				AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_menu_global_backcancel);
			}
			else
			{
				CloseScreen();
				AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_menu_global_backcancel);
			}
			SkillNameMiddle.text = string.Empty;
			SkillNameBody.text = string.Empty;
			SkillDescriptionBody.text = LocalizationManager.Instance.Get(1829);
			SkillIconBody.mainTexture = null;
			SkillLevelBody.text = string.Empty;
		}

		private void OnProfileUpdated(params object[] _params)
		{
			UpdateDataAndRepresentation();
		}

		private void UpdateDataAndRepresentation()
		{
			Assassin assassin = null;
			SkillWheel skillWheel = null;
			if (Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin != null)
			{
				assassin = Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin;
				if (Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin.skillWheel != null)
				{
					skillWheel = assassin.skillWheel;
				}
			}
			if (assassin == null)
			{
				return;
			}
			switch (assassin.classTypeId)
			{
			case Assassin.EClassTypeId.Berserker:
				m_baseClassImage.spriteName = "aUI_img_chrDual_baserker";
				m_baseClassName.text = LocalizationManager.Instance.Get(298);
				break;
			case Assassin.EClassTypeId.Shadowblade:
				m_baseClassImage.spriteName = "aUI_img_chrDual_shadowblade";
				m_baseClassName.text = LocalizationManager.Instance.Get(301);
				break;
			case Assassin.EClassTypeId.Thief:
				m_baseClassImage.spriteName = "aUI_img_chrDual_thief";
				m_baseClassName.text = LocalizationManager.Instance.Get(300);
				break;
			case Assassin.EClassTypeId.Trickster:
				m_baseClassImage.spriteName = "aUI_img_chrDual_trickster";
				m_baseClassName.text = LocalizationManager.Instance.Get(302);
				break;
			}
			switch (assassin.extendedClassTypeId)
			{
			case Assassin.EClassTypeId.None:
				m_extendedClassImage.spriteName = "aUI_img_chrDual_empty";
				m_extendedClassName.text = LocalizationManager.Instance.Get(1522);
				break;
			case Assassin.EClassTypeId.Berserker:
				m_extendedClassImage.spriteName = "aUI_img_chrDual_baserker";
				m_extendedClassName.text = LocalizationManager.Instance.Get(298);
				break;
			case Assassin.EClassTypeId.Shadowblade:
				m_extendedClassImage.spriteName = "aUI_img_chrDual_shadowblade";
				m_extendedClassName.text = LocalizationManager.Instance.Get(301);
				break;
			case Assassin.EClassTypeId.Thief:
				m_extendedClassImage.spriteName = "aUI_img_chrDual_thief";
				m_extendedClassName.text = LocalizationManager.Instance.Get(300);
				break;
			case Assassin.EClassTypeId.Trickster:
				m_extendedClassImage.spriteName = "aUI_img_chrDual_trickster";
				m_extendedClassName.text = LocalizationManager.Instance.Get(302);
				break;
			}
			if (assassin.extendedClassTypeId != Assassin.EClassTypeId.None)
			{
				m_extendedClassUnlocked = true;
				for (int i = 4; i < m_skillWheelModels.Count; i++)
				{
					m_skillWheelModels[i].Collider.enabled = true;
					m_skillWheelModels[i].Background.color = Color.white;
					m_skillWheelModels[i].Level.gameObject.SetActive(true);
				}
			}
			else
			{
				m_extendedClassUnlocked = false;
				for (int j = 4; j < m_skillWheelModels.Count; j++)
				{
					m_skillWheelModels[j].Collider.enabled = false;
					m_skillWheelModels[j].Background.color = new Color32(120, 120, 120, byte.MaxValue);
					m_skillWheelModels[j].Level.gameObject.SetActive(false);
				}
			}
			Assets.Scripts.UI.Utils.Extensions.SkillData[][] skills = assassin.GetSkills(true);
			Assets.Scripts.UI.Utils.Extensions.SkillData[][] skills2 = assassin.GetSkills(false);
			for (int k = 0; k < m_skillWheelModels.Count; k++)
			{
				SkillModel skillModel = m_skillWheelModels[k];
				skillModel.GoldenFrame.alpha = 0f;
				Assets.Scripts.UI.Utils.Extensions.SkillData[][] array = ((k >= 4) ? skills2 : skills);
				if (!m_extendedClassUnlocked || array[k % 4][0].Level == 0)
				{
					skillModel.ResetSkillModel();
					continue;
				}
				if (array[k % 4][0].Level == 5)
				{
					skillModel.GoldenFrame.alpha = 1f;
					skillModel.Level.gameObject.SetActive(false);
				}
				else
				{
					skillModel.Level.gameObject.SetActive(true);
					skillModel.Level.text = array[k % 4][0].Level.ToString();
				}
				skillModel.ClassTypeID = array[k % 4][0].AvatarType;
				skillModel.TierID = array[k % 4][0].TierId;
				skillModel.Collider.enabled = true;
				SkillWheelExtensions.SetIcon(skillModel.SkillIcon, skillModel.TierID, skillModel.ClassTypeID);
			}
			for (int l = 0; l < m_skillWheelButtons.Count; l++)
			{
				m_skillWheelButtons[l].entry = null;
				if (skillWheel != null)
				{
					for (int m = 0; m < skillWheel.entries.Length; m++)
					{
						if (skillWheel.entries[m].position == m_skillWheelButtons[l].Index)
						{
							m_skillWheelButtons[l].entry = skillWheel.entries[m];
							SkillWheelExtensions.SetIcon(m_skillWheelButtons[l].SkillIcon, m_skillWheelButtons[l].entry.tierId, m_skillWheelButtons[l].entry.classType);
							break;
						}
					}
				}
				m_skillWheelButtons[l].empty = m_skillWheelButtons[l].entry == null;
				if (m_skillWheelButtons[l].empty)
				{
					m_skillWheelButtons[l].SkillIcon.ClearTexture();
				}
				if (m_skillWheelButtons[l].Index == 5)
				{
					Assets.Scripts.Mission.Definitions.SkillData.ISkillDefinition definition = "Eagle_Vision".ToEnum<SkillTypes>().GetDefinition();
					m_skillWheelButtons[l].SkillIcon.mainTexture = definition.IconUp.AccessObject<Texture2D>();
					m_skillWheelButtons[l].empty = false;
				}
				else if (m_skillWheelButtons[l].Index == 6)
				{
					Assets.Scripts.Mission.Definitions.SkillData.ISkillDefinition definition2 = "Call_Hireling".ToEnum<SkillTypes>().GetDefinition();
					m_skillWheelButtons[l].SkillIcon.mainTexture = definition2.IconUp.AccessObject<Texture2D>();
					m_skillWheelButtons[l].empty = false;
				}
			}
		}

		private void ClickedSkillModel()
		{
			m_changesDone = true;
			if (m_lastClickedSkillModel != null)
			{
				m_lastClickedSkillModel.Highlight.PlayReverse();
			}
			SkillModel component = UIEventTrigger.current.GetComponent<SkillModel>();
			component.Highlight.ResetToBeginning();
			component.Highlight.PlayForward();
			SwapSkills(component, null);
			UpdateDisplay(component.TierID, false, component.ClassTypeID);
			m_lastClickedSkillModel = component;
		}

		private void ClickedSkillWheelButton()
		{
			m_changesDone = true;
			if (m_lastClickedSkillWheelButton != null)
			{
				if (!m_lastClickedSkillWheelButton.empty)
				{
					if (m_lastClickedSkillWheelButton.locked)
					{
						Assets.Scripts.Mission.Definitions.SkillData.ISkillDefinition skillDefinition = null;
						if (m_lastClickedSkillWheelButton.Index == 5)
						{
							skillDefinition = "Eagle_Vision".ToEnum<SkillTypes>().GetDefinition();
						}
						if (m_lastClickedSkillWheelButton.Index == 6)
						{
							skillDefinition = "Call_Hireling".ToEnum<SkillTypes>().GetDefinition();
						}
						m_lastClickedSkillWheelButton.SkillIcon.mainTexture = skillDefinition.IconUp.AccessObject<Texture2D>();
					}
					else
					{
						SkillWheelExtensions.SetIcon(m_lastClickedSkillWheelButton.SkillIcon, m_lastClickedSkillWheelButton.entry.tierId, m_lastClickedSkillWheelButton.entry.classType);
					}
				}
				m_lastClickedSkillWheelButton.Highlight.alpha = 0f;
				if (m_lastClickedSkillWheelButton.HighLightTween.value > 0f)
				{
					m_lastClickedSkillWheelButton.HighLightTween.PlayReverse();
				}
			}
			SkillWheelButton component = UIEventTrigger.current.GetComponent<SkillWheelButton>();
			component.Highlight.alpha = 1f;
			if (!component.empty)
			{
				component.HighLightTween.ResetToBeginning();
				component.HighLightTween.PlayForward();
				if (component.locked)
				{
					Assets.Scripts.Mission.Definitions.SkillData.ISkillDefinition skillDefinition2 = null;
					if (component.Index == 5)
					{
						skillDefinition2 = "Eagle_Vision".ToEnum<SkillTypes>().GetDefinition();
					}
					if (component.Index == 6)
					{
						skillDefinition2 = "Call_Hireling".ToEnum<SkillTypes>().GetDefinition();
					}
					component.SkillIcon.mainTexture = skillDefinition2.IconDown.AccessObject<Texture2D>();
				}
				else
				{
					SkillWheelExtensions.SetIcon(component.SkillIcon, component.entry.tierId, true, component.entry.classType);
				}
			}
			SwapSkills(null, component);
			if (component.Index < 5)
			{
				if (!component.empty)
				{
					UpdateDisplay(component.entry.tierId, false, component.entry.classType);
				}
				else
				{
					UpdateDisplay(0, true, Assassin.EClassTypeId.None);
				}
			}
			else
			{
				UpdateDisplay(component.Index, false, Assassin.EClassTypeId.None);
			}
			m_lastClickedSkillWheelButton = component;
		}

		private void SwapSkills(SkillModel _skillModel, SkillWheelButton _skillWheelButton)
		{
			m_currentSkillModel = _skillModel ?? m_currentSkillModel;
			m_currentSkillWheelButton = _skillWheelButton ?? m_currentSkillWheelButton;
			if (!(m_currentSkillModel != null) || !(m_currentSkillWheelButton != null) || m_currentSkillWheelButton.Index >= 5)
			{
				return;
			}
			for (int i = 0; i < m_skillWheelButtons.Count; i++)
			{
				if (m_skillWheelButtons[i].entry != null && m_skillWheelButtons[i].entry.tierId == m_currentSkillModel.TierID && m_skillWheelButtons[i].entry.classType == m_currentSkillModel.ClassTypeID)
				{
					m_skillWheelButtons[i].empty = true;
					m_skillWheelButtons[i].entry = null;
					m_skillWheelButtons[i].SkillIcon.ClearTexture();
				}
			}
			m_currentSkillWheelButton.entry = new SkillWheelEntry();
			m_currentSkillWheelButton.entry.tierId = m_currentSkillModel.TierID;
			m_currentSkillWheelButton.entry.classType = m_currentSkillModel.ClassTypeID;
			m_currentSkillWheelButton.entry.position = m_currentSkillWheelButton.Index;
			m_currentSkillWheelButton.empty = false;
			SkillWheelExtensions.SetIcon(m_currentSkillWheelButton.SkillIcon, m_currentSkillWheelButton.entry.tierId, m_currentSkillWheelButton.entry.classType);
			SkillWheelExtensions.SetIcon(m_currentSkillModel.SkillIcon, m_currentSkillModel.TierID, m_currentSkillModel.ClassTypeID);
			AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_menu_global_subaccept);
			m_currentSkillModel = null;
			m_currentSkillWheelButton = null;
		}

		private void UpdateDisplay(int tierIDorIndex, bool empty, Assassin.EClassTypeId classType)
		{
			if (!empty)
			{
				Assets.Scripts.Mission.Definitions.SkillData.ISkillDefinition skillDefinition = null;
				if (tierIDorIndex < 5)
				{
					skillDefinition = UbiCore.Utils.Singleton<Assets.Scripts.Mission.Definitions.SkillData>.Instance.GetDefinition(SkillWheelExtensions.GetSkillID(tierIDorIndex, classType), (SkillWheelExtensions.GetSkillLevel(tierIDorIndex, classType) <= 0) ? 1 : SkillWheelExtensions.GetSkillLevel(tierIDorIndex, classType));
				}
				if (tierIDorIndex == 5)
				{
					skillDefinition = "Eagle_Vision".ToEnum<SkillTypes>().GetDefinition();
				}
				if (tierIDorIndex == 6)
				{
					skillDefinition = "Call_Hireling".ToEnum<SkillTypes>().GetDefinition();
				}
				SkillNameMiddle.text = LocalizationManager.Instance.Get(skillDefinition.NameLocaKey.Key);
				SkillNameBody.text = ColorExtensions.ToNGUIColorString(UIDataMapper.UITextColors.SkillColor) + LocalizationManager.Instance.Get(skillDefinition.NameLocaKey.Key);
				RuleData.IRuleDefinition ruleDefinition = skillDefinition.RuleDefinition;
				StringMapper stringMapper = new StringMapper();
				stringMapper.Add("rank", SkillWheelExtensions.GetSkillLevel(tierIDorIndex, classType));
				StringMapper stringMapper2 = stringMapper;
				stringMapper2.Append(ruleDefinition.GetStringMapperForRules(skillDefinition.DescriptionLocaKey.Key, 0));
				SkillDescriptionBody.text = LocalizationManager.Instance.Get(skillDefinition.DescriptionLocaKey.Key, stringMapper2);
				SkillIconBody.mainTexture = skillDefinition.IconUp.AccessObject<Texture2D>();
				if (tierIDorIndex < 5)
				{
					SkillLevelBody.text = SkillWheelExtensions.GetSkillLevel(tierIDorIndex, classType) + " / " + 5;
				}
				else
				{
					SkillLevelBody.text = string.Empty;
				}
			}
			else
			{
				SkillNameMiddle.text = string.Empty;
				SkillNameBody.text = string.Empty;
				UIExtendedLabel skillDescriptionBody = SkillDescriptionBody;
				string text = LocalizationManager.Instance.Get(1829);
				SkillDescriptionBody.text = text;
				skillDescriptionBody.text = text;
				SkillLevelBody.text = string.Empty;
				SkillIconBody.ClearTexture();
			}
		}

		private void SendDataAsynchronous()
		{
			Assassin currentAssassin = Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin;
			SkillWheel currentSkillwheel = currentAssassin.skillWheel;
			UINetworkAdaptor.BlockedNetworkRequest(delegate(AsyncCallback _asyncResult)
			{
				Singleton<StateManager>.Instance.ProfileManager.UpdateSkillWheel(currentAssassin.id, currentSkillwheel, _asyncResult);
			}, null, delegate(IAsyncResult _result)
			{
				if (!_result.IsError())
				{
					CloseScreen();
				}
			});
		}

		private void DeactivateAllHighLights()
		{
			for (int i = 0; i < m_skillWheelButtons.Count; i++)
			{
				m_skillWheelButtons[i].Highlight.alpha = 0f;
				m_skillWheelButtons[i].HighLightTween.value = 0f;
			}
			if (m_lastClickedSkillModel != null)
			{
				m_lastClickedSkillModel.Highlight.ResetToBeginning();
			}
		}

		private void CloseScreen()
		{
			EScreenID screenId = Singleton<UIScreenManager>.Instance.PreviousScreen.ScreenId;
			Singleton<UIScreenManager>.Instance.ChangeScreen(screenId, new UITweenHandle(null, null));
			OptionsMenuNavigation.OverlayMenuClosed();
			Singleton<UIScreenManager>.Instance.SceneData.UIMenuBars.Show();
			DeactivateAllHighLights();
		}
	}
}
