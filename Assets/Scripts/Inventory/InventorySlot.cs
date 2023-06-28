using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventory
{
    public class InventorySlot : MonoBehaviour, IDropHandler
    {
        [SerializeField]
        private Image _image;
        [SerializeField]
        private Color _selectedColor, _defaultColor;

        private void Start()
        {
            _defaultColor = _image.color;
        }
        public void SelectSlot()
        {
            _image.color = _selectedColor;
        }
        public void DeselectSlot()
        {
            _image.color = _defaultColor;
        }
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