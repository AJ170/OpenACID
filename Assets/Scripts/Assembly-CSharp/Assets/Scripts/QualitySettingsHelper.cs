using System;
using System.Text;
using System.Text.RegularExpressions;
using AcierProtocol;
using Assets.Scripts.Database;
using Assets.Scripts.Tracking;
using Assets.Scripts.UI.Utils;
using SharedBaseLib.Core;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts
{
	public static class QualitySettingsHelper
	{
		public enum DeviceProfile
		{
			Low = 0,
			LowMid = 1,
			HighMid = 2,
			High = 3,
			PC = 4,
			PSVita = 5
		}

		// Index of the "PSVita" level in ProjectSettings/QualitySettings.asset.
		public const int PSVitaQualityLevel = 6;

		public static string OriginalDeviceIdentifier;

		private static string s_overriddenDeviceIdentifier;

		public static BBDictionary<int, AcierProtocol.QualitySettings> FallbackQualityProfiles;

		public static DeviceProfile CurrentDeviceProfile
		{
			get
			{
				if (Application.platform == RuntimePlatform.PSP2)
				{
					return DeviceProfile.PSVita;
				}
				string graphicsDeviceName = SystemInfo.graphicsDeviceName;
				DeviceProfile deviceProfile = DeviceProfile.Low;
				if (graphicsDeviceName.Contains("Adreno"))
				{
					if (graphicsDeviceName.Contains("320"))
					{
						deviceProfile = DeviceProfile.LowMid;
					}
					else if (graphicsDeviceName.Contains("330"))
					{
						deviceProfile = DeviceProfile.HighMid;
					}
					else if (Convert.ToInt32(Regex.Match(graphicsDeviceName, "\\d+").Value) >= 420)
					{
						deviceProfile = DeviceProfile.High;
					}
				}
				else if (graphicsDeviceName.Contains("Mali"))
				{
					if (graphicsDeviceName.Contains("T628"))
					{
						deviceProfile = DeviceProfile.High;
					}
					else if (graphicsDeviceName.Contains("400"))
					{
						deviceProfile = DeviceProfile.Low;
					}
				}
				else if (graphicsDeviceName.Contains("PowerVR") && graphicsDeviceName.Contains("SGX") && graphicsDeviceName.Contains("540"))
				{
					deviceProfile = DeviceProfile.Low;
				}
				if (deviceProfile == DeviceProfile.Low && UIUtil.GetScreenInches() > UIUtil.MAX_PHONE_INCHES)
				{
					deviceProfile = DeviceProfile.LowMid;
				}
				return deviceProfile;
			}
		}

		static QualitySettingsHelper()
		{
			OriginalDeviceIdentifier = string.Empty;
			s_overriddenDeviceIdentifier = string.Empty;
			FallbackQualityProfiles = new BBDictionary<int, AcierProtocol.QualitySettings>();
			FallbackQualityProfiles.Add(0, new AcierProtocol.QualitySettings
			{
				assetBundleQuality = EQualityAssetBundle.Low,
				renderingLOD = EQualityShaderLOD.Diffuse_200,
				qualityLevel = 2,
				crowdQuality = 0.2f,
				virtualStickScale = 0f,
				downScaling = true,
				textureType = EQualityTextureType.Generic
			});
			FallbackQualityProfiles.Add(1, new AcierProtocol.QualitySettings
			{
				assetBundleQuality = EQualityAssetBundle.Low,
				renderingLOD = EQualityShaderLOD.Diffuse_200,
				qualityLevel = 2,
				crowdQuality = 0.2f,
				virtualStickScale = 0f,
				downScaling = false,
				textureType = EQualityTextureType.Generic
			});
			FallbackQualityProfiles.Add(2, new AcierProtocol.QualitySettings
			{
				assetBundleQuality = EQualityAssetBundle.Low,
				renderingLOD = EQualityShaderLOD.Bumped_300,
				qualityLevel = 2,
				crowdQuality = 0.2f,
				virtualStickScale = 0f,
				downScaling = false,
				textureType = EQualityTextureType.Generic
			});
			FallbackQualityProfiles.Add(3, new AcierProtocol.QualitySettings
			{
				assetBundleQuality = EQualityAssetBundle.High,
				renderingLOD = EQualityShaderLOD.Bumped_300,
				qualityLevel = 3,
				crowdQuality = 0.2f,
				virtualStickScale = 0f,
				downScaling = false,
				textureType = EQualityTextureType.Generic
			});
			FallbackQualityProfiles.Add(4, new AcierProtocol.QualitySettings
			{
				assetBundleQuality = EQualityAssetBundle.High,
				renderingLOD = EQualityShaderLOD.Maximum_500,
				qualityLevel = 5,
				crowdQuality = 0.65f,
				virtualStickScale = 0f,
				downScaling = false,
				textureType = EQualityTextureType.Generic
			});
			// PS Vita: low quality bundles (unbumped _LQ shaders), native 960x544 instead of the
			// half resolution "downScaling" path, vsync locked quality level.
			FallbackQualityProfiles.Add((int)DeviceProfile.PSVita, new AcierProtocol.QualitySettings
			{
				assetBundleQuality = EQualityAssetBundle.Low,
				renderingLOD = EQualityShaderLOD.Diffuse_200,
				qualityLevel = PSVitaQualityLevel,
				crowdQuality = 0.2f,
				virtualStickScale = 0f,
				downScaling = false,
				textureType = EQualityTextureType.Generic
			});
		}

		public static string GetDeviceIdentifier()
		{
			string text = string.Empty;
			string text2 = "_IL2CPP";
			if (string.IsNullOrEmpty(text))
			{
				text = SystemInfo.deviceModel + "_" + SystemInfo.graphicsDeviceName + text2;
			}
			if (UbiCore.Utils.Singleton<LocalStorage>.IsAvailable)
			{
				string qualityDeviceProfileName = UbiCore.Utils.Singleton<LocalStorage>.Instance.QualityDeviceProfileName;
				if (!string.IsNullOrEmpty(qualityDeviceProfileName))
				{
					OriginalDeviceIdentifier = text;
					s_overriddenDeviceIdentifier = UbiCore.Utils.Singleton<LocalStorage>.Instance.QualityDeviceProfileName;
					UbiCore.Utils.Singleton<LocalStorage>.Instance.QualityDeviceProfileName = string.Empty;
				}
			}
			return (!string.IsNullOrEmpty(s_overriddenDeviceIdentifier)) ? s_overriddenDeviceIdentifier : text;
		}

		public static string ToString(this AcierProtocol.QualitySettings _qualitySettings, string _devideId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("QualityDefinition \n");
			stringBuilder.AppendFormat("Quality Name: {0} \n", DeviceManager.QualitySettingsName);
			stringBuilder.AppendFormat("DeviceID: {0} \n", _devideId);
			stringBuilder.AppendFormat("AssetBundleQuality: {0} \n", _qualitySettings.assetBundleQuality);
			stringBuilder.AppendFormat("RenderingLOD: {0} \n", _qualitySettings.renderingLOD);
			stringBuilder.AppendFormat("CrowdQuality: {0:0.00} \n", _qualitySettings.crowdQuality);
			stringBuilder.AppendFormat("VirtualStickScale: {0:0.00} \n", _qualitySettings.virtualStickScale);
			stringBuilder.AppendFormat("DownScaling: {0} \n", _qualitySettings.downScaling);
			stringBuilder.AppendFormat("TextureType: {0} \n", _qualitySettings.textureType);
			stringBuilder.AppendFormat("EveryplayEnabled: {0} \n", _qualitySettings.everyPlayEnabled);
			return stringBuilder.ToString();
		}
	}
}
