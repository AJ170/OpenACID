using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI.ScreenManager.Layout
{
	[Serializable]
	public class TagFamilyLayoutData
	{
		public static FamilyLayoutData s_CurrentFamily;

		[SerializeField]
		public string TagNameId;

		[SerializeField]
		public List<FamilyLayoutData> FamiliesData;

		public FamilyLayoutData GetFamilyLayoutData(EFamilyType _familyType)
		{
			if (FamiliesData == null)
			{
				return null;
			}
			for (int i = 0; i < FamiliesData.Count; i++)
			{
				FamilyLayoutData familyLayoutData = FamiliesData[i];
				if (familyLayoutData.Type == _familyType)
				{
					return familyLayoutData;
				}
			}
			return null;
		}
	}
}
