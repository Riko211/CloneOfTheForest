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

    [SerializeField, Header("Oak growth height")]
    private float _minOakHeight;
    [SerializeField]
    private float _maxOakHeight;

    [SerializeField, Header("Pine growth height")]
    private float _minPineHeight;
    [SerializeField]
    private float _maxPineHeight;

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

    [SerializeField, Header("Start spawn trees")]
    private bool _spawnPalms = false;
    [SerializeField]
    private bool _spawnOaks = false;
    [SerializeField]
    private bool _spawnPines = false;
    [SerializeField, Header("Stop spawn trees")]
    private bool _stopSpawnPalms = false;
    [SerializeField]
    private bool _stopSpawnOaks = false;
    [SerializeField]
    private bool _stopSpawnPines = false;

    void Update()
    {
        if (_spawnPalms == true)
        {
            StartCoroutine(SpawnPalms());
            _spawnPalms = false;
        }
        if (_spawnOaks == true)
        {
            StartCoroutine(SpawnOaks());
            _spawnOaks = false;
        }
        if (_spawnPines == true)
        {
            StartCoroutine(SpawnPines());
            _spawnPines = false;
        }

        if (_stopSpawnPalms == true)
        {
            StopCoroutine(SpawnPalms());
            _stopSpawnPalms = false;
        }
        if (_stopSpawnOaks == true)
        {
            StopCoroutine(SpawnOaks());
            _stopSpawnOaks = false;
        }
        if (_stopSpawnPines == true)
        {
            StopCoroutine(SpawnPines());
            _stopSpawnPines = false;
        }
    }

    private IEnumerator SpawnPalms()
    {
        while (true)
        {
            bool _palmPlanted = false;
            while (!_palmPlanted)
            {
                Vector3 castPoint = new Vector3(Random.Range(_minLimitPoint.x, _maxLimitPoint.x), 500f, Random.Range(_minLimitPoint.y, _maxLimitPoint.y));
                if (Physics.Raycast(castPoint, Vector3.down, out RaycastHit hit, _terrainLayer))
                {
                    Vector3 hitPoint = hit.point;
                    if (hitPoint.y >= _minPalmHeight && hitPoint.y <= _maxPalmHeight)
                    {
                        GameObject newPalm = Instantiate(_palms[Random.Range(0, _palms.Length)], hitPoint - new Vector3(0, 0.1f, 0), Quaternion.Euler(0, Random.Range(0, 360), 0));
                        newPalm.transform.SetParent(_parent);
                        _palmPlanted = true;
                    }
                }
            }
            _palmPlanted = false;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator SpawnOaks()
    {
        while (true)
        {
            bool _oakPlanted = false;
            while (!_oakPlanted)
            {
                Vector3 castPoint = new Vector3(Random.Range(_minLimitPoint.x, _maxLimitPoint.x), 500f, Random.Range(_minLimitPoint.y, _maxLimitPoint.y));
                if (Physics.Raycast(castPoint, Vector3.down, out RaycastHit hit, _terrainLayer))
                {
                    Vector3 hitPoint = hit.point;
                    if (hitPoint.y >= _minOakHeight && hitPoint.y <= _maxOakHeight)
                    {
                        GameObject newOak = Instantiate(_oaks[Random.Range(0, _oaks.Length)], hitPoint - new Vector3(0, 0.1f, 0), Quaternion.Euler(0, Random.Range(0, 360), 0));
                        newOak.transform.SetParent(_parent);
                        _oakPlanted = true;
                    }
                }
            }
            _oakPlanted = false;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator SpawnPines()
    {
        while (true)
        {
            bool _pinePlanted = false;
            while (!_pinePlanted)
            {
                Vector3 castPoint = new Vector3(Random.Range(_minLimitPoint.x, _maxLimitPoint.x), 500f, Random.Range(_minLimitPoint.y, _maxLimitPoint.y));
                if (Physics.Raycast(castPoint, Vector3.down, out RaycastHit hit, _terrainLayer))
                {
                    Vector3 hitPoint = hit.point;
                    if (hitPoint.y >= _minPineHeight && hitPoint.y <= _maxPineHeight)
                    {
                        GameObject newPine = Instantiate(_pines[Random.Range(0, _pines.Length)], hitPoint - new Vector3(0, 0.1f, 0), Quaternion.Euler(0, Random.Range(0, 360), 0));
                        newPine.transform.SetParent(_parent);
                        _pinePlanted = true;
                    }
                }
            }
            _pinePlanted = false;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
