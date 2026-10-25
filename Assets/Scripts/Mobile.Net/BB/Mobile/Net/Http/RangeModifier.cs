using System.Net;

namespace BB.Mobile.Net.Http
{
	public sealed class RangeModifier : IHttpRequestModifier
	{
		private readonly int mStart;

		private readonly int mEnd;

		public int Start
		{
			get
			{
				return mStart;
			}
		}

		public int End
		{
			get
			{
				return mEnd;
			}
		}

		public RangeModifier(int start, int end)
		{
			mStart = start;
			mEnd = end;
		}

		public void ModifyHttpRequest(HttpWebRequest httpRequest)
		{
			httpRequest.AddRange(mStart, mEnd);
		}
	}
}
