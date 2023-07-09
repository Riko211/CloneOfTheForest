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
        [SerializeField]
        private SlotSelector _slotSelector;

        private bool _isOpened = false;

        private InputSystem _inputSystem;

        private float _dropItemOffset = 0.75f;
        private float _multipleDropItemOffset = 0.1f;


        private void Start()
        {
            _isOpened = _mainInventoryGroup.activeSelf;
            _inputSystem = AllServices.Container.Single<InputSystem>();

            _inputSystem.OpenInventoryAction += ChangeInventoryState;
            _inputSystem.DropItemAction += DropItemFromSelectedSlot;
        }
        private void OnDestroy()
        {
            _inputSystem.OpenInventoryAction -= ChangeInventoryState;
            _inputSystem.DropItemAction -= DropItemFromSelectedSlot;

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
                    if (itemData.type == ItemDataSO.ItemType.Tool && _slotSelector.GetSelectedSlot() == i) _slotSelector.CreateToolInHands(itemData);
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
                    if (itemData.type == ItemDataSO.ItemType.Tool && _slotSelector.GetSelectedSlot() == i) _slotSelector.CreateToolInHands(itemData);
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
                    if (itemData.type == ItemDataSO.ItemType.Tool && _slotSelector.GetSelectedSlot() == i) _slotSelector.CreateToolInHands(itemData);
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
        private void DropItemFromSelectedSlot()
        {
            InventoryItem itemForDrop = _inventorySlots[_slotSelector.GetSelectedSlot()].GetComponentInChildren<InventoryItem>();
            if (itemForDrop != null && _slotSelector.GetSelectedSlot() >= 0)
            {
                Vector3 dropPosition = transform.TransformPoint(new Vector3(0f, 0f, _dropItemOffset));
                GameObject item = Instantiate(itemForDrop.GetItemData().collectablePrefab, dropPosition, Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y - 90, 0)));
                item.GetComponent<Rigidbody>().AddForce(transform.forward, ForceMode.VelocityChange);
                itemForDrop.RemoveItem();
                if (itemForDrop.IsItemTool()) _slotSelector.RemoveItemFromArms();
            }
        }
        public void DropItems(InventoryItem itemForDrop)
        {
            int itemCount = itemForDrop.GetItemCount();
            ItemDataSO itemData = itemForDrop.GetItemData();
            for (int i = 0; i < itemCount; i++)
            {
                Vector3 dropPosition = transform.TransformPoint(new Vector3(0f, _multipleDropItemOffset * i, _dropItemOffset));
                GameObject item = Instantiate(itemData.collectablePrefab, dropPosition, Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y - 90, 0)));
                item.GetComponent<Rigidbody>().AddForce(transform.forward, ForceMode.VelocityChange);
            }
        }
        public void DropItem(ItemDataSO itemData)
        {
            Vector3 dropPosition = transform.TransformPoint(new Vector3(0f, 0f, _dropItemOffset));
            GameObject item = Instantiate(itemData.collectablePrefab, dropPosition, Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y - 90, 0)));
            item.GetComponent<Rigidbody>().AddForce(transform.forward, ForceMode.VelocityChange);
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
            _slotSelector.CheckForToolInSelectedSlot();


            _inputSystem.UnlockControl();
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void OpenInventory()
        {
            _inputSystem.LockControl();
            Cursor.lockState = CursorLockMode.Confined;

            _mainInventoryGroup.SetActive(true);
            _isOpened = true;
            _slotSelector.RemoveItemFromArms();
        }
    }
}