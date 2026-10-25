using Assets.Scripts.Database;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Motion.Hotspots;
using Database;
using SharedEnvironmentLib.MapData;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class CodexEntry : Component
	{
		private CodexDefinition m_Data;

		private GameObject m_effectGo;

		public string Guid { get; private set; }

		public bool Valid
		{
			get
			{
				return m_Data != null;
			}
		}

		public CodexDefinition Data
		{
			get
			{
				return m_Data;
			}
			set
			{
				m_Data = value;
				EStateType eStateType = ((!Valid) ? EStateType.Object_Disabled : EStateType.Object_Enabled);
				ObjectInteraction objectInteraction = Entity.Cast<IInteractableObjectStateMachinesCarrier>().InteractableObjectStateMachines.ObjectInteraction;
				if (objectInteraction.CurrentStateType != eStateType)
				{
					objectInteraction.ChangeState(eStateType);
				}
				else
				{
					ObjectInteractionOnStateChanged(objectInteraction, EStateType.Invalid, eStateType);
				}
			}
		}

		public override int Priority
		{
			get
			{
				return 16;
			}
		}

		public bool IsActive { get; set; }

		public CodexEntry(IEntity _entity)
			: base(_entity)
		{
			ObjectInteraction objectInteraction = Entity.Cast<IInteractableObjectStateMachinesCarrier>().InteractableObjectStateMachines.ObjectInteraction;
			objectInteraction.StateChanged += ObjectInteractionOnStateChanged;
			IsActive = false;
		}

		private void ObjectInteractionOnStateChanged(IStateMachine stateMachine, EStateType from, EStateType to)
		{
			bool flag = to == EStateType.Object_Enabled;
			if (flag && m_effectGo == null)
			{
				m_effectGo = EffectHelper.SpawnEffectInWorld(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_effectCodex, GameEntity.Position);
			}
			if (m_effectGo != null)
			{
				EffectParticles component = m_effectGo.GetComponent<EffectParticles>();
				if (flag)
				{
					component.On();
				}
				else
				{
					component.Off();
				}
				MeshRenderer[] componentsInChildren = m_effectGo.GetComponentsInChildren<MeshRenderer>();
				MeshRenderer[] array = componentsInChildren;
				foreach (MeshRenderer meshRenderer in array)
				{
					meshRenderer.enabled = flag;
				}
			}
		}

		public override void OnUnregistered()
		{
			base.OnUnregistered();
			Object.Destroy(m_effectGo);
			m_effectGo = null;
		}

		public override void OnActivate()
		{
			StorageId component = GameObject.GetComponent<StorageId>();
			Guid = component.Guid.Trim();
			if (Guid == string.Empty)
			{
				Guid = "15f235c5-e9ab-445c-9895-f1e0c04dc64b";
			}
			GameID gameID = Main.Instance.GameDB.FindGameIdFromGuid(Guid);
			if (UbiCore.Utils.Singleton<LocalStorage>.Instance.CodexEntryIsUnlocked(gameID))
			{
				Data = null;
			}
			else
			{
				Data = Main.Instance.GameDB.GetItem<CodexDefinition>(gameID);
			}
		}

		public void OnStartInteraction()
		{
			IsActive = true;
		}

		public void OnEndInteraction()
		{
			if (IsActive)
			{
				UbiCore.Utils.Singleton<HotspotService>.Instance.CodexManager.LeaveInteraction(PlayerHelper.Entity, Vector3.zero);
				IsActive = false;
			}
		}
	}
}
