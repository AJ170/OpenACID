using System;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class WarnBox : HelpBox
{
	public WarnBox(string message)
		: base(message)
	{
	}
}
