using UnityEngine;

namespace Assets.Scripts.Mission.Logic.Entities
{
	public class Haystack : HideoutObject
	{
		public override EntityTypes.EType Type
		{
			get
			{
				return EntityTypes.EType.Haystack;
			}
		}

		public Animator Animator { get; set; }

		public override bool HighlightUsesRimColor
		{
			get
			{
				return true;
			}
		}

		public void TriggerJumpInAnimation()
		{
			if (Animator != null)
			{
				Animator.SetHaystackJumpIn();
			}
		}
	}
}
