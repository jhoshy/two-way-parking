using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class UITween_ActionClipMixerBehavior : PlayableBehaviour
{
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        UITween_ActionClipObject trackBinding = playerData as UITween_ActionClipObject;

        if (!trackBinding)
            return;

        int inputCount = playable.GetInputCount(); //get the number of all clips on this track

        int Sum=0;

        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            ScriptPlayable<UITween_ActionClipBehavior> inputPlayable = (ScriptPlayable<UITween_ActionClipBehavior>)playable.GetInput(i);
            UITween_ActionClipBehavior input = inputPlayable.GetBehaviour();

            if (inputWeight > 0)
            {                
                if (input.DeactiveClip)
                {
                    if (trackBinding.gameObject.activeSelf)
                        trackBinding.gameObject.SetActive(false);
                }
                else
                {
                    if (!trackBinding.gameObject.activeSelf)
                        trackBinding.gameObject.SetActive(true);
                }
                Sum++;
            }
        }

        if (Sum == 0)
        {
            if(trackBinding.Execution == null)
            {
                trackBinding.Setup();
            }
            trackBinding.Execution.ResetToRest();
        }

        //assign the result to the bound object
        //trackBinding.intensity = finalIntensity;
        //trackBinding.color = finalColor;
    }
}
