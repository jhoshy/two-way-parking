using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Events;

[System.Serializable]
//[ExecuteInEditMode]
public class UITween_Action
{

    public string Name;
    public float Delay;
    [FormerlySerializedAs("Duartion")]
    public float Duration;
    public bool Loop;
    public bool Reset;
    public bool Revert = false;
    [HideInInspector]
    public float time=0, delaytime=0;
    [HideInInspector]
    public float actionTime;
    [HideInInspector]
    public bool isPlaying, delayed;
    public UITween_Effect Effect;
    public bool NewRestTranform;
    public Transform targetTransform;
    public UnityEvent OnStart;
    public UnityEvent OnDelay;
    public UnityEvent OnStop;
    public UnityEvent OnClipFinish;
    public int Seed;

    public void Update()
    {
        if (isPlaying)
        {
            if (delayed)
            {
                time += Time.deltaTime;
                actionTime = (time / Duration);
            }
            else
            {
                delaytime += Time.deltaTime;

                if (delaytime > Delay) delayed = true;
            }

            if (actionTime >= 1)
            {
                OnClipFinish.Invoke();

                if (Loop)
                {
                    time = 0;
                }
                else
                {
                    isPlaying = false;
                }

                if (Reset)
                {
                    time = 0;
                    actionTime = 0;
                }
            }
        }
    }

    
    public void Play()
    {
        time = 0;
        OnStart.Invoke();
        NewSeed(Name.Length+10);
        if (actionTime < 1)
        {
            isPlaying = true;
        }
        else
        {
            isPlaying = true;
            time = 0;
            actionTime = 0;
        }
        setDelay();
    }

    public void Play(int seed)
    {
        time = 0;
        OnStart.Invoke();
        NewSeed(seed + 10);
        if (actionTime < 1)
        {
            isPlaying = true;
        }
        else
        {
            isPlaying = true;
            time = 0;
            actionTime = 0;
        }
        setDelay();
    }

    void setDelay()
    {
        if (Delay == 0)
        {
            delayed = true;
            OnDelay.Invoke();
        }
        else
        {
            delayed = false;
            delaytime = 0;

        }

    }

    public void Stop()
    {
        isPlaying = false;
        time = 0;
        OnStop.Invoke();
        if (Reset)
        {
            time = 0;
            setDelay();
        }
    }

    public void Pause()
    {
        isPlaying = false;
    }

    public float GetActionTime()
    {
        //Debug.Log(actionTime);
        return (!Revert) ? actionTime : 1 - actionTime;
    }

    public void NewSeed(int variation)
    {
        Seed = Time.frameCount * variation;
    }

    public float getNormalizedRandom()
    {
        Random.InitState(Seed);
        return Random.Range(0.0f,1.0f);
    }
}
