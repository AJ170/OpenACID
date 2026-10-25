using System;
using AcierProtocol;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Types;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Development;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.AssassinInventoryScreen
{
	public class ModifierValueHandler : MonoBehaviour
	{
		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_nameLabel;

		[SerializeField]
		[CheckAssigned]
		private UIExtendedLabel m_valueLabel;

		[SerializeField]
		[CheckAssigned]
		private UISprite m_bottomBorder;

		[CheckAssigned]
		[SerializeField]
		private Color m_positiveColor;

		[CheckAssigned]
		[SerializeField]
		private Color m_negativeColor;

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

		private void Awake()
		{
		}

		public void Init(StringId _name, float _value, float _delta, StatTypes.EStatType _statType, StatTypes.EStatUnit _unit, EModuleModifier _moduleModifierType)
		{
			Color color = ((_moduleModifierType == EModuleModifier.ClientModifier) ? Color.white : UIDataMapper.UITextColors.LegendaryColor);
			m_nameLabel.text = LocalizationManager.Instance.Get(_name);
			m_nameLabel.color = color;
			m_bottomBorder.color = color;
			string empty = string.Empty;
			string empty2 = string.Empty;
			StringId nameId = UbiCore.Utils.Singleton<UIDataMapper>.Instance.UnitDescriptions[(int)_unit].NameId;
			if (_moduleModifierType == EModuleModifier.ClientModifier)
			{
				UIRule value = new UIRule(_value, _statType, _unit);
				UIRule value2 = new UIRule(_delta, _statType, _unit, _value != 0f && _delta != _value);
				empty = LocalizationManager.Instance.Get(nameId, new StringMapper { { "value", value } });
				empty2 = LocalizationManager.Instance.Get(nameId, new StringMapper { { "value", value2 } });
			}
			else
			{
				string integer = LocalizationManager.Instance.GetInteger((int)_value);
				string integer2 = LocalizationManager.Instance.GetInteger((int)_delta);
				empty = LocalizationManager.Instance.Get(nameId, new StringMapper { { "value", integer } });
				empty2 = LocalizationManager.Instance.Get(nameId, new StringMapper { { "value", integer2 } });
			}
			if (Math.Abs(_delta) > 0f)
			{
				m_valueLabel.supportEncoding = true;
				Color color2 = ((!(_delta < 0f)) ? m_positiveColor : m_negativeColor);
				string text = ((!(_delta < 0f)) ? "+" : string.Empty);
				m_valueLabel.text = string.Format("{0}{1}{2} ({3}{4})[-]", color.ToNGUIColorString(), empty, color2.ToNGUIColorString(), text, empty2);
			}
			else
			{
				m_valueLabel.text = string.Format("{0}{1}[-]", color.ToNGUIColorString(), empty);
			}
			m_nameLabel.supportEncoding = true;
		}

		public void InitLevel(int _level, string _name, int _equipableLevel, int _currentLevel)
		{
			bool flag = false;
			m_nameLabel.text = _name;
			Color color = ((!flag) ? Color.white : UIDataMapper.UITextColors.LegendaryColor);
			UIRule value = new UIRule(_level, StatTypes.EStatType.Invalid, StatTypes.EStatUnit.Invalid);
			string arg = LocalizationManager.Instance.Get(UbiCore.Utils.Singleton<UIDataMapper>.Instance.UnitDescriptions[0].NameId, new StringMapper { { "value", value } });
			Color color2 = ((_equipableLevel <= _currentLevel) ? color : m_negativeColor);
			m_valueLabel.text = string.Format("{0}{1}[-]", color2.ToNGUIColorString(), arg);
			m_nameLabel.color = color;
			m_bottomBorder.color = color;
		}

		public void InitBaseValue(int _thisValue, string _name, int _otherValue = -1)
		{
			bool flag = false;
			m_nameLabel.text = _name;
			StatTypes.EStatUnit eStatUnit = StatTypes.EStatUnit.Invalid;
			UIRule value = new UIRule(_thisValue, StatTypes.EStatType.Invalid, eStatUnit);
			string text = LocalizationManager.Instance.Get(UbiCore.Utils.Singleton<UIDataMapper>.Instance.UnitDescriptions[(int)eStatUnit].NameId, new StringMapper { { "value", value } });
			int num = _thisValue - _otherValue;
			Color color = ((!flag) ? Color.white : UIDataMapper.UITextColors.LegendaryColor);
			string text2 = ((num >= 0) ? "+" : string.Empty);
			Color color2 = ((num >= 0) ? m_positiveColor : m_negativeColor);
			if ((float)Math.Abs(num) > 0f && _otherValue >= 0)
			{
				m_valueLabel.text = string.Format("{0}{1}{2} ({3}{4})[-]", color.ToNGUIColorString(), text, color2.ToNGUIColorString(), text2, num);
			}
			else
			{
				m_valueLabel.text = string.Format("{0}{1}[-]", color.ToNGUIColorString(), text);
			}
			m_nameLabel.color = color;
			m_bottomBorder.color = color;
		}
	}
}
