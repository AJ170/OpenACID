using Assets.Scripts.Mission.Logic.Combat;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;
using SharedBaseLib.Core;
using UbiCore;
using UbiCore.Utils;

namespace Assets.Scripts.Mission.UI.Combat
{
	public class ParticipantHandler
	{
		private CombatParticipantProxy m_Participant;

		private float m_attackPreparationDelta;

		private float m_attackPreparationTime;

		private Character m_attackTarget;

		private readonly BBList<ICombatAnnotation> m_ActiveAnnotations = new BBList<ICombatAnnotation>(4);

		public CombatParticipantProxy CombatParticipant
		{
			get
			{
				return m_Participant;
			}
			set
			{
				if (m_Participant == value)
				{
					return;
				}
				if (m_Participant != null)
				{
					UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.RemoveListener<CombatUiEvent>(OnCombatUiEvent);
					ResetIcon();
					while (m_ActiveAnnotations.Count > 0)
					{
						m_ActiveAnnotations[0].Release();
					}
					if (HealthBar != null)
					{
						HealthBar.FadeOut();
					}
					HealthBar = null;
				}
				m_Participant = value;
				m_attackPreparationDelta = (m_attackPreparationTime = 0f);
				if (m_Participant != null)
				{
					UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.AddListener<CombatUiEvent>(OnCombatUiEvent);
					DisableIcon();
					if (!m_Participant.IsPlayer)
					{
						EntityStats entityStats = m_Participant.Entity.EntityStats;
						HealthBar = HealthBar.Get(m_Participant.GameEntity, entityStats.Get_float(StatTypes.EStatType.Character_Conflict_Health) / entityStats.Get_float(StatTypes.EStatType.Character_Conflict_ActualMaxHealth));
					}
				}
			}
		}

		private CombatParticipant TargetParticipant { get; set; }

		private CombatUI Owner { get; set; }

		private EntityIcon ActiveAttachment { get; set; }

		internal HealthBar HealthBar { get; private set; }

		public ParticipantHandler(CombatUI _owner)
		{
			Owner = _owner;
		}

		public void Update(float _deltaTime)
		{
			if (CombatParticipant != null)
			{
				UpdateAttacker();
			}
		}

		internal void DisableIcon()
		{
			EntityIcon iconAttachment = CombatParticipant.Character.HeadIcon.IconAttachment;
			iconAttachment.SetInteractableKeepAction(false);
		}

		internal void ResetIcon()
		{
			EntityIcon iconAttachment = CombatParticipant.Character.HeadIcon.IconAttachment;
			iconAttachment.SetIcon(IconMaterial.Invalid, 4);
			iconAttachment.SetInteractableKeepAction(true);
		}

		private void OnCombatUiEvent(CombatUiEvent _event)
		{
			if (_event.Target == null)
			{
				return;
			}
			if (_event.Target.Id == CombatParticipant.Entity.Id)
			{
				switch (_event.Type)
				{
				case CombatUiEvent.EType.Hit:
					if (!MathHelper.Approximately(_event.Delta, 0f) && HealthBar != null)
					{
						HealthBar.HealthBarPercentage = _event.Target.EntityStats.Get_float(StatTypes.EStatType.Character_Conflict_Health) / _event.Target.EntityStats.Get_float(StatTypes.EStatType.Character_Conflict_ActualMaxHealth);
					}
					break;
				case CombatUiEvent.EType.Exit:
					ResetIcon();
					CombatParticipant = null;
					break;
				}
			}
			if (_event.Source == null || _event.Source.Id != CombatParticipant.Entity.Id)
			{
				return;
			}
			CombatUiEvent.EType type = _event.Type;
			if (type == CombatUiEvent.EType.PrepareAttack)
			{
				m_attackPreparationDelta = _event.Delta;
				m_attackPreparationTime = UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.GameTime;
				if (m_attackTarget != null)
				{
					EntityIcon iconAttachment = m_attackTarget.HeadIcon.IconAttachment;
					iconAttachment.SetIcon(IconMaterial.Invalid, 4);
				}
				m_attackTarget = _event.Source;
			}
		}

		private void AddAnnotation(ICombatAnnotation _annotation)
		{
			_annotation.OnRelease += OnAnnotationRelease;
			m_ActiveAnnotations.Add(_annotation);
		}

		private void OnAnnotationRelease(ICombatAnnotation _o)
		{
			m_ActiveAnnotations.Remove(_o);
		}

		private void UpdateAttacker()
		{
			if (m_attackPreparationDelta + m_attackPreparationTime > UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.GameTime)
			{
				EntityIcon iconAttachment = m_attackTarget.HeadIcon.IconAttachment;
				iconAttachment.SetIcon(IconMaterial.CombatEnter, 4);
			}
			else if (m_attackTarget != null)
			{
				EntityIcon iconAttachment2 = m_attackTarget.HeadIcon.IconAttachment;
				iconAttachment2.SetIcon(IconMaterial.Invalid, 4);
			}
		}
	}
}
