using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UITween_PointerHandler : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{

    public UnityEvent OnClick;
    public UnityEvent OnMouseEnter;
    public UnityEvent OnMouseExit;
    public UnityEvent OnMouseDown;
    public UnityEvent OnMouseUp;

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        OnClick.Invoke();
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        OnMouseEnter.Invoke();
    }
    
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        OnMouseExit.Invoke();
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        OnMouseDown.Invoke();
    }
    
    public void OnPointerUp(PointerEventData pointerEventData)
    {
        OnMouseUp.Invoke();
    }

}
