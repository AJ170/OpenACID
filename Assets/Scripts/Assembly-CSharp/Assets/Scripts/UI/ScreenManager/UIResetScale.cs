using UnityEngine;

namespace Assets.Scripts.UI.ScreenManager
{
	public class UIResetScale : MonoBehaviour
	{
		public Vector3 m_scale = new Vector3(1f, 1f, 1f);

		public void ResetScale()
		{
			base.gameObject.transform.localScale = new Vector3(m_scale.x, m_scale.y, m_scale.z);
		}
	}
}
