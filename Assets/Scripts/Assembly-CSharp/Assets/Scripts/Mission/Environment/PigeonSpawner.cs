using Database;
using SharedBaseLib.Core;
using UbiCore.Utils;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.Mission.Environment
{
	public static class PigeonSpawner
	{
		private static BBList<GameObject> m_vSpawnedPigeons = new BBList<GameObject>();

		private static void CollectLocators(Transform _root, BBList<Transform> _data)
		{
			Transform transform = _root.transform;
			int childCount = transform.childCount;
			for (int i = 0; childCount > i; i++)
			{
				Transform child = transform.GetChild(i);
				if (child.name.Contains("Pigeon"))
				{
					_data.Add(child);
				}
				else if (child.childCount > 0)
				{
					CollectLocators(child, _data);
				}
			}
		}

		public static void OnMissionLoaded()
		{
			for (int i = 0; i < m_vSpawnedPigeons.Count; i++)
			{
				Object.Destroy(m_vSpawnedPigeons[i].gameObject);
			}
			m_vSpawnedPigeons.Clear();
			ItemDef item = Main.Instance.GameDB.GetItem<ItemDef>(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_envInPigeonTrigger);
			Object @object = item.ItemPrefab.GetObject<Object>();
			BBList<Transform> bBList = new BBList<Transform>();
			GameObject gameObject = GameObject.Find("EnvScenario/MoodScenario/Birds");
			if (gameObject != null)
			{
				CollectLocators(gameObject.transform, bBList);
			}
			for (int j = 0; j < bBList.Count; j++)
			{
				Transform parent = bBList[j];
				GameObject gameObject2 = Object.Instantiate(@object) as GameObject;
				m_vSpawnedPigeons.Add(gameObject2);
				if (gameObject2 != null)
				{
					ItemHelper.SetupLOD(gameObject2, 0.05f);
					GameObject go = gameObject2.FindChildByName("animal_pigeon");
					Rendering.ConfigureLightProbes(go);
					Transform transform = gameObject2.transform;
					transform.parent = parent;
					transform.localPosition = Vector4.zero;
					transform.localRotation = Quaternion.identity;
					transform.localScale = Vector4.one;
				}
			}
		}
	}
}
