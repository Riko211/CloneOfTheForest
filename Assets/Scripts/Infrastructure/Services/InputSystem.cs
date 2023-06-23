using UnityEngine;
using UnityEngine.InputSystem;

namespace Infrastructure.Services
{
    public class InputSystem : MonoBehaviour, IService
    {
        public Vector2 Axis { get; private set; }
        public Vector2 CameraAxis { get; private set; }

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
        }

        private void BindFuncs()
        {

        }
    }
}