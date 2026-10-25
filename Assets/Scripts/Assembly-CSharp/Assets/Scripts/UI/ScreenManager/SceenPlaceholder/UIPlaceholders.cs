using UnityEngine;

namespace Assets.Scripts.UI.ScreenManager.SceenPlaceholder
{
	[AddComponentMenu("UI/Placeholders")]
	public class UIPlaceholders : MonoBehaviour
	{
		public enum EPlaceholderType
		{
			Assassin = 0,
			CreateAssassin = 1,
			CreateAssassinStencil = 2,
			MysteryBox = 3,
			Inventory = 4,
			AssassinPreview = 5
		}

		public UIPlaceholder GetPlaceholder(EPlaceholderType _type)
		{
			for (int i = 0; i < base.transform.childCount; i++)
			{
				UIPlaceholder component = base.transform.GetChild(i).GetComponent<UIPlaceholder>();
				if (component != null && component.PlaceholderType == _type)
				{
					return component;
				}
			}
			return null;
		}
	}
}
