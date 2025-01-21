using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITween_ActionClipObject : MonoBehaviour
{
    public UITween_ActionExecution Execution;
    [HideInInspector]
    public bool Started;

    private void Start()
    {
        Setup();
    }
    // Start is called before the first frame update
    public void Setup()
    {
        Execution = new UITween_ActionExecution(this.gameObject);
        Started = true;
    }

    
}
