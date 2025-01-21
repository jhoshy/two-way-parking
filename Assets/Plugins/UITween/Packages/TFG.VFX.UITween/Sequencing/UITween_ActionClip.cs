using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class UITween_ActionClip : PlayableAsset, ITimelineClipAsset
{
    [SerializeField]
    private UITween_ActionClipBehavior template = new UITween_ActionClipBehavior();

    public ClipCaps clipCaps
    {
        get { return ClipCaps.None; }
    }

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {          
        return ScriptPlayable<UITween_ActionClipBehavior>.Create(graph, template);
    }


}
