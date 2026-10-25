using AcierProtocol;
using Assets.Scripts.UI.Utils;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Behaviours
{
	public class UIRecivedResourceFromMysteryBox : UIRecivedBase
	{
		[SerializeField]
		private UISprite m_coinSprite;

		[SerializeField]
		private MysteryBoxEffects m_effects;

		protected override void OnAwake()
		{
		}

		public void PlayMysteryBoxEffects(Quality.EQuality _quality)
		{
			if (m_effects != null)
			{
				m_effects.PlayEffect(_quality);
			}
		}

		public void SetSpriteData(UISprite _sprite)
		{
			SetSpriteData(_sprite.spriteName);
		}

		public void SetSpriteData(string _spriteName)
		{
			m_coinSprite.spriteName = _spriteName;
		}

		public void SetSpriteData(EResourceType _resType, bool _useLargeIcon)
		{
			CurrencyDescription value = null;
			if (UbiCore.Utils.Singleton<UIDataMapper>.Instance.CurrencyDescriptions.TryGetValue((int)_resType, out value))
			{
				m_coinSprite.spriteName = ((!_useLargeIcon) ? value.SmallIconName : value.IconName);
				m_coinSprite.MakePixelPerfect();
			}
		}
	}
}
