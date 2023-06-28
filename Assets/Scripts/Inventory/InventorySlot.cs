using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Inventory
{
    public class InventorySlot : MonoBehaviour, IDropHandler
    {
        public void OnDrop(PointerEventData eventData)
        {
            if (transform.childCount == 0)
            {
                InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
                inventoryItem.SetSlot(transform);
            }
            else
            {
                InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
                InventoryItem itemInSlot = GetComponentInChildren<InventoryItem>();

                Transform previousItemSlot = inventoryItem.GetSlot();
                itemInSlot.transform.SetParent(previousItemSlot);
                inventoryItem.SetSlot(transform);
            }
        }
    }
}