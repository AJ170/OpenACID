using UbiCore.Messenger;
using UnityEngine;

internal sealed class MessengerManager : MonoBehaviour
{
	public static bool IsDestroyed { get; private set; }

	private void Awake()
	{
		Object.DontDestroyOnLoad(this);
	}

	public void OnDisable()
	{
		Messenger.Instance.Clean();
	}

	private void OnDestroy()
	{
		IsDestroyed = true;
	}
}
