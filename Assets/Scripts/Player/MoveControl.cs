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
        private Transform _groundCheckPoint;

        [SerializeField]
        private float _mouseSensitivity = 1f;

        [SerializeField]
        private float _normalMoveSpeed = 2.8f;
        [SerializeField]
        private float _inertiaCoef = 0.07f;
        [SerializeField]
        private float _sprintMultiplier;
        [SerializeField]
        private float _jumpForce = 4f;

        [SerializeField]
        private float _playerWidht = 0.5f;
        [SerializeField]
        private LayerMask _whatIsGround;
        [SerializeField]
        private bool _isGrounded;
        [SerializeField]
        private float _gravitation = 9.81f;


        private InputSystem _inputSystem;

        private float _curSpeedMultiplier = 1f;
        private Vector3 _curSpeed;
        private float _mouseX, _mouseY;
        private float _xMove, _yMove, _zMove;
        private Vector2 _mouseDelta;
        private float _xRotation = 0f;
        private Vector3 _moveDirection;
        private bool _jumpLock;


        private void Start()
        {
            _inputSystem = AllServices.Container.Single<InputSystem>();

            _inputSystem.JumpAction += Jump;
        }
        private void OnDestroy()
        {
            _inputSystem.JumpAction -= Jump;
        }

        private void Update()
        {
            InputMouseMove();
            InputMove();

            GroundCheck();
            CalculateGravitation();

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
        private void CalculateGravitation()
        {
            if (_isGrounded && !_jumpLock)
            {
                _yMove = -3f;
                _charController.Move(new Vector3(0, _yMove, 0) * Time.deltaTime);
            }
            else
            {
                _yMove -= _gravitation * Time.deltaTime;
                _charController.Move(new Vector3(0, _yMove, 0) * Time.deltaTime);
            }

        }
        private void Jump()
        {
            if (_isGrounded && !_jumpLock)
            {
                _jumpLock = true;
                StartCoroutine(nameof(JumpUnlock));
                _yMove = _jumpForce;
            }
        }
        private IEnumerator JumpUnlock()
        {
            yield return new WaitForSeconds(0.3f);
            _jumpLock = false;
        }
        private void GroundCheck()
        {
            //if (Physics.OverlapSphere(transform.position, _playerWidht, _whatIsGround).Length > 0) _isGrounded = true;
            //else _isGrounded = false;
            if (_charController.isGrounded || Physics.CheckSphere(_groundCheckPoint.position, _playerWidht, _whatIsGround)) _isGrounded = true;
            else _isGrounded = false;
        }
    }
}