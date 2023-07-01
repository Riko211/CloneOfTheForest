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
        [SerializeField]
        private Transform _treeCheckPoint;
        [SerializeField]
        private LayerMask _harvestableLayer;

        private InputSystem _inputSystem;
        private WaitForSeconds _waitForFixedTime = new WaitForSeconds(0.02f);
        private Vector3 _previousTreeCheckPoint;

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
            if (!_animation.isPlaying)
            {
                _previousTreeCheckPoint = _treeCheckPoint.position;
                _animation.Play();
                StartCoroutine(nameof(CheckForTree));
            }
        }

        private IEnumerator CheckForTree()
        {
            while (true)
            {
                Physics.Linecast(_previousTreeCheckPoint, _treeCheckPoint.position, out RaycastHit hitInfo, _harvestableLayer);
                if (hitInfo.collider != null)
                {
                    HarvestableTree tree = hitInfo.collider.transform.GetComponent<HarvestableTree>();
                    if (tree != null)
                    {
                        tree.HitTree();
                        StopCoroutine(nameof(CheckForTree));
                    }
                }
                _previousTreeCheckPoint = _treeCheckPoint.position;
                yield return _waitForFixedTime;
            }
        }

        private void EndOfAnimation()
        {
            StopCoroutine(nameof(CheckForTree));
        }
    }
}