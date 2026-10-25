using AcierProtocol;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.HUD;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.UI.Localization;
using UbiCore.Messenger;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.Mission.UI
{
	public class NpcInteractionUi : IUISubSystemLayout, IUiSubSystem
	{
		private IEntityIcon m_AcceptIcon;

		public bool IsActive
		{
			get
			{
				return UbiCore.Utils.Singleton<UiMain>.Instance.MissionType != EMissionType.Animus_EMissionType;
			}
		}

		public Vector2 BorderOffset { get; set; }

		public ScreenAnchor ScreenAnchor { get; set; }

		public EUISubSystem SubSystem
		{
			get
			{
				return EUISubSystem.NpcInteraction;
			}
		}

		public bool InitiallyVisible
		{
			get
			{
				return true;
			}
		}

		public void UpdateFrame(float _deltaTime)
		{
		}

		public bool Tap(TapEvent _event)
		{
			return false;
		}

		public void OnMissionLoad()
		{
		}

		public void OnMissionUnload()
		{
		}

		public void OnMissionOutroStarted()
		{
		}

		public void OnMissionLoaded()
		{
			if (IsActive)
			{
				m_AcceptIcon = UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.CreateEntityIcon();
				m_AcceptIcon.SetIcon(IconMaterial.InteractBuyMedicine, 7);
				m_AcceptIcon.IsHiddenOnBorders = false;
				m_AcceptIcon.SetInteractable(true, OnAcceptTapped);
				m_AcceptIcon.Offset2D = new Vector3(0f, 0f, 0f);
				UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.AddListener<NpcInteractionEvent>(OnNpcInteraction);
			}
		}

		public void OnMissionUnloaded()
		{
			if (IsActive)
			{
				UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.RemoveListener<NpcInteractionEvent>(OnNpcInteraction);
				if (m_AcceptIcon != null)
				{
					UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.ReleaseEntityIcon(m_AcceptIcon);
					m_AcceptIcon = null;
				}
			}
		}

		public virtual void OnSharedUILoaded()
		{
		}

		public virtual void OnUIAlignmentChanged(bool _isRightHanded)
		{
		}

		public virtual void OnMissionEnded()
		{
		}

		public bool IsOnUi(Vector2 _mousePosition)
		{
			return false;
		}

		public void OnSceneLoaded()
		{
		}

		public void OnSceneUnload()
		{
		}

		public void OnCameraCreated()
		{
		}

		public void OnUIReadyToUse()
		{
		}

		public void SetVisible(bool _value)
		{
		}

		private void OnNpcInteraction(NpcInteractionEvent _event)
		{
			switch (_event.SubType)
			{
			case NpcInteractionEvent.ESubType.OfferOpen:
				OpenOfferUi(_event.NpcId);
				break;
			case NpcInteractionEvent.ESubType.OfferAccepted:
			{
				CloseOfferUi(_event.NpcId);
				InteractionCharacter entity = _event.NpcId.GetEntity<InteractionCharacter>();
				NpcData.NpcDefinition.ENpcInteractionType interactionType = entity.NpcDefinition.Definition.InteractionType;
				if (interactionType == NpcData.NpcDefinition.ENpcInteractionType.Merchant)
				{
					if (entity.EntityStats.Get_bool(StatTypes.EStatType.Interaction_Triggered))
					{
						UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UIOnScreenMessage>().ShowMessage(LocalizationManager.Instance.Get(1505));
					}
					else
					{
						UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UIOnScreenMessage>().ShowMessage(LocalizationManager.Instance.Get(5417));
					}
				}
				break;
			}
			case NpcInteractionEvent.ESubType.OfferRejected:
				CloseOfferUi(_event.NpcId);
				break;
			}
		}

		private void OpenOfferUi(EntityId _npcId)
		{
			m_AcceptIcon.Visible = true;
			UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.AddEntityIcon(_npcId, m_AcceptIcon);
		}

		private void CloseOfferUi(EntityId _npcId)
		{
			UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.RemoveEntityIcon(_npcId, m_AcceptIcon);
			m_AcceptIcon.Visible = false;
		}

		private bool OnAcceptTapped(IEntityIcon _entityIcon)
		{
			UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.CharacterInteraction_AcceptOffer(_entityIcon.EntityId);
			return true;
		}

		private bool OnRejectTapped(IEntityIcon _entityIcon)
		{
			UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.CharacterInteraction_RejectOffer(_entityIcon.EntityId);
			return true;
		}
	}
}
