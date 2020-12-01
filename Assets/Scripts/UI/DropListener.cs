using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropListener : MonoBehaviour, IDropHandler
{
    public Vector3 MyPosition;
    public void OnDrop(PointerEventData eventData){
        if (eventData.pointerDrag != null){
            eventData.pointerDrag.transform.SetParent(transform);
            eventData.pointerDrag.GetComponent<RectTransform>().localPosition = 
            new Vector3(0,-14,0);
            eventData.pointerDrag.GetComponent<DragDrop>().MyCreature.SetPosition(MyPosition);
        }
    }
}
