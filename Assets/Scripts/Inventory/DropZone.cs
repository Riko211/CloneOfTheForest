using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Inventory
{
    public class DropZone : MonoBehaviour, IDropHandler
    {
        public Action<InventoryItem> OnItemDrop;

        public void OnDrop(PointerEventData eventData)
        {
            OnItemDrop?.Invoke(eventData.pointerDrag.GetComponent<InventoryItem>());
            Destroy(eventData.pointerDrag.gameObject);
        }
    }
}