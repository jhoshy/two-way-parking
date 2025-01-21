using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UITween_Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    UITween_ActionExecution Execution;

    //public List<UITween_Action> Actions;
    public UITween_Action PointerDown, PointerUp, PointerEnter, PointerExit;

    // Start is called before the first frame update
    void Awake()
    {
        Execution = new UITween_ActionExecution(this.gameObject);
    }

    void LateUpdate()
    {
        if (PointerDown.isPlaying) Execution.UpdateAction(PointerDown);
        if (PointerUp.isPlaying) Execution.UpdateAction(PointerUp);
        if (PointerEnter.isPlaying) Execution.UpdateAction(PointerEnter);
        if (PointerExit.isPlaying) Execution.UpdateAction(PointerExit);
    }


    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        PointerEnter.Play();
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        PointerExit.Play();
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        PointerDown.Play();
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        PointerUp.Play();
    }
}