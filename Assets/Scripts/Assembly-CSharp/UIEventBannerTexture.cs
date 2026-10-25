using UnityEngine;

[AddComponentMenu("NGUI/BB/UI/UIEventBannerTexture")]
[ExecuteInEditMode]
public class UIEventBannerTexture : UICustomShaderTexture
{
	[HideInInspector]
	[SerializeField]
	private Texture m_stencilTexture;

	[SerializeField]
	[HideInInspector]
	private Vector4 m_alphaMaskOffset;

	public void SetMaterialData(Texture _mainTexture, Texture _stencilTexture)
	{
		RemoveFromPanel();
		if (_mainTexture != null)
		{
			m_texture = _mainTexture;
		}
		if (_stencilTexture != null)
		{
			m_stencilTexture = _stencilTexture;
		}
		Trigger();
		MarkAsChanged();
	}

	public void SetTextureOffset(Vector2 _alphaMaskOffset)
	{
		m_alphaMaskOffset = _alphaMaskOffset;
		Trigger();
		MarkAsChanged();
	}

	protected override void UpdateDynamicMaterial(Material _material)
	{
		base.UpdateDynamicMaterial(_material);
		if (_material != null)
		{
			if (m_stencilTexture != null)
			{
				_material.SetTexture("_Stencil", m_stencilTexture);
			}
			if (_material.HasProperty("_YOffsetVec"))
			{
				_material.SetVector("_YOffsetVec", m_alphaMaskOffset);
			}
		}
	}
}
