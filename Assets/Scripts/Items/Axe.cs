using Infrastructure.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Items
{
    public class Axe : MonoBehaviour
    {
        [SerializeField]
        private Animation _animation;

        private InputSystem _inputSystem;

        private void Start()
        {
            _inputSystem = AllServices.Container.Single<InputSystem>();

            _inputSystem.LMBClickAction += AxeAttack;
        }
        private void OnDestroy()
        {
            _inputSystem.LMBClickAction -= AxeAttack;
        }
        private void AxeAttack()
        {
            if(!_animation.isPlaying) _animation.Play();
        }
    }
}