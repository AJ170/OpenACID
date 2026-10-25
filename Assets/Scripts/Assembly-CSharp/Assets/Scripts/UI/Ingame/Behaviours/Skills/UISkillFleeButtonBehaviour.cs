using Assets.Scripts.Mission.UI.LogicInterface;

namespace Assets.Scripts.UI.Ingame.Behaviours.Skills
{
	public class UISkillFleeButtonBehaviour : UISkillButtonBaseBehaviour
	{
		public override void SetSkillState(ESkillStatus _status, int _gameTurnDuration, int _startGameTurn)
		{
			if (m_currentSkillStatus != _status)
			{
				switch (m_currentSkillStatus)
				{
				case ESkillStatus.ActivatedCooldown:
					m_cooldownStartGameTurn = 0;
					m_cooldownGameTurnDuration = 0;
					CooldownIndicator.gameObject.SetActive(false);
					break;
				case ESkillStatus.CoolDown:
					m_cooldownStartGameTurn = 0;
					m_cooldownGameTurnDuration = 0;
					CooldownIndicator.gameObject.SetActive(false);
					OnCoolDownCompleted();
					break;
				}
			}
			switch (_status)
			{
			case ESkillStatus.CoolDown:
			case ESkillStatus.ActivatedCooldown:
				if (m_currentSkillStatus != _status)
				{
					m_cooldownStartGameTurn = _startGameTurn;
					m_cooldownGameTurnDuration = _gameTurnDuration;
					CooldownIndicator.gameObject.SetActive(true);
					CooldownIndicator.fillAmount = 0f;
				}
				break;
			case ESkillStatus.Removed:
				Init(SkillTypes.Invalid, false);
				break;
			}
			m_currentSkillStatus = _status;
			UpdateSkillLayout();
		}

		protected override void UpdateSkillLayout()
		{
			CooldownDecal.gameObject.SetActive(false);
		}

		protected override void OnUpdateSkill()
		{
		}
	}
}
