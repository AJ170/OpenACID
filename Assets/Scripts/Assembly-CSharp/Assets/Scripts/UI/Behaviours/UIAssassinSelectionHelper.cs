using System.Collections;
using AcierProtocol;
using Assets.Scripts.Animation;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.EnvironmentBridge;
using Assets.Scripts.UI.Loading;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.ScreenManager.SceenPlaceholder;
using Assets.Scripts.UI.Screens.AssassinSelectionScreen;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Types;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using Database;
using Humanoids;
using SharedBaseLib.System;
using UbiCore.Utils;
using UbiCore.Utils.Extensions;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.UI.Behaviours
{
	public class UIAssassinSelectionHelper : UIScreenChild
	{
		public delegate void AssassinLoadedCallback(GameObject _obj);

		[SerializeField]
		private Material m_placeHolderMaterial;

		[SerializeField]
		private int m_assassinOffset;

		[SerializeField]
		private int m_backgroundSize;

		[SerializeField]
		private UIWidget m_loadingAnimContainer;

		public UIRotateObject AssassinRotater;

		private CameraMenuManager m_cameraManager;

		private bool m_isScreenActive;

		private GameObject m_previousAvatar;

		private GameObject m_currentAvatar;

		private Transform m_background;

		private AssassinLoadedCallback m_assassinLoadedCallback;

		private bool m_isWaitingToSetupAvatars;

		private LoadAnimationManager.SLoadObjectInfo m_loadInfo;

		private LoadAnimationManager m_loadAninMgr;

		private bool m_isCreatingAssassin;

		private bool m_showAvatarWhenCreated;

		private bool m_shouldResetIdleAnimationOnLoad;

		private static Quaternion s_assassinRotation = Quaternion.identity;

		private static int s_assassinAnimationNameHash;

		private static float s_assassinAnimationNormalizedTime;

		private static GameObject s_transferredAssassin;

		private static bool s_weaponDrawn;

		private bool IsCreatingAssassin
		{
			get
			{
				return m_isCreatingAssassin;
			}
			set
			{
				m_isCreatingAssassin = value;
				Singleton<UIScreenManager>.Instance.SceneData.AssassinContainer.Display.CanSwitchAssassins = !m_isCreatingAssassin;
			}
		}

		public void UpdateAssassin(bool _transition, bool _weaponEquiped)
		{
			DisableEventHandlers();
			SetupAvatars(_transition, false, false, _weaponEquiped);
			EnableEventHandlers();
		}

		public void FadeAvatar(bool _fadeIn, float _duration)
		{
			m_showAvatarWhenCreated = _fadeIn;
			if (m_currentAvatar != null)
			{
				ObjectFade fade = m_currentAvatar.AddComponentIfNotExist<ObjectFade>();
				m_currentAvatar.gameObject.SetActive(true);
				m_currentAvatar.transform.localPosition = Vector3.zero;
				float duration = fade.FadeDuration;
				EFadeMode mode = fade.FadeMode;
				fade.FadeMode = EFadeMode.Simple_EFadeMode;
				fade.FadeDuration = _duration;
				fade.OnFadeDoneCallback = delegate
				{
					fade.FadeDuration = duration;
					fade.FadeMode = mode;
				};
				if (_fadeIn)
				{
					fade.FadeIn();
				}
				else
				{
					fade.FadeOut();
				}
			}
		}

		protected override void OnScreenInit()
		{
			m_loadAninMgr = LoadAnimationManager.CreateNewInstance(base.transform);
			m_cameraManager = BehaviourSingleton<EnvironmentBridgeMain>.Instance.CameraMenuManager;
			m_background = BehaviourSingleton<EnvironmentBridgeMain>.Instance.Background;
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
			m_loadAninMgr = null;
			m_cameraManager = null;
			m_background = null;
		}

		private void Update()
		{
			StoreAssassinRotation();
		}

		private void OnDisable()
		{
			DisableScreen();
		}

		private void OnDestroy()
		{
			m_loadAninMgr = null;
			m_cameraManager = null;
			m_background = null;
		}

		private void DisableScreen()
		{
			DisableEventHandlers();
			ClearAvatars();
		}

		private void SetupAvatars(bool _transition, bool _newSlot, bool _gotoFirstEmptySlot, bool _weaponAnimation)
		{
			if (IsCreatingAssassin)
			{
				if (!m_isWaitingToSetupAvatars)
				{
					Main.Instance.StartCoroutine(QueueSetupAvatars(_transition));
				}
				return;
			}
			if (s_transferredAssassin == null)
			{
				ClearAvatars();
			}
			if (!Singleton<UIScreenManager>.Instance.SceneData.AssassinContainer.Display.HasAssassins)
			{
				return;
			}
			Singleton<UIScreenManager>.Instance.SceneData.AssassinContainer.Display.FocusValidAssassin(_newSlot, _gotoFirstEmptySlot);
			UIAssassinRankDisplay.AssassinHandle handle = Singleton<UIScreenManager>.Instance.SceneData.AssassinContainer.Display.Current;
			if (s_transferredAssassin == null)
			{
				CreateAssassin(handle, _transition, delegate(GameObject _obj)
				{
					m_currentAvatar = _obj;
					AnimatorSoundStart animatorSoundStart = m_currentAvatar.AddComponentIfNotExist<AnimatorSoundStart>();
					animatorSoundStart.Attach();
					if (AssassinRotater != null)
					{
						AssassinRotater.Object = _obj;
					}
					if (!handle.IsPlaceHolder && handle.RawAssassin != null)
					{
						Singleton<StateManager>.Instance.ProfileManager.SelectedAssassin = handle.RawAssassin.id;
						if (handle.RawAssassin.equippedElements != null)
						{
							for (int i = 0; i < handle.RawAssassin.equippedElements.Length; i++)
							{
								EquippedElement equippedElement = handle.RawAssassin.equippedElements[i];
								if (equippedElement.slot == EInventorySlot.SlotWeapon)
								{
									PlayerPreviewIdleBehaviour component = _obj.GetComponent<PlayerPreviewIdleBehaviour>();
									if (component != null)
									{
										if (_weaponAnimation)
										{
											component.EquipSword();
										}
										else if (s_weaponDrawn)
										{
											component.CancelSwordAnimation();
										}
										s_weaponDrawn = false;
										component.WeaponHandler.WeaponDrawn = s_weaponDrawn;
									}
									break;
								}
							}
						}
					}
				});
			}
			else
			{
				m_currentAvatar = s_transferredAssassin;
				if (AssassinRotater != null)
				{
					AssassinRotater.Object = s_transferredAssassin;
				}
			}
		}

		private void SetupScreen()
		{
			bool transition = true;
			if (Singleton<UIScreenManager>.Instance.PreviousScreen != null)
			{
				switch (Singleton<UIScreenManager>.Instance.PreviousScreen.ScreenId)
				{
				case EScreenID.AssassinSelection:
				case EScreenID.AssasinStats:
				case EScreenID.AssassinInventory:
				case EScreenID.AssassinModule:
					transition = false;
					break;
				default:
					s_assassinRotation = Quaternion.identity;
					break;
				}
			}
			HumanoidData.PreloadAdditionalPrefab(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_animatorPlayerAnimus);
			SetupAvatars(transition, false, false, false);
			Singleton<UIScreenManager>.Instance.SceneData.AssassinContainer.Display.Refresh();
			EnableEventHandlers();
		}

		private void EnableEventHandlers()
		{
			EventDelegate.Add(AssassinCreationHandler.onAssassinSlotPurchased, OnAssassinSlotBought);
			UIDeleteAssassin.OnAssassinDeleted += OnAssassinDeleted;
			Singleton<UIScreenManager>.Instance.SceneData.AssassinContainer.Display.OnSelectionChanged += OnSelectionChanged;
			if (m_cameraManager != null)
			{
				m_cameraManager.OnChangeEnded += OnCameraEnd;
			}
		}

		private void DisableEventHandlers()
		{
			EventDelegate.Remove(AssassinCreationHandler.onAssassinSlotPurchased, OnAssassinSlotBought);
			UIDeleteAssassin.OnAssassinDeleted -= OnAssassinDeleted;
			if (Singleton<UIScreenManager>.Instance.SceneData != null)
			{
				Singleton<UIScreenManager>.Instance.SceneData.AssassinContainer.Display.OnSelectionChanged -= OnSelectionChanged;
			}
			if (m_cameraManager != null)
			{
				m_cameraManager.OnChangeEnded -= OnCameraEnd;
			}
		}

		private bool ScreenHasAssassin(EScreenID _screenId)
		{
			switch (_screenId)
			{
			case EScreenID.MissionLoadout:
			case EScreenID.AssassinSelection:
			case EScreenID.AssasinStats:
			case EScreenID.AssassinInventory:
			case EScreenID.InventoryScreen:
				return true;
			default:
				return false;
			}
		}

		private void ClearAvatars()
		{
			if ((bool)m_currentAvatar)
			{
				if (s_transferredAssassin == null)
				{
					Singleton<HumanoidFactory>.Instance.DestroyHumanoid(m_currentAvatar);
				}
				m_currentAvatar = null;
				if (AssassinRotater != null)
				{
					AssassinRotater.Object = null;
				}
			}
			if ((bool)m_previousAvatar)
			{
				Singleton<HumanoidFactory>.Instance.DestroyHumanoid(m_previousAvatar);
				m_previousAvatar = null;
			}
		}

		private void CreateAssassin(UIAssassinRankDisplay.AssassinHandle _handle, bool _transition, AssassinLoadedCallback _assassinLoadedCallback)
		{
			if (!IsCreatingAssassin)
			{
				IsCreatingAssassin = true;
				m_showAvatarWhenCreated = true;
				m_loadInfo = new LoadAnimationManager.SLoadObjectInfo(m_loadingAnimContainer);
				m_loadAninMgr.AddLoadAnim(m_loadInfo);
				HumanoidDef humanoidToUse;
				if (!_handle.IsPlaceHolder)
				{
					humanoidToUse = _handle.RawAssassin.FinalizeAppearance(_handle.Appearance);
				}
				else
				{
					humanoidToUse = Main.Instance.GameDB.GetItem<HumanoidDef>(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_silhouette);
				}
				m_assassinLoadedCallback = _assassinLoadedCallback;
				humanoidToUse.AssetBundleInfo.Provide(delegate(bool _success)
				{
					Main.Instance.StartCoroutine(LoadEquipedItems(_success, _handle, humanoidToUse, _transition));
				});
			}
		}

		private IEnumerator LoadEquipedItems(bool _success, UIAssassinRankDisplay.AssassinHandle _handle, HumanoidDef _humanoidDef, bool _transition)
		{
			if (!_handle.IsPlaceHolder)
			{
				bool isWaitingForSword = false;
				bool isWaitingForInsignia = false;
				GameDB db = Main.Instance.GameDB;
				EquippedElement insigniaItem = _handle.RawAssassin.equippedElements.Find((EquippedElement _item) => _item.slot == EInventorySlot.SlotInsignia);
				bool _success2 = default(bool);
				if (insigniaItem != null)
				{
					UIGUID insigniaGuid = new UIGUID(insigniaItem.element.guid);
					if (insigniaGuid.Valid)
					{
						GameID itemId2 = db.FindGameIdFromGuid(insigniaGuid);
						ItemDef itemDef2 = Main.Instance.GameDB.GetItem<ItemDef>(itemId2);
						isWaitingForInsignia = true;
						itemDef2.AssetBundleInfo.Provide(delegate(bool _successItemDef)
						{
							_success2 |= _successItemDef;
							isWaitingForInsignia = false;
						});
					}
				}
				EquippedElement swordItem = _handle.RawAssassin.equippedElements.Find((EquippedElement _item) => _item.slot == EInventorySlot.SlotWeapon);
				if (swordItem != null)
				{
					UIGUID swordGuid = new UIGUID(swordItem.element.guid);
					if (swordGuid.Valid)
					{
						GameID itemId = db.FindGameIdFromGuid(swordGuid);
						ItemDef itemDef = Main.Instance.GameDB.GetItem<ItemDef>(itemId);
						isWaitingForSword = true;
						itemDef.AssetBundleInfo.Provide(delegate(bool _successItemDef)
						{
							_success2 |= _successItemDef;
							isWaitingForSword = false;
						});
					}
				}
				WaitForEndOfFrame wait = new WaitForEndOfFrame();
				while (isWaitingForSword || isWaitingForInsignia)
				{
					yield return wait;
				}
			}
			OnAssetBundlesLoaded(_success, _handle, _humanoidDef, _transition);
		}

		private void OnAssetBundlesLoaded(bool _success, UIAssassinRankDisplay.AssassinHandle _handle, HumanoidDef _humanoidDef, bool _transition)
		{
			if (_success && m_isScreenActive)
			{
				Main.Instance.StartCoroutine(InstanciateAssassinAsync(_handle, _humanoidDef, _transition, m_assassinLoadedCallback));
				return;
			}
			m_loadAninMgr.RemoveLoadAnim(m_loadInfo);
			m_assassinLoadedCallback = null;
			IsCreatingAssassin = false;
			if (!m_isScreenActive)
			{
				DisableScreen();
			}
		}

		private IEnumerator InstanciateAssassinAsync(UIAssassinRankDisplay.AssassinHandle _handle, HumanoidDef _def, bool _transition, AssassinLoadedCallback _assassinLoadedCallback)
		{
			yield return Main.Instance.StartCoroutine(Singleton<HumanoidFactory>.Instance.PreloadHumanoidFromDefinitionAsync(_def, _handle.Appearance));
			GameObject obj = null;
			if (m_isScreenActive)
			{
				obj = InstanciateAssassin(_handle, _def, _transition);
				if (_assassinLoadedCallback != null)
				{
					_assassinLoadedCallback(obj);
				}
			}
			else
			{
				DisableScreen();
			}
			m_loadAninMgr.RemoveLoadAnim(m_loadInfo);
			m_assassinLoadedCallback = null;
			IsCreatingAssassin = false;
			if (_transition && obj != null)
			{
				Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
				for (int i = 0; i < renderers.Length; i++)
				{
					renderers[i].enabled = false;
				}
			}
		}

		private GameObject InstanciateAssassin(UIAssassinRankDisplay.AssassinHandle _handle, HumanoidDef _def, bool _transition)
		{
			GameObject gameObject = Singleton<HumanoidFactory>.Instance.CreateHumanoidFromDefinition(_def, true, _handle.Appearance, null, UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_animatorPlayerAnimus);
			int num = 0;
			if (!_handle.IsPlaceHolder && _handle.RawAssassin.equippedElements != null)
			{
				for (int i = 0; i < _handle.RawAssassin.equippedElements.Length; i++)
				{
					EquippedElement equippedElement = _handle.RawAssassin.equippedElements[i];
					if (equippedElement.slot == EInventorySlot.SlotWeapon)
					{
						GameID gameID = Main.SafeGameDb.FindGameIdFromGuid(equippedElement.element.guid);
						ItemDef item = Main.Instance.GameDB.GetItem<ItemDef>(gameID);
						WeaponHandler weaponHandler = gameObject.AddComponentIfNotExist<WeaponHandler>();
						weaponHandler.Initialize(item);
						num = s_assassinAnimationNameHash;
						break;
					}
				}
			}
			gameObject.GetComponent<NavMeshAgent>().enabled = false;
			gameObject.SetLayerRecursively(Layers.CharacterLayer);
			UIPlaceholders.EPlaceholderType type = ((!UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.HasCompletedAssassinCreation) ? UIPlaceholders.EPlaceholderType.CreateAssassinStencil : UIPlaceholders.EPlaceholderType.Assassin);
			UIPlaceholder placeholder = Singleton<UIScreenManager>.Instance.SceneData.Placeholders.GetPlaceholder(type);
			gameObject.transform.parent = placeholder.transform;
			gameObject.transform.localScale = Vector3.one;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = s_assassinRotation;
			PlayerPreviewIdleBehaviour playerPreviewIdleBehaviour = gameObject.AddComponentIfNotExist<PlayerPreviewIdleBehaviour>();
			if (!m_shouldResetIdleAnimationOnLoad && num != 0)
			{
				Animator component = gameObject.GetComponent<Animator>();
				component.Play(num, 0, s_assassinAnimationNormalizedTime);
			}
			else
			{
				playerPreviewIdleBehaviour.CancelSwordAnimation();
			}
			m_shouldResetIdleAnimationOnLoad = false;
			playerPreviewIdleBehaviour.enabled = !_handle.IsPlaceHolder;
			if (_handle.IsPlaceHolder && m_placeHolderMaterial != null)
			{
				Transform transform = gameObject.transform.FindChild("Body_LOD0");
				if (transform != null)
				{
					SkinnedMeshRenderer component2 = transform.GetComponent<SkinnedMeshRenderer>();
					if (component2 != null)
					{
						component2.sharedMaterials = new Material[2] { m_placeHolderMaterial, m_placeHolderMaterial };
					}
				}
			}
			gameObject.gameObject.SetActive(m_showAvatarWhenCreated);
			if (_transition)
			{
				ObjectFade objectFade = gameObject.AddComponentIfNotExist<ObjectFade>();
				objectFade.FadeMode = EFadeMode.Fancy_EFadeMode;
				objectFade.FadeDuration = 1.5f;
				objectFade.OnFadeDoneCallback = ObjectFadeCallbacks.OnFadeFinishedDestroySelf;
				objectFade.FadeIn();
			}
			return gameObject;
		}

		private void UpdateBackground(UIAssassinRankDisplay.EDirection _direction)
		{
			Vector3 position = m_background.transform.position;
			position.x += ((_direction != 0) ? (-m_assassinOffset) : m_assassinOffset);
			if (position.x <= (float)(-m_backgroundSize) || position.x >= (float)m_backgroundSize)
			{
				position.x = 0f;
			}
			m_background.transform.position = position;
		}

		private void StoreAssassinRotation()
		{
			if (m_currentAvatar != null)
			{
				s_assassinRotation = m_currentAvatar.transform.localRotation;
				Animator component = m_currentAvatar.GetComponent<Animator>();
				AnimatorStateInfo currentAnimatorStateInfo = component.GetCurrentAnimatorStateInfo(0);
				s_assassinAnimationNameHash = currentAnimatorStateInfo.fullPathHash;
				s_assassinAnimationNormalizedTime = currentAnimatorStateInfo.normalizedTime;
				PlayerPreviewIdleBehaviour component2 = m_currentAvatar.GetComponent<PlayerPreviewIdleBehaviour>();
				if (component2 != null)
				{
					s_weaponDrawn = component2.WeaponHandler.WeaponDrawn;
					m_shouldResetIdleAnimationOnLoad = component2.IsPlayingWeaponEquipAnimation;
				}
			}
		}

		private void OnSelectionChanged(UIAssassinRankDisplay.EDirection _direction)
		{
			if (m_assassinLoadedCallback != null || IsCreatingAssassin)
			{
				return;
			}
			StoreAssassinRotation();
			m_previousAvatar = m_currentAvatar;
			float newX = 0f;
			if (m_previousAvatar != null)
			{
				newX = ((_direction != 0) ? (m_previousAvatar.transform.position.x - (float)m_assassinOffset) : (m_previousAvatar.transform.position.x + (float)m_assassinOffset));
			}
			CreateAssassin(Singleton<UIScreenManager>.Instance.SceneData.AssassinContainer.Display.Current, false, delegate(GameObject _obj)
			{
				if (m_previousAvatar != null)
				{
					m_previousAvatar.SetActive(false);
					m_previousAvatar.transform.position = new Vector3(newX, m_previousAvatar.transform.position.y, m_previousAvatar.transform.position.z);
				}
				m_currentAvatar = _obj;
				if (AssassinRotater != null)
				{
					AssassinRotater.Object = _obj;
				}
				m_cameraManager.SetCurrentInstant((_direction != 0) ? CameraMenuManager.EType.AssassinsStartTweenLeft : CameraMenuManager.EType.AssassinsStartTweenRight);
				UpdateBackground(_direction);
				m_cameraManager.Current = CameraMenuManager.EType.Assassins;
			});
		}

		private IEnumerator QueueSetupAvatars(bool _transition)
		{
			m_isWaitingToSetupAvatars = true;
			WaitForEndOfFrame wait = new WaitForEndOfFrame();
			while (IsCreatingAssassin)
			{
				yield return wait;
			}
			m_isWaitingToSetupAvatars = false;
			SetupAvatars(_transition, false, false, false);
		}

		private void OnScreenActivate()
		{
			Singleton<UIScreenManager>.Instance.SceneData.AssassinContainer.Display.Close();
			Singleton<UIScreenManager>.Instance.OnScreenChange += OnChangeScreen;
			m_isScreenActive = true;
			DisableScreen();
			SetupScreen();
			s_transferredAssassin = null;
		}

		private void OnScreenDeactivate()
		{
			if (Singleton<UIScreenManager>.Instance.SceneData != null)
			{
				Singleton<UIScreenManager>.Instance.SceneData.AssassinContainer.Display.Close();
				if (!Singleton<UIScreenManager>.Instance.SceneData.AssassinContainer.Display.ValidateAssassin())
				{
					s_transferredAssassin = null;
				}
			}
			Singleton<UIScreenManager>.Instance.OnScreenChange -= OnChangeScreen;
			m_isScreenActive = false;
			DisableScreen();
			StopAllCoroutines();
		}

		private void OnChangeScreen(EScreenID _from, EScreenID _to)
		{
			if (ScreenHasAssassin(_from) && ScreenHasAssassin(_to))
			{
				s_transferredAssassin = m_currentAvatar;
			}
			Singleton<UIScreenManager>.Instance.SceneData.AssassinContainer.Display.Close();
		}

		private void OnCameraEnd(CameraMenuManager.EType _eCurrent, CameraMenuManager.EType _eOld)
		{
			if ((bool)m_previousAvatar)
			{
				Singleton<HumanoidFactory>.Instance.DestroyHumanoid(m_previousAvatar);
			}
		}

		private void OnAssassinSlotBought()
		{
			SetupAvatars(true, true, false, false);
		}

		private void OnAssassinDeleted(UIAssasinId _assasinId)
		{
			SetupScreen();
		}
	}
}
