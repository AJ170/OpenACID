using UnityEngine;

namespace Assets.Scripts.UI.Localization
{
	public abstract class UILocalization : MonoBehaviour
	{
		[SerializeField]
		private int m_key = -1;

		public int Key
		{
			get
			{
				return m_key;
			}
			set
			{
				m_key = value;
				Localize(m_key);
			}
		}

		private void Start()
		{
			EventDelegate.Add(LocalizationManager.onLanguageChange, OnLanguageChange);
			Localize(m_key);
		}

		private void OnDestroy()
		{
			EventDelegate.Remove(LocalizationManager.onLanguageChange, OnLanguageChange);
		}

		private void OnLanguageChange()
		{
			Localize(m_key);
		}

		private void Localize(StringId _key)
		{
			if (_key.Valid)
			{
				DoLocalize(m_key);
			}
		}

		protected abstract void DoLocalize(StringId _key);
	}
}
