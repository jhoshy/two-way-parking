using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class UITween_ActionClipBehavior : PlayableBehaviour
{
    public bool DeactiveClip, LastFrameClip;
    [SerializeField] public UITween_Action Action;
    [SerializeField] public ExposedReference<Transform> MoveTo;

    UITween_ActionClipObject Obj;
    UITween_ActionExecution Execution;

    public override void OnGraphStart(Playable playable)
    {
        Action.targetTransform = MoveTo.Resolve(playable.GetGraph().GetResolver());
        base.OnGraphStart(playable);
    }


    public override void ProcessFrame(Playable thisPlayable, FrameData info, object playerData)
    {
        if (playerData == null) return;

        if (!Action.NewRestTranform)
        {
            if (Obj == null)
            {
                Obj = playerData as UITween_ActionClipObject;

                if (Obj.Execution == null)
                {
                    Obj.Setup();
                }

                if (Action.Effect != null)
                {
                    if (!Obj.Started)
                    {
                        if (Action.Effect.MoveTowards)
                        {
                            Action.targetTransform = MoveTo.Resolve(thisPlayable.GetGraph().GetResolver());
                        }

                        Obj.Setup();
                    }
                }
            }

            if (Action.Effect != null)
            {
                if (LastFrameClip)
                {
                    Obj.Execution.UpdateActionClip(Action, 1, 1, Action.Duration);
                }
                else
                {
                    Obj.Execution.UpdateActionClip(Action, (float)thisPlayable.GetTime<Playable>(), (float)thisPlayable.GetDuration<Playable>(), Action.Duration);
                }
            }
        }
        else
        {
            if (Execution == null)
            {
                if (Action.Effect.MoveTowards)
                {
                    Action.targetTransform = MoveTo.Resolve(thisPlayable.GetGraph().GetResolver());
                }

                Obj = playerData as UITween_ActionClipObject;
                Execution = new UITween_ActionExecution(Obj.gameObject);
            }

            if (Action.Effect != null)
            {
                Execution.UpdateActionClip(Action, (float)thisPlayable.GetTime<Playable>(), (float)thisPlayable.GetDuration<Playable>(), Action.Duration);
            }
        }
    }

    

    public override void OnPlayableDestroy(Playable playable)
    {
        try
        {
            Obj.Execution.ResetToRest();
            Obj.Started = false;
        }
        catch
        {

        }
    }
}
