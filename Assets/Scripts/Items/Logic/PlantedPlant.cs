using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Items.Logic
{
    public class PlantedPlant : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] _grownTree;

        [SerializeField]
        private float _timeToGrow = 15f;

        private void Start()
        {
            StartCoroutine(nameof(Grow));
        }

        private IEnumerator Grow()
        {
            yield return new WaitForSeconds(_timeToGrow);
            Instantiate(_grownTree[Random.Range(0, _grownTree.Length)], transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}