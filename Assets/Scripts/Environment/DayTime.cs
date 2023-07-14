using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Environment
{
    public class DayTime : MonoBehaviour
    {
        [SerializeField]
        private Transform _sunAndMoonRotation;

        private Vector3 _newRotation;

        private void Start()
        {
            _newRotation = _sunAndMoonRotation.rotation.eulerAngles;
        }
        private void Update()
        {
            _newRotation.x += 1 * Time.deltaTime;
            _sunAndMoonRotation.rotation = Quaternion.Euler(_newRotation);
        }
    }
}