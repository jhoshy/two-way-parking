using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


[ExecuteInEditMode]
public class UITween_FlipBook : MonoBehaviour
{
    public bool PlayOnAwake;
    public Vector2 Tiles;
    public enum SheetMode { WholeSheet, SingleRow };
    public SheetMode Mode;
    public bool RandomRow = true;
    public ParticleSystem.MinMaxCurve StartRow;
    public float Duration;
    float time;
    public ParticleSystem.MinMaxCurve FrameOverTime;
    public int StartFrame;
    public int Cycles = 1;
    public bool Loop;
    Image image;
    int seed;
    public bool IsPlaying;

    public UnityEvent OnStart;
    public UnityEvent OnStop;
    public UnityEvent OnFinish;
    public UnityEvent OnLoop;

    // Start is called before the first frame update
    void Start()
    {
        image = this.GetComponent<Image>();
        if (PlayOnAwake) Play();
    }

    void Update()
    {
    }  

    void ExecuteWholeSheet()
    {
        int index = (int)(FrameOverTime.Evaluate(time/Duration)*Tiles.x*Tiles.y) + StartFrame;
        Vector2 size = new Vector2(1.0f / Tiles.x, 1.0f / Tiles.y);

        float vIndex = Mathf.FloorToInt((float)index / Tiles.x);
        float uIndex = index - vIndex * Tiles.x;
        
        Vector2 offset = new Vector2(uIndex * size.x, 1.0f - size.y - vIndex * size.y);

        image.material.SetTextureOffset("_MainTex", offset);
        image.material.SetTextureScale("_MainTex", size);
    }

    void ExecuteSingleRow()
    {
        int row = 0;

        if (RandomRow)
        {
            Random.InitState(seed);
            row = (int)Random.Range(0,Tiles.y);
        }
        else
        {
            if (StartRow.mode == ParticleSystemCurveMode.Constant)
            {
                row = (int)StartRow.constant;
            }
            else
            {
                Random.InitState(seed);
                row = (int)Random.Range(StartRow.constantMin, StartRow.constantMax);
            }
        }

        int index = (int)((FrameOverTime.Evaluate(time / Duration) * Tiles.x) + StartFrame);

        Vector2 size = new Vector2(1.0f / Tiles.x, 1.0f / Tiles.y);

        float vIndex = Mathf.FloorToInt((float)(index+ (row * Tiles.x ))/ Tiles.x);
        float uIndex = (index - vIndex * Tiles.x) ;
        
        Vector2 offset = new Vector2(uIndex * size.x, 1.0f - size.y - vIndex * size.y);

        image.material.SetTextureOffset("_MainTex", offset);
        image.material.SetTextureScale("_MainTex", size);
    }

    IEnumerator Execute()
    {
        time = 0;
        while (time <= Duration)
        {
            time += Time.deltaTime;
            if (time > Duration)
            {
                if (Loop)
                {
                    SetSeed();
                    OnLoop.Invoke();
                    time = 0;
                }
            }

            if (!IsPlaying)
            {
                IsPlaying = true;
            }

            switch (Mode)
            {
                case SheetMode.SingleRow:
                    ExecuteSingleRow();
                    break;

                case SheetMode.WholeSheet:
                    ExecuteWholeSheet();
                    break;
            }

            yield return new WaitForEndOfFrame();
        }
        time = 0;
        IsPlaying = false;
        OnFinish.Invoke();
    }


    public void Play()
    {
        OnStart.Invoke();
        StartCoroutine(Execute());
        SetSeed();
    }

    public void Stop()
    {
        StopCoroutine(Execute());
        StopAllCoroutines();
        time = 0;
        IsPlaying = false;
    }

    public void SetSeed()
    {
        seed = (int)(Time.frameCount * transform.position.magnitude);
    }
}
