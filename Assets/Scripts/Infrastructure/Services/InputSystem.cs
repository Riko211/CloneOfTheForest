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

        public Action JumpAction;

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
            Axis = _mainInputAction.Player.Movement.ReadValue<Vector2>();
            CameraAxis = _mainInputAction.Player.Camera.ReadValue<Vector2>();
            MousePosition = _mainInputAction.Player.MousePos.ReadValue<Vector2>();
        }

        private void BindFuncs()
        {
            _mainInputAction.Player.Jump.performed += JumpCallBack;
        }
        private void JumpCallBack(InputAction.CallbackContext obj)
        {
            if (JumpAction != null) JumpAction.Invoke();
        }
    }
}