using System;
using System.Collections;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Database;
using Assets.Scripts.UI.Behaviours;
using Assets.Scripts.UI.Loading;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Network;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.ScreenManager.SceenPlaceholder;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.ProfileManager;
using Assets.Scripts.UI.Utils.Development;
using Assets.Scripts.UI.Utils.Extensions;
using Database;
using Humanoids;
using UbiCore.Utils;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.AssassinNamingScreen
{
	public class UICreateAssassinDetails : UIScreenChild
	{
		public delegate void SimpleVoidDelegate();

		[SerializeField]
		[CheckAssigned]
		private UIExtendedButton m_randomButton;

		[SerializeField]
		[CheckAssigned]
		private UIExtendedButton m_createButton;

		[CheckAssigned]
		[SerializeField]
		private UIButton m_nextButton;

		[CheckAssigned]
		[SerializeField]
		private UIButton m_PreviousButton;

		[CheckAssigned]
		[SerializeField]
		private EScreenID m_nextScreenID;

		[CheckAssigned]
		[SerializeField]
		private List<UISprite> m_headIndex;

		[CheckAssigned]
		[SerializeField]
		private UIWidget m_LoadAnimContainer;

		[CheckAssigned]
		[SerializeField]
		private ClassChooseContainer m_classChooseContainer;

		[CheckAssigned]
		[SerializeField]
		private UIInput m_inputField;

		private int[] m_firstNameLocaIds;

		private int[] m_lastNameLocaIds;

		private int m_currentFirstNameLocaId;

		private int m_currentLastNameLocaId;

		private int m_currentHeadIdx;

		private AvatarClassDefinitions m_avatarDefinitions;

		private GameObject m_currentAvatar;

		private bool m_isCreatingAssassin;

		private bool m_isScreenActive;

		private LoadAnimationManager m_loadAnimMgr;

		public UIRotateObject AssassinRotater;

		private string CurrentHeadGuid
		{
			get
			{
				AvatarClassDefinitions.AvatarClass definitionByType = m_avatarDefinitions.GetDefinitionByType(Singleton<StateManager>.Instance.AssassinCreationSelection.AvatarType);
				return definitionByType.heads.headOptions[m_currentHeadIdx];
			}
		}

		public event SimpleVoidDelegate OnAssassinCreateFailed;

		public event SimpleVoidDelegate OnAssassinCreateSuccess;

		protected override void OnScreenInit()
		{
			if ((bool)m_screen)
			{
				EventDelegate.Add(m_screen.onActivate, OnScreenActivated);
				EventDelegate.Add(m_screen.onDeActivate, OnScreenDeactivated);
			}
			m_inputField.IgnoreDeselectionsFromGameObject = m_randomButton.gameObject;
			m_loadAnimMgr = LoadAnimationManager.CreateNewInstance(base.transform);
		}

		private void OnChangedCustomName()
		{
			m_createButton.isEnabled = m_inputField.value != string.Empty;
		}

		private void OnDeselectInput()
		{
			if (string.IsNullOrEmpty(m_inputField.value))
			{
				m_inputField.value = m_inputField.defaultText;
				m_createButton.isEnabled = m_inputField.value != string.Empty;
			}
			else
			{
				m_inputField.defaultText = m_inputField.value;
			}
		}

		private void OnSubmitCustomName()
		{
			if (!string.IsNullOrEmpty(m_inputField.value))
			{
				m_inputField.defaultText = m_inputField.value;
			}
		}

		protected override void OnScreenDestroy()
		{
			if ((bool)m_screen)
			{
				EventDelegate.Remove(m_screen.onActivate, OnScreenActivated);
				EventDelegate.Remove(m_screen.onDeActivate, OnScreenDeactivated);
			}
			m_loadAnimMgr = null;
		}

		private void OnScreenActivated()
		{
			m_isScreenActive = true;
			if (Singleton<StateManager>.Instance.AssassinCreationSelection.Valid)
			{
				m_currentHeadIdx = 0;
				SetButtonState(false);
				m_classChooseContainer.Init(Singleton<StateManager>.Instance.AssassinCreationSelection.AvatarType);
				m_classChooseContainer.SetStaticViewing();
				EventDelegate.Add(m_inputField.onSubmit, OnSubmitCustomName);
				EventDelegate.Add(m_inputField.onChange, OnChangedCustomName);
				EventDelegate.Add(m_inputField.onDeselect, OnDeselectInput);
				SetupScreen();
			}
		}

		private void OnScreenDeactivated()
		{
			m_isScreenActive = false;
			EnableButtonListeners(false);
			StopAllCoroutines();
			Singleton<StateManager>.Instance.AssassinCreationSelection.Clear();
			EventDelegate.Remove(m_inputField.onSubmit, OnSubmitCustomName);
			EventDelegate.Remove(m_inputField.onChange, OnChangedCustomName);
			EventDelegate.Remove(m_inputField.onDeselect, OnDeselectInput);
			if ((bool)m_currentAvatar)
			{
				Singleton<HumanoidFactory>.Instance.DestroyHumanoid(m_currentAvatar);
				m_currentAvatar = null;
			}
			m_firstNameLocaIds = null;
			m_lastNameLocaIds = null;
			m_avatarDefinitions = null;
			m_currentHeadIdx = -1;
			m_isCreatingAssassin = false;
			m_loadAnimMgr.RemoveLoadAnim(new LoadAnimationManager.SLoadObjectInfo(m_LoadAnimContainer));
		}

		private void EnableButtonListeners(bool _enable)
		{
			if (_enable)
			{
				EventDelegate.Add(m_randomButton.onClick, OnRandomButton);
				EventDelegate.Add(m_createButton.onClick, OnCreateButton);
				EventDelegate.Add(m_PreviousButton.onClick, OnPreviousButton);
				EventDelegate.Add(m_nextButton.onClick, OnNextButton);
			}
			else
			{
				EventDelegate.Remove(m_randomButton.onClick, OnRandomButton);
				EventDelegate.Remove(m_createButton.onClick, OnCreateButton);
				EventDelegate.Remove(m_PreviousButton.onClick, OnPreviousButton);
				EventDelegate.Remove(m_nextButton.onClick, OnNextButton);
			}
		}

		private void OnRandomButton()
		{
			RandomizeName();
		}

		private void RandomizeName()
		{
			m_createButton.isEnabled = true;
			m_currentFirstNameLocaId = m_firstNameLocaIds[UnityEngine.Random.Range(0, m_firstNameLocaIds.Length - 1)];
			m_currentLastNameLocaId = m_lastNameLocaIds[UnityEngine.Random.Range(0, m_lastNameLocaIds.Length - 1)];
			m_inputField.value = BuildName(m_currentFirstNameLocaId, m_currentLastNameLocaId);
			m_inputField.defaultText = m_inputField.value;
			if (!m_inputField.isSelected || !TouchScreenKeyboard.visible)
			{
				m_inputField.isSelected = true;
			}
		}

		private void OnCreateButton()
		{
			CreateAssassinAsync(Singleton<StateManager>.Instance.AssassinCreationSelection.AvatarType, CurrentHeadGuid, m_inputField.value, m_currentFirstNameLocaId, m_currentLastNameLocaId);
		}

		private void OnPreviousButton()
		{
			if (!m_isCreatingAssassin)
			{
				m_currentHeadIdx--;
				if (m_currentHeadIdx < 0)
				{
					AvatarClassDefinitions.AvatarClass definitionByType = m_avatarDefinitions.GetDefinitionByType(Singleton<StateManager>.Instance.AssassinCreationSelection.AvatarType);
					m_currentHeadIdx = definitionByType.heads.headOptions.Length - 1;
				}
				UpdateHead();
			}
		}

		private void OnNextButton()
		{
			if (!m_isCreatingAssassin)
			{
				m_currentHeadIdx++;
				AvatarClassDefinitions.AvatarClass definitionByType = m_avatarDefinitions.GetDefinitionByType(Singleton<StateManager>.Instance.AssassinCreationSelection.AvatarType);
				if (m_currentHeadIdx > definitionByType.heads.headOptions.Length - 1)
				{
					m_currentHeadIdx = 0;
				}
				UpdateHead();
			}
		}

		private void SetButtonState(bool _enabled)
		{
			m_randomButton.isEnabled = _enabled;
			m_createButton.isEnabled = _enabled;
			m_PreviousButton.isEnabled = _enabled;
			m_nextButton.isEnabled = _enabled;
		}

		private void CreateAssassinAsync(Assassin.EClassTypeId _avatarType, string _headGuid, string _customName, int _firstNameLocaId, int _lastNameLocaId)
		{
			ProfileManager profileMgr = Singleton<StateManager>.Instance.ProfileManager;
			UINetworkAdaptor.BlockedNetworkRequest(delegate(AsyncCallback _asyncResult)
			{
				profileMgr.CreateAssassin(_avatarType, _headGuid, _customName, _firstNameLocaId, _lastNameLocaId, _asyncResult);
			}, null, delegate(IAsyncResult _result)
			{
				if (_result.IsError())
				{
					if (this.OnAssassinCreateFailed != null)
					{
						this.OnAssassinCreateFailed();
					}
				}
				else
				{
					int num = profileMgr.Profile.assassins.assassins.Length - 1;
					profileMgr.SelectedAssassin = profileMgr.Profile.assassins.assassins[num].id;
					if (this.OnAssassinCreateSuccess != null)
					{
						this.OnAssassinCreateSuccess();
					}
					else
					{
						Singleton<UIScreenManager>.Instance.ChangeScreen(m_nextScreenID, new UITweenHandle());
					}
				}
			});
		}

		private void SetupScreen()
		{
			m_avatarDefinitions = Singleton<StateManager>.Instance.TemplateManager.AvatarDefinition;
			if (m_avatarDefinitions != null)
			{
				SetupPreview();
				SetupNames();
				SetButtonState(true);
				EnableButtonListeners(true);
			}
		}

		private void SetupPreview()
		{
			UpdateHead();
		}

		private void SetupNames()
		{
			AvatarClassDefinitions.AvatarClass definitionByType = m_avatarDefinitions.GetDefinitionByType(Singleton<StateManager>.Instance.AssassinCreationSelection.AvatarType);
			m_firstNameLocaIds = m_avatarDefinitions.avatarNameSettings.firstNameLocaIds;
			m_lastNameLocaIds = m_avatarDefinitions.avatarNameSettings.lastNameLocaIds;
			m_currentFirstNameLocaId = definitionByType.defaultFirstNameLocaId;
			m_currentLastNameLocaId = definitionByType.defaultLastNameLocaId;
			m_inputField.value = BuildName(m_currentFirstNameLocaId, m_currentLastNameLocaId);
			m_inputField.defaultText = m_inputField.value;
		}

		private string BuildName(int _firstNameLocaId, int _lastNameLocaId)
		{
			return BuildName(LocalizationManager.Instance.Get(_firstNameLocaId), LocalizationManager.Instance.Get(_lastNameLocaId));
		}

		private string BuildName(string _firstName, string _lastName)
		{
			return string.Format("{0} {1}", _firstName, _lastName);
		}

		private void UpdateHead()
		{
			MonoBehaviour.print(m_currentHeadIdx);
			for (int i = 0; i < m_headIndex.Count; i++)
			{
				m_headIndex[i].enabled = false;
			}
			m_headIndex[m_currentHeadIdx].enabled = true;
			AvatarClassDefinitions.AvatarClass definitionByType = m_avatarDefinitions.GetDefinitionByType(Singleton<StateManager>.Instance.AssassinCreationSelection.AvatarType);
			HumanoidDef humanoidDef = Main.Instance.GameDB.GetItem<HumanoidDef>(Main.Instance.GameDB.FindGameIdFromGuid(definitionByType.humanoidGuid));
			HeadDef item = Main.Instance.GameDB.GetItem<HeadDef>(Main.Instance.GameDB.FindGameIdFromGuid(CurrentHeadGuid));
			if (humanoidDef == null || item == null)
			{
				return;
			}
			CharacterAppearance appearance = new CharacterAppearance(humanoidDef, item);
			ColorDef item2 = Main.SafeGameDb.GetItem<ColorDef>(Main.SafeGameDb.FindGameIdFromGuid(definitionByType.colorSetting));
			if (item2 != null)
			{
				appearance.Color0 = item2.Color0;
				appearance.Color1 = item2.Color1;
			}
			m_isCreatingAssassin = true;
			m_loadAnimMgr.AddLoadAnim(new LoadAnimationManager.SLoadObjectInfo(m_LoadAnimContainer));
			humanoidDef.AssetBundleInfo.Provide(delegate(bool _success)
			{
				if (_success && m_isScreenActive)
				{
					StartCoroutine(CreateAssassin(humanoidDef, appearance));
				}
				else
				{
					m_isCreatingAssassin = false;
					m_loadAnimMgr.RemoveLoadAnim(new LoadAnimationManager.SLoadObjectInfo(m_LoadAnimContainer));
				}
			});
		}

		private IEnumerator CreateAssassin(HumanoidDef _def, ICharacterAppearance _appearance)
		{
			yield return StartCoroutine(Singleton<HumanoidFactory>.Instance.PreloadHumanoidFromDefinitionAsync(_def, _appearance));
			GameObject avatar = Singleton<HumanoidFactory>.Instance.CreateHumanoidFromDefinition(_def, true, _appearance, null, UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_animatorPlayerAnimus);
			if (avatar != null)
			{
				if ((bool)m_currentAvatar)
				{
					Animator current_Animator = m_currentAvatar.GetComponent<Animator>();
					if (current_Animator != null)
					{
						AnimatorStateInfo asi = current_Animator.GetCurrentAnimatorStateInfo(0);
						Animator new_Animator = avatar.GetComponent<Animator>();
						if (new_Animator != null && asi.fullPathHash != 0)
						{
							new_Animator.Play(asi.fullPathHash, 0, asi.normalizedTime);
						}
					}
					Singleton<HumanoidFactory>.Instance.DestroyHumanoid(m_currentAvatar);
				}
				UIPlaceholder placeholder = Singleton<UIScreenManager>.Instance.SceneData.Placeholders.GetPlaceholder(UIPlaceholders.EPlaceholderType.CreateAssassin);
				m_currentAvatar = avatar;
				m_currentAvatar.transform.parent = placeholder.transform;
				m_currentAvatar.transform.localPosition = Vector3.zero;
				m_currentAvatar.transform.localRotation = Quaternion.identity;
				m_currentAvatar.transform.localScale = Vector3.one;
				AssassinRotater.Object = avatar;
				ObjectFade cf = m_currentAvatar.AddComponentIfNotExist<ObjectFade>();
				cf.FadeMode = EFadeMode.Fancy_EFadeMode;
				cf.FadeDuration = 1.5f;
				cf.OnFadeDoneCallback = ObjectFadeCallbacks.OnFadeFinishedDestroySelf;
				cf.FadeIn();
				if (m_currentAvatar != null)
				{
					Renderer[] renderers = m_currentAvatar.GetComponentsInChildren<Renderer>();
					for (int i = 0; i < renderers.Length; i++)
					{
						renderers[i].enabled = false;
					}
				}
			}
			m_isCreatingAssassin = false;
			m_loadAnimMgr.RemoveLoadAnim(new LoadAnimationManager.SLoadObjectInfo(m_LoadAnimContainer));
		}
	}
}
