using System.Collections.Generic;
using UbiCore.Utils.Extensions;
using UnityEngine;

public abstract class FindAndInstantiateManager<TFindClass, TInstanceClass> where TFindClass : MonoBehaviour where TInstanceClass : MonoBehaviour
{
	protected List<TInstanceClass> Instantiations = new List<TInstanceClass>();

	public virtual void Init()
	{
		Shutdown();
		FindAndSetup();
	}

	public virtual void Shutdown()
	{
		for (int num = Instantiations.Count - 1; num >= 0; num--)
		{
			TInstanceClass obj = Instantiations[num];
			Object.Destroy(obj);
		}
	}

	protected abstract void SetupInstance(TInstanceClass _instance);

	private void FindAndSetup()
	{
		TFindClass[] array = Object.FindObjectsOfType<TFindClass>();
		if (array != null && array.Length > 0)
		{
			for (int i = 0; i < array.Length; i++)
			{
				TInstanceClass val = array[i].gameObject.AddComponentIfNotExist<TInstanceClass>();
				Instantiations.Add(val);
				SetupInstance(val);
			}
		}
	}
}
