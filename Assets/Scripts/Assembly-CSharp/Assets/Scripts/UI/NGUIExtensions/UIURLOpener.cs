using UnityEngine;

namespace Assets.Scripts.UI.NGUIExtensions
{
	[AddComponentMenu("NGUI/Interaction/URL Opener")]
	public class UIURLOpener : MonoBehaviour
	{
		[SerializeField]
		private string m_URL;

		public string URL
		{
			get
			{
				return m_URL;
			}
			set
			{
				m_URL = value;
			}
		}

		private void OnClick()
		{
			if (base.enabled)
			{
				Application.OpenURL(m_URL);
			}
		}
	}
}
