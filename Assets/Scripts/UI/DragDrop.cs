using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    RectTransform _rectTransform;
    CanvasGroup _canvasGroup;
    public CreatureLogic MyCreature;

    void Start(){
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData){
        _canvasGroup.blocksRaycasts = false;
    }
    public void OnDrag(PointerEventData eventData){
        _rectTransform.anchoredPosition += eventData.delta;
    }
    public void OnEndDrag(PointerEventData eventData){
        _canvasGroup.alpha = 1f;
        _canvasGroup.blocksRaycasts = true;
    }
    public void OnPointerDown(PointerEventData eventData){
    }
}
