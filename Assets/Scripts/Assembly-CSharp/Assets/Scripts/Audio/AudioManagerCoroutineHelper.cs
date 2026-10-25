using UnityEngine;

namespace Assets.Scripts.Audio
{
	public class AudioManagerCoroutineHelper : MonoBehaviour
	{
		private void Start()
		{
			InvokeRepeating("CallResourceManagersScheduledUnload", 8f, 2f);
		}

		private void CallResourceManagersScheduledUnload()
		{
			AudioResourceManager.Instance.HandleScheduledUnloads();
		}

		private void Update()
		{
			AudioManager.Instance.DuckingSystem.Update();
			AudioManager.Instance.Update();
		}
	}
}
