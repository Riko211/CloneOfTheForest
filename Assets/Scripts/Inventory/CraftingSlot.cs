using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Inventory
{
    public class CraftingSlot : MonoBehaviour, IDropHandler
    {
        public Action OnItemDropAction;

        public void OnDrop(PointerEventData eventData)
        {
            OnItemDropAction?.Invoke();
        }
    }
}