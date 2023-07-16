using Infrastructure.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class SlotSelector : MonoBehaviour
    {
        [SerializeField]
        private Transform _toolSlot;
        [SerializeField]
        private InventorySlot[] _inventorySlots;

        private int _selectedSlot = 1;
        private bool _isToolInArms = false;
        private InputSystem _inputSystem;
        private EventManager _eventManager;

        private const int _slotsCount = 8;

        private void Start()
        {
            _inputSystem = AllServices.Container.Single<InputSystem>();
            _eventManager = AllServices.Container.Single<EventManager>();

            ChangeSelectedSlot(_selectedSlot);

            _eventManager.SelectedItemUseAction += UseSelectedItem;

            _inputSystem.ToolbarAction += ChangeSelectedSlot;
            _inputSystem.NextItemAction += ChangeSlotToNext;
            _inputSystem.PreviousItemAction += ChangeSlotToPrevious;
        }
        private void OnDestroy()
        {
            _eventManager.SelectedItemUseAction -= UseSelectedItem;

            _inputSystem.ToolbarAction -= ChangeSelectedSlot;
            _inputSystem.NextItemAction -= ChangeSlotToNext;
            _inputSystem.PreviousItemAction -= ChangeSlotToPrevious;
        }
        public void ChangeSelectedSlot(int newSlot)
        {
            if (_selectedSlot >= 0) _inventorySlots[_selectedSlot].DeselectSlot();
            if (_isToolInArms) RemoveItemFromArms();

            _selectedSlot = newSlot - 1;
            _inventorySlots[_selectedSlot].SelectSlot();

            InventoryItem itemInSlot = _inventorySlots[_selectedSlot].GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null)
            {
                ItemDataSO itemData = itemInSlot.GetItemData();
                if (itemData.type == ItemDataSO.ItemType.Tool)
                {
                    CreateToolInHands(itemData);
                }
            }
            else _isToolInArms = false;
        }
        public void RemoveItemFromArms()
        {
            if (_toolSlot.childCount > 0) Destroy(_toolSlot.GetChild(0).gameObject);            
            _isToolInArms = false;
        }
        public void CheckForToolInSelectedSlot()
        {
            InventoryItem item = _inventorySlots[_selectedSlot].GetComponentInChildren<InventoryItem>();
            if (item != null)
            {
                if (item.IsItemTool()) CreateToolInHands(item.GetItemData());
            }
        }
        public void CreateToolInHands(ItemDataSO itemData)
        {
            GameObject toolInHands;
            if (itemData.inHandPrefab != null)
            {
                toolInHands = Instantiate(itemData.inHandPrefab, _toolSlot.position, _toolSlot.rotation);
                toolInHands.transform.parent = _toolSlot;
                _isToolInArms = true;
            }
            else Debug.Log("Construction bluprint not setted - potik bachok");
        }
        public int GetSelectedSlot()
        {
            return _selectedSlot;
        }
        
        private void ChangeSlotToNext()
        {
            int nextSlot = (_selectedSlot + 1) + 1;
            if (nextSlot > _slotsCount) nextSlot = 1;
            ChangeSelectedSlot(nextSlot);
        }
        private void ChangeSlotToPrevious()
        {
            int previousSlot = (_selectedSlot + 1) - 1;
            if (previousSlot < 1) previousSlot = _slotsCount;
            ChangeSelectedSlot(previousSlot);
        }
        private void UseSelectedItem()
        {
            InventoryItem selectedItem = _inventorySlots[_selectedSlot].GetComponentInChildren<InventoryItem>();
            if (selectedItem.GetItemCount() == 1)
            {
                selectedItem.RemoveItem();
                if (_toolSlot.childCount > 0) Destroy(_toolSlot.GetChild(0).gameObject);
            }
            else selectedItem.RemoveItem();
        }

    }
}