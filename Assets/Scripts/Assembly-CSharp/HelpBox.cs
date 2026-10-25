using System;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class HelpBox : Attribute
{
	public HelpBox(string message)
	{
	}
}
