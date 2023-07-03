using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    private bool _stopSpawnTrees = false;

    private int _treesSpawned = 0;


    [MenuItem("Examples/Instantiate trees")]
    static void InstantiatePrefab()
    {
        int treesToPlant = 10000;

        GameObject[] palms = new GameObject[4];
        palms[0] = Resources.Load<GameObject>("Prefabs/Trees/Palms/Palm4");
        palms[1] = Resources.Load<GameObject>("Prefabs/Trees/Palms/Palm5");
        palms[2] = Resources.Load<GameObject>("Prefabs/Trees/Palms/Palm6");
        palms[3] = Resources.Load<GameObject>("Prefabs/Trees/Palms/Palm7");

        while (treesToPlant>0)
        {
            Vector3 castPoint = new Vector3(Random.Range(0f, 2500f), 500f, Random.Range(0f, 2500f));
            if (Physics.Raycast(castPoint, Vector3.down, out RaycastHit hit))
            {
                Vector3 hitPoint = hit.point;

                if (hitPoint.y >= 1.5f && hitPoint.y <= 10f)
                {
                    Object SpawnedTree = PrefabUtility.InstantiatePrefab(palms[Random.Range(0, palms.Length)]);
                    GameObject go = SpawnedTree as GameObject;
                    go.transform.position = hitPoint - new Vector3(0, 0.1f, 0);
                    go.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                    go.transform.parent = GameObject.Find("NewTrees").transform;
                    treesToPlant--;
                }
            }
        }

    }

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

        if (_stopSpawnTrees == true)
        {
            StopAllCoroutines();
            _stopSpawnTrees = false;
            Debug.Log("Spawned trees: " + _treesSpawned.ToString());
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
                        _treesSpawned++;
                    }
                }
            }
            _palmPlanted = false;
            yield return new WaitForSeconds(0.01f);
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
                        _treesSpawned++;
                    }
                }
            }
            _oakPlanted = false;
            yield return new WaitForSeconds(0.01f);
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
                        _treesSpawned++;
                    }
                }
            }
            _pinePlanted = false;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
