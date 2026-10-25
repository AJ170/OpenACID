using System;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Network;
using Assets.Scripts.Network.Error;
using Assets.Scripts.UI.Mail;
using Assets.Scripts.UI.Network;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Utils.Extensions;
using SharedBaseLib.System.Extensions;

namespace Assets.Scripts.UI.State.MailManager
{
	public class MailManager
	{
		public delegate void GiftProcessedCallback(MailHeader _header, MailBody _body);

		public delegate void HandleMailsChanged(List<MailHeader> _headers);

		public delegate void AsyncHeadersCallback(List<MailHeader> _headers);

		public delegate void AsyncMailBodyCallback(MailBody _body);

		public int UnreadMails
		{
			get
			{
				List<MailHeader> headers = MailCache.Headers;
				if (headers == null)
				{
					return 0;
				}
				return headers.Count((MailHeader _x) => !_x.isRead);
			}
		}

		public event HandleMailsChanged OnMailsChanged;

		public event HandleMailsChanged OnHeaderRemoved;

		public event HandleMailsChanged OnHeaderAdded;

		public event HandleMailsChanged OnMailWasRead;

		public event GiftProcessedCallback OnGiftProcessedSuccessful;

		public void Init()
		{
			MailCache.Init();
			MailCache.OnHeaderAdded += OnCacheHeaderAdded;
			MailCache.OnMailRemoved += OnCacheMailRemoved;
			MailCache.OnMailRead += OnMailRead;
		}

		~MailManager()
		{
			MailCache.OnHeaderAdded -= OnCacheHeaderAdded;
			MailCache.OnMailRemoved -= OnCacheMailRemoved;
			MailCache.OnMailRead -= OnMailRead;
		}

		public UIRequestToken GetMailHeaders(AsyncHeadersCallback _callback)
		{
			long lastMail = MailCache.LastSyncTime;
			AsyncCallback callback = delegate(IAsyncResult _ar)
			{
				OnMailHeadersResult(_ar, _callback ?? new AsyncHeadersCallback(DefaultHeaderHandler));
			};
			return UINetworkAdaptor.NetworkRequest(delegate(AsyncCallback _result)
			{
				Singleton<StateManager>.Instance.ProfileManager.GetMailHeaders(lastMail, 0, _result);
			}, null, callback);
		}

		public UIRequestToken GetMailBody(MailHeader _header, AsyncMailBodyCallback _callback)
		{
			_callback = _callback ?? new AsyncMailBodyCallback(DefaultBodyHandler);
			MailBody mailBody = MailCache.GetMailBody(_header);
			if (mailBody != null)
			{
				_callback(mailBody);
				return new UIRequestToken();
			}
			AsyncCallback callback = delegate(IAsyncResult _ar)
			{
				OnMailBodyResult(_ar, _callback, _header);
			};
			return UINetworkAdaptor.BlockedNetworkRequest(delegate(AsyncCallback _result)
			{
				Singleton<StateManager>.Instance.ProfileManager.GetMailBody(_header.headerId, _result);
			}, null, callback);
		}

		public UIRequestToken DeleteMail(MailHeader _header, AsyncHeadersCallback _callback)
		{
			return DeleteMails(new List<MailHeader> { _header }, _callback);
		}

		public UIRequestToken DeleteMails(List<MailHeader> _headers, AsyncHeadersCallback _callback)
		{
			List<string> ids = _headers.ConvertAll((MailHeader _x) => _x.headerId);
			if (ids.Count == 0)
			{
				return new UIRequestToken();
			}
			AsyncCallback callback = delegate(IAsyncResult _ar)
			{
				OnDeleteMailResult(_ar, _callback ?? new AsyncHeadersCallback(DefaultHeaderHandler), ids);
			};
			return UINetworkAdaptor.BlockedNetworkRequest(delegate(AsyncCallback _result)
			{
				Singleton<StateManager>.Instance.ProfileManager.DeleteMails(ids, _result);
			}, null, callback);
		}

		public UIRequestToken ProcessMail(MailHeader _header, MailBody _body, bool _accept, AsyncHeadersCallback _callback)
		{
			if (_body.IsResourceGift() && _accept)
			{
				Singleton<UIScreenManager>.Instance.SceneData.UIMenuBars.WalletHandler.ProfileSyncEnabled = false;
			}
			AsyncCallback callback = delegate(IAsyncResult _ar)
			{
				OnProcessMailResult(_ar, _callback ?? new AsyncHeadersCallback(DefaultHeaderHandler), _header, _body, _accept);
			};
			return UINetworkAdaptor.BlockedNetworkRequest(delegate(AsyncCallback _result)
			{
				Singleton<StateManager>.Instance.ProfileManager.ProcessMail(_header.headerId, _accept, _result);
			}, null, callback);
		}

		private void OnMailHeadersResult(IAsyncResult _asyncResult, AsyncHeadersCallback _callback)
		{
			if (!_asyncResult.IsError())
			{
				try
				{
					MailHeader[] headers = ((AsyncDataResult<GetMailHeadersResponse>)_asyncResult).Result().headers.headers;
					MailCache.AddHeaders(headers);
				}
				catch (NullReferenceException)
				{
				}
				catch (AcierErrorException)
				{
				}
			}
			_callback(MailCache.Headers);
		}

		private void OnMailBodyResult(IAsyncResult _asyncResult, AsyncMailBodyCallback _callback, MailHeader _header)
		{
			if (_asyncResult.IsError())
			{
				HandleMailRemoval(_asyncResult, _header);
				_callback(null);
				return;
			}
			MailBody body = null;
			try
			{
				body = ((AsyncDataResult<GetMailBodyResponse>)_asyncResult).Result().mailBody;
				MailCache.AddBody(_header, body);
			}
			catch (AcierErrorException)
			{
			}
			_callback(body);
		}

		private void OnDeleteMailResult(IAsyncResult _asyncResult, AsyncHeadersCallback _callback, List<string> _toDelete)
		{
			if (_asyncResult.IsError())
			{
				_callback(MailCache.Headers);
			}
			else
			{
				try
				{
					string[] failed = ((AsyncDataResult<DeleteMailsResponse>)_asyncResult).Result().failedIds;
					_toDelete.RemoveAll((string x) => failed.Contains(x));
					MailCache.RemoveMails(_toDelete);
				}
				catch (NullReferenceException)
				{
				}
				catch (AcierErrorException)
				{
				}
			}
			_callback(MailCache.Headers);
		}

		private void OnProcessMailResult(IAsyncResult _asyncResult, AsyncHeadersCallback _callback, MailHeader _header, MailBody _body, bool _accepted)
		{
			if (_asyncResult.IsError())
			{
				Singleton<UIScreenManager>.Instance.SceneData.UIMenuBars.WalletHandler.ProfileSyncEnabled = true;
				HandleMailRemoval(_asyncResult, _header);
			}
			else
			{
				try
				{
					if (_accepted && _body.attachment.gift != null && this.OnGiftProcessedSuccessful != null)
					{
						this.OnGiftProcessedSuccessful(_header, _body);
					}
					else
					{
						Singleton<UIScreenManager>.Instance.SceneData.UIMenuBars.WalletHandler.ProfileSyncEnabled = true;
					}
					if (((AsyncDataResult<ProcessMailResponse>)_asyncResult).Result().removeMail)
					{
						MailCache.RemoveHeader(_header);
					}
				}
				catch (AcierErrorException)
				{
				}
			}
			_callback(MailCache.Headers);
		}

		private void OnCacheChanged()
		{
			if (this.OnMailsChanged != null)
			{
				this.OnMailsChanged(MailCache.Headers);
			}
		}

		private void OnCacheMailRemoved()
		{
			if (this.OnHeaderRemoved != null)
			{
				this.OnHeaderRemoved(MailCache.Headers);
			}
			OnCacheChanged();
		}

		private void OnCacheHeaderAdded()
		{
			if (this.OnHeaderAdded != null)
			{
				this.OnHeaderAdded(MailCache.Headers);
			}
			OnCacheChanged();
		}

		private void HandleMailRemoval(IAsyncResult _asyncResult, MailHeader _header)
		{
			AsyncErrorResult asyncErrorResult = _asyncResult as AsyncErrorResult;
			if (asyncErrorResult != null && asyncErrorResult.ShouldRemoveMail())
			{
				MailCache.RemoveHeader(_header);
			}
		}

		private void OnMailRead(MailHeader _header, bool _read)
		{
			if (this.OnMailWasRead != null && _read)
			{
				this.OnMailWasRead(new List<MailHeader> { _header });
			}
		}

		private static void DefaultHeaderHandler(List<MailHeader> _headers)
		{
		}

		private static void DefaultBodyHandler(MailBody _body)
		{
		}
	}
}
