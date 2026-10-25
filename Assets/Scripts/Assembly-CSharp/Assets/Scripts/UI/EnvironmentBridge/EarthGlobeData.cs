using System;
using System.Collections.Generic;
using Assets.Scripts.UI.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI.EnvironmentBridge
{
	public class EarthGlobeData
	{
		public class EarthPinLocation
		{
			public Vector3 LookAt { get; private set; }

			public GameObject EarthPin { get; private set; }

			public Dictionary<int, GameObject> CityPins { get; private set; }

			public EarthPinLocation(Vector3 _lookAt, GameObject _earthPin, Dictionary<int, GameObject> _cityPins)
			{
				LookAt = _lookAt;
				EarthPin = _earthPin;
				CityPins = _cityPins;
			}
		}

		public readonly Dictionary<int, EarthPinLocation> EarthPins = new Dictionary<int, EarthPinLocation>();

		public Camera GameCamera { get; private set; }

		public Camera UiMainCamera { get; private set; }

		public GameObject EarthParent { get; private set; }

		public GameObject EarthPinsContainer { get; private set; }

		public GameObject EarthLight { get; private set; }

		public Transform EarthGlobe { get; private set; }

		public Transform EarthLookAt { get; private set; }

		public Transform EarthCamSpace { get; private set; }

		public Transform EarthOuterAim { get; private set; }

		public Transform EarthLiftRotate { get; private set; }

		public bool Initialized { get; private set; }

		public bool IsGlobeActive
		{
			get
			{
				return EarthParent != null && EarthParent.activeSelf;
			}
			set
			{
				if (EarthParent != null)
				{
					EarthParent.SetActive(value);
				}
			}
		}

		public bool IsLightActive
		{
			get
			{
				return EarthLight != null && EarthLight.gameObject.activeSelf;
			}
			set
			{
				if (EarthLight != null)
				{
					EarthLight.gameObject.SetActive(value);
				}
			}
		}

		public EarthPinLocation GetPinLocation(EraExtensions.EEarthPin _type)
		{
			EarthPinLocation value = null;
			if (EarthPins.TryGetValue((int)_type, out value))
			{
				return value;
			}
			return null;
		}

		public Vector3 GetCityPinPosition(EraExtensions.EEarthPin _type, CityExtensions.CityIdentifier _cityIdentifier)
		{
			EarthPinLocation pinLocation = GetPinLocation(_type);
			if (pinLocation != null)
			{
				GameObject value = null;
				if (pinLocation.CityPins.TryGetValue((int)_cityIdentifier, out value))
				{
					Vector3 position = GameCamera.WorldToViewportPoint(value.transform.position);
					position = UiMainCamera.ViewportToWorldPoint(position);
					return new Vector3(position.x, position.y, 0f);
				}
			}
			return Vector3.zero;
		}

		public void Init(GameObject _parent, Camera _gameCamera, Camera _uiMainCamera)
		{
			if (!Initialized)
			{
				GameCamera = _gameCamera;
				UiMainCamera = _uiMainCamera;
				CollectEarthPins();
				Initialized = true;
			}
		}

		public bool InitEarthGlobeObjects()
		{
			GameObject _target;
			if (!EnvironmentBridgeMain.TryFind("EnvScenario/Menu Objects/Object Earth", out _target))
			{
				return false;
			}
			GameObject _target2;
			if (!EnvironmentBridgeMain.TryFind("EnvScenario/Directional light", out _target2))
			{
				return false;
			}
			GameObject _target3;
			if (!EnvironmentBridgeMain.TryFind("EnvScenario/Menu Objects/Object Earth/skROOT_Globe", out _target3))
			{
				return false;
			}
			GameObject _target4;
			if (!EnvironmentBridgeMain.TryFind("EnvScenario/Menu Objects/Object Earth/skROOT_Globe/EarthPins", out _target4))
			{
				return false;
			}
			GameObject _target5;
			if (!EnvironmentBridgeMain.TryFind("EnvScenario/Menu Objects/Object Earth/CamSpace/LiftRotate/LookAt", out _target5))
			{
				return false;
			}
			GameObject _target6;
			if (!EnvironmentBridgeMain.TryFind("EnvScenario/Menu Objects/Object Earth/CamSpace", out _target6))
			{
				return false;
			}
			GameObject _target7;
			if (!EnvironmentBridgeMain.TryFind("EnvScenario/Menu Objects/Object Earth/OuterAim", out _target7))
			{
				return false;
			}
			GameObject _target8;
			if (!EnvironmentBridgeMain.TryFind("EnvScenario/Menu Objects/Object Earth/CamSpace/LiftRotate", out _target8))
			{
				return false;
			}
			EarthParent = _target;
			EarthLight = _target2;
			EarthPinsContainer = _target4;
			EarthGlobe = _target3.transform;
			EarthLookAt = _target5.transform;
			EarthCamSpace = _target6.transform;
			EarthOuterAim = _target7.transform;
			EarthLiftRotate = _target8.transform;
			EarthParent.transform.rotation = Quaternion.identity;
			return true;
		}

		public void Clear()
		{
			GameCamera = null;
			UiMainCamera = null;
			EarthParent = null;
			EarthPinsContainer = null;
			EarthLight = null;
			EarthGlobe = null;
			EarthLookAt = null;
			EarthCamSpace = null;
			EarthOuterAim = null;
			EarthLiftRotate = null;
			EarthPins.Clear();
		}

		private void CollectEarthPins()
		{
			Transform transform = EarthPinsContainer.transform;
			for (int i = 0; i < transform.childCount; i++)
			{
				Transform child = transform.GetChild(i);
				string value = child.name.Substring(9);
				EraExtensions.EEarthPin key = (EraExtensions.EEarthPin)(int)Enum.Parse(typeof(EraExtensions.EEarthPin), value, true);
				Dictionary<int, GameObject> dictionary = new Dictionary<int, GameObject>(child.childCount);
				for (int j = 0; j < child.childCount; j++)
				{
					Transform child2 = child.GetChild(j);
					CityExtensions.CityIdentifier key2 = (CityExtensions.CityIdentifier)(int)Enum.Parse(typeof(CityExtensions.CityIdentifier), child2.name, true);
					dictionary.Add((int)key2, child2.gameObject);
				}
				Vector2 vector = PrecalculateEarthTarget(child.gameObject);
				EarthPins.Add((int)key, new EarthPinLocation(vector, child.gameObject, dictionary));
			}
		}

		private Vector2 PrecalculateEarthTarget(GameObject go)
		{
			Vector3 vector = go.transform.position - EarthGlobe.transform.position;
			float magnitude = vector.magnitude;
			float y = vector.y;
			float num = y * y / magnitude;
			float num2 = magnitude - num;
			float num3 = Mathf.Sqrt(num2 * num);
			float num4 = Mathf.Atan(num3 / num2);
			float num5 = Mathf.Atan2(vector.x, vector.z) * 57.29578f;
			return new Vector2(num4 * 57.29578f * -1f, num5 * -1f);
		}
	}
}
