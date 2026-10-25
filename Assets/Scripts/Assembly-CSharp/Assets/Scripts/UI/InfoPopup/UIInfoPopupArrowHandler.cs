using UnityEngine;

namespace Assets.Scripts.UI.InfoPopup
{
	public class UIInfoPopupArrowHandler : MonoBehaviour
	{
		private const string c_arrowPrefabPath = "UI/Animus/Widgets/InfoPopup/SelectionArrow";

		private static readonly Vector2 s_leftArrowAnchorPoint = new Vector2(-54f, 24f);

		private static readonly Vector2 s_rightArrowAnchorPoint = new Vector2(-24f, 54f);

		private UIWidget m_parentWidget;

		private UIWidget m_arrowFadeBoundsWidget;

		private UISprite m_leftArrow;

		private UISprite m_rightArrow;

		private Transform m_followTransform;

		private bool m_fadeAtBorder = true;

		private bool m_isActive;

		public bool IsActive
		{
			get
			{
				return m_isActive;
			}
			set
			{
				m_isActive = value;
				if (m_isActive)
				{
					UpdateArrowPosition();
					return;
				}
				m_leftArrow.gameObject.SetActive(false);
				m_rightArrow.gameObject.SetActive(false);
			}
		}

		public bool FadeAtBorder
		{
			get
			{
				return m_fadeAtBorder;
			}
			set
			{
				m_fadeAtBorder = value;
			}
		}

		public void Init(UIWidget _parentWidget, bool _fadeAtBorder, UIWidget _arrowFadeBoundsWidgetbool)
		{
			m_parentWidget = _parentWidget;
			m_arrowFadeBoundsWidget = ((!(_arrowFadeBoundsWidgetbool != null)) ? m_parentWidget : _arrowFadeBoundsWidgetbool);
			FadeAtBorder = _fadeAtBorder;
			GameObject prefab = Resources.Load("UI/Animus/Widgets/InfoPopup/SelectionArrow", typeof(GameObject)) as GameObject;
			UISprite component = NGUITools.AddChild(_parentWidget.gameObject, prefab).GetComponent<UISprite>();
			UIRect.AnchorPoint leftAnchor = component.leftAnchor;
			Transform target = base.transform;
			Vector2 vector = s_leftArrowAnchorPoint;
			leftAnchor.Set(target, 0f, vector.x);
			UIRect.AnchorPoint rightAnchor = component.rightAnchor;
			Transform target2 = base.transform;
			Vector2 vector2 = s_leftArrowAnchorPoint;
			rightAnchor.Set(target2, 0f, vector2.y);
			component.flip = UIBasicSprite.Flip.Horizontally;
			component.pivot = UIWidget.Pivot.Left;
			m_leftArrow = component;
			UISprite component2 = NGUITools.AddChild(_parentWidget.gameObject, prefab).GetComponent<UISprite>();
			UIRect.AnchorPoint leftAnchor2 = component2.leftAnchor;
			Transform target3 = base.transform;
			Vector2 vector3 = s_rightArrowAnchorPoint;
			leftAnchor2.Set(target3, 1f, vector3.x);
			UIRect.AnchorPoint rightAnchor2 = component2.rightAnchor;
			Transform target4 = base.transform;
			Vector2 vector4 = s_rightArrowAnchorPoint;
			rightAnchor2.Set(target4, 1f, vector4.y);
			component2.flip = UIBasicSprite.Flip.Nothing;
			component2.pivot = UIWidget.Pivot.Right;
			m_rightArrow = component2;
		}

		public void ConfigContent(InfoPopUpConfiguration _config, Transform _followTransform)
		{
			m_followTransform = _followTransform;
			IsActive = m_followTransform != null;
			if (m_followTransform != null)
			{
				m_rightArrow.color = _config.FrameColor;
				m_leftArrow.color = _config.FrameColor;
			}
		}

		private void OnDestroy()
		{
			m_followTransform = null;
			NGUITools.Destroy(m_rightArrow);
			m_rightArrow = null;
			NGUITools.Destroy(m_leftArrow);
			m_leftArrow = null;
		}

		private void Update()
		{
			UpdateArrowPosition();
		}

		private void UpdateArrowPosition()
		{
			if (!(m_followTransform == null) && IsActive)
			{
				bool flag = (m_leftArrow.transform.position - m_followTransform.position).x < 0f;
				m_leftArrow.gameObject.SetActive(!flag);
				m_rightArrow.gameObject.SetActive(flag);
				UISprite uISprite = ((!flag) ? m_leftArrow : m_rightArrow);
				uISprite.transform.position = new Vector3(uISprite.transform.position.x, m_followTransform.position.y, uISprite.transform.position.z);
				if (m_fadeAtBorder)
				{
					UpdateArrowAlpha(m_arrowFadeBoundsWidget, uISprite);
				}
			}
		}

		private void UpdateArrowAlpha(UIWidget _bounds, UISprite _arrow)
		{
			Vector3 vector = _arrow.worldCenter - _bounds.worldCenter;
			Vector3 vector2 = ((!(vector.y >= 0f)) ? _bounds.worldCorners[3] : _bounds.worldCorners[2]);
			float num = ((!(vector.y < 0f)) ? 1f : (-1f));
			float value = (_arrow.worldCenter.y - vector2.y) / (_arrow.worldCenter.y - _arrow.worldCorners[1].y) * num;
			_arrow.alpha = Mathf.Clamp01(value);
		}
	}
}
