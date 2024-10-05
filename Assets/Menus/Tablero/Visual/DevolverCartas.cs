using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

public class DevolverCartas : MonoBehaviour, IDropHandler
{
    public GameObject _item;
    // Start is called before the first frame update
    public void OnDrop(PointerEventData eventData)
    {
        if (_item != null)
        {
            return;
        }
        GameObject newitem = eventData.pointerDrag;
        _item = newitem;
        DragItem compitem = _item.GetComponent<DragItem>();
        _item.transform.SetParent(transform);
        _item.transform.position = transform.position + new Vector3(0, 0, 1);
        compitem.IsDropped(true);
        compitem.SetInGame(false);
    }
}
