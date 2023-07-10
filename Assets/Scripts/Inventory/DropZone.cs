using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
    public Action OnItemDrop;

    public void OnDrop(PointerEventData eventData)
    {
        OnItemDrop?.Invoke();
    }
}
