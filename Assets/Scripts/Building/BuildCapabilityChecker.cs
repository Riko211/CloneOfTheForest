using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildCapabilityChecker : MonoBehaviour
{
    [SerializeField]
    private LayerMask _interferingLayers;
    [SerializeField]
    private Transform[] _checkPoints;
    [SerializeField]
    private Collider _collider;

    private float _checkRadius = 0.05f;

    private void Start()
    {
        CheckPointsForInterfering();
    }

    private void CheckPointsForInterfering()
    {
        _collider.enabled = false;

        foreach (Transform point in _checkPoints)
        {
            if (!Physics.CheckSphere(point.position, _checkRadius, _interferingLayers)) point.gameObject.SetActive(true);
            //else Destroy(point.gameObject);
        }

        _collider.enabled = true;
    }
}
