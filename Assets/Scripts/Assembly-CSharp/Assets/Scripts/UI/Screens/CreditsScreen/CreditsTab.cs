using UnityEngine;

namespace Assets.Scripts.UI.Screens.CreditsScreen
{
	public abstract class CreditsTab : MonoBehaviour
	{
		public bool Visible
		{
			get
			{
				return Distance <= 0f;
			}
		}

		public float Distance { get; set; }

		public ICreditsItem Item { get; private set; }

		public virtual void Init()
		{
		}

		public virtual void UpdateItem(ICreditsItem _item)
		{
			if (Item != null && Item != _item)
			{
				Item.Tab = null;
			}
			Item = _item;
			Item.Tab = this;
			base.transform.localPosition = new Vector3(0f, 0f - _item.Position, 0f);
		}
	}
}
