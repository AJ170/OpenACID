using System;
using System.Collections.Generic;
using Database;
using SharedBaseLib.Core;
using SharedBaseLib.Serialization;
using UbiCore.Utils;
using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{
	public enum EConditionMode
	{
		AllTrue = 0,
		AllFalse = 1,
		AnyTrue = 2,
		AnyFalse = 3
	}

	public class Condition : DataSerializableBase
	{
		public ECondition What;

		public bool Negate;

		protected override void SerializeInternal(DataSerializerBase _s, object _customData = null)
		{
			What = (ECondition)_s.SerializeEnum("What", What.GetType(), (int)What);
			Negate = _s.SerializeBool("Negate", Negate);
		}
	}

	public enum ECondition
	{
		Never = 0,
		ExitRooftopGarden = 1,
		ExitHaystack = 2,
		ExitBench = 3,
		FanOfKnives_Level5 = 4,
		ReducedGore = 5,
		Always = 6
	}

	private BBList<GameID> m_EffectPackages = new BBList<GameID>();

	private BBDictionary<string, BBList<EffectPackageElement>> m_MyDic = new BBDictionary<string, BBList<EffectPackageElement>>();

	private BBHashSet<GameObject> m_PreloadPrefabCache;

	private BBHashSet<EffectParameter> m_PreloadParameterCache;

	private HashSet<EffectBase> m_ActiveEffects;

	private HashSet<int> m_Conditions;

	public EffectManager()
	{
		m_Conditions = new HashSet<int>();
		m_ActiveEffects = new HashSet<EffectBase>();
	}

	public void Init()
	{
	}

	public bool AreConditionsMet(EConditionMode _mode, List<Condition> _conditions, BBList<int> _characterConditions)
	{
		int num = 0;
		int count = _conditions.Count;
		for (int i = 0; count > i; i++)
		{
			Condition condition = _conditions[i];
			bool flag = m_Conditions.Contains((int)condition.What);
			if (_characterConditions != null)
			{
				foreach (int _characterCondition in _characterConditions)
				{
					flag |= _characterCondition == (int)condition.What;
				}
			}
			num += (((!condition.Negate) ? flag : (!flag)) ? 1 : 0);
		}
		switch (_mode)
		{
		case EConditionMode.AllFalse:
			return num == 0;
		case EConditionMode.AllTrue:
			return num == count;
		case EConditionMode.AnyFalse:
			return num < count;
		case EConditionMode.AnyTrue:
			return num > 0;
		default:
			return false;
		}
	}

	public void ActivateCondition(ECondition _condition)
	{
		if (!m_Conditions.Contains((int)_condition))
		{
			m_Conditions.Add((int)_condition);
		}
	}

	public void RemoveCondition(ECondition _condition)
	{
		if (m_Conditions.Contains((int)_condition))
		{
			m_Conditions.Remove((int)_condition);
		}
	}

	public bool HasCondition(ECondition _condition)
	{
		if (m_Conditions.Contains((int)_condition))
		{
			return true;
		}
		return false;
	}

	public void ClearConditions()
	{
		m_Conditions.Clear();
	}

	public void setupEffectPackages()
	{
		GameDB gameDB = Main.Instance.GameDB;
		m_PreloadPrefabCache = new BBHashSet<GameObject>();
		m_PreloadParameterCache = new BBHashSet<EffectParameter>();
		m_MyDic.Clear();
		int count = m_EffectPackages.Count;
		for (int i = 0; count > i; i++)
		{
			EffectPackage item = gameDB.GetItem<EffectPackage>(m_EffectPackages[i]);
			if (item != null)
			{
				int count2 = item.Elements.Count;
				for (int j = 0; count2 > j; j++)
				{
					EffectPackageElement effectPackageElement = item.Elements[j];
					BBList<EffectPackageElement> value;
					if (m_MyDic.TryGetValue(effectPackageElement.Marker, out value))
					{
						value.Add(effectPackageElement);
						continue;
					}
					value = new BBList<EffectPackageElement>();
					value.Add(effectPackageElement);
					m_MyDic.Add(effectPackageElement.Marker, value);
				}
			}
			else
			{
				Debug.LogWarning("EffectManager cannot find effect GUID: " + m_EffectPackages[i]);
			}
		}
		foreach (BBList<EffectPackageElement> value2 in m_MyDic.Values)
		{
			foreach (EffectPackageElement item2 in value2)
			{
				m_PreloadPrefabCache.Add(item2.PreFab.GetObject<GameObject>());
				m_PreloadParameterCache.Add(item2.Parameter.GetObject<EffectParameter>());
			}
		}
	}

	public GameObject[] SpawnEffectOnCharacter(string _effectMarker, GameObject _base, HumanoidDef.ECharType _type)
	{
		Transform transform = _base.transform;
		EffectController componentInChildren = _base.GetComponentInChildren<EffectController>();
		BBList<GameObject> bBList = new BBList<GameObject>();
		BBList<EffectPackageElement> value;
		if (m_MyDic.TryGetValue(_effectMarker, out value))
		{
			int count = value.Count;
			for (int i = 0; count > i; i++)
			{
				EffectPackageElement effectPackageElement = value[i];
				if (effectPackageElement.TargetCharType != (EffectPackageElement.ETargetCharType)_type)
				{
					continue;
				}
				Transform transform2 = null;
				if (effectPackageElement.TargetBone == EffectPackageElement.ETargetBone.Root)
				{
					transform2 = transform;
				}
				else
				{
					string name = Enum.GetName(typeof(EffectPackageElement.ETargetBone), effectPackageElement.TargetBone);
					transform2 = transform.FindChildRecursive(name);
				}
				if (transform2 != null && effectPackageElement.PreFab != null && (!effectPackageElement.UseCondition || AreConditionsMet(effectPackageElement.ConditionMode, effectPackageElement.Conditions, (!(componentInChildren != null)) ? null : componentInChildren.Conditions)))
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(effectPackageElement.PreFab.GetObject<GameObject>());
					if (gameObject != null)
					{
						gameObject.name = gameObject.name.Replace("(Clone)", string.Empty);
						Transform transform3 = gameObject.transform;
						transform3.parent = transform2;
						transform3.localPosition = Vector3.zero;
						transform3.localRotation = Quaternion.identity;
						transform3.localScale = Vector3.one;
						EffectHelper.ForwardEffectParameter(effectPackageElement, gameObject);
						bBList.Add(gameObject);
					}
				}
			}
		}
		return bBList.ToArray();
	}

	public void BeginUsingEffects()
	{
		m_EffectPackages.Clear();
		m_EffectPackages.Add(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_invalidEffectPackage);
	}

	public void PrepareEffects(GameDB _gdb, GameID[] _availableSkills)
	{
		foreach (GameID gameID in _availableSkills)
		{
			if (gameID.Guid != 0)
			{
				m_EffectPackages.Add(gameID);
			}
		}
	}

	public void DestroyEffects()
	{
		m_EffectPackages.Clear();
	}

	public void RegisterEffect(EffectBase _fx)
	{
		m_ActiveEffects.Add(_fx);
	}

	public void DeregisterEffect(EffectBase _fx)
	{
		m_ActiveEffects.Remove(_fx);
	}

	public void DestroyActiveEffects()
	{
		foreach (EffectBase activeEffect in m_ActiveEffects)
		{
			activeEffect.DeregisterAndDestroy();
		}
		m_ActiveEffects.Clear();
	}
}
