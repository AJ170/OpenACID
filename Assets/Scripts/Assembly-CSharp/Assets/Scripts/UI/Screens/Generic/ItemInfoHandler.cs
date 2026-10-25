using System.Globalization;
using Assets.Scripts.UI.Localization;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.Generic
{
	public class ItemInfoHandler : MonoBehaviour
	{
		public delegate void SenderDelegate(ItemInfoHandler _sender);

		public static bool s_ShowRollingCode;

		[SerializeField]
		private UIExtendedLabel m_titleLabel;

		[SerializeField]
		private UIExtendedLabel m_valueLabel;

		[SerializeField]
		private UIExtendedLabel m_unitLabel;

		[SerializeField]
		private UIWidget m_levelWidget;

		[SerializeField]
		private UIWidget m_valueWidget;

		[SerializeField]
		private BoxCollider m_collider;

		public event SenderDelegate Click;

		private void InvokeClick()
		{
			SenderDelegate click = this.Click;
			if (click != null)
			{
				click(this);
			}
		}

		private void Awake()
		{
			Init();
		}

		protected virtual void Init()
		{
		}

		private void OnClick()
		{
			InvokeClick();
		}

		protected void Init(bool _isSlotFilled, int _itemName, string _rollingCode, string _unit, int _baseValue)
		{
			if (_isSlotFilled)
			{
				m_levelWidget.gameObject.SetActive(true);
				m_valueWidget.gameObject.SetActive(true);
				m_collider.enabled = true;
				m_titleLabel.color = Color.white;
				m_titleLabel.text = ((!s_ShowRollingCode) ? LocalizationManager.Instance.Get(_itemName) : _rollingCode);
				m_unitLabel.text = _unit;
				m_valueLabel.text = _baseValue.ToString(CultureInfo.InvariantCulture);
				m_valueWidget.gameObject.SetActive(_baseValue != 0);
			}
			else
			{
				m_titleLabel.text = LocalizationManager.Instance.Get(1827);
				m_titleLabel.color = Color.grey;
				m_levelWidget.gameObject.SetActive(false);
				m_valueWidget.gameObject.SetActive(false);
				m_collider.enabled = false;
			}
		}
	}
}
