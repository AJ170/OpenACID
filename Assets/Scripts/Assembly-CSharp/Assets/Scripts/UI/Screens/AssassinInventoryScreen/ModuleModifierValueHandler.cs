using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Utils;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.AssassinInventoryScreen
{
	public class ModuleModifierValueHandler : MonoBehaviour
	{
		[SerializeField]
		private UIExtendedLabel m_nameLabel;

		[SerializeField]
		private UIExtendedLabel m_valueLabel;

		public bool Enable
		{
			get
			{
				return base.gameObject.activeSelf;
			}
			set
			{
				base.gameObject.SetActive(value);
			}
		}

		public void Init(string _name, int _value)
		{
			m_nameLabel.text = _name;
			string arg = LocalizationManager.Instance.Get(5385) + LocalizationManager.Instance.Get(UbiCore.Utils.Singleton<UIDataMapper>.Instance.UnitDescriptions[6].NameId, new StringMapper { { "value", _value } });
			Color white = Color.white;
			m_valueLabel.text = string.Format("{0}{1}", white.ToNGUIColorString(), arg);
		}

		private void Awake()
		{
		}
	}
}
