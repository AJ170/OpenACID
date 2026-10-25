using UnityEngine;

namespace Assets.Scripts.Audio
{
	public class AudioThreeDSource : MonoBehaviour
	{
		public Transform ParentTransform { get; set; }

		private void Update()
		{
			if (ParentTransform != null)
			{
				base.transform.position = ParentTransform.position;
				base.transform.rotation = ParentTransform.rotation;
			}
		}
	}
}
