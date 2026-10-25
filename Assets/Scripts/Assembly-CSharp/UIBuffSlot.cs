using Assets.Scripts.Mission.UI.LogicInterface;
using UnityEngine;

public class UIBuffSlot : MonoBehaviour
{
	public UISprite Icon;

	public UISprite Timer;

	public UISprite Background;

	private int m_duration;

	private int m_endTime;

	private void Awake()
	{
	}

	public void SetTimer(IActiveBuff _buff)
	{
		m_duration = _buff.Duration;
		m_endTime = _buff.EndTime;
		bool flag = !LogicMain.LogicInterface.Prototype_BuffTimerDisabled;
		Timer.gameObject.SetActive(flag);
		Background.gameObject.SetActive(flag);
		if (flag)
		{
			UpdateTimer();
		}
	}

	public void UpdateTimer()
	{
		float value = 1f;
		if (m_endTime < int.MaxValue)
		{
			value = (float)(m_endTime - LogicMain.CurrentTurn) / (float)m_duration;
		}
		value = 1f - Mathf.Clamp01(value);
		Timer.fillAmount = value;
	}
}
