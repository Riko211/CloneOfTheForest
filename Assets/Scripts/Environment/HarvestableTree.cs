using System.Collections;
using UnityEngine;

namespace Environment
{
    public class HarvestableTree : MonoBehaviour
    {
        [SerializeField]
        private float _hitsToDestroy = 3f;

        [SerializeField]
        private Transform[] _logDropPoints;
        [SerializeField]
        private ItemDataSO _logData, _stickData, _saplingData;
        [SerializeField]
        private Vector2 _saplingsDropCount = new Vector2(1, 2);

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
            int saplingsDrop = Random.Range((int)_saplingsDropCount.x, (int)_saplingsDropCount.y + 1);
            for (int i = 0; i < saplingsDrop; i++)
            {
                Instantiate(_saplingData.collectablePrefab, _logDropPoints[_logDropPoints.Length - 1].position + Vector3.up, Quaternion.identity);
            }
        }
        private IEnumerator Falling()
        {
            yield return new WaitForSeconds(Random.Range(4, 5));
            SpawnDrop();
            Destroy(gameObject);
        }
    }
}