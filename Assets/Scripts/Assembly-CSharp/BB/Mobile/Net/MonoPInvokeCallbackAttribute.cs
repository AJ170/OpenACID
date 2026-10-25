using System;

namespace BB.Mobile.Net
{
	public class MonoPInvokeCallbackAttribute : Attribute
	{
		private readonly Type mType;

		public Type Type
		{
			get
			{
				return mType;
			}
		}

		public MonoPInvokeCallbackAttribute(Type type)
		{
			mType = type;
		}
	}
}
