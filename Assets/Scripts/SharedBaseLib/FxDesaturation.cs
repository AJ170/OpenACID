using UnityEngine;

public class FxDesaturation : BasePostFx
{
	private DesaturationSettings m_Settings;

	private int m_DesaturationWeightsAndMixId;

	public FxDesaturation()
	{
		m_DesaturationWeightsAndMixId = Shader.PropertyToID("_DesaturationWeightsAndMix");
	}

	public override void Reset(bool _skipExpensive)
	{
		if (m_Settings == null || !_skipExpensive)
		{
			m_Settings = Object.FindObjectOfType(typeof(DesaturationSettings)) as DesaturationSettings;
		}
		IsActive = m_Settings != null;
	}

	public override void ExecuteOrSkip(Material _ComboPass)
	{
		if (IsActive && IsDesaturationThresholdReached(m_Settings.desaturation))
		{
			_ComboPass.SetVector(m_DesaturationWeightsAndMixId, new Vector4(m_Settings.desaturationWeights.x, m_Settings.desaturationWeights.y, m_Settings.desaturationWeights.z, m_Settings.desaturation));
			_ComboPass.EnableKeyword("DESATURATE_ON");
		}
		else
		{
			_ComboPass.DisableKeyword("DESATURATE_ON");
		}
	}

	public static bool IsDesaturationThresholdReached(float desaturation)
	{
		return desaturation > 1f / 15f;
	}
}
