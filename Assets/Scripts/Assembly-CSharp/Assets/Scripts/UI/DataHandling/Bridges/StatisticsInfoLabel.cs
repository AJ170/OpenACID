using UnityEngine;

namespace Assets.Scripts.UI.DataHandling.Bridges
{
	public class StatisticsInfoLabel : MonoBehaviour
	{
		public UIExtendedLabel NameLabel;

		public UIExtendedLabel ValueLabel;

		private void Awake()
		{
		}

		public void Set(string _name, string _value, Color _color, bool _isBonus = false)
		{
			string format = ((!_isBonus) ? "{0}{1}[-]" : "{0}+{1}[-]");
			NameLabel.text = _name;
			ValueLabel.supportEncoding = true;
			ValueLabel.text = string.Format(format, _color.ToNGUIColorString(), _value);
		}
	}
}
