using Infrastructure.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class MoveControl : MonoBehaviour
    {
        [SerializeField]
        private CharacterController _charController;

        [SerializeField]
        private Transform _playerBody;
        [SerializeField]
        private Transform _playerHead;

        [SerializeField]
        private float _mouseSensitivity = 1f;

        [SerializeField]
        private float _normalMoveSpeed = 2.8f;
        [SerializeField]
        private float _inertiaCoef = 0.07f;
        [SerializeField]
        private float _sprintMultiplier;

        [SerializeField]
        private float _playerHeight = 2f;
        [SerializeField]
        private LayerMask _whatIsGround;
        [SerializeField]
        private bool _isGrounded;

        private InputSystem _inputSystem;

        private float _curSpeedMultiplier = 1f;
        private Vector3 _curSpeed;
        private float _mouseX, _mouseY;
        private float _xMove, _zMove;
        private Vector2 _mouseDelta;
        private float _xRotation = 0f;
        private Vector3 _moveDirection;

        private void Start()
        {
            _inputSystem = AllServices.Container.Single<InputSystem>();
        }

        private void Update()
        {
            InputMouseMove();
            InputMove();

            GroundCheck();
            PlayerRotation();
            PlayerMovement();
        }
        private void InputMove()
        {
            _xMove = _inputSystem.Axis.x;
            _zMove = _inputSystem.Axis.y;
        }
        private void InputMouseMove()
        {
            _mouseDelta = _inputSystem.CameraAxis;
            _mouseX = _mouseDelta.x * _mouseSensitivity;
            _mouseY = _mouseDelta.y * _mouseSensitivity;
        }
        private void PlayerRotation()
        {
            _playerBody.Rotate(Vector3.up * _mouseX);

            _xRotation -= _mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

            _playerHead.transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        }

        private void PlayerMovement()
        {
            _moveDirection = transform.forward * _zMove + transform.right * _xMove;
            _curSpeed = Vector3.Lerp(_curSpeed, _moveDirection.normalized * _normalMoveSpeed, _inertiaCoef * Time.deltaTime);
            _charController.Move(_curSpeed * Time.deltaTime);
        }
        private void GroundCheck()
        {
            _isGrounded = Physics.Raycast(transform.position, Vector3.down, _playerHeight / 2 + 0.2f, _whatIsGround);
        }
    }
}