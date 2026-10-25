using Assets.Scripts.Mission.Logic;
using UnityEngine;

public static class ObjectFadeCallbacks
{
	public static void OnFadeFinishedDestroySelf(GameObject _gameObject, MonoBehaviour _fadeBehavior)
	{
		ObjectFade objectFade = _fadeBehavior as ObjectFade;
		if (objectFade != null)
		{
			objectFade.ReenableRenderers();
		}
		Object.Destroy(_fadeBehavior);
	}

	public static void OnFadeFinishedDeactivateGameObject(GameObject _gameObject, MonoBehaviour _fadeBehavior)
	{
		EntityWrapperComponent component = _gameObject.GetComponent<EntityWrapperComponent>();
		if (component != null && component.Entity != null)
		{
			component.Entity = null;
		}
		_gameObject.SetActive(false);
		OnFadeFinishedDestroySelf(_gameObject, _fadeBehavior);
	}
}
