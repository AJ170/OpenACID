using UbiCore;
using UnityEngine;

namespace Assets.Scripts.UI.Utils
{
	[ExecuteInEditMode]
	public class UIRescaleCollision : MonoBehaviour
	{
		private const float DefaultScaleX = 1f;

		private const float DefaultScaleY = 1f;

		private const bool DefaultIsRelative = false;

		private const float DefaultPaddingX = 0f;

		private const float DefaultPaddingY = 0f;

		[SerializeField]
		private bool m_onlyEnableOnSmallScreens = true;

		[SerializeField]
		private float m_scaleX = 1f;

		[SerializeField]
		private float m_scaleY = 1f;

		[SerializeField]
		private bool m_relative;

		[SerializeField]
		private float m_paddingX;

		[SerializeField]
		private float m_paddingY;

		private BoxCollider m_collider;

		private Vector2 m_colliderDefaultSize = Vector2.one;

		private bool m_initialized;

		private BoxCollider Collider
		{
			get
			{
				if (m_collider == null)
				{
					m_collider = GetComponent<BoxCollider>();
					return GetComponent<BoxCollider>();
				}
				return m_collider;
			}
		}

		private void Start()
		{
			if (!m_initialized)
			{
				Init();
				Refresh();
			}
		}

		private void Init()
		{
			if (!m_initialized)
			{
				m_initialized = true;
				SetDefaultSizeToCurrent();
				if (!HasCustomValues())
				{
					SetScaleRelativeToDpi();
				}
			}
		}

		private bool HasCustomValues()
		{
			return m_scaleX != 1f || m_scaleY != 1f || m_relative || m_paddingX != 0f || m_paddingY != 0f;
		}

		private void SetScaleRelativeToDpi()
		{
			if (IsScreenIphoneSize())
			{
				SetPaddingiPhone();
			}
		}

		private bool IsScreenIphoneSize()
		{
			float dpi = Screen.dpi;
			if (!MathHelper.Approximately(dpi, 0f) && dpi > 264f && Screen.height < UIUtil.ReferenceScreenHeight)
			{
				return true;
			}
			return false;
		}

		[ContextMenu("SetPaddingiPhone")]
		private void SetPaddingiPhone()
		{
			m_paddingX = 30f;
			m_paddingY = 30f;
		}

		[ContextMenu("SetDefaultSizeToCurrent")]
		private void SetDefaultSizeToCurrent()
		{
			m_colliderDefaultSize = Collider.size;
		}

		private void SetRelative(float _defaultX, float _defaultY, float _x, float _y)
		{
			Collider.size = new Vector3(_defaultX * _x, _defaultY * _y);
		}

		private void SetAbsolute(float _defaultX, float _defaultY, float _x, float _y)
		{
			Collider.size = new Vector3(_defaultX + _x, _defaultY + _y);
		}

		[ContextMenu("Refresh")]
		private void Refresh()
		{
			if (base.enabled && (!m_onlyEnableOnSmallScreens || IsScreenIphoneSize() || Application.isEditor))
			{
				if (m_relative)
				{
					SetRelative(m_colliderDefaultSize.x, m_colliderDefaultSize.y, m_scaleX, m_scaleY);
				}
				else
				{
					SetAbsolute(m_colliderDefaultSize.x, m_colliderDefaultSize.y, m_paddingX, m_paddingY);
				}
			}
		}

		public void OnCollisionBoxChangedExtrenally()
		{
			if (base.enabled)
			{
				if (!m_initialized)
				{
					Init();
				}
				else
				{
					SetDefaultSizeToCurrent();
				}
				Refresh();
			}
		}
	}
}
