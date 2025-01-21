using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;



public class UITween_CurrenciesToTarget : MonoBehaviour
{
    public Sprite _Sprite;
    public GameObject _ParticlePrefab;
    public Vector2 _Range;
    public float _Thickness;
    public float _Delay, _Duration, _CurentTime;
    public bool _Loop;
    public bool _PlayOnAwake;
    public bool _ParentToCanvas;


    public Vector2 _StartSize;
    public ParticleSystem.MinMaxCurve _Rate, _StartLife, _StartScale, _StartRotation;
    public ParticleSystem.MinMaxGradient _StartColor;

    public ParticleSystem.MinMaxCurve _RotationOverLifeTime, _ScaleOverLifeTime;

    //Move to target
    [Header("Currencies Behavior")]
    public ParticleSystem.MinMaxCurve _MoveCurve;
    public ParticleSystem.MinMaxCurve _MoveXOffset;
    public ParticleSystem.MinMaxCurve _MoveYOffset;
    public Vector2 _Noise;
    public ParticleSystem.MinMaxCurve _NoiseOverLife;

    public GameObject _Target;
    public float _DelayToTarget;
    Vector3 _AuxPos;

    bool IsPlaying;

    public Burst[] _Bursts;

    //Pool Events
    List<UITween_ParticleBehavior> Pool;
    bool FirsHit, LastHit;
    public UnityEvent _OnFirstHit;
    public UnityEvent _OnLasttHit;
    public UnityEvent _OnHit;

    readonly HashSet<UITween_ParticleBehavior> particlesHash = new HashSet<UITween_ParticleBehavior>();

    [System.Serializable]
    public struct Burst
    {
        public float time;
        public ParticleSystem.MinMaxCurve count;
        public int repete;
        public float interval;
    }

    // Start is called before the first frame update
    void Start()
    {
        Pool = new List<UITween_ParticleBehavior>();
        if (_PlayOnAwake) Play();
    }

    public void Play()
    {
        if (Pool == null)
        {
            Pool = new List<UITween_ParticleBehavior>();
        }
        
        Pool.Clear();
        _CurentTime = 0;
        IsPlaying = true;
        FirsHit = LastHit = false;
        StartCoroutine(Timer());
        StartCoroutine(Spawn());
        StartBursts();
    }

    public void Stop()
    {
        _CurentTime = 0;
        IsPlaying = false;
        CleanAll();
    }

    public void CleanAll()
    {
        foreach (var par in Pool)
        {
            Destroy(par.gameObject);
        }
        FirsHit = LastHit = false;
        Pool.Clear();
    }

    public void UpdateBursts(int repeat)
    {
        for (int i = 0; i < _Bursts.Length; ++i)
        {
            _Bursts[i].repete = repeat;
        }
    }

    IEnumerator Timer()
    {
        while (_CurentTime <= _Duration)
        {

            //Pool Events
            PoolCheckingEvents();

            if (_CurentTime > _Delay)
            {
                foreach (UITween_ParticleBehavior particle in Pool)
                {
                    MoveToTarget(particle);
                    Rotate(particle);
                    Scale(particle);
                }
            }

            _CurentTime += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        if (_Loop) Play();
        else Stop();
    }    

    //-----Spawn
    IEnumerator Spawn()
    {
        while (true)
        {
            if (IsPlaying && _CurentTime > _Delay)
            {
                SpawnParticle();
            }
            if (_Rate.Evaluate(_CurentTime) > 0)
                yield return new WaitForSeconds(1.0f / _Rate.Evaluate(_CurentTime));
            else
                break;
        }
    }

    //-----CheckingPool
    void PoolCheckingEvents()
    {
        foreach(UITween_ParticleBehavior aux in Pool)
        {
            if (!FirsHit) //OnFirstHit
            {
                if(aux.GetNormalTimer() >= 1)
                {
                    _OnFirstHit.Invoke();
                    FirsHit = true;
                }
            }            
        }

        if (!LastHit && FirsHit) //OnLastHit
        {
            if (Pool.Count == 0)
            {
                _OnLasttHit.Invoke();
                LastHit = true;
            }
        }
    }

    void StartBursts()
    {
        foreach (Burst burst in _Bursts)
        {
            for (int i = 0; i < burst.repete; i++)
            {
                StartCoroutine(SpawnBursts(burst, i));
            }
        }
    }

    IEnumerator SpawnBursts(Burst burst, int iteration)
    {        
        yield return new WaitForSeconds(burst.time + _Delay + burst.interval * iteration);
        for (int i = 0; i < burst.count.Evaluate(_CurentTime/_Duration); i++)
        {
            SpawnParticle();
        }
    }


    void SpawnParticle()
    {
        GameObject particle;
        Image img;
        if (_ParticlePrefab == null)
        {
            particle = new GameObject("UI_Particle");
            img = particle.AddComponent<Image>();            
        }
        else
        {
            particle = (GameObject) Instantiate(_ParticlePrefab,this.transform.position,_ParticlePrefab.transform.rotation);
            img = particle.GetComponent<Image>();
        }

        SetPosition(particle);
        SetRotation(particle);
        
        particle.transform.SetParent(this.transform);
        particle.transform.localScale = Vector3.one;
        UITween_ParticleBehavior behav = particle.AddComponent<UITween_ParticleBehavior>();
        img.sprite = _Sprite;
        img.color = _StartColor.Evaluate(_CurentTime / _Duration, behav.RandomValue());
        behav.StartParticle(_StartLife.Evaluate(_CurentTime/_Duration, behav.RandomValue()), _StartSize, img, ref Pool);
        behav.StartAltTime(_DelayToTarget);
        SetScale(particle);
        if(_ParentToCanvas) particle.transform.SetParent(particle.transform.root);
    }


    //-----Transforms
    void MoveToTarget(UITween_ParticleBehavior particle)
    {
        _AuxPos = Vector3.LerpUnclamped(particle.GetOrigin(), _Target.transform.position, _MoveCurve.Evaluate(particle.GetNormalAltTimer(),particle.RandomValue()));
        _AuxPos.x += _MoveXOffset.Evaluate(particle.GetNormalAltTimer()) + GetNoise(particle) * _Noise.x;        
        _AuxPos.y += _MoveYOffset.Evaluate(particle.GetNormalAltTimer()) + GetNoise(particle) * _Noise.y;
        particle.transform.position = _AuxPos;

        var distance = Vector3.Distance(particle.transform.position, _Target.transform.position);
        if (distance < 0.1f && !particlesHash.Contains(particle))
        {
            _OnHit.Invoke();
            particlesHash.Add(particle);
        }
    }

    float GetNoise(UITween_ParticleBehavior particle)
    {
        return (Mathf.PerlinNoise(Time.time , particle.gameObject.GetInstanceID())*2 - 1 ) * _NoiseOverLife.Evaluate(particle.GetNormalTimer());
    }

    void SetPosition(GameObject particle)
    {
        Vector3 pos = this.transform.position;
        Random.InitState(Time.frameCount + particle.gameObject.GetInstanceID());
        pos.x += Random.Range(-1.0f, 1.0f) * _Range.x;
        pos.y += Random.Range(-1.0f, 1.0f) * _Range.y;
        particle.transform.position = pos;
    }

    void SetScale(GameObject particle)
    {
        particle.transform.localScale = Vector3.one * _ScaleOverLifeTime.Evaluate(0);
    }

    void SetRotation(GameObject particle)
    {
        particle.transform.eulerAngles = Vector3.forward * Random.Range(_StartRotation.constantMin, _StartRotation.constantMax);
    }

    void Rotate(UITween_ParticleBehavior particle)
    {
        particle.transform.Rotate(Vector3.forward * _RotationOverLifeTime.Evaluate(particle.GetNormalTimer()));
    }

    void Scale(UITween_ParticleBehavior particle)
    {
        particle.transform.localScale = Vector3.one * _ScaleOverLifeTime.Evaluate(particle.GetNormalTimer());
    }



}
