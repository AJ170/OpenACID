using System;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class Tooltip : Attribute
{
	private string m_message;

	public string message
	{
		get
		{
			return m_message;
		}
	}

	public Tooltip(string message)
	{
		m_message = message;
	}
}
