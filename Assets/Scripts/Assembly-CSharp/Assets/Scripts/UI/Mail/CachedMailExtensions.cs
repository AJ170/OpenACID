using Assets.Scripts.UI.Utils.Extensions;

namespace Assets.Scripts.UI.Mail
{
	public static class CachedMailExtensions
	{
		public static void MarkAsRead(this CachedMail _mail, bool _read)
		{
			_mail.Header.isRead = _read;
			_mail.Save();
		}

		public static bool IsExpired(this CachedMail _mail)
		{
			return _mail.Header.IsExpired();
		}
	}
}
