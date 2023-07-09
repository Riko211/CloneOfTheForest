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
        private Transform _toolSlot;
        [SerializeField]
        private GameObject _inventoryItemPrefab;

        private bool _isOpened = false;
        private int _selectedSlot = 1;
        private bool _isToolInArms = false;

        private InputSystem _inputSystem;
        private EventManager _eventManager;

        private float _dropItemOffset = 0.75f;
        private const int _slotsCount = 8;

        private void Start()
        {
            _isOpened = _mainInventoryGroup.activeSelf;
            ChangeSelectedSlot(_selectedSlot);

            _inputSystem = AllServices.Container.Single<InputSystem>();
            _eventManager = AllServices.Container.Single<EventManager>();

            _eventManager.SelectedItemUseAction += UseSelectedItem;

            _inputSystem.OpenInventoryAction += ChangeInventoryState;
            _inputSystem.ToolbarAction += ChangeSelectedSlot;
            _inputSystem.DropItemAction += DropItem;
            _inputSystem.NextItemAction += ChangeSlotToNext;
            _inputSystem.PreviousItemAction += ChangeSlotToPrevious;
        }
        private void OnDestroy()
        {
            _eventManager.SelectedItemUseAction -= UseSelectedItem;

            _inputSystem.OpenInventoryAction -= ChangeInventoryState;
            _inputSystem.ToolbarAction -= ChangeSelectedSlot;
            _inputSystem.DropItemAction -= DropItem;
            _inputSystem.NextItemAction -= ChangeSlotToNext;
            _inputSystem.PreviousItemAction -= ChangeSlotToPrevious;
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
                    if (itemData.type == ItemDataSO.ItemType.Tool && _selectedSlot == i) CreateToolInHands(itemData);
                    return true;
                }
            }

            return false;
        }
        public bool AddItemsToInventoryInFreeSlot(ItemDataSO itemData, int count) 
        {
            for (int i = 0; i < _inventorySlots.Length; i++)
            {
                InventorySlot slot = _inventorySlots[i];
                InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
                if (itemInSlot == null)
                {
                    SpawnItemInSlot(itemData, slot, count);
                    if (itemData.type == ItemDataSO.ItemType.Tool && _selectedSlot == i) CreateToolInHands(itemData);
                    return true;
                }
            }

            return false;
        }
        public bool AddCurrentItemToInventory(InventoryItem itemToAdd)
        {
            for (int i = 0; i < _inventorySlots.Length; i++)
            {
                InventorySlot slot = _inventorySlots[i];
                InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
                ItemDataSO itemData = itemToAdd.GetItemData();

                if (itemInSlot != null && itemData.stackable && itemInSlot.GetItemData() == itemData && itemInSlot.GetItemCount() < itemData.maxStackSize)
                {
                    int itemToAddCount = itemToAdd.GetItemCount();
                    int itemInSlotCount = itemInSlot.GetItemCount();
                    int stackSize = itemInSlot.GetItemData().maxStackSize;

                    if (itemInSlotCount + itemToAddCount <= stackSize)
                    {
                        itemInSlot.AddItems(itemToAddCount);
                        Destroy(itemToAdd.gameObject);
                        return true;
                    }
                    else
                    {
                        itemInSlot.AddItems(stackSize - itemInSlotCount);
                        itemToAdd.RemoveItems(stackSize - itemInSlotCount);
                    }
                }
            }

            for (int i = 0; i < _inventorySlots.Length; i++)
            {
                InventorySlot slot = _inventorySlots[i];
                InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
                if (itemInSlot == null)
                {
                    itemToAdd.transform.SetParent(slot.transform);
                    ItemDataSO itemData = itemToAdd.GetItemData();
                    if (itemData.type == ItemDataSO.ItemType.Tool && _selectedSlot == i) CreateToolInHands(itemData);
                    return true;
                }
            }

            return false;
        }
        public void SpawnItemInSlot(ItemDataSO itemData, InventorySlot inventorySlot)
        {
            GameObject newItemGO = Instantiate(_inventoryItemPrefab, inventorySlot.transform);
            InventoryItem inventoryItem = newItemGO.GetComponent<InventoryItem>();
            inventoryItem.InitializeItem(itemData, _inventoryRoot, this);
        }
        public void SpawnItemInSlot(ItemDataSO itemData, InventorySlot inventorySlot, int count)
        {
            GameObject newItemGO = Instantiate(_inventoryItemPrefab, inventorySlot.transform);
            InventoryItem inventoryItem = newItemGO.GetComponent<InventoryItem>();
            inventoryItem.InitializeItem(itemData, _inventoryRoot, count, this);         
        }
        private void DropItem()
        {
            InventoryItem itemForDrop = _inventorySlots[_selectedSlot].GetComponentInChildren<InventoryItem>();
            if (itemForDrop != null && _selectedSlot >= 0)
            {
                Vector3 dropPosition = transform.TransformPoint(new Vector3(0f, 0f, _dropItemOffset));
                GameObject item = Instantiate(itemForDrop.GetItemData().collectablePrefab, dropPosition, Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y - 90, 0)));
                item.GetComponent<Rigidbody>().AddForce(transform.forward, ForceMode.VelocityChange);
                itemForDrop.RemoveItem();
                if (itemForDrop.IsItemTool()) RemoveItemFromArms();
            }
        }
        public void DropItem(InventoryItem itemForDrop)
        {
            Vector3 dropPosition = transform.TransformPoint(new Vector3(0f, 0f, _dropItemOffset));
            GameObject item = Instantiate(itemForDrop.GetItemData().collectablePrefab, dropPosition, Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y - 90, 0)));
            item.GetComponent<Rigidbody>().AddForce(transform.forward, ForceMode.VelocityChange);
        }
        private void ChangeSelectedSlot(int newSlot)
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

        private void CreateToolInHands(ItemDataSO itemData)
        {
            GameObject spawnedItem = Instantiate(itemData.inHandPrefab, _toolSlot.position, _toolSlot.rotation);
            spawnedItem.transform.parent = _toolSlot;
            _isToolInArms = true;
        }
        private void CheckForToolInSelectedSlot()
        {
            InventoryItem item = _inventorySlots[_selectedSlot].GetComponentInChildren<InventoryItem>();
            if (item != null)
            {
                if (item.IsItemTool()) CreateToolInHands(item.GetItemData());
            }
        }
        private void UseSelectedItem()
        {
            _inventorySlots[_selectedSlot].GetComponentInChildren<InventoryItem>().RemoveItem();
        }

        private void RemoveItemFromArms()
        {
            if (_toolSlot.childCount > 0) Destroy(_toolSlot.GetChild(0).gameObject);
            _isToolInArms = false;
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
            CheckForToolInSelectedSlot();


            _inputSystem.UnlockControl();
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void OpenInventory()
        {
            _inputSystem.LockControl();
            Cursor.lockState = CursorLockMode.Confined;

            _mainInventoryGroup.SetActive(true);
            _isOpened = true;
            RemoveItemFromArms();
        }
    }
}