using UnityEngine;

public class FxThunderstorm : BasePostFx
{
	private ThunderstormSettings m_Settings;

	private int m_ThunderstormPreAddId;

	private int m_ThunderstormExpId;

	public FxThunderstorm()
	{
		m_ThunderstormPreAddId = Shader.PropertyToID("_ThunderstormPreAdd");
		m_ThunderstormExpId = Shader.PropertyToID("_ThunderstormExp");
	}

	public override void Reset(bool _skipExpensive)
	{
		if (m_Settings == null || !_skipExpensive)
		{
			m_Settings = Object.FindObjectOfType(typeof(ThunderstormSettings)) as ThunderstormSettings;
		}
		IsActive = m_Settings != null;
	}

	public override void Prepare(RenderTexture _src)
	{
		if (m_Settings != null)
		{
			Shader.SetGlobalColor(m_ThunderstormPreAddId, m_Settings.thunderColor * m_Settings.thunderAdd);
			Shader.SetGlobalColor(m_ThunderstormExpId, Color.white + m_Settings.thunderColor * m_Settings.thunderExp);
		}
	}

	public override void ExecuteOrSkip(Material _ComboPass)
	{
		if (IsActive && m_Settings != null && IsThunderThresholdReached(m_Settings.thunderColor))
		{
			_ComboPass.EnableKeyword("THUNDER_ON");
		}
		else
		{
			_ComboPass.DisableKeyword("THUNDER_ON");
		}
	}

	public static bool IsThunderThresholdReached(Color thunder)
	{
		return thunder.r + thunder.g + thunder.b > 2f / 51f;
	}
}
