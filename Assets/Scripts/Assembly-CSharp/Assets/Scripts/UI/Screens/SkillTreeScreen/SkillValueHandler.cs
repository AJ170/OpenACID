using System.Collections;
using AcierProtocol;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.SkillTreeScreen
{
	public class SkillValueHandler : MonoBehaviour
	{
		[SerializeField]
		private ValueHandler m_valueHandler;

		[SerializeField]
		private UISprite m_LockedSprite;

		[SerializeField]
		private GameObject m_effectPrefab;

		private GameObject m_effectObject;

		private UnityEngine.Animation m_animation;

		private bool m_animationIsRunning;

		private SkillTreeData m_data;

		private int m_tierIndex;

		private AssassinSkillInTraining.SkillType m_type;

		private bool m_oldLockedStatus;

		private Assassin m_lastSelectedAssasin;

		private bool m_visible;

		public bool Visible
		{
			get
			{
				return m_visible;
			}
			set
			{
				m_visible = value;
				base.gameObject.SetActive(value);
			}
		}

		public void InitScreen()
		{
			if (m_effectObject == null)
			{
				m_effectObject = NGUITools.AddChild(base.transform.parent.gameObject, m_effectPrefab);
				m_effectObject.SetActive(false);
				m_animation = m_effectObject.transform.GetChild(0).GetComponent<UnityEngine.Animation>();
			}
		}

		public void Init(SkillTreeData _data, int _tierIndex, AssassinSkillInTraining.SkillType _type)
		{
			m_effectObject.SetActive(false);
			if (_type == AssassinSkillInTraining.SkillType.PassiveOne || _type == AssassinSkillInTraining.SkillType.PassiveTwo)
			{
				if (m_lastSelectedAssasin == _data.Assassin)
				{
					m_oldLockedStatus = m_data.GetSkill(m_data.PrimarySelected, m_tierIndex, m_type).Locked;
				}
				else
				{
					m_oldLockedStatus = _data.GetSkill(_data.PrimarySelected, _tierIndex, _type).Locked;
				}
			}
			m_data = _data;
			m_tierIndex = _tierIndex;
			m_type = _type;
			m_animationIsRunning = false;
			m_lastSelectedAssasin = _data.Assassin;
			m_valueHandler.Init(_data);
		}

		public void UpdateHandler(SkillData _skill = null)
		{
			SkillData skill = m_data.GetSkill(m_data.PrimarySelected, m_tierIndex, m_type);
			DisableAll();
			AssassinSkillTier_ tier = Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin.GetTier(skill.TierId, (!m_data.PrimarySelected) ? AssassinSkillInTraining.SkillClass.ExtendedClass : AssassinSkillInTraining.SkillClass.BaseClass);
			if (skill.Locked && !tier.Locked())
			{
				if (!m_animationIsRunning)
				{
					if (m_oldLockedStatus)
					{
						m_LockedSprite.alpha = 1f;
						m_LockedSprite.gameObject.SetActive(true);
					}
					else
					{
						StartCoroutine(PlayAnimation(m_animation.clip.length));
					}
				}
			}
			else
			{
				m_valueHandler.Visible = true;
				m_valueHandler.Value = skill.Level;
				if (m_animationIsRunning)
				{
					StopAllCoroutines();
				}
			}
			if (tier.Locked() || !skill.Locked)
			{
				m_LockedSprite.gameObject.SetActive(false);
				m_effectObject.SetActive(false);
			}
			if (tier.Locked() || skill.Locked)
			{
				m_valueHandler.Locked = true;
			}
			m_oldLockedStatus = skill.Locked;
		}

		private void OnDestroy()
		{
			m_animation = null;
			m_lastSelectedAssasin = null;
			m_data = null;
			if (m_effectObject != null)
			{
				NGUITools.Destroy(m_effectObject);
			}
			m_effectObject = null;
		}

		private void DisableAll()
		{
			m_LockedSprite.gameObject.SetActive(false);
			m_valueHandler.Visible = false;
		}

		private IEnumerator PlayAnimation(float _time)
		{
			float counter = 0f;
			m_animationIsRunning = true;
			m_LockedSprite.alpha = 0f;
			m_effectObject.SetActive(true);
			m_animation.Play();
			while (true)
			{
				counter += Time.deltaTime;
				if (counter > _time)
				{
					break;
				}
				yield return null;
			}
			m_effectObject.SetActive(false);
			m_LockedSprite.alpha = 1f;
			m_LockedSprite.gameObject.SetActive(true);
			m_animationIsRunning = false;
		}

		public void StartAnimationExternally()
		{
			SkillData skill = m_data.GetSkill(m_data.PrimarySelected, m_tierIndex, m_type);
			AssassinSkillTier_ tier = Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin.GetTier(skill.TierId, (!m_data.PrimarySelected) ? AssassinSkillInTraining.SkillClass.ExtendedClass : AssassinSkillInTraining.SkillClass.BaseClass);
			if (!tier.Locked() && skill.Locked && (skill.SkillType == AssassinSkillInTraining.SkillType.PassiveOne || skill.SkillType == AssassinSkillInTraining.SkillType.PassiveTwo))
			{
				if (m_animation.isPlaying)
				{
					StopAllCoroutines();
					m_animation.GetComponent<UnityEngine.Animation>()["SkillLockEffect"].time = 0f;
				}
				StartCoroutine(PlayAnimation(m_animation.clip.length));
			}
		}
	}
}
