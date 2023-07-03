using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    [SerializeField, Header("Limits")]
    private Vector2 _minLimitPoint;
    [SerializeField]
    private Vector2 _maxLimitPoint;

    [SerializeField, Header("Palm growth height")]
    private float _minPalmHeight;
    [SerializeField]
    private float _maxPalmHeight;

    [SerializeField, Header("Parent transform for trees")]
    private Transform _parent;

    [SerializeField, Header("Terrain layer")]
    private LayerMask _terrainLayer;

    [SerializeField, Header("Tree prefabs")]
    private GameObject[] _palms;
    [SerializeField]
    private GameObject[] _oaks;
    [SerializeField]
    private GameObject[] _pines;

    [SerializeField, Header("Spawn trees")]
    private bool _spawnPalms = false, _spawnOaks = false, _spawnPines = false;
    
    void Update()
    {
        if (_spawnPalms == true)
        {
            StartCoroutine(SpawnPalms());
        }
    }

    private IEnumerator SpawnPalms()
    {
        while (_spawnPalms == true)
        {
            bool _palmPlanted = false;
            int tryesToSpawn = 0;
            while (!_palmPlanted)
            {
                Vector3 castPoint = new Vector3(Random.Range(_minLimitPoint.x, _maxLimitPoint.x), 500f, Random.Range(_minLimitPoint.y, _maxLimitPoint.y));
                if (Physics.Raycast(castPoint, Vector3.down, out RaycastHit hit, _terrainLayer))
                {
                    Vector3 hitPoint = hit.point;
                    if (hitPoint.y >= _minPalmHeight && hitPoint.y <= _maxPalmHeight)
                    {
                        Debug.Log("Palm planted in: " + hitPoint.ToString());
                        GameObject newPalm = Instantiate(_palms[Random.Range(0, _palms.Length)], hitPoint - new Vector3(0, 0.1f, 0), Quaternion.Euler(0, Random.Range(0, 360), 0));
                        newPalm.transform.SetParent(_parent);
                        _palmPlanted = true;
                    }
                    else Debug.Log("Palm NOT planted in: " + hitPoint.ToString());
                }
                //_spawnPalms = false;
                tryesToSpawn++;
            }
            _palmPlanted = false;
            Debug.Log(tryesToSpawn);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
