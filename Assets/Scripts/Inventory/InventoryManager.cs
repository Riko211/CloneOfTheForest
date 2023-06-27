using Infrastructure.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject _mainInventoryGroup;

        private bool _isOpened = false;

        private InputSystem _inputSystem;

        private void Start()
        {
            _isOpened = _mainInventoryGroup.activeSelf;

            _inputSystem = AllServices.Container.Single<InputSystem>();

            _inputSystem.OpenInventoryAction += ChangeInventoryState;
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