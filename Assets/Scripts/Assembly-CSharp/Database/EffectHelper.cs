using UnityEngine;

namespace Database
{
	public static class EffectHelper
	{
		public static AnimationCurve m_easeCurve;

		public static AnimationCurve m_fadeCurve;

		static EffectHelper()
		{
			Keyframe[] array = new Keyframe[4];
			array[0].time = 0f;
			array[1].time = 0.1f;
			array[2].time = 0.9f;
			array[3].time = 1f;
			array[0].value = 0f;
			array[1].value = 1f;
			array[2].value = 1f;
			array[3].value = 0f;
			m_easeCurve = new AnimationCurve(array);
			array[0].time = 0f;
			array[1].time = 0.01f;
			array[2].time = 0.99f;
			array[3].time = 1f;
			array[0].value = 0f;
			array[1].value = 0f;
			array[2].value = 1f;
			array[3].value = 1f;
			m_fadeCurve = new AnimationCurve(array);
		}

		public static GameObject SpawnEffectInWorld(GameID _gameID)
		{
			return SpawnEffectInWorld(_gameID, Vector3.zero);
		}

		public static GameObject SpawnEffectInWorld(GameID _gameID, Quaternion _orientation)
		{
			return SpawnEffectInWorld(_gameID, Vector3.zero, _orientation);
		}

		public static GameObject SpawnEffectInWorld(GameID _gameID, Vector3 _position)
		{
			return SpawnEffectInWorld(_gameID, _position, Quaternion.identity);
		}

		public static GameObject SpawnEffectInWorld(GameID _gameID, Vector3 _position, Quaternion _orientation)
		{
			GameDB gameDB = Main.Instance.GameDB;
			EffectPackage item = gameDB.GetItem<EffectPackage>(_gameID);
			int count = item.Elements.Count;
			for (int i = 0; count > i; i++)
			{
				EffectPackageElement effectPackageElement = item.Elements[i];
				if (effectPackageElement.TargetCharType == EffectPackageElement.ETargetCharType.World && (!effectPackageElement.UseCondition || Singleton<EffectManager>.Instance.AreConditionsMet(effectPackageElement.ConditionMode, effectPackageElement.Conditions, null)))
				{
					GameObject gameObject = Object.Instantiate(effectPackageElement.PreFab.GetObject<GameObject>());
					if ((bool)gameObject)
					{
						gameObject.name = item.Name;
						Transform transform = gameObject.transform;
						transform.localPosition = _position;
						transform.localRotation = _orientation;
						ForwardEffectParameter(effectPackageElement, gameObject);
					}
					return gameObject;
				}
			}
			return null;
		}

		public static void ForwardEffectParameter(EffectPackageElement _epe, GameObject _prefab)
		{
			EffectParameter @object = _epe.Parameter.GetObject<EffectParameter>();
			if (!(@object != null))
			{
				return;
			}
			EffectBase[] componentsInChildren = _prefab.GetComponentsInChildren<EffectBase>();
			if (componentsInChildren != null)
			{
				int num = componentsInChildren.Length;
				for (int i = 0; i < num; i++)
				{
					EffectBase effectBase = componentsInChildren[i];
					effectBase.Parameters(@object);
				}
			}
		}
	}
}
