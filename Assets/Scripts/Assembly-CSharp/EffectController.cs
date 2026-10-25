using System.Collections.Generic;
using Database;
using SharedBaseLib.Core;
using UnityEngine;

public class EffectController : MonoBehaviour
{
	private class ActiveEffect
	{
		public string Name;

		private int m_Active;

		private int m_OnCounter;

		public List<EffectBase> EffectList;

		public List<EffectBase> InactiveEffects;

		public bool Active
		{
			get
			{
				return m_Active > 0;
			}
			private set
			{
				if (value)
				{
					m_Active++;
				}
				else if (m_Active > 0)
				{
					m_Active--;
				}
			}
		}

		public ActiveEffect(string _name, EffectBase[] _effects)
		{
			m_Active = 0;
			m_OnCounter = 0;
			EffectList = new List<EffectBase>();
			InactiveEffects = new List<EffectBase>();
			Activate(_name, _effects);
		}

		public void On()
		{
			if (m_OnCounter == 0)
			{
				Reactivate();
				int count = EffectList.Count;
				for (int i = 0; count > i; i++)
				{
					EffectList[i].On();
				}
			}
			m_OnCounter++;
		}

		public void Off()
		{
			if (m_OnCounter <= 0)
			{
				return;
			}
			m_OnCounter--;
			if (m_OnCounter == 0)
			{
				int count = EffectList.Count;
				int num = count - 1;
				while (0 <= num)
				{
					EffectList[num].Off();
					num--;
				}
			}
		}

		public void Activate(string _name, EffectBase[] _effects)
		{
			Name = _name;
			if (EffectList.Count > 0)
			{
				for (int i = 0; _effects.Length > i; i++)
				{
					if (!EffectList.Contains(_effects[i]))
					{
						EffectList.Add(_effects[i]);
					}
				}
			}
			else
			{
				EffectList.AddRange(_effects);
			}
			Active = true;
		}

		public void Reactivate()
		{
			int count = InactiveEffects.Count;
			if (count <= 0)
			{
				return;
			}
			for (int i = 0; count > i; i++)
			{
				if (InactiveEffects[i] != null)
				{
					EffectList.Add(InactiveEffects[i]);
				}
			}
			InactiveEffects.Clear();
		}

		public bool Deactivate(EffectBase _who)
		{
			if (Active)
			{
				InactiveEffects.Add(_who);
				EffectList.Remove(_who);
				if (EffectList.Count == 0)
				{
					Active = false;
					Name = string.Empty;
					return true;
				}
			}
			return false;
		}
	}

	public BBList<int> Conditions = new BBList<int>();

	public HumanoidDef.ECharType EntityType;

	private List<ActiveEffect> m_ActiveEffects = new List<ActiveEffect>();

	private List<Transform> m_Transforms = new List<Transform>();

	public static void Copy(EffectController _source, GameObject _dest)
	{
		EffectController effectController = _dest.AddComponent<EffectController>();
		effectController.EntityType = _source.EntityType;
	}

	public bool ActivateCondition(EffectManager.ECondition _condition)
	{
		if (Conditions.Contains((int)_condition))
		{
			return true;
		}
		Conditions.Add((int)_condition);
		return false;
	}

	public bool RemoveCondition(EffectManager.ECondition _condition)
	{
		return Conditions.Remove((int)_condition);
	}

	private void Start()
	{
		RelinkBones();
	}

	public void OnEffectTrigger(string _name)
	{
		SpawnIfNotYetActive(_name);
		int count = m_ActiveEffects.Count;
		for (int i = 0; count > i; i++)
		{
			ActiveEffect activeEffect = m_ActiveEffects[i];
			if (activeEffect.Name == _name)
			{
				int count2 = activeEffect.EffectList.Count;
				for (int j = 0; count2 > j; j++)
				{
					activeEffect.EffectList[j].Trigger();
				}
			}
		}
	}

	public void OnEffectOn(string _name)
	{
		SpawnIfNotYetActive(_name);
		int count = m_ActiveEffects.Count;
		for (int i = 0; count > i; i++)
		{
			ActiveEffect activeEffect = m_ActiveEffects[i];
			if (activeEffect.Name == _name)
			{
				activeEffect.On();
			}
		}
	}

	public void OnEffectOff(string _name)
	{
		int count = m_ActiveEffects.Count;
		int num = count - 1;
		while (0 <= num)
		{
			ActiveEffect activeEffect = m_ActiveEffects[num];
			if (activeEffect.Name == _name)
			{
				activeEffect.Off();
			}
			num--;
		}
	}

	public void Reset()
	{
		int count = m_ActiveEffects.Count;
		for (int i = 0; count > i; i++)
		{
			ActiveEffect activeEffect = m_ActiveEffects[i];
			if (activeEffect.Active)
			{
				for (int num = activeEffect.EffectList.Count - 1; num >= 0; num--)
				{
					activeEffect.EffectList[num].DeregisterAndDestroy();
				}
			}
		}
	}

	public void RelinkBones()
	{
		m_Transforms = new List<Transform>(EffectPackageElement.s_TargetBonesNames.Length);
		Transform target = base.transform;
		for (int i = 0; i < EffectPackageElement.s_TargetBonesNames.Length; i++)
		{
			Transform item = target.FindChildRecursive(EffectPackageElement.s_TargetBonesNames[i]);
			m_Transforms.Add(item);
		}
	}

	public bool RemoveActiveEffect(EffectBase _who)
	{
		int count = m_ActiveEffects.Count;
		int num = count - 1;
		while (0 <= num)
		{
			ActiveEffect activeEffect = m_ActiveEffects[num];
			if (activeEffect.EffectList.Contains(_who))
			{
				return activeEffect.Deactivate(_who);
			}
			num--;
		}
		return false;
	}

	private void RegisterActiveEffect(string _name, EffectBase[] _effects)
	{
		int count = m_ActiveEffects.Count;
		for (int i = 0; count > i; i++)
		{
			if (!m_ActiveEffects[i].Active)
			{
				m_ActiveEffects[i].Activate(_name, _effects);
				return;
			}
		}
		m_ActiveEffects.Add(new ActiveEffect(_name, _effects));
	}

	private void SpawnIfNotYetActive(string _name)
	{
		int count = m_ActiveEffects.Count;
		int num = 0;
		for (int i = 0; count > i; i++)
		{
			ActiveEffect activeEffect = m_ActiveEffects[i];
			if (activeEffect.Name == _name)
			{
				num++;
			}
		}
		if (num != 0)
		{
			return;
		}
		GameObject[] array = Singleton<EffectManager>.Instance.SpawnEffectOnCharacter(_name, base.gameObject, EntityType);
		if (array == null)
		{
			return;
		}
		int num2 = array.Length;
		for (int j = 0; num2 > j; j++)
		{
			EffectBase[] components = array[j].GetComponents<EffectBase>();
			if (components != null)
			{
				int num3 = components.Length;
				for (int k = 0; num3 > k; k++)
				{
					components[k].ParentController = this;
				}
				RegisterActiveEffect(_name, components);
			}
			else
			{
				Debug.LogError("This object is not a valid effect prefab: " + array[j].name + " in (" + _name + ")");
			}
		}
	}
}
