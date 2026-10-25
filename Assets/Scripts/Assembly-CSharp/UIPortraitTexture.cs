using Assets.Scripts.UI.NGUI_Extensions.Model;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("NGUI/BB/UI/UIPortraitTexture")]
public class UIPortraitTexture : UIBasicSprite
{
	public delegate void OnMaterialUpdated();

	public static Vector4 s_defaultUVCoord = new Vector4(0f, 0f, 1f, 1f);

	[SerializeField]
	[HideInInspector]
	private Rect m_rect = new Rect(0f, 0f, 1f, 1f);

	[SerializeField]
	[HideInInspector]
	private Color m_bodyColor1 = Color.white;

	[HideInInspector]
	[SerializeField]
	private Color m_rarityColor = Color.white;

	[SerializeField]
	[HideInInspector]
	private float m_desaturation;

	[HideInInspector]
	[SerializeField]
	private Texture m_texture;

	[HideInInspector]
	[SerializeField]
	private Texture m_maskingTexture;

	[SerializeField]
	[HideInInspector]
	private Texture m_foregroundTexture;

	[HideInInspector]
	[SerializeField]
	private Texture m_themeTexture;

	[HideInInspector]
	[SerializeField]
	private Material m_materialRef;

	[HideInInspector]
	[SerializeField]
	private Material m_dynamicMaterial;

	[SerializeField]
	[HideInInspector]
	private bool m_backgroundOff;

	[SerializeField]
	[HideInInspector]
	private bool m_resizeWidgetToOrgSize;

	[SerializeField]
	[HideInInspector]
	private Vector4 m_border = Vector4.zero;

	[HideInInspector]
	[SerializeField]
	private bool m_fixedAspect;

	private string m_materialName = string.Empty;

	public override Texture mainTexture
	{
		get
		{
			return m_texture;
		}
		set
		{
			m_texture = value;
			if (this.MaterialUpdated != null)
			{
				UpdateMaterial();
			}
			MarkAsChanged();
		}
	}

	public Texture MaskingTexture
	{
		get
		{
			return m_maskingTexture;
		}
	}

	public Texture ForegroundTexture
	{
		get
		{
			return m_foregroundTexture;
		}
	}

	public bool IsBackgroundOff
	{
		get
		{
			return m_backgroundOff;
		}
	}

	public Vector2 OrgSize { get; private set; }

	public bool HasCustomUVCoords { get; private set; }

	public bool IsResizeWidgetToOrgSize
	{
		get
		{
			return m_resizeWidgetToOrgSize;
		}
		set
		{
			m_resizeWidgetToOrgSize = value;
			if (m_resizeWidgetToOrgSize)
			{
				base.width = (int)OrgSize.x;
				base.height = (int)OrgSize.y;
			}
		}
	}

	public override Material material
	{
		get
		{
			if (m_dynamicMaterial == null && m_materialRef != null)
			{
				m_dynamicMaterial = new Material(m_materialRef);
				m_materialName = "[NGUI] " + m_materialRef.name;
				UpdateMaterial();
			}
			return m_dynamicMaterial;
		}
		set
		{
			if (m_materialRef != value)
			{
				RemoveFromPanel();
				m_materialRef = value;
				NGUITools.DestroyImmediate(m_dynamicMaterial);
				m_dynamicMaterial = new Material(m_materialRef);
				m_materialName = "[NGUI] " + m_materialRef.name;
				UpdateMaterial();
				MarkAsChanged();
			}
		}
	}

	public override float alpha
	{
		get
		{
			return mColor.a;
		}
		set
		{
			if (mColor.a != value)
			{
				mColor.a = value;
				Invalidate(true);
				if (drawCall != null && drawCall.dynamicMaterial != null)
				{
					drawCall.dynamicMaterial.SetColor("_Color0", mColor);
				}
			}
		}
	}

	public override Vector4 border
	{
		get
		{
			return m_border;
		}
		set
		{
			if (m_border != value)
			{
				m_border = value;
				MarkAsChanged();
			}
		}
	}

	public Color BodyColor0
	{
		get
		{
			return mColor;
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
		set
		{
			m_rarityColor = value;
			Invalidate(true);
			if (drawCall != null && drawCall.dynamicMaterial != null && drawCall.dynamicMaterial.HasProperty("_Rarity"))
			{
				drawCall.dynamicMaterial.SetColor("_Rarity", mColor);
			}
		}
	}

	public Rect uvRect
	{
		get
		{
			return m_rect;
		}
		set
		{
			if (m_rect != value)
			{
				m_rect = value;
				MarkAsChanged();
			}
		}
	}

	public float Desaturation
	{
		get
		{
			return m_desaturation;
		}
		set
		{
			if (Mathf.Abs(m_desaturation - value) > 0.001f)
			{
				m_desaturation = value;
				UpdateMaterial();
			}
		}
	}

	public override Vector4 drawingDimensions
	{
		get
		{
			Vector2 vector = base.pivotOffset;
			float num = (0f - vector.x) * (float)mWidth;
			float num2 = (0f - vector.y) * (float)mHeight;
			float num3 = num + (float)mWidth;
			float num4 = num2 + (float)mHeight;
			if (m_texture != null && mType != Type.Tiled)
			{
				int num5 = m_texture.width;
				int num6 = m_texture.height;
				int num7 = 0;
				int num8 = 0;
				float num9 = 1f;
				float num10 = 1f;
				if (num5 > 0 && num6 > 0 && (mType == Type.Simple || mType == Type.Filled))
				{
					if (((uint)num5 & (true ? 1u : 0u)) != 0)
					{
						num7++;
					}
					if (((uint)num6 & (true ? 1u : 0u)) != 0)
					{
						num8++;
					}
					num9 = 1f / (float)num5 * (float)mWidth;
					num10 = 1f / (float)num6 * (float)mHeight;
				}
				if (mFlip == Flip.Horizontally || mFlip == Flip.Both)
				{
					num += (float)num7 * num9;
				}
				else
				{
					num3 -= (float)num7 * num9;
				}
				if (mFlip == Flip.Vertically || mFlip == Flip.Both)
				{
					num2 += (float)num8 * num10;
				}
				else
				{
					num4 -= (float)num8 * num10;
				}
			}
			float num11;
			float num12;
			if (m_fixedAspect)
			{
				num11 = 0f;
				num12 = 0f;
			}
			else
			{
				Vector4 vector2 = border;
				num11 = vector2.x + vector2.z;
				num12 = vector2.y + vector2.w;
			}
			float x = Mathf.Lerp(num, num3 - num11, mDrawRegion.x);
			float y = Mathf.Lerp(num2, num4 - num12, mDrawRegion.y);
			float z = Mathf.Lerp(num + num11, num3, mDrawRegion.z);
			float w = Mathf.Lerp(num2 + num12, num4, mDrawRegion.w);
			return new Vector4(x, y, z, w);
		}
	}

	public bool fixedAspect
	{
		get
		{
			return m_fixedAspect;
		}
		set
		{
			if (m_fixedAspect != value)
			{
				m_fixedAspect = value;
				mDrawRegion = new Vector4(0f, 0f, 1f, 1f);
				MarkAsChanged();
			}
		}
	}

	public event OnMaterialUpdated MaterialUpdated;

	public void SetBasicMaterialData(UIPortraitTexture _texture)
	{
		Color[] colors = new Color[3] { _texture.BodyColor0, _texture.BodyColor1, _texture.RarityColor };
		TextureModel s_textureModel = TextureModel.s_textureModel;
		s_textureModel.Clear();
		s_textureModel.Colors = colors;
		s_textureModel.MainTexture = _texture.mainTexture;
		s_textureModel.MaskingTexture = _texture.MaskingTexture;
		s_textureModel.ForegroundTexture = _texture.ForegroundTexture;
		SetMaterialData(s_textureModel);
	}

	public void SetMaterialData(TextureModel _model)
	{
		Vector4? uvCoord = _model.UvCoord;
		Vector4 vector = ((!uvCoord.HasValue) ? s_defaultUVCoord : uvCoord.Value);
		RemoveFromPanel();
		if (_model.Colors.Length > 0)
		{
			base.color = _model.Colors[0];
		}
		if (_model.Colors.Length > 1)
		{
			m_bodyColor1 = _model.Colors[1];
		}
		if (_model.Colors.Length > 2)
		{
			m_rarityColor = _model.Colors[2];
		}
		m_desaturation = _model.Desaturation;
		m_backgroundOff = _model.HideBackground;
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
		if (_model.OrgSize != Vector2.zero)
		{
			OrgSize = _model.OrgSize;
		}
		else
		{
			OrgSize = new Vector2(256f, 256f);
		}
		if (m_resizeWidgetToOrgSize)
		{
			base.width = (int)OrgSize.x;
			base.height = (int)OrgSize.y;
		}
		HasCustomUVCoords = vector != s_defaultUVCoord;
		if (HasCustomUVCoords)
		{
			m_rect = new Rect(vector.x, vector.y, vector.w, vector.z);
		}
		else
		{
			float num = 1f / OrgSize.x;
			float num2 = 1f / OrgSize.y;
			if (Mathf.Abs(OrgSize.x - OrgSize.y) > 0.0001f)
			{
				m_rect = new Rect(0f - num, 0.5f - num2, 1f, 0.5f);
			}
			else
			{
				m_rect = new Rect(0f - num, 0f - num2, 1f, 1f);
			}
		}
		base.transform.localPosition = _model.OffsetPos;
		if (this.MaterialUpdated != null)
		{
			this.MaterialUpdated();
		}
		MarkAsChanged();
	}

	public override void MakePixelPerfect()
	{
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();
		if (!m_fixedAspect)
		{
			return;
		}
		Texture texture = mainTexture;
		if (texture != null)
		{
			int num = texture.width;
			int num2 = texture.height;
			if ((num & 1) == 1)
			{
				num++;
			}
			if ((num2 & 1) == 1)
			{
				num2++;
			}
			float num3 = mWidth;
			float num4 = mHeight;
			float num5 = num3 / num4;
			float num6 = (float)num / (float)num2;
			if (num6 < num5)
			{
				float num7 = (num3 - num4 * num6) / num3 * 0.5f;
				base.drawRegion = new Vector4(num7, 0f, 1f - num7, 1f);
			}
			else
			{
				float num8 = (num4 - num3 / num6) / num4 * 0.5f;
				base.drawRegion = new Vector4(0f, num8, 1f, 1f - num8);
			}
		}
	}

	public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		Texture texture = mainTexture;
		if (!(texture == null))
		{
			Rect rect = new Rect(m_rect.x * (float)texture.width, m_rect.y * (float)texture.height, (float)texture.width * m_rect.width, (float)texture.height * m_rect.height);
			Rect inner = rect;
			Vector4 vector = border;
			inner.xMin += vector.x;
			inner.yMin += vector.y;
			inner.xMax -= vector.z;
			inner.yMax -= vector.w;
			float num = 1f / (float)texture.width;
			float num2 = 1f / (float)texture.height;
			rect.xMin *= num;
			rect.xMax *= num;
			rect.yMin *= num2;
			rect.yMax *= num2;
			inner.xMin *= num;
			inner.xMax *= num;
			inner.yMin *= num2;
			inner.yMax *= num2;
			int size = verts.size;
			Fill(verts, uvs, cols, rect, inner);
			if (onPostFill != null)
			{
				onPostFill(this, size, verts, uvs, cols);
			}
			UpdateMaterial();
		}
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		NGUITools.DestroyImmediate(m_dynamicMaterial);
	}

	private void OnDestroy()
	{
		ClearTextures();
	}

	private void ClearTextures()
	{
		if ((m_texture != null || m_maskingTexture != null || m_foregroundTexture != null || m_themeTexture != null || m_materialRef != null || m_dynamicMaterial != null) && Main.IsInitialized)
		{
			Main.Instance.RequestResourceUnload();
		}
		m_texture = null;
		m_maskingTexture = null;
		m_foregroundTexture = null;
		m_themeTexture = null;
		m_materialRef = null;
		m_dynamicMaterial = null;
	}

	private void UpdateMaterial()
	{
		if (m_dynamicMaterial != null)
		{
			UpdateDynamicMaterial(m_dynamicMaterial);
			if (drawCall != null && drawCall.dynamicMaterial != null && drawCall.dynamicMaterial.name.Equals(m_materialName))
			{
				UpdateDynamicMaterial(drawCall.dynamicMaterial);
			}
		}
	}

	private void UpdateDynamicMaterial(Material _material)
	{
		if (!(_material != null))
		{
			return;
		}
		if (m_backgroundOff)
		{
			_material.EnableKeyword("BACKGROUND_OFF");
		}
		else
		{
			_material.DisableKeyword("BACKGROUND_OFF");
		}
		if (_material.HasProperty("_Desaturation"))
		{
			_material.SetFloat("_Desaturation", m_desaturation);
		}
		Color bodyColor = BodyColor0;
		bodyColor.a = finalAlpha;
		_material.SetColor("_Color0", bodyColor);
		_material.SetColor("_Color1", BodyColor1);
		if (_material.HasProperty("_Rarity"))
		{
			_material.SetColor("_Rarity", RarityColor);
		}
		if (m_texture != null)
		{
			_material.SetTexture("_MainTex", m_texture);
		}
		if (m_maskingTexture != null)
		{
			_material.SetTexture("_MaskTex", m_maskingTexture);
		}
		if (m_foregroundTexture != null)
		{
			_material.SetTexture("_Foreground", m_foregroundTexture);
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
