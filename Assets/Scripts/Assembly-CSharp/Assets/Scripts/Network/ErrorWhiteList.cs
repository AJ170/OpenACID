using System.Collections.Generic;
using AcierProtocol;

namespace Assets.Scripts.Network
{
	public class ErrorWhiteList
	{
		private sealed class Pair
		{
			public ErrorCategory Category { get; private set; }

			public ErrorCode Code { get; private set; }

			public Pair(ErrorCategory _category, ErrorCode _code)
			{
				Category = _category;
				Code = _code;
			}

			public override bool Equals(object obj)
			{
				if (GetType() != obj.GetType())
				{
					return false;
				}
				Pair pair = (Pair)obj;
				return Category == pair.Category && Code == pair.Code;
			}

			public override int GetHashCode()
			{
				return (int)Category * 29 + (int)Code * 47;
			}
		}

		private static ErrorWhiteList instance;

		private HashSet<Pair> whiteList;

		public ErrorWhiteList()
		{
			whiteList = new HashSet<Pair>();
			whiteList.Add(new Pair(ErrorCategory.SERVER_SESSION, ErrorCode.SESSION_INVALID));
			whiteList.Add(new Pair(ErrorCategory.SERVER_SESSION, ErrorCode.SESSION_INVALID_REQUEST_TOKEN));
			whiteList.Add(new Pair(ErrorCategory.SERVER_MAIL, ErrorCode.MAIL_ALREADY_FRIENDS));
			whiteList.Add(new Pair(ErrorCategory.SERVER_FRIENDLIST, ErrorCode.REACHED_MAX_FRIEND_COUNT));
			whiteList.Add(new Pair(ErrorCategory.SERVER_FRIENDLIST, ErrorCode.FRIEND_NOT_FOUND));
		}

		public static bool IsOnWhiteList(ErrorCategory _category, ErrorCode _code)
		{
			return getInstance().whiteList.Contains(new Pair(_category, _code));
		}

		public static ErrorWhiteList getInstance()
		{
			if (instance == null)
			{
				instance = new ErrorWhiteList();
			}
			return instance;
		}
	}
}
