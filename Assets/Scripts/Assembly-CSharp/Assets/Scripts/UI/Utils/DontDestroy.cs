using UnityEngine;

namespace Assets.Scripts.UI.Utils
{
	public class DontDestroy : MonoBehaviour
	{
		private void Awake()
		{
			Object.DontDestroyOnLoad(base.gameObject);
		}
	}
}
