using Infrastructure.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField]
        private InventorySlot[] _inventorySlots;
        [SerializeField]
        private GameObject _mainInventoryGroup;
        [SerializeField, Tooltip("Inventory canvas")]
        private Transform _inventoryRoot;
        [SerializeField]
        private GameObject _inventoryItemPrefab;

        private bool _isOpened = false;
        private int _selectedSlot = -1;

        private InputSystem _inputSystem;

        [SerializeField]
        private ItemDataSO[] _itemData;


        private void Start()
        {
            _isOpened = _mainInventoryGroup.activeSelf;

            _inputSystem = AllServices.Container.Single<InputSystem>();

            _inputSystem.OpenInventoryAction += ChangeInventoryState;
            _inputSystem.ToolbarAction += ChangeSelectedSlot;
        }
        private void OnDestroy()
        {
            _inputSystem.OpenInventoryAction -= ChangeInventoryState;
            _inputSystem.ToolbarAction -= ChangeSelectedSlot;
        }
        public bool AddItemToInventory(ItemDataSO itemData)
        {
            for (int i = 0; i < _inventorySlots.Length; i++)
            {
                InventorySlot slot = _inventorySlots[i];
                InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
                if (itemInSlot != null && itemData.stackable && itemInSlot.GetItemData() == itemData && itemInSlot.GetItemCount() < itemData.maxStackSize)
                {
                    itemInSlot.AddItem();
                    return true;
                }
            }

            for (int i = 0; i < _inventorySlots.Length; i++)
            {
                InventorySlot slot = _inventorySlots[i];
                InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
                if (itemInSlot == null)
                {
                    SpawnItemInSlot(itemData, slot);
                    return true;
                }
            }

            return false;
        }
        private void ChangeSelectedSlot(int newSlot)
        {
            if (_selectedSlot >= 0) _inventorySlots[_selectedSlot].DeselectSlot();
            _inventorySlots[newSlot - 1].SelectSlot();
            _selectedSlot = newSlot - 1;
        }
        private void SpawnItemInSlot(ItemDataSO itemData, InventorySlot inventorySlot)
        {
            GameObject newItemGO = Instantiate(_inventoryItemPrefab, inventorySlot.transform);
            InventoryItem inventoryItem = newItemGO.GetComponent<InventoryItem>();
            inventoryItem.InitializeItem(itemData, _inventoryRoot);
        }
        private void ChangeInventoryState()
        {
            if (!_isOpened) OpenInventory();
            else CloseInventory();
        }

        private void CloseInventory()
        {
            _mainInventoryGroup.SetActive(false);
            _isOpened = false;

            _inputSystem.UnlockControl();
        }

        private void OpenInventory()
        {
            _inputSystem.LockControl();

            _mainInventoryGroup.SetActive(true);
            _isOpened = true;

            bool isItemAdded = AddItemToInventory(_itemData[Random.Range(0, _itemData.Length)]);
            Debug.Log("Item is added to inventory : " + isItemAdded.ToString());
        }
    }
}