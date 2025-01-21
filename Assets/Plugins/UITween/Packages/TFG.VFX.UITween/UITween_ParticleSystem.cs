using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UITween_ParticleSystem : MonoBehaviour
{
    Sprite _Sprite;
    Vector2 _Range;
    [SerializeField] float _Duration, _CurentTime;
    ParticleSystem.MinMaxCurve _Rate, _StartSize, _StartVelocity, StartRotation;
    ParticleSystem.MinMaxGradient _StartColor;

    ParticleSystem.MinMaxCurve _RotationOverLifeTime;

    //Move to target
    ParticleSystem.MinMaxCurve _MoveCurve, _MoveXOffset, _MoveYOffset;
    GameObject _Target;
    Vector3 _AuxPos;

    List<UITween_ParticleBehavior> pool;

    // Start is called before the first frame update
    void Start()
    {
        pool = new List<UITween_ParticleBehavior>();
        StartCoroutine(Timer());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Timer()
    {
        while (_CurentTime<_Duration)
        {
            _CurentTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        } 
    }

    //transforms
    void MoveToTarget(GameObject particle, Vector3 origin)
    {
        _AuxPos = Vector3.LerpUnclamped(origin, _Target.transform.position,_MoveCurve.Evaluate(_CurentTime));
        _AuxPos.x += _MoveXOffset.Evaluate(_CurentTime);
        _AuxPos.y += _MoveYOffset.Evaluate(_CurentTime);

        particle.transform.position = _AuxPos;
    }

}
