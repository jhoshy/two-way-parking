using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

[TrackColor(0f,0.8f,0.8f)]
[TrackBindingType(typeof(UITween_ActionClipObject))]
[TrackClipType(typeof(UITween_ActionClip))]
public class UITween_ActionTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<UITween_ActionClipMixerBehavior>.Create(graph, inputCount);
    }
}
