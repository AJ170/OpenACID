using UnityEngine;

namespace Assets.Scripts.UI.Ingame
{
	public static class Constants
	{
		public enum EntityIconPriorities
		{
			Default = 0,
			AwarenessIcon = 1,
			ConditionIcon = 2,
			ConditionOverwriteIcon = 3,
			CombatIcon = 4,
			ApproachIcon = 5,
			SelectionIcon = 6,
			NpcInteraction = 7,
			DisableReason = 8,
			Count = 9
		}

		public static readonly string IconPoolTag = "IconPool";

		public static readonly string TextPoolTag = "TextPool";

		public static readonly Vector3 PatrolIconOffset = Vector3.up * 2f;
	}
}
