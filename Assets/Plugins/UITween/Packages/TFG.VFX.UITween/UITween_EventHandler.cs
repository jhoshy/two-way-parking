using UnityEngine;
using UnityEngine.Events;

public class UITween_EventHandler : MonoBehaviour
{
    public UnityEvent onStart;
    public UnityEvent onAwake;
    public UnityEvent onEnable;
    public UnityEvent onDisable;
    //public UnityEvent on;

    void Start()
    {
        onStart.Invoke();
    }

    private void Awake()
    {
        onAwake.Invoke();
    }

    private void OnEnable()
    {
        onEnable.Invoke();
    }

    private void OnDisable()
    {
        onDisable.Invoke();
    }

}
