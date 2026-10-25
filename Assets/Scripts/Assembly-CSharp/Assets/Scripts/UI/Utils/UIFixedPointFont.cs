using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Utils
{
	[RequireComponent(typeof(UILabel))]
	public class UIFixedPointFont : MonoBehaviour
	{
		private static int IPAD_RETINA_DPI = 264;

		private UILabel m_label;

		private void Awake()
		{
			m_label = GetComponent<UILabel>();
		}

		private void Start()
		{
			float points = DisplayHelpers.PixelToPoints(m_label.fontSize, IPAD_RETINA_DPI);
			float num = DisplayHelpers.PointToPixels(points, DisplayHelpers.DPI);
			m_label.fontSize = (int)num;
		}
	}
}
