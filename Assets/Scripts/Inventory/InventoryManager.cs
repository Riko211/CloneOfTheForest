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

        [SerializeField]
        private ItemDataSO[] _itemData;
        private float _dropItemOffset = 0.75f;

        private void Start()
        {
            _isOpened = _mainInventoryGroup.activeSelf;
            ChangeSelectedSlot(_selectedSlot);

            _inputSystem = AllServices.Container.Single<InputSystem>();

            _inputSystem.OpenInventoryAction += ChangeInventoryState;
            _inputSystem.ToolbarAction += ChangeSelectedSlot;
            _inputSystem.DropItemAction += DropItem;
        }
        private void OnDestroy()
        {
            _inputSystem.OpenInventoryAction -= ChangeInventoryState;
            _inputSystem.ToolbarAction -= ChangeSelectedSlot;
            _inputSystem.DropItemAction -= DropItem;
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

        private void RemoveItemFromArms()
        {
            if (_toolSlot.childCount > 0) Destroy(_toolSlot.GetChild(0).gameObject);
            _isToolInArms = false;
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

            bool isItemAdded = AddItemToInventory(_itemData[Random.Range(0, _itemData.Length)]);
            Debug.Log("Item is added to inventory : " + isItemAdded.ToString());
        }
    }
}