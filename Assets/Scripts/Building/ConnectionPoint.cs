using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Building
{
    public class ConnectionPoint : MonoBehaviour
    {
        [SerializeField]
        private ItemDataSO.ConstructionType _constructionType;

        public ItemDataSO.ConstructionType GetPointType()
        {
            return _constructionType;
        }
    }
}