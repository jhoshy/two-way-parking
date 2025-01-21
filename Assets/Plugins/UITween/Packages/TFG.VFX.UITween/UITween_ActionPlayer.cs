using System.Collections.Generic;
using UnityEngine;


public class UITween_ActionPlayer : MonoBehaviour
{
    UITween_ActionExecution Execution;
    public List<UITween_Action> Actions;

    // Start is called before the first frame update
    void Awake()
    {
        Execution = new UITween_ActionExecution(this.gameObject);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        UpdateActions();
    }

    void UpdateActions()
    {
        foreach (UITween_Action action in Actions)
        {
            Execution.UpdateAction(action);
        }
    }


    public void PlayActionByName(string Name)
    {
        foreach (UITween_Action action in Actions)
        {
            if (action.Name == Name)
            {
                if (action.NewRestTranform) Execution.NewRestTransform(this.gameObject);
                Execution.SetFirstFrame(action);
                action.Play();
            }
        }
    }

    public void PlayActions()
    {
        foreach (UITween_Action action in Actions)
        {
            if (action.NewRestTranform) Execution.NewRestTransform(this.gameObject);
            Execution.SetFirstFrame(action);
            action.Play();
        }
    }

    public void PlayActionByID(int id)
    {
        if (Actions[id].NewRestTranform) Execution.NewRestTransform(this.gameObject);
        Execution.SetFirstFrame(Actions[id]);
        Actions[id].Play();
    }

    public void StopActions()
    {
        foreach (UITween_Action action in Actions)
        {
            action.Stop();
        }
    }

    public void StopActionByName(string Name)
    {
        foreach (UITween_Action action in Actions)
        {
            if (action.Name == Name)
            {
                action.Stop();
            }
        }
    }

    public void StopActionByID(int id)
    {
        Actions[id].Stop();
    }

}
