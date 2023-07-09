using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Inventory
{
    public class CraftingSlot : MonoBehaviour, IDropHandler
    {
        public Action OnItemDrop;

        public void OnDrop(PointerEventData eventData)
        {
            OnItemDrop?.Invoke();
        }
    }
}