using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventory
{
    public class InventorySlot : MonoBehaviour, IDropHandler
    {
        public Action OnItemTake;

        [SerializeField]
        private Image _image;
        [SerializeField]
        private Color _selectedColor, _defaultColor;

        [SerializeField]
        private bool _canPutItem = true;

        public void SelectSlot()
        {
            _image.color = _selectedColor;
        }
        public void DeselectSlot()
        {
            _image.color = _defaultColor;
        }
        public bool IsCanPutItem()
        {
            return _canPutItem;
        }
        public void OnDrop(PointerEventData eventData)
        {
            if (_canPutItem)
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

                    if (previousItemSlot.GetComponent<InventorySlot>().IsCanPutItem() && _canPutItem)
                    {
                        itemInSlot.transform.SetParent(previousItemSlot);
                        inventoryItem.SetSlot(transform);
                    }
                }
            }
        }
    }
}