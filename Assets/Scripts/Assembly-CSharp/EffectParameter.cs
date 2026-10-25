using System;
using GenericUnityParameter;
using UnityEngine;

[Serializable]
public class EffectParameter : ScriptableObject
{
	[SerializeField]
	private string[] names;

	[SerializeField]
	private Parameter[] values;

	public string[] Names
	{
		get
		{
			return names;
		}
		set
		{
			names = value;
		}
	}

	public Parameter[] Values
	{
		get
		{
			return values;
		}
		set
		{
			values = value;
		}
	}

	public EffectParameter(string[] _names, Parameter[] _values)
	{
		names = _names;
		values = _values;
	}
}
