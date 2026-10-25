using UnityEngine;

namespace BB.Mobile.Net
{
	public sealed class UnityInvoke : MonoBehaviour
	{
		private static readonly SynchronizeInvokeDispatcher mApplicationDispatcher = new SynchronizeInvokeDispatcher(false);

		private static readonly AsyncCallbackDispatcher mApplicationCallbackDispatcher = new AsyncCallbackDispatcher();

		public static IExtendedSynchronizeInvoke Application
		{
			get
			{
				return mApplicationDispatcher;
			}
		}

		public static AsyncCallbackDispatcher ApplicationCallbackDispatcher
		{
			get
			{
				return mApplicationCallbackDispatcher;
			}
		}

		private void Awake()
		{
			mApplicationDispatcher.TakeOwnership();
		}

		private void Update()
		{
			mApplicationDispatcher.Dispatch();
			mApplicationCallbackDispatcher.Dispatch();
		}
	}
}
