using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    [RequireComponent(typeof(Rigidbody))]
    public class CollectableItem : MonoBehaviour, ICollectable
    {
        [SerializeField]
        private ItemDataSO _itemData;

        public void DestroyItem()
        {
            Destroy(gameObject);
        }
        public void SetItemData(ItemDataSO itemData)
        {
            _itemData = itemData;
        }

        public ItemDataSO CollectItem()
        {
            return _itemData;
        }
    }
}