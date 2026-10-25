using System;
using System.Collections.Generic;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.UI.Utils;
using SharedBaseLib.Core;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Ingame.Behaviours
{
	public class UIUnitFrameBehaviour : UIBaseBehaviour
	{
		private const int MaxIconIndex = 35;

		public static readonly List<int> IgnoredBuffs = new List<int> { -1, 6 };

		public UITweenHelper TweenHelper;

		public UILabel UnitName;

		public UIBuffSlot[] BuffSlots;

		public UIPortraitTexture PotraitTexture;

		public UIProgressBar HealthProgressBar;

		public UIProgressBar DamageProgressBar;

		public float BarAnimationDuration = 1f;

		private BBList<BBList<IActiveBuff>> m_buffs = new BBList<BBList<IActiveBuff>>(35);

		private float m_health;

		private float m_healthHigh;

		protected bool IsInitialized;

		private float m_animateStartTime;

		private bool m_isAnimating;

		private int m_lastTimerUpdate = -1;

		public float Health
		{
			get
			{
				return m_health;
			}
			set
			{
				if (!(Math.Abs(m_health - value) < float.Epsilon))
				{
					m_healthHigh = m_health;
					m_health = Mathf.Clamp01(value);
					StartAnimateHealthChange();
				}
			}
		}

		public virtual void Init()
		{
			if (!IsInitialized)
			{
				IsInitialized = true;
				DamageProgressBar.gameObject.SetActive(false);
				InitBuffs();
			}
		}

		public virtual void Exclude()
		{
			base.gameObject.SetActive(false);
		}

		public virtual void Hide()
		{
			ClearBuffs();
			SetVisibility(false);
			StopAnimateHealthChange();
		}

		public void SetUserName(string _name)
		{
			UnitName.text = _name;
			if (UnitName.processedText.Equals(_name))
			{
				return;
			}
			string[] array = _name.Split(' ');
			string text = array[0];
			if (array.Length > 1)
			{
				int result = 0;
				if (int.TryParse(array[1], out result))
				{
					text = text + " " + array[1];
				}
			}
			UnitName.text = text;
		}

		public void SetPortraitData(EntityId _entityId)
		{
			UIPortraitUtil.UpdateInMissionPortrait(_entityId, PotraitTexture);
		}

		public virtual void Show(string _name, float _healthNormalized, IActiveBuff[] _currentBuffs)
		{
			SetUserName(_name);
			m_health = _healthNormalized;
			SetProgress(Health, HealthProgressBar);
			HealthProgressBar.ForceUpdate();
			SetVisibility(true);
			ClearBuffs();
			AddBuffRange(_currentBuffs);
		}

		public void AddBuff(BuffUiEvent _message)
		{
			if (_message.Type != EBuffType.Invalid)
			{
				BBList<IActiveBuff> activeBuffs = GetActiveBuffs(_message.Type);
				if (activeBuffs != null)
				{
					activeBuffs.Add(new LogicInterface.ActiveBuff(_message));
					ReconstructBuffDisplay();
				}
			}
		}

		public void AddBuffRange(IActiveBuff[] _currentBuffs)
		{
			bool flag = false;
			foreach (IActiveBuff activeBuff in _currentBuffs)
			{
				if (activeBuff.Type != EBuffType.Invalid)
				{
					BBList<IActiveBuff> activeBuffs = GetActiveBuffs(activeBuff.Type);
					if (activeBuffs != null)
					{
						activeBuffs.Add(activeBuff);
						flag = true;
					}
				}
			}
			if (flag)
			{
				ReconstructBuffDisplay();
			}
		}

		public void RemoveBuff(BuffUiEvent _message)
		{
			if (_message.Type == EBuffType.Invalid)
			{
				return;
			}
			BBList<IActiveBuff> activeBuffs = GetActiveBuffs(_message.Type);
			if (activeBuffs != null)
			{
				int num = activeBuffs.FindIndex((IActiveBuff _ab) => _ab.Id == _message.Id);
				if (num >= 0)
				{
					activeBuffs.RemoveAt(num);
					ReconstructBuffDisplay();
				}
			}
		}

		public void InitBuffs()
		{
			for (int i = 0; i < BuffSlots.Length; i++)
			{
				BuffSlots[i].gameObject.SetActive(false);
			}
			if (m_buffs.Count < 35)
			{
				m_buffs.Capacity = 35;
				m_buffs.Resize(35);
			}
			for (int j = 0; j < m_buffs.Count; j++)
			{
				if (m_buffs[j] == null)
				{
					m_buffs[j] = new BBList<IActiveBuff>(2);
				}
				else
				{
					m_buffs[j].ClearIfNotEmpty();
				}
			}
			m_lastTimerUpdate = -1;
		}

		public void ClearBuffs()
		{
			for (int i = 0; i < BuffSlots.Length; i++)
			{
				BuffSlots[i].gameObject.SetActive(false);
			}
			for (int j = 0; j < m_buffs.Count; j++)
			{
				m_buffs[j].ClearIfNotEmpty();
			}
		}

		private void Update()
		{
			AnimateHealthChange();
			if (!LogicMain.LogicInterface.Prototype_BuffTimerDisabled && LogicMain.CurrentTurn != m_lastTimerUpdate)
			{
				for (int i = 0; i < BuffSlots.Length && BuffSlots[i].gameObject.activeSelf; i++)
				{
					BuffSlots[i].UpdateTimer();
				}
				m_lastTimerUpdate = LogicMain.CurrentTurn;
			}
		}

		private void StartAnimateHealthChange()
		{
			m_isAnimating = true;
			DamageProgressBar.gameObject.SetActive(true);
			SetProgress(m_health, HealthProgressBar);
			SetProgress(m_healthHigh, DamageProgressBar);
			m_animateStartTime = UbiCore.Utils.Singleton<Timer>.Instance.realTime;
		}

		private void AnimateHealthChange()
		{
			if (m_isAnimating)
			{
				if (UbiCore.Utils.Singleton<Timer>.Instance.realTime - m_animateStartTime < BarAnimationDuration)
				{
					float t = Mathf.Clamp01((UbiCore.Utils.Singleton<Timer>.Instance.realTime - m_animateStartTime) / BarAnimationDuration);
					float health = Mathf.Lerp(m_healthHigh, m_health, t);
					SetProgress(health, DamageProgressBar);
				}
				else
				{
					StopAnimateHealthChange();
				}
			}
		}

		private void StopAnimateHealthChange()
		{
			SetProgress(m_health, DamageProgressBar);
			DamageProgressBar.gameObject.SetActive(false);
			m_isAnimating = false;
		}

		private void ReconstructBuffDisplay()
		{
			if (!base.gameObject.activeSelf)
			{
				return;
			}
			int num = 0;
			int currentTurn = LogicMain.CurrentTurn;
			for (int i = 0; i < m_buffs.Count; i++)
			{
				BBList<IActiveBuff> bBList = m_buffs[i];
				if (bBList == null || bBList.Count == 0)
				{
					continue;
				}
				int num2 = -1;
				int num3 = -1;
				int num4 = -1;
				for (int j = 0; j < bBList.Count; j++)
				{
					if (bBList[j].StartTime <= currentTurn)
					{
						int endTime = bBList[j].EndTime;
						bool flag = endTime == int.MaxValue;
						if (flag && num2 == -1)
						{
							num2 = j;
						}
						if (endTime > num3 && !flag)
						{
							num4 = j;
							num3 = endTime;
						}
					}
				}
				num4 = ((num4 >= 0) ? num4 : num2);
				if (num4 >= 0 && num < BuffSlots.Length)
				{
					IActiveBuff activeBuff = bBList[num4];
					UIBuffSlot uIBuffSlot = BuffSlots[num];
					uIBuffSlot.gameObject.SetActive(true);
					uIBuffSlot.SetTimer(activeBuff);
					SetBuffIcon(activeBuff.Type, uIBuffSlot.Icon);
					num++;
				}
			}
			for (int k = num; k < BuffSlots.Length; k++)
			{
				BuffSlots[k].gameObject.SetActive(false);
			}
		}

		private static void SetBuffIcon(EBuffType _buffType, UISprite _buffSprite)
		{
			if (_buffSprite != null)
			{
				string spriteName = "buff_icons_" + GetBuffSpriteIndex(_buffType);
				UISpriteData sprite = _buffSprite.atlas.GetSprite(spriteName);
				if (sprite != null)
				{
					_buffSprite.spriteName = spriteName;
				}
			}
		}

		private void SetProgress(float _health, UIProgressBar _progressBar)
		{
			_progressBar.value = _health;
			_progressBar.gameObject.SetActive(_progressBar.value > 1E-07f);
		}

		private BBList<IActiveBuff> GetActiveBuffs(EBuffType _buffType)
		{
			int buffSpriteIndex = GetBuffSpriteIndex(_buffType);
			if (buffSpriteIndex < 0)
			{
				return null;
			}
			if (m_buffs.Count <= buffSpriteIndex)
			{
				m_buffs.Capacity = buffSpriteIndex + 1;
				for (int i = m_buffs.Count; i <= buffSpriteIndex; i++)
				{
					m_buffs.Add(new BBList<IActiveBuff>(2));
				}
			}
			return m_buffs[buffSpriteIndex];
		}

		private static int GetBuffSpriteIndex(EBuffType _buffType)
		{
			BuffData.IBuffDefinition definition = UbiCore.Utils.Singleton<BuffData>.Instance.GetDefinition(_buffType);
			return (definition != null) ? definition.IconSpriteIndex : (-1);
		}
	}
}
