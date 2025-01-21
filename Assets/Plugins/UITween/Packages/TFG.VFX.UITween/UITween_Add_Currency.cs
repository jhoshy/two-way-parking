using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class UITween_Add_Currency : MonoBehaviour
{
    Text text;
    public bool FromCurrent;
    public int From, To;
    public float Duration;
    public bool ScaleDuration;
    public ParticleSystem.MinMaxCurve Curve;
    public string Prefix, Sufix;

    bool isPlaying;
    float time;
    float normalTime;

    public bool PlayOnEnable;
    public UnityEvent OnStopEvent;
    public bool keepAddedValue;
    
    // Start is called before the first frame update
    void Start()
    {
        text = this.GetComponent<Text>();
        if (FromCurrent) From = int.Parse(text.text);
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying) Add();
    }

    void Add()
    {
        time += Time.deltaTime;
        normalTime = time / Duration;        
        text.text = Prefix + "" + (int)Mathf.Lerp(From, To, Curve.Evaluate(normalTime)) + "" + Sufix;

        if (normalTime >= 1)
        {
            StopToFinal();            
        }
    }

    public void Play()
    {
        time = 0;
        isPlaying = true;
    }

    public void PlayTo(int Value)
    {
        To = Value;
        time = 0;
        isPlaying = true;
    }

    public void PlayAdd(int Value)
    {
        To = From + Value;
        time = 0;
        isPlaying = true;
        keepAddedValue = true;
    }

    public void Stop()
    {
        isPlaying = false;
    }

    public void StopToFinal()
    {
        isPlaying = false;        
        text.text = Prefix + "" + To + "" + Sufix;
        OnStopEvent.Invoke();
        if (keepAddedValue) From = To;
    }

    private void OnEnable()
    {
        if (PlayOnEnable) Play();
    }
}
