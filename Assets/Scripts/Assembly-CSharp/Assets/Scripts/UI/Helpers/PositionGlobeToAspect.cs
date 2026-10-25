using UnityEngine;

namespace Assets.Scripts.UI.Helpers
{
	public class PositionGlobeToAspect : MonoBehaviour
	{
		public float Factor = -4.77f;

		private void Update()
		{
			float num = 1.3333334f;
			float num2 = (float)Screen.width / (float)Screen.height;
			float z = (num2 - num) * Factor;
			base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y, z);
		}
	}
}
