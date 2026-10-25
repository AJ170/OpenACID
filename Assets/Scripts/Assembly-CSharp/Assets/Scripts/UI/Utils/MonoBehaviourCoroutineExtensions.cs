using System.Collections;
using UnityEngine;

namespace Assets.Scripts.UI.Utils
{
	public static class MonoBehaviourCoroutineExtensions
	{
		public static Coroutine<T> StartCoroutine<T>(this MonoBehaviour _behaviour, IEnumerator _enumerator)
		{
			Coroutine<T> coroutine = new Coroutine<T>();
			coroutine.coroutine = _behaviour.StartCoroutine(_enumerator);
			return coroutine;
		}
	}
}
