using System;
using UnityEngine;

namespace Assets.Scripts.UI.ScreenManager.Layout
{
	[Serializable]
	public class FamilyLayoutData
	{
		[SerializeField]
		public EFamilyType Type;

		[SerializeField]
		public FontLayoutData FontData;

		[SerializeField]
		public float Scale = 1f;

		public bool Isvalid()
		{
			return FontData != null;
		}
	}
}
