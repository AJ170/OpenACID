using Assets.Scripts.Mission.Logic;
using UnityEngine;

namespace Assets.Scripts.Mission.UI.Combat
{
	public abstract class ICombatAnnotation : MonoBehaviour
	{
		public delegate void Handler(ICombatAnnotation _object);

		public abstract IGameEntity Target { get; }

		public abstract float Height { get; }

		public abstract float StackOffset { get; set; }

		public event Handler OnRelease;

		protected void TriggerOnRelease()
		{
			if (this.OnRelease != null)
			{
				this.OnRelease(this);
			}
		}

		public abstract void Release();
	}
}
