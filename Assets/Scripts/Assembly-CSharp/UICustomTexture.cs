using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/NGUI UICustomTexture")]
public class UICustomTexture : UITexture
{
	[NonSerialized]
	private Material m_dynamicMaterial;

	public override Material material
	{
		get
		{
			if (m_dynamicMaterial == null && mMat != null)
			{
				m_dynamicMaterial = new Material(mMat);
			}
			return m_dynamicMaterial;
		}
		set
		{
			if (mMat != value)
			{
				RemoveFromPanel();
				mShader = null;
				mMat = value;
				mPMA = -1;
				NGUITools.DestroyImmediate(m_dynamicMaterial);
				m_dynamicMaterial = new Material(mMat);
				MarkAsChanged();
			}
		}
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		NGUITools.DestroyImmediate(m_dynamicMaterial);
		m_dynamicMaterial = null;
	}
}
