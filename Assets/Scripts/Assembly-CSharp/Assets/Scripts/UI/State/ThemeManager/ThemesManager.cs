using System;
using Assets.Scripts.Audio;
using Assets.Scripts.UI.Interfaces;
using Assets.Scripts.UI.Utils;
using SharedBaseLib.Core;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.State.ThemeManager
{
	public class ThemesManager : IValidated
	{
		public enum EFlavorTheme
		{
			Standard = 0,
			Pirate = 1,
			ACM = 2
		}

		public SimpleDelegate OnThemeChanged;

		private EFlavorTheme m_activeTheme;

		public bool IsThemeEnabled
		{
			get
			{
				return m_activeTheme != EFlavorTheme.Standard;
			}
		}

		public EFlavorTheme ActiveTheme
		{
			get
			{
				return m_activeTheme;
			}
			set
			{
				m_activeTheme = value;
				OnThemeChanged.Trigger();
				UpdateAudioTheme();
				OnThemeChanged.Trigger();
			}
		}

		public bool Valid { get; private set; }

		public ThemesManager()
		{
			OnThemeChanged = new SimpleDelegate(this, new Type[0]);
		}

		public int GetLocaReplacementThemeKey(int _key)
		{
			if (!Application.isPlaying)
			{
				return _key;
			}
			int result = _key;
			ThemeLocaDescription value;
			if (UbiCore.Utils.Singleton<UIDataMapper>.IsAvailable && UbiCore.Utils.Singleton<UIDataMapper>.Instance.ThemeLocaDescriptions.TryGetValue((int)ActiveTheme, out value))
			{
				int value2 = 0;
				if (value.ThemeLocaReplacement.TryGetValue(_key, out value2))
				{
					result = value2;
				}
			}
			return result;
		}

		public string GetThemeIconReplacement(string _iconName)
		{
			ThemeIconsDescription value;
			if (IsThemeEnabled && UbiCore.Utils.Singleton<UIDataMapper>.Instance.ThemeIconsDescriptions.TryGetValue(_iconName, out value))
			{
				return value.PirateIconSpriteName;
			}
			return _iconName;
		}

		private void UpdateAudioTheme()
		{
			AudioManager.ActiveSoundTheme = (IsThemeEnabled ? ESoundTheme.Pirates : ESoundTheme.Standard);
		}
	}
}
