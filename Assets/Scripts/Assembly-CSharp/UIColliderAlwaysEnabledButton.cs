using UnityEngine;

[AddComponentMenu("NGUI/BB/UI/Collider Always Enabled Button")]
public class UIColliderAlwaysEnabledButton : UIButton
{
	private bool m_isEnabled;

	public override bool isEnabled
	{
		get
		{
			return base.enabled && m_isEnabled;
		}
		set
		{
			m_isEnabled = value;
			Collider component = GetComponent<Collider>();
			if (component != null)
			{
				component.enabled = true;
				SetState((!value) ? State.Disabled : State.Normal, false);
			}
		}
	}

	protected override void OnClick()
	{
		UIButton.current = this;
		EventDelegate.Execute(onClick);
		UIButton.current = null;
	}
}
