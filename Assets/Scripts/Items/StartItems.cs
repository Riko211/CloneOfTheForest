using Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Items
{
    public class StartItems : MonoBehaviour
    {
        [SerializeField]
        private InventoryManager _inventoryManager;

        [SerializeField]
        private StartItem[] _startItems;
        [System.Serializable]
        private struct StartItem
        {
            public ItemDataSO itemData;
            public int count;
        }

        private void Start()
        {
            foreach(StartItem item in _startItems)
            {
                for(int i = 0; i<item.count; i++)
                {
                    _inventoryManager.AddItemToInventory(item.itemData);
                }
            }
        }
    }
}