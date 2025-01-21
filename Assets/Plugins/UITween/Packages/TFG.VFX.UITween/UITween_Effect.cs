using UnityEngine;


[CreateAssetMenu(fileName = "UITween_Effect", menuName = "TFG/UI Tween Effect", order = 1)]
public class UITween_Effect : ScriptableObject
{
    //Swipe
    public bool Swipe = false;
    public enum SwipeFrom {Top, Botton, Right, Left};
    public SwipeFrom swipeFrom = 0;
    public ParticleSystem.MinMaxCurve SwipeCurve = new ParticleSystem.MinMaxCurve();

    //Move
    public bool Move = false;
    public Vector3 OffSetPosition;
    public bool XYMove = false;
    public ParticleSystem.MinMaxCurve MoveCurve_X = new ParticleSystem.MinMaxCurve();
    public ParticleSystem.MinMaxCurve MoveCurve_Y = new ParticleSystem.MinMaxCurve();

    //Scale
    public bool Scale = false;
    public float ScaleMultiplier;
    public bool XYScale;
    public ParticleSystem.MinMaxCurve ScaleCurve_X = new ParticleSystem.MinMaxCurve();
    public ParticleSystem.MinMaxCurve ScaleCurve_Y = new ParticleSystem.MinMaxCurve();

    //Rotation
    public bool Rotate = false;
    public Vector3 EulerAngles;
    public bool XYZRotation = false;
    public ParticleSystem.MinMaxCurve RotationCurve_X = new ParticleSystem.MinMaxCurve();
    public ParticleSystem.MinMaxCurve RotationCurve_Y = new ParticleSystem.MinMaxCurve();
    public ParticleSystem.MinMaxCurve RotationCurve_Z = new ParticleSystem.MinMaxCurve();

    //MoveTowards
    public bool MoveTowards = false;
    public ParticleSystem.MinMaxCurve OffsetMoveTowardsCurve_X = new ParticleSystem.MinMaxCurve();
    public ParticleSystem.MinMaxCurve OffsetMoveTowardsCurve_Y = new ParticleSystem.MinMaxCurve();
    public bool XYMoveTowards = false;
    public ParticleSystem.MinMaxCurve MoveTowardsCurve_X = new ParticleSystem.MinMaxCurve();
    public ParticleSystem.MinMaxCurve MoveTowardsCurve_Y = new ParticleSystem.MinMaxCurve();

    //Recolor
    public bool Recolor = false;
    public ParticleSystem.MinMaxGradient RecolorGradient = new ParticleSystem.MinMaxGradient();     
    public enum BlendMode {Switch, Mix, Add, Multiply, AlphaOnly};
    public BlendMode RecolorBlend = 0;

    //Canvas Group Recolor
    public bool CanvasAlpha = false;
    public ParticleSystem.MinMaxCurve CanvasAlphaCurve = new ParticleSystem.MinMaxCurve();
}
