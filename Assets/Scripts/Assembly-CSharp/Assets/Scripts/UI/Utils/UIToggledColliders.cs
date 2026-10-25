using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI.Utils
{
	[RequireComponent(typeof(UIToggle))]
	public class UIToggledColliders : MonoBehaviour
	{
		public List<Collider> Activate;

		public List<Collider> Deactivate;

		private void Awake()
		{
			UIToggle component = GetComponent<UIToggle>();
			EventDelegate.Add(component.onChange, Toggle);
		}

		public void Toggle()
		{
			if (base.enabled)
			{
				for (int i = 0; i < Activate.Count; i++)
				{
					Collider collider = Activate[i];
					collider.enabled = UIToggle.current.value;
				}
				for (int j = 0; j < Deactivate.Count; j++)
				{
					Collider collider2 = Deactivate[j];
					collider2.enabled = !UIToggle.current.value;
				}
			}
		}
	}
}
