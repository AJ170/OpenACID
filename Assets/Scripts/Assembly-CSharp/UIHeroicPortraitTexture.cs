using Assets.Scripts.UI.NGUI_Extensions.Model;
using UnityEngine;

[AddComponentMenu("NGUI/BB/UI/UIHeroicPortraitTexture")]
[ExecuteInEditMode]
public class UIHeroicPortraitTexture : UICustomShaderTexture
{
	[SerializeField]
	[HideInInspector]
	private Color m_bodyColor1 = Color.white;

	[HideInInspector]
	[SerializeField]
	private Color m_rarityColor = Color.white;

	[SerializeField]
	[HideInInspector]
	private Texture m_maskingTexture;

	[HideInInspector]
	[SerializeField]
	private Texture m_foregroundTexture;

	[HideInInspector]
	[SerializeField]
	private Texture m_themeTexture;

	[SerializeField]
	[HideInInspector]
	private Texture m_heroicFrameTexture;

	[HideInInspector]
	[SerializeField]
	private Texture m_heroicFrameStencilTexture;

	[HideInInspector]
	[SerializeField]
	private bool m_resetMaskTextureUV;

	[HideInInspector]
	[SerializeField]
	private bool m_resizeWidgetToFullSize;

	public Vector2 OrgSize { get; private set; }

	public bool IsResizeWidgetToFullSize
	{
		get
		{
			return m_resizeWidgetToFullSize;
		}
		set
		{
			m_resizeWidgetToFullSize = value;
			if (m_resizeWidgetToFullSize)
			{
				base.width = (int)OrgSize.x;
				base.height = (int)OrgSize.y;
			}
		}
	}

	public Color BodyColor1
	{
		get
		{
			return m_bodyColor1;
		}
	}

	public Color RarityColor
	{
		get
		{
			return m_rarityColor;
		}
	}

	public void SetMaterialData(HeroicTextureModel _model)
	{
		RemoveFromPanel();
		if (_model.Colors.Length > 0)
		{
			base.color = new Color(_model.Colors[0].r, _model.Colors[0].g, _model.Colors[0].b, base.color.a);
		}
		if (_model.Colors.Length > 1)
		{
			m_bodyColor1 = _model.Colors[1];
		}
		if (_model.Colors.Length > 2)
		{
			m_rarityColor = _model.Colors[2];
		}
		if (_model.MainTexture != null)
		{
			m_texture = _model.MainTexture;
		}
		if (_model.MaskingTexture != null)
		{
			m_maskingTexture = _model.MaskingTexture;
		}
		if (_model.ForegroundTexture != null)
		{
			m_foregroundTexture = _model.ForegroundTexture;
		}
		m_themeTexture = _model.ThemeTexture;
		if (_model.HeroicFrameTexture != null)
		{
			m_heroicFrameTexture = _model.HeroicFrameTexture;
		}
		if (_model.HeroicFrameStencilTexture != null)
		{
			m_heroicFrameStencilTexture = _model.HeroicFrameStencilTexture;
		}
		if (_model.OrgSize != Vector2.zero)
		{
			OrgSize = _model.OrgSize;
		}
		else
		{
			OrgSize = new Vector2(256f, 256f);
		}
		if (m_resizeWidgetToFullSize)
		{
			base.width = (int)OrgSize.x;
			base.height = (int)OrgSize.y;
		}
		Trigger();
		MarkAsChanged();
	}

	protected override void ClearTextures()
	{
		base.ClearTextures();
		if ((m_maskingTexture != null || m_foregroundTexture != null || m_themeTexture != null || m_heroicFrameTexture != null || m_heroicFrameStencilTexture != null) && Main.IsInitialized)
		{
			Main.Instance.RequestResourceUnload();
		}
		m_maskingTexture = null;
		m_foregroundTexture = null;
		m_themeTexture = null;
		m_heroicFrameTexture = null;
		m_heroicFrameStencilTexture = null;
	}

	protected override void UpdateDynamicMaterial(Material _material)
	{
		base.UpdateDynamicMaterial(_material);
		if (!(_material != null))
		{
			return;
		}
		_material.SetColor("_Color1", BodyColor1);
		if (m_maskingTexture != null)
		{
			_material.SetTexture("_MaskTex", m_maskingTexture);
			if (m_resetMaskTextureUV)
			{
				_material.SetTextureOffset("_MaskTex", Vector2.zero);
				_material.SetTextureScale("_MaskTex", Vector2.one);
			}
		}
		if (m_foregroundTexture != null)
		{
			_material.SetTexture("_Foreground", m_foregroundTexture);
		}
		if (m_foregroundTexture != null)
		{
			_material.SetTexture("_HeroicFrame", m_heroicFrameTexture);
		}
		if (m_heroicFrameStencilTexture != null)
		{
			_material.SetTexture("_HeroicStencil", m_heroicFrameStencilTexture);
		}
		if (_material.HasProperty("_ShowInsigniaItem"))
		{
			int value = ((m_themeTexture != null) ? 1 : 0);
			_material.SetInt("_ShowInsigniaItem", value);
			if (m_themeTexture != null)
			{
				_material.SetTexture("_InsigniaItem", m_themeTexture);
			}
		}
	}
}
