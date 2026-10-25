using System;
using UnityEngine;

namespace Assets.Scripts.UI.Ingame.Behaviours
{
	[Serializable]
	public class UIMissionIntroductionPlaceHolders
	{
		[SerializeField]
		private Introduction.EUIAlignment m_alignment;

		[SerializeField]
		private Transform m_parent;

		public Introduction.EUIAlignment UIAlignment
		{
			get
			{
				return m_alignment;
			}
		}

		public Transform ParentTransform
		{
			get
			{
				return m_parent;
			}
		}
	}
}
