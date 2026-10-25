using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[ExecuteInEditMode]
public class CombinedPostEffect : MonoBehaviour
{
	private List<BasePostFx> m_AvailableFx;

	private Material m_ComboPostFxMaterial;

	public const int kMinRequiredQualityLevel = 2;

	public ReadOnlyCollection<BasePostFx> AvailableFx
	{
		get
		{
			if (m_AvailableFx == null)
			{
				m_AvailableFx = new List<BasePostFx>();
				m_AvailableFx.Add(new FxBasicBloom());
				m_AvailableFx.Add(new FxColorCorrection());
				m_AvailableFx.Add(new FxDesaturation());
				m_AvailableFx.Add(new FxThunderstorm());
			}
			return m_AvailableFx.AsReadOnly();
		}
	}

	public T FindFx<T>() where T : BasePostFx
	{
		Type typeFromHandle = typeof(T);
		foreach (BasePostFx item in m_AvailableFx)
		{
			if (typeFromHandle == item.GetType())
			{
				return item as T;
			}
		}
		return null;
	}

	public static bool IsSupported()
	{
		bool flag = QualitySettings.GetQualityLevel() >= 2;
		flag &= SystemInfo.supportsImageEffects;
		flag &= SystemInfo.supportsRenderTextures;
		flag &= SystemInfo.npotSupport != NPOTSupport.None;
		if (flag)
		{
			Shader shader = Shader.Find("Hidden/PostFxComboPass");
			flag &= shader.isSupported;
		}
		return flag;
	}

	private void Start()
	{
		Debug.Log("CombinedPostEffect::Start");
		Shader shader = Shader.Find("Hidden/PostFxComboPass");
		m_ComboPostFxMaterial = new Material(shader);
		Refresh();
	}

	public void Refresh(bool _skipExpensive = false)
	{
		foreach (BasePostFx item in AvailableFx)
		{
			item.Reset(_skipExpensive);
		}
	}

	private void OnRenderImage(RenderTexture _src, RenderTexture _dst)
	{
		int count = AvailableFx.Count;
		bool flag = false;
		for (int i = 0; count > i; i++)
		{
			if (AvailableFx[i].IsActive)
			{
				flag = true;
				AvailableFx[i].Prepare(_src);
			}
		}
		if (!flag)
		{
			Graphics.Blit(_src, _dst);
			return;
		}
		_src.filterMode = FilterMode.Bilinear;
		for (int j = 0; count > j; j++)
		{
			AvailableFx[j].ExecuteOrSkip(m_ComboPostFxMaterial);
		}
		Graphics.Blit(_src, _dst, m_ComboPostFxMaterial);
		for (int k = 0; count > k; k++)
		{
			if (AvailableFx[k].IsActive)
			{
				AvailableFx[k].ReleaseTemporaries();
			}
		}
	}
}
