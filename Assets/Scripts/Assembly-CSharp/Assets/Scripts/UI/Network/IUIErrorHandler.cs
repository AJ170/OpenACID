using System;
using Assets.Scripts.Network.Error;

namespace Assets.Scripts.UI.Network
{
	public interface IUIErrorHandler
	{
		void Handle(AcierErrorException _error, UINetworkAdaptor.DataStoreRequest _request, AsyncCallback _callback, bool _isBlocked);
	}
}
