using System.Collections;
using Assets.Scripts.Database;
using Assets.Scripts.UI.EnvironmentBridge;
using Assets.Scripts.UI.Loading;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.NGUIExtensions;
using Assets.Scripts.UI.NGUIExtensions.Tweens;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using Database;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.MissionScreen.EarthPins
{
	public class EarthPinBase : MonoBehaviour
	{
		private const float c_tweenOutDuration = 0.1f;

		private const float c_tweenInDuration = 0.3f;

		[SerializeField]
		private BBTweenAlpha m_alphaTween;

		[SerializeField]
		private UIExtendedLabel m_NameLabel;

		[SerializeField]
		private UITexture m_EarthPinImage;

		[SerializeField]
		private GameObject m_TextPlateContainer;

		[SerializeField]
		private UIAlignmentBox m_AlignmentBox;

		private RegionDefinition m_regionDefinition;

		private RegionDefinition.AppearanceOption m_appearanceOption;

		private bool m_isSelected;

		public bool PlayTween = true;

		public bool IsSelected
		{
			get
			{
				return m_isSelected;
			}
			set
			{
				if (m_isSelected != value)
				{
					if (value)
					{
						if (!m_isSelected && PlayTween)
						{
							if (m_alphaTween != null)
							{
								m_alphaTween.duration = 0.3f;
								m_alphaTween.StartTweenPlayForward();
							}
						}
						else if (m_alphaTween != null)
						{
							m_alphaTween.SetTweenToEnd();
						}
					}
					else if (m_isSelected && PlayTween)
					{
						if (m_alphaTween != null)
						{
							m_alphaTween.duration = 0.1f;
							m_alphaTween.StartTweenPlayReverse();
						}
					}
					else if (m_alphaTween != null)
					{
						m_alphaTween.SetTweenToBeginning();
					}
				}
				m_isSelected = value;
				m_NameLabel.gameObject.SetActive(m_isSelected);
				m_TextPlateContainer.SetActive(m_isSelected);
				Update();
			}
		}

		private void Awake()
		{
			m_isSelected = false;
			if (m_alphaTween != null)
			{
				m_alphaTween.SetTweenToBeginning();
			}
			m_NameLabel.gameObject.SetActive(false);
			m_TextPlateContainer.SetActive(false);
		}

		private void OnEnable()
		{
			EventDelegate.Add(m_AlignmentBox.OnReposition, OnRepositioned);
		}

		private void OnDisable()
		{
			if (m_NameLabel != null && m_NameLabel.gameObject != null)
			{
				m_NameLabel.text = string.Empty;
				m_NameLabel.gameObject.SetActive(false);
			}
			if (m_TextPlateContainer != null)
			{
				m_TextPlateContainer.SetActive(false);
			}
			m_regionDefinition = null;
			m_appearanceOption = null;
			m_isSelected = false;
			if (m_alphaTween != null)
			{
				m_alphaTween.SetTweenToBeginning();
			}
			if (m_AlignmentBox != null)
			{
				EventDelegate.Remove(m_AlignmentBox.OnReposition, OnRepositioned);
			}
		}

		public void SetTargetRegion(RegionDefinition _region, string _optionalAppearanceName)
		{
			m_regionDefinition = _region;
			m_appearanceOption = _region.GetRegionAppearanceOption(_optionalAppearanceName);
			base.transform.localScale = Vector3.one;
			LocaKey regionEarthPinLoca = _region.GetRegionEarthPinLoca(m_appearanceOption);
			m_NameLabel.text = LocalizationManager.Instance.Get(regionEarthPinLoca.Key);
			StartCoroutine(UpdateTexture());
			Update();
		}

		public void Update()
		{
			if (m_regionDefinition != null)
			{
				base.transform.position = BehaviourSingleton<EnvironmentBridgeMain>.Instance.EarthGlobeData.GetCityPinPosition(m_regionDefinition.Age.GetEarthPinByEra(), m_regionDefinition.CityIdentifier);
			}
		}

		private IEnumerator UpdateTexture()
		{
			UnityResource resource = m_regionDefinition.GetRegionEarthPinImg(m_appearanceOption);
			if (m_EarthPinImage.mainTexture != null && resource.GetResourcePath().Contains(m_EarthPinImage.mainTexture.name))
			{
				m_AlignmentBox.RepositionNow = true;
				yield break;
			}
			yield return new WaitForEndOfFrame();
			if (m_EarthPinImage != null)
			{
				m_EarthPinImage.mainTexture = null;
			}
			LoadAnimationManager loadManager = Singleton<UIScreenManager>.Instance.SceneData.LoadAnimationManager;
			yield return StartCoroutine(loadManager.BeginLoadUnityResource(resource, new LoadAnimationManager.SLoadObjectInfo(m_EarthPinImage)));
			UnityResource image = m_regionDefinition.GetRegionEarthPinImg(m_appearanceOption);
			if (resource == image)
			{
				m_EarthPinImage.mainTexture = image.AccessObject<Texture>();
			}
			m_AlignmentBox.RepositionNow = true;
		}

		private void OnRepositioned()
		{
			if (PlayTween)
			{
				m_alphaTween.StartTweenPlayForward();
			}
			else
			{
				m_alphaTween.SetTweenToEnd();
			}
			PlayTween = true;
		}
	}
}
