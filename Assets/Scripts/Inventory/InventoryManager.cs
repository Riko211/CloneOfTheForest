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
        [SerializeField]
        private GameObject _inventoryItemPrefab;

        private bool _isOpened = false;

        private InputSystem _inputSystem;


        private void Start()
        {
            _isOpened = _mainInventoryGroup.activeSelf;

            _inputSystem = AllServices.Container.Single<InputSystem>();

            _inputSystem.OpenInventoryAction += ChangeInventoryState;
        }
        public void AddItemToInventory(InventoryItemSO itemData)
        {
            for (int i = 0; i < _inventorySlots.Length; i++)
            {
                InventorySlot slot = _inventorySlots[i];
                InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
                if (itemInSlot == null)
                {
                    SpawnItemInSlot(itemData, slot);
                    return;
                }
            }
        }
        private void SpawnItemInSlot(InventoryItemSO itemData, InventorySlot inventorySlot)
        {
            GameObject newItemGO = Instantiate(_inventoryItemPrefab, inventorySlot.transform);
            InventoryItem inventoryItem = newItemGO.GetComponent<InventoryItem>();
            inventoryItem.InitializeItem(itemData);
        }
        private void ChangeInventoryState()
        {
            if (_isOpened) OpenInventory();
            else CloseInventory();
        }

        private void CloseInventory()
        {
            _mainInventoryGroup.SetActive(true);
            _isOpened = true;
        }

        private void OpenInventory()
        {
            _mainInventoryGroup.SetActive(false);
            _isOpened = false;
        }
    }
}