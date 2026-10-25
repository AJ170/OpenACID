using UnityEngine;

namespace Assets.Scripts.UI.Animations.ValueObservers
{
	public class UIDurationObserver : IValueObserver
	{
		[SerializeField]
		[HideInInspector]
		private float m_duration;

		protected override float Value
		{
			get
			{
				return m_duration;
			}
		}
	}
}
