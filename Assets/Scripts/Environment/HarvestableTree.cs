using System.Collections;
using UnityEngine;

public class HarvestableTree : MonoBehaviour
{
    [SerializeField]
    private float _hitsToDestroy = 3f;

    [SerializeField]
    private Transform[] _logDropPoints;
    [SerializeField]
    private ItemDataSO _logData, _stickData;

    public void HitTree()
    {
        _hitsToDestroy--;
        CheckHits();
    }
    private void CheckHits()
    {
        if (_hitsToDestroy <= 0) FallTree();
    }
    private void FallTree()
    {
        Rigidbody _rb = GetComponent<Rigidbody>();
        _rb.constraints = RigidbodyConstraints.None;
        StartCoroutine(nameof(Falling));
    }
    private void SpawnDrop()
    {
        for (int i = 0; i < _logDropPoints.Length; i++)
        {
            Instantiate(_logData.collectablePrefab, _logDropPoints[i].position, _logDropPoints[i].rotation);
        }
    }   
    private IEnumerator Falling()
    {
        yield return new WaitForSeconds(Random.Range(4,5));
        SpawnDrop();
        Destroy(gameObject);
    }
}
