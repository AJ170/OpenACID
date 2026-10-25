using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Database;
using UbiCore.Utils;

namespace Assets.Scripts.UI.Utils
{
	public class ItemNotificationManager
	{
		public List<EventDelegate> onInventoryNotificationChange = new List<EventDelegate>();

		public void SendUpdate(Inventory _newInventory, Inventory _oldInventory, List<EquippedElement> _equippedItems)
		{
			LocalStorage instance = UbiCore.Utils.Singleton<LocalStorage>.Instance;
			if (instance != null)
			{
				instance.BeginGroupEdit();
				if (instance.ItemCount == 0)
				{
					_oldInventory = null;
				}
				for (int i = 0; i < _newInventory.elements.Length; i++)
				{
					InventoryElement newItem = _newInventory.elements[i];
					if (_oldInventory == null || _oldInventory.elements.FindIndex((InventoryElement _oldItem) => _oldItem.id == newItem.id) < 0)
					{
						instance.ItemAdd(newItem.id);
					}
				}
				for (int j = 0; j < _equippedItems.Count; j++)
				{
					EquippedElement equippedElement = _equippedItems[j];
					instance.ItemSetSeen(equippedElement.element.id);
				}
				instance.EndGroupEditAndSave();
			}
			EventDelegate.Execute(onInventoryNotificationChange);
		}
	}
}
