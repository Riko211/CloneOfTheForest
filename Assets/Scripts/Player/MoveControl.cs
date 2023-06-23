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
        private float _mouseSensitivity = 100f;

        [SerializeField]
        private float _normalMoveSpeed = 2.8f;
        [SerializeField]
        private float _sprintMultiplier;

        private InputSystem _inputSystem;

        private float _curSpeedMultiplier = 1f;
        private Vector3 _currSpeed;
        private float _mouseX, _mouseY;
        private float _xMove, _zMove;
        private Vector2 _mouseDelta;
        private float _xRotation = 0f;

        private void Start()
        {
            _inputSystem = AllServices.Container.Single<InputSystem>();
        }

        private void Update()
        {
            InputMouseMove();
            InputMove();

            PlayerRotation();
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
            //Vector3 moveDir = CalculateMove(_sprint);
            Vector3 moveDir = transform.right * _xMove + transform.forward * _zMove;

            _currSpeed = moveDir * _normalMoveSpeed * _curSpeedMultiplier;

            //if (_currSpeed.magnitude > 0) AdjustFootstepsAudio(_curSpeedMultiplier);
            //else AdjustFootstepsAudio(_curSpeedMultiplier, true);
            _charController.Move(_currSpeed * Time.deltaTime);
        }
        private Vector3 CalculateMove(bool sprint)
        {
            if (sprint) _curSpeedMultiplier = _sprintMultiplier;

            else _curSpeedMultiplier = 1f;

            Vector3 result = transform.right * _xMove + transform.forward * _zMove;
            return result;
        }
    }
}