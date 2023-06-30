using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Infrastructure.Services
{
    public class InputSystem : MonoBehaviour, IService
    {
        public Vector2 Axis { get; private set; }
        public Vector2 CameraAxis { get; private set; }

        public Vector2 MousePosition { get; private set; }

        public Action JumpAction, OpenInventoryAction, ItemPickUpAction, DropItemAction, LMBClickAction;
        public Action<int> ToolbarAction;
        public bool IsControlLocked { get; private set; }

        private MainInputAction _mainInputAction;


        private void Awake()
        {
            _mainInputAction = new MainInputAction();
            _mainInputAction.Player.Enable();

            BindFuncs();
            DontDestroyOnLoad(this);
        }

        private void Update()
        {
            ReadControls();
        }

        private void ReadControls()
        {
            if (!IsControlLocked)
            {
                Axis = _mainInputAction.Player.Movement.ReadValue<Vector2>();
                CameraAxis = _mainInputAction.Player.Camera.ReadValue<Vector2>();
            }
            MousePosition = _mainInputAction.Player.MousePos.ReadValue<Vector2>();
        }

        private void BindFuncs()
        {
            _mainInputAction.Player.Jump.performed += JumpCallBack;
            _mainInputAction.Player.OpenInventory.performed += OpenInventoryCallBack;
            _mainInputAction.Player.Toolbar.performed += ToolbarCallBack;
            _mainInputAction.Player.ItemPickUp.performed += ItemPickUpCallBack;
            _mainInputAction.Player.DropItem.performed += DropItemCallBack;
            _mainInputAction.Player.LMBClick.performed += LMBClickCallBack;
        }
        public void LMBClickCallBack(InputAction.CallbackContext obj)
        {
            if (LMBClickAction != null) LMBClickAction.Invoke();
        }
        public void JumpCallBack(InputAction.CallbackContext obj)
        {
            if (JumpAction != null) JumpAction.Invoke();
        }
        public void OpenInventoryCallBack(InputAction.CallbackContext obj)
        {
            if (OpenInventoryAction != null) OpenInventoryAction.Invoke();
        }
        public void ToolbarCallBack(InputAction.CallbackContext obj)
        {
            if (ToolbarAction != null) ToolbarAction.Invoke(int.Parse(obj.control.displayName));
        }
        public void ItemPickUpCallBack(InputAction.CallbackContext obj)
        {
            if (ItemPickUpAction != null) ItemPickUpAction.Invoke();
        }
        public void DropItemCallBack(InputAction.CallbackContext obj)
        {
            if (DropItemAction != null) DropItemAction.Invoke();
        }

        public void LockControl()
        {
            IsControlLocked = true;
            Axis = Vector2.zero;
            CameraAxis = Vector2.zero;
        }
        public void UnlockControl()
        {
            IsControlLocked = false;
        }
    }
}