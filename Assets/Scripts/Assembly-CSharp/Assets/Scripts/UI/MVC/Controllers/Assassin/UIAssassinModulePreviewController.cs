using System.Collections;
using AcierProtocol;
using Assets.Scripts.Animation;
using Assets.Scripts.UI.Behaviours;
using Assets.Scripts.UI.Loading;
using Assets.Scripts.UI.ScreenManager.SceenPlaceholder;
using Assets.Scripts.UI.Types;
using Assets.Scripts.UI.Utils.Development;
using Assets.Scripts.UI.Utils.Extensions;
using Database;
using Humanoids;
using SharedBaseLib.System;
using UbiCore.Utils;
using UbiCore.Utils.Extensions;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.UI.MVC.Controllers.Assassin
{
	public class UIAssassinModulePreviewController : UIControllerBase
	{
		public delegate void AssassinLoadedCallback(GameObject _obj);

		private static Quaternion s_assassinRotation = Quaternion.identity;

		private static int s_assassinAnimationNameHash;

		private static float s_assassinAnimationNormalizedTime;

		private static GameObject s_transferredAssassin;

		private static bool s_weaponDrawn;

		[CheckAssigned(NullAllowed = true)]
		[SerializeField]
		private Material m_placeHolderMaterial;

		[CheckAssigned]
		[SerializeField]
		private UIWidget m_loadingAnimContainer;

		[CheckAssigned]
		[SerializeField]
		private UIRotateObject m_assassinRotater;

		private bool m_isWaitingToSetupAvatars;

		private GameObject m_currentAvatar;

		private AssassinLoadedCallback m_assassinLoadedCallback;

		private LoadAnimationManager.SLoadObjectInfo m_loadInfo;

		private LoadAnimationManager m_loadAninMgr;

		private bool m_showAvatarWhenCreated;

		private bool m_shouldResetIdleAnimationOnLoad;

		public UIPlaceholder Placeholder { get; set; }

		public UIAssassinRankDisplay.AssassinHandle AssassinHandle { get; set; }

		public bool AllowTransition { get; set; }

		public bool IsCreatingAssassin { get; private set; }

		private void Init()
		{
			if (m_loadAninMgr == null)
			{
				m_loadAninMgr = LoadAnimationManager.CreateNewInstance(base.transform);
			}
		}

		public void UpdateController(UIAssassinRankDisplay.AssassinHandle _assassinHandle, bool _allowTransition, UIPlaceholder _placeholder)
		{
			Init();
			base.IsActive = true;
			Placeholder = _placeholder;
			AssassinHandle = _assassinHandle;
			AllowTransition = _allowTransition;
			DisableScreen();
			SetupScreen();
			s_transferredAssassin = null;
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

		protected override void OnAwakeUnity()
		{
			base.OnAwakeUnity();
			Init();
		}

		protected override void OnDestroyUnity()
		{
			base.OnDestroyUnity();
			m_loadAninMgr = null;
		}

		protected override void OnUpdateUnity()
		{
			base.OnUpdateUnity();
			StoreAssassinRotation();
		}

		protected override void OnDisableUnity()
		{
			base.OnDisableUnity();
			base.IsActive = false;
			DisableScreen();
			StopAllCoroutines();
		}

		private void DisableScreen()
		{
			ClearAvatars();
		}

		private void SetupScreen()
		{
			if (!AllowTransition)
			{
				s_assassinRotation = Quaternion.identity;
			}
			HumanoidData.PreloadAdditionalPrefab(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_animatorPlayerAnimus);
			SetupAvatars(AllowTransition, false);
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
				if (m_assassinRotater != null)
				{
					m_assassinRotater.Object = null;
				}
			}
		}

		private void SetupAvatars(bool _transition, bool _weaponAnimation)
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
			if (AssassinHandle == null)
			{
				return;
			}
			UIAssassinRankDisplay.AssassinHandle handle = AssassinHandle;
			if (s_transferredAssassin == null)
			{
				CreateAssassin(handle, _transition, delegate(GameObject _obj)
				{
					m_currentAvatar = _obj;
					AnimatorSoundStart animatorSoundStart = m_currentAvatar.AddComponentIfNotExist<AnimatorSoundStart>();
					animatorSoundStart.Attach();
					if (m_assassinRotater != null)
					{
						m_assassinRotater.Object = _obj;
					}
					if (!handle.IsPlaceHolder && handle.RawAssassin != null && handle.RawAssassin.equippedElements != null)
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
				});
			}
			else
			{
				m_currentAvatar = s_transferredAssassin;
				if (m_assassinRotater != null)
				{
					m_assassinRotater.Object = s_transferredAssassin;
				}
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
				if (_handle.IsPlaceHolder)
				{
					humanoidToUse = Main.Instance.GameDB.GetItem<HumanoidDef>(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_silhouette);
				}
				else
				{
					humanoidToUse = _handle.RawAssassin.FinalizeAppearance(_handle.Appearance);
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
			if (_success && base.IsActive)
			{
				Main.Instance.StartCoroutine(InstanciateAssassinAsync(_handle, _humanoidDef, _transition, m_assassinLoadedCallback));
				return;
			}
			m_loadAninMgr.RemoveLoadAnim(m_loadInfo);
			m_assassinLoadedCallback = null;
			IsCreatingAssassin = false;
			if (!base.IsActive)
			{
				DisableScreen();
			}
		}

		private IEnumerator InstanciateAssassinAsync(UIAssassinRankDisplay.AssassinHandle _handle, HumanoidDef _def, bool _transition, AssassinLoadedCallback _assassinLoadedCallback)
		{
			yield return Main.Instance.StartCoroutine(Singleton<HumanoidFactory>.Instance.PreloadHumanoidFromDefinitionAsync(_def, _handle.Appearance));
			GameObject obj = null;
			if (base.IsActive)
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
			gameObject.transform.parent = Placeholder.transform;
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

		private IEnumerator QueueSetupAvatars(bool _transition)
		{
			m_isWaitingToSetupAvatars = true;
			WaitForEndOfFrame wait = new WaitForEndOfFrame();
			while (IsCreatingAssassin)
			{
				yield return wait;
			}
			m_isWaitingToSetupAvatars = false;
			SetupAvatars(_transition, false);
		}
	}
}
