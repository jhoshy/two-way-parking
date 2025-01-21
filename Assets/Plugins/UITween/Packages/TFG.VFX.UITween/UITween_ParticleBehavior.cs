using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UITween_ParticleBehavior : MonoBehaviour
{
    float lifetime, timer=0, alttimer=0, normaltimer, normalalttimer, altlifetime;
    Image image;
    Vector3 origin;
    List<UITween_ParticleBehavior> Pool;

    IEnumerator Timer()
    {
        while (timer < lifetime)
        {
            timer += Time.deltaTime;
            normaltimer = timer / lifetime;
            yield return new WaitForEndOfFrame();
        }

        Pool.Remove(this);
        Destroy(this.gameObject);
    }

    IEnumerator AltTimer()
    {
        while (timer < lifetime)
        {
            alttimer += Time.deltaTime;
            normalalttimer = alttimer / altlifetime;
            yield return new WaitForEndOfFrame();
        }
    }

    public void StartParticle(float lifetime, Vector2 startSize, Image image, ref List<UITween_ParticleBehavior> Pool)
    {
        timer = 0;
        this.origin = this.transform.position;

        this.image = image;
        image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, startSize.x);
        image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, startSize.y);

        this.lifetime = lifetime;
        this.Pool = Pool;
        this.Pool.Add(this);
        StartCoroutine(Timer());
    }

    public float GetTimer()
    {
        return timer;
    }

    public float GetNormalTimer()
    {
        return normaltimer;
    }

    public float GetNormalAltTimer()
    {
        return normalalttimer;
    }

    public Vector3 GetOrigin()
    {
        return origin;
    }

    public float RandomValue()
    {
        Random.InitState(gameObject.GetInstanceID()*100);
        return Random.Range(0.0f, 1.0f);
    }

    public void StartAltTime()
    {
        altlifetime = lifetime - timer;
        StartCoroutine(AltTimer());
    }

    public void StartAltTime(float delay)
    {
        Invoke("StartAltTime", delay);
    }

    public float GetLifeTime()
    {
        return lifetime;
    }

    public float GetAltLifeTime()
    {
        return altlifetime;
    }
}
