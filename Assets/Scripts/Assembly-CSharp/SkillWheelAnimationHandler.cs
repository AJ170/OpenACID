using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Audio;
using Assets.Scripts.UI.NGUI_Extensions.Tweens;
using UbiCore.Utils;
using UbiCore.Utils.Extensions;
using UnityEngine;

public class SkillWheelAnimationHandler : MonoBehaviour
{
	[SerializeField]
	private List<TweenScale> innerElements;

	[SerializeField]
	private List<TweenPosition> outerElements1;

	[SerializeField]
	private List<TweenPosition> outerElements2;

	[SerializeField]
	private List<TweenPosition> m_idependentElements1;

	[SerializeField]
	private List<TweenAlpha> m_idependentElements2;

	public void GetElementsAndPlayAnimations()
	{
		InitializeElements();
	}

	private void OnEnable()
	{
		AudioManager.Instance.Load(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_skillwheel_hitwheel);
		AudioManager.Instance.Load(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_skillwheel_screen_enter);
	}

	private void OnDisable()
	{
		AudioManager.Instance.Unload(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_skillwheel_hitwheel);
		AudioManager.Instance.Unload(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_skillwheel_screen_enter);
	}

	private void InitializeElements()
	{
		for (int i = 0; i < outerElements1.Count; i++)
		{
			TweenPosition tweenPosition = outerElements1[i];
			tweenPosition.from = new Vector3(-2000f, tweenPosition.transform.localPosition.y, 1f);
			tweenPosition.to = new Vector3(tweenPosition.transform.localPosition.x, tweenPosition.transform.localPosition.y, 1f);
			tweenPosition.delay = (float)i * 0.1f;
			tweenPosition.ResetToBeginning();
			tweenPosition.PlayForward();
			if (i == 0)
			{
				StartCoroutine(PlayOnEnterDelayed(tweenPosition.delay));
			}
		}
		for (int j = 0; j < outerElements2.Count; j++)
		{
			TweenPosition tweenPosition2 = outerElements2[j];
			tweenPosition2.from = new Vector3(2000f, tweenPosition2.transform.localPosition.y, 1f);
			tweenPosition2.to = new Vector3(tweenPosition2.transform.localPosition.x, tweenPosition2.transform.localPosition.y, 1f);
			tweenPosition2.delay = 0.4f + (float)j * 0.1f;
			tweenPosition2.ResetToBeginning();
			tweenPosition2.PlayForward();
			if (j == 0)
			{
				StartCoroutine(PlayOnEnterDelayed(tweenPosition2.delay));
			}
		}
		for (int k = 0; k < innerElements.Count; k++)
		{
			TweenScale tweenScale = innerElements[k];
			tweenScale.delay = (float)k * 0.1f;
			BBTweenCustom bBTweenCustom = tweenScale.gameObject.AddComponentIfNotExist<BBTweenCustom>();
			bBTweenCustom.animationCurve = tweenScale.animationCurve;
			bBTweenCustom.delay = tweenScale.delay;
			bBTweenCustom.duration = tweenScale.duration * 0.84f;
			EventDelegate.Add(bBTweenCustom.onFinished, delegate
			{
				AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_skillwheel_hitwheel);
			}, true);
			bBTweenCustom.StartTweenPlayForward();
			tweenScale.ResetToBeginning();
			tweenScale.PlayForward();
		}
		for (int l = 0; l < m_idependentElements1.Count; l++)
		{
			TweenPosition tweenPosition3 = m_idependentElements1[l];
			tweenPosition3.ResetToBeginning();
			tweenPosition3.PlayForward();
			if (l == 0)
			{
				StartCoroutine(PlayOnEnterDelayed(tweenPosition3.delay));
			}
		}
		for (int m = 0; m < m_idependentElements2.Count; m++)
		{
			m_idependentElements2[m].ResetToBeginning();
			m_idependentElements2[m].PlayForward();
		}
	}

	private IEnumerator PlayOnEnterDelayed(float _delay)
	{
		yield return new WaitForSeconds(_delay);
		AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_skillwheel_screen_enter);
	}
}
