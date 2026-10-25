using UnityEngine;

namespace Assets.Scripts.UI.Utils
{
	[RequireComponent(typeof(LineRenderer))]
	public class SimpleLineRendererController : MonoBehaviour
	{
		private LineRenderer m_lineRenderer;

		private Color m_color;

		private float m_width;

		public Vector2 PointA { get; set; }

		public Vector2 PointB { get; set; }

		public Color Color
		{
			get
			{
				return m_color;
			}
			set
			{
				m_color = value;
				m_lineRenderer.SetColors(m_color, m_color);
			}
		}

		public float Width
		{
			get
			{
				return m_width;
			}
			set
			{
				m_width = value;
				m_lineRenderer.SetWidth(m_width / (float)Screen.currentResolution.width, m_width / (float)Screen.currentResolution.width);
			}
		}

		private void Awake()
		{
			m_lineRenderer = GetComponent<LineRenderer>();
			m_lineRenderer.SetVertexCount(2);
			Width = 8f;
			Color = Color.white;
		}

		private void OnEnable()
		{
			m_lineRenderer.enabled = true;
		}

		private void OnDisable()
		{
			m_lineRenderer.enabled = false;
		}

		private void Update()
		{
			m_lineRenderer.SetPosition(0, new Vector3(PointA.x, PointA.y, 0f));
			m_lineRenderer.SetPosition(1, new Vector3(PointB.x, PointB.y, 0f));
		}
	}
}
