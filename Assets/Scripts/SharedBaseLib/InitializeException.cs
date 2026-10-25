using System;
using UnityEngine;

public class InitializeException : ApplicationException
{
	protected UnityEngine.Object _owner;

	public override string Message
	{
		get
		{
			MonoBehaviour monoBehaviour = _owner as MonoBehaviour;
			string text;
			if (monoBehaviour != null)
			{
				text = "attached to GameObject '" + monoBehaviour.gameObject.name + "' ";
				monoBehaviour.enabled = false;
			}
			else
			{
				text = string.Empty;
			}
			return "Script of type '" + _owner.GetType().FullName + "' " + text + "with name '" + _owner.name + "' raised exception: " + base.Message;
		}
	}

	public InitializeException(MonoBehaviour owner, string message)
		: base(message)
	{
		_owner = owner;
	}

	public InitializeException(ScriptableObject owner, string message)
		: base(message)
	{
		_owner = owner;
	}
}
