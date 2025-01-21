using UnityEngine;

public class UITween_OnEvent : MonoBehaviour
{
    public UITween_Action onEnable, onAwake, onStart;
    UITween_ActionExecution Execution;

    // Start is called before the first frame update
    void Start()
    {
        

        if (onStart.Effect != null)
        {
            onStart.Play((int)transform.position.magnitude);
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
       Execution.UpdateAction(onEnable);
       Execution.UpdateAction(onAwake);
       Execution.UpdateAction(onStart);
    }

    private void OnEnable()
    {
        if (onEnable.Effect != null)
        {
            onEnable.Play((int)transform.position.magnitude);
        }
    }

    private void Awake()
    {
        Execution = new UITween_ActionExecution(this.gameObject);
        if (onAwake.Effect != null)
        {
            onAwake.Play((int)transform.position.magnitude);
        }
    }
}