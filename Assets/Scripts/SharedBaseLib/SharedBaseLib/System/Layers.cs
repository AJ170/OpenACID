using System.Diagnostics;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace SharedBaseLib.System
{
	public static class Layers
	{
		public const string LayerNameDefault = "Default";

		public const string LayerNameInteractableUi = "UI_Interactable";

		public const string LayerNameNonInteractableUi = "UI_NonInteractable";

		public const string LayerNameCharacter = "Character";

		public const string LayerNameStaticOverlay = "StaticOverlay";

		public const string LayerNameStaticColliderNoShadow = "StaticCollider_NoShadow";

		public const string LayerNameStaticColliderNoShadowCameraSeeThrough = "StaticCollider_NoShadow_CameraSeeThrough";

		public const string LayerNameInteractors = "Interactors";

		public const string LayerNameCameraBlocker = "CameraBlocker";

		public const string LayerNameStaticColliderShadow = "StaticCollider_Shadow";

		public const string LayerNameOffMeshLink = "OffMeshLink";

		public const string LayerNameUI = "UI";

		public const string LayerNameUIShared = "UIShared";

		public const string LayerNameUI3D = "UI3D";

		public const string LayerNameMissionBarrier = "MissionBarrier";

		private static int m_defaultLayer = int.MinValue;

		private static int m_interactableUiLayer = int.MinValue;

		private static int m_nonInteractableUiLayer = int.MinValue;

		private static int m_characterLayer = int.MinValue;

		private static int m_staticOverlayLayer = int.MinValue;

		private static int m_staticColliderNoShadowLayer = int.MinValue;

		private static int m_staticColliderNoShadowCameraSeeThroughLayer = int.MinValue;

		private static int m_interactorsLayer = int.MinValue;

		private static int m_cameraBlockerLayer = int.MinValue;

		private static int m_staticColliderShadowLayer = int.MinValue;

		private static int m_offMeshLinkLayer = int.MinValue;

		private static int m_uiLayer = int.MinValue;

		private static int m_uiSharedLayer = int.MinValue;

		private static int m_ui3DLayer = int.MinValue;

		private static int m_missionBarrierLayer = int.MinValue;

		public static readonly LayerMask InteractableUiLayerMask = LayerMaskExtensions.Create("UI_Interactable");

		public static readonly LayerMask NonInteractableUiLayerMask = LayerMaskExtensions.Create("UI_NonInteractable");

		public static readonly LayerMask CharacterLayerMask = LayerMaskExtensions.Create("Character");

		public static readonly LayerMask StaticOverlayLayerMask = LayerMaskExtensions.Create("StaticOverlay");

		public static readonly LayerMask StaticColliderNoShadowLayerMask = LayerMaskExtensions.Create("StaticCollider_NoShadow");

		public static readonly LayerMask LayerNameStaticColliderNoShadowCameraSeeThroughMask = LayerMaskExtensions.Create("StaticCollider_NoShadow_CameraSeeThrough");

		public static readonly LayerMask InteractorsLayerMask = LayerMaskExtensions.Create("Interactors");

		public static readonly LayerMask CameraBlockerLayerMask = LayerMaskExtensions.Create("CameraBlocker");

		public static readonly LayerMask StaticColliderShadowLayerMask = LayerMaskExtensions.Create("StaticCollider_Shadow");

		public static readonly LayerMask OffMeshLinkLayerMask = LayerMaskExtensions.Create("OffMeshLink");

		public static readonly LayerMask UILayerMask = LayerMaskExtensions.Create("UI");

		public static readonly LayerMask UISharedLayerMask = LayerMaskExtensions.Create("UIShared");

		public static readonly LayerMask UI3DLayerMask = LayerMaskExtensions.Create("UI3D");

		public static readonly LayerMask MissionBarrierLayerMask = LayerMaskExtensions.Create("MissionBarrier");

		public static readonly LayerMask UILayersMaskPreset = LayerMaskExtensions.Create("UI_Interactable", "UI_NonInteractable", "UI3D");

		public static readonly LayerMask StaticCollidersLayersMaskPreset = LayerMaskExtensions.Create("StaticCollider_NoShadow", "StaticCollider_Shadow", "StaticCollider_NoShadow_CameraSeeThrough");

		public static readonly LayerMask StaticColliderCameraBlockersLayersMaskPreset = LayerMaskExtensions.Create("StaticCollider_NoShadow", "StaticCollider_Shadow", "CameraBlocker");

		public static readonly LayerMask StaticColliderCameraSeeThroughLayersMaskPreset = LayerMaskExtensions.Create("StaticCollider_NoShadow_CameraSeeThrough");

		public static readonly LayerMask InteractionLayersMaskPreset = LayerMaskExtensions.Create("StaticCollider_NoShadow", "StaticCollider_Shadow", "Interactors", "UI_Interactable");

		public static int DefaultLayer
		{
			get
			{
				return Validate(m_defaultLayer);
			}
		}

		public static int InteractableUiLayer
		{
			get
			{
				return Validate(m_interactableUiLayer);
			}
		}

		public static int NonInteractableUiLayer
		{
			get
			{
				return Validate(m_nonInteractableUiLayer);
			}
		}

		public static int CharacterLayer
		{
			get
			{
				return Validate(m_characterLayer);
			}
		}

		public static int StaticOverlayLayer
		{
			get
			{
				return Validate(m_staticOverlayLayer);
			}
		}

		public static int StaticColliderNoShadowLayer
		{
			get
			{
				return Validate(m_staticColliderNoShadowLayer);
			}
		}

		public static int StaticColliderNoShadowCameraSeeThroughLayer
		{
			get
			{
				return Validate(m_staticColliderNoShadowCameraSeeThroughLayer);
			}
		}

		public static int InteractorsLayer
		{
			get
			{
				return Validate(m_interactorsLayer);
			}
		}

		public static int CameraBlockerLayer
		{
			get
			{
				return Validate(m_cameraBlockerLayer);
			}
		}

		public static int StaticColliderShadowLayer
		{
			get
			{
				return Validate(m_staticColliderShadowLayer);
			}
		}

		public static int OffMeshLinkLayer
		{
			get
			{
				return Validate(m_offMeshLinkLayer);
			}
		}

		public static int UILayer
		{
			get
			{
				return Validate(m_uiLayer);
			}
		}

		public static int UISharedLayer
		{
			get
			{
				return Validate(m_uiSharedLayer);
			}
		}

		public static int UI3DLayer
		{
			get
			{
				return Validate(m_ui3DLayer);
			}
		}

		public static int MissionBarrierLayer
		{
			get
			{
				return Validate(m_missionBarrierLayer);
			}
		}

		public static void Init()
		{
			m_defaultLayer = LayerMask.NameToLayer("Default");
			m_interactableUiLayer = LayerMask.NameToLayer("UI_Interactable");
			m_nonInteractableUiLayer = LayerMask.NameToLayer("UI_NonInteractable");
			m_characterLayer = LayerMask.NameToLayer("Character");
			m_staticOverlayLayer = LayerMask.NameToLayer("StaticOverlay");
			m_staticColliderNoShadowLayer = LayerMask.NameToLayer("StaticCollider_NoShadow");
			m_staticColliderNoShadowCameraSeeThroughLayer = LayerMask.NameToLayer("StaticCollider_NoShadow_CameraSeeThrough");
			m_interactorsLayer = LayerMask.NameToLayer("Interactors");
			m_cameraBlockerLayer = LayerMask.NameToLayer("CameraBlocker");
			m_staticColliderShadowLayer = LayerMask.NameToLayer("StaticCollider_Shadow");
			m_offMeshLinkLayer = LayerMask.NameToLayer("OffMeshLink");
			m_uiLayer = LayerMask.NameToLayer("UI");
			m_uiSharedLayer = LayerMask.NameToLayer("UIShared");
			m_ui3DLayer = LayerMask.NameToLayer("UI3D");
			m_missionBarrierLayer = LayerMask.NameToLayer("MissionBarrier");
		}

		[Conditional("UNITY_DEBUG")]
		private static void ValidateAndThrowException(int _layer)
		{
			if (_layer == int.MinValue)
			{
				throw new LayerUsedTooEarlyException();
			}
		}

		private static int Validate(int _layer)
		{
			return _layer;
		}
	}
}
