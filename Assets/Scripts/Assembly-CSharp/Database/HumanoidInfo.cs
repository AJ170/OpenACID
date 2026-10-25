using AcierProtocol;
using AssetBundleData;
using Assets.Scripts.Database;
using Humanoids;
using UbiCore.Utils;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Database
{
	public class HumanoidInfo
	{
		public GameID GamePrefabGameID;

		public GameObject GetPrefab(GameID _overridePrefab = null)
		{
			GameObject _go = null;
			GameID gameID = _overridePrefab ?? GamePrefabGameID;
			if (!HumanoidFactoryHelpers.TryGetCachedGo(gameID, out _go))
			{
				AnimatorDef item = Main.SafeGameDb.GetItem<AnimatorDef>(gameID);
				_go = item.AnimatorPrefab.GetObject<GameObject>();
				HumanoidFactoryHelpers.AddGoToCache(gameID, _go);
			}
			if (_go != null)
			{
				GameObject gameObject = Object.Instantiate(_go);
				if (gameObject != null)
				{
					gameObject.name = _go.name;
				}
				return gameObject;
			}
			return null;
		}

		public void ClearPreloads()
		{
		}

		public void PreloadPrefab()
		{
			AnimatorDef item = Main.SafeGameDb.GetItem<AnimatorDef>(GamePrefabGameID);
			GameObject @object = item.AnimatorPrefab.GetObject<GameObject>();
			HumanoidFactoryHelpers.AddGoToCache(GamePrefabGameID, @object);
			UbiCore.Utils.Singleton<AnimationMain>.Instance.GetAnimatorStateMachineStatic(@object.GetComponent<Animator>());
			ObjectFade objectFade = @object.AddComponentIfNotExist<ObjectFade>();
			objectFade.FadeIn();
		}

		public void Preload(AssetBundleInfo _info, HumanoidDef.EAtlasPage _atlasPage, EEra _age)
		{
			PreloadPrefab();
		}
	}
}
