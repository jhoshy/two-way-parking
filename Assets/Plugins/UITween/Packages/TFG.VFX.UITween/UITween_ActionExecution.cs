using UnityEngine;
using UnityEngine.UI;
using TMPro;

//[ExecuteInEditMode]
public class UITween_ActionExecution
{
    GameObject TargetObject;
    Vector3 Position, LocalPosition, Rotation, Scale;
    Vector3 auxPosition, auxLocalPosition, auxRotation, auxScale;
    Image image;
    RawImage rawimage;
    Text text;
    TextMeshProUGUI textM;
    Color color, auxcolor;
    RectTransform rect;
    CanvasGroup canvasGroup;
    float canvasAlpha, auxCanvasAlpha;

    public UITween_ActionExecution(GameObject TargetObject)
    {
        this.TargetObject = TargetObject;
        Position = TargetObject.transform.position;
        LocalPosition = TargetObject.transform.localPosition;
        Rotation = TargetObject.transform.eulerAngles;
        Scale = TargetObject.transform.localScale;
        rect = TargetObject.GetComponent<RectTransform>();
        image = TargetObject.GetComponent<Image>();
        rawimage = TargetObject.GetComponent<RawImage>();
        text = TargetObject.GetComponent<Text>();
        textM = TargetObject.GetComponent<TextMeshProUGUI>();
        canvasGroup = TargetObject.GetComponent<CanvasGroup>();

        if (image != null)
            color = image.color;

        if (rawimage != null)
            color = rawimage.color;

        if (text != null)
            color = text.color;

        if (textM != null)
            color = textM.color;

        if (canvasGroup != null)
            canvasAlpha = canvasGroup.alpha;
    }

    public void NewRestTransform(GameObject TargetObject)
    {
        this.TargetObject = TargetObject;
        Position = TargetObject.transform.position;
        LocalPosition = TargetObject.transform.localPosition;
        Rotation = TargetObject.transform.eulerAngles;
        Scale = TargetObject.transform.localScale;
        rect = TargetObject.GetComponent<RectTransform>();
        image = TargetObject.GetComponent<Image>();
        rawimage = TargetObject.GetComponent<RawImage>();
        text = TargetObject.GetComponent<Text>();
        textM = TargetObject.GetComponent<TextMeshProUGUI>();
        canvasGroup = TargetObject.GetComponent<CanvasGroup>();

        if (image != null)
            color = image.color;

        if (rawimage != null)
            color = rawimage.color;

        if (text != null)
            color = text.color;

        if (textM != null)
            color = textM.color;

        if (canvasGroup != null)
            canvasAlpha = canvasGroup.alpha;
    }

    public void ResetToRest()
    {
        TargetObject.transform.position = Position;
        TargetObject.transform.localPosition = LocalPosition;
        TargetObject.transform.eulerAngles = Rotation;
        TargetObject.transform.localScale = Scale;

        if (image != null)
            image.color = color;

        if (rawimage != null)
            rawimage.color = color;

        if (text != null)
            text.color = color;

        if (textM != null)
            color = textM.color;

        if (canvasGroup != null)
            canvasGroup.alpha = canvasAlpha;
    }


    public void UpdateAction(UITween_Action action)
    {
        if (action.isPlaying)
        {
            if (action.Effect != null && action.Duration > 0)
            {
                action.Update();
                if (action.Effect.Scale) ActionScale(action);
                if (action.Effect.Move) ActionMove(action);
                if (action.Effect.MoveTowards) ActionMoveTowards(action);
                if (action.Effect.Rotate) ActionRotation(action);
                if (action.Effect.Swipe) ActionSwipe(action);
                if (action.Effect.Recolor) ActionRecolor(action);
                if (action.Effect.CanvasAlpha) ActionCanvasAlpha(action);
            }
        }
    }

    public void UpdateActionClip(UITween_Action action, float time, float duration, float repeat)
    {
        if (action.Effect != null)
        {
            if (action.Loop)
            {
                float c = time / (duration / repeat);
                c = c - Mathf.Floor(c);
                action.actionTime = c;
            }
            else
            {
                action.actionTime = time / duration;
            }
            if (action.Effect.Scale) ActionScale(action);
            if (action.Effect.Move) ActionMove(action);
            if (action.Effect.MoveTowards) ActionMoveTowards(action);
            if (action.Effect.Rotate) ActionRotation(action);
            if (action.Effect.Swipe) ActionSwipe(action);
            if (action.Effect.Recolor) ActionRecolor(action);
            if (action.Effect.CanvasAlpha) ActionCanvasAlpha(action);
        }
    }

    public void SetFirstFrame(UITween_Action action)
    {
        if (action.Effect != null && action.Duration > 0)
        {
            if (action.Effect.Scale) ActionScale(action);
            if (action.Effect.Move) ActionMove(action);
            if (action.Effect.MoveTowards) ActionMoveTowards(action);
            if (action.Effect.Rotate) ActionRotation(action);
            if (action.Effect.Swipe) ActionSwipe(action);
            if (action.Effect.Recolor) ActionRecolor(action);
            if (action.Effect.CanvasAlpha) ActionCanvasAlpha(action);
        }
    }

    //-------------MOVE----------------
    void ActionMove(UITween_Action Action)
    {
        if (!Action.Effect.XYMove)
        {
            auxLocalPosition = Vector3.LerpUnclamped(LocalPosition, LocalPosition + Action.Effect.OffSetPosition, Action.Effect.MoveCurve_X.Evaluate(Action.GetActionTime(), Action.getNormalizedRandom()));
        }
        else
        {
            auxLocalPosition.x = Mathf.LerpUnclamped(LocalPosition.x, LocalPosition.x + Action.Effect.OffSetPosition.x, Action.Effect.MoveCurve_X.Evaluate(Action.GetActionTime(), Action.getNormalizedRandom()));
            auxLocalPosition.y = Mathf.LerpUnclamped(LocalPosition.y, LocalPosition.y + Action.Effect.OffSetPosition.y, Action.Effect.MoveCurve_Y.Evaluate(Action.GetActionTime(), Action.getNormalizedRandom()));
        }

        TargetObject.transform.localPosition = auxLocalPosition;
    }


    //----------SCALE------------
    void ActionScale(UITween_Action Action)
    {
        auxScale = Vector3.one;
        if (!Action.Effect.XYScale)
        {
            TargetObject.transform.localScale = Scale * Action.Effect.ScaleCurve_X.Evaluate(Action.GetActionTime(), Action.getNormalizedRandom()) * Action.Effect.ScaleMultiplier;
        }
        else
        {
            auxScale.x = Scale.x * Action.Effect.ScaleCurve_X.Evaluate(Action.GetActionTime(), Action.getNormalizedRandom()) * Action.Effect.ScaleMultiplier;
            auxScale.y = Scale.y * Action.Effect.ScaleCurve_Y.Evaluate(Action.GetActionTime(), Action.getNormalizedRandom()) * Action.Effect.ScaleMultiplier;
            TargetObject.transform.localScale = auxScale;
        }
    }

    //----------ROTATION------------
    void ActionRotation(UITween_Action Action)
    {
        if (!Action.Effect.XYZRotation)
        {
            auxRotation = Vector3.LerpUnclamped(Rotation, Action.Effect.EulerAngles, Action.Effect.RotationCurve_X.Evaluate(Action.GetActionTime(), Action.getNormalizedRandom()));
        }
        else
        {
            auxRotation.x = Mathf.LerpUnclamped(Rotation.x, Action.Effect.EulerAngles.x, Action.Effect.RotationCurve_X.Evaluate(Action.GetActionTime(), Action.getNormalizedRandom()));
            auxRotation.y = Mathf.LerpUnclamped(Rotation.y, Action.Effect.EulerAngles.y, Action.Effect.RotationCurve_Z.Evaluate(Action.GetActionTime(), Action.getNormalizedRandom()));
            auxRotation.z = Mathf.LerpUnclamped(Rotation.z, Action.Effect.EulerAngles.z, Action.Effect.RotationCurve_Y.Evaluate(Action.GetActionTime(), Action.getNormalizedRandom()));
        }

        TargetObject.transform.localEulerAngles = auxRotation;
    }

    //-------------MOVE TOWARDS----------------
    void ActionMoveTowards(UITween_Action Action)
    {
        if (!Action.Effect.XYMoveTowards)
        {
            auxPosition = Vector3.LerpUnclamped(Position, Action.targetTransform.position, Action.Effect.MoveTowardsCurve_X.Evaluate(Action.GetActionTime(), Action.getNormalizedRandom()));
            auxPosition.x += Action.Effect.OffsetMoveTowardsCurve_X.Evaluate(Action.GetActionTime());
            auxPosition.y += Action.Effect.OffsetMoveTowardsCurve_Y.Evaluate(Action.GetActionTime());
            TargetObject.transform.position = auxPosition;
        }
        else
        {
            auxPosition.x = Mathf.LerpUnclamped(Position.x, Action.targetTransform.position.x, Action.Effect.MoveTowardsCurve_X.Evaluate(Action.GetActionTime(), Action.getNormalizedRandom()));
            auxPosition.y = Mathf.LerpUnclamped(Position.y, Action.targetTransform.position.y, Action.Effect.MoveTowardsCurve_Y.Evaluate(Action.GetActionTime(), Action.getNormalizedRandom()));
            auxPosition.x += Action.Effect.OffsetMoveTowardsCurve_X.Evaluate(Action.GetActionTime(), Action.getNormalizedRandom());
            auxPosition.y += Action.Effect.OffsetMoveTowardsCurve_Y.Evaluate(Action.GetActionTime(), Action.getNormalizedRandom());
            TargetObject.transform.position = auxPosition;
        }
    }

    //------------SWIPE---------------------
    void ActionSwipe(UITween_Action Action)
    {
        auxPosition = LocalPosition;
        switch (Action.Effect.swipeFrom)
        {
            case UITween_Effect.SwipeFrom.Left:
                auxPosition.x = -Screen.width * 0.5f - rect.rect.width * 2;
                //auxPosition.x *= TargetObject.transform.root.transform.localScale.x;
                break;

            case UITween_Effect.SwipeFrom.Botton:
                auxPosition.y = -Screen.height * 0.5f - rect.rect.height * 2;
                //auxPosition.y *= TargetObject.transform.root.transform.localScale.y;
                break;

            case UITween_Effect.SwipeFrom.Right:
                auxPosition.x = Screen.width * 0.5f + rect.rect.width * 2;
                //auxPosition.x *= TargetObject.transform.root.transform.localScale.x;
                break;

            case UITween_Effect.SwipeFrom.Top:
                auxPosition.y = Screen.height * 0.5f + rect.rect.height * 2;
                //auxPosition.y *= TargetObject.transform.root.transform.localScale.y;
                break;
        }

        TargetObject.transform.localPosition = Vector3.LerpUnclamped(auxPosition, LocalPosition, Action.Effect.SwipeCurve.Evaluate(Action.GetActionTime(), Action.getNormalizedRandom()));
    }


    //----------RECOLOR--------------
    void ActionRecolor(UITween_Action Action)
    {
        if (image != null) RecolorImage(Action);
        if (rawimage != null) RecolorRawImage(Action);
        if (text != null) RecolorText(Action);
        if (textM != null) RecolorTextM(Action);
    }

    void RecolorImage(UITween_Action Action)
    {
        switch (Action.Effect.RecolorBlend)
        {
            case UITween_Effect.BlendMode.Add:
                image.color = color + Action.Effect.RecolorGradient.Evaluate(Action.GetActionTime(), Action.getNormalizedRandom());
                break;

            case UITween_Effect.BlendMode.Mix:
                image.color = Color.Lerp(color, Action.Effect.RecolorGradient.Evaluate(Action.GetActionTime(), Action.getNormalizedRandom()), Action.GetActionTime());
                break;

            case UITween_Effect.BlendMode.Switch:
                image.color = Action.Effect.RecolorGradient.Evaluate(Action.GetActionTime(), Action.getNormalizedRandom());
                break;

            case UITween_Effect.BlendMode.Multiply:
                image.color = color * Action.Effect.RecolorGradient.Evaluate(Action.GetActionTime(), Action.getNormalizedRandom());
                break;

            case UITween_Effect.BlendMode.AlphaOnly:
                auxcolor = color;
                auxcolor.a = Action.Effect.RecolorGradient.Evaluate(Action.GetActionTime(), Action.getNormalizedRandom()).a;
                image.color = auxcolor;
                break;
        }
    }

    void RecolorRawImage(UITween_Action Action)
    {
        switch (Action.Effect.RecolorBlend)
        {
            case UITween_Effect.BlendMode.Add:
                rawimage.color = color + Action.Effect.RecolorGradient.Evaluate(Action.GetActionTime(), Action.getNormalizedRandom());
                break;

            case UITween_Effect.BlendMode.Mix:
                rawimage.color = Color.Lerp(color, Action.Effect.RecolorGradient.Evaluate(Action.GetActionTime(), Action.getNormalizedRandom()), Action.GetActionTime());
                break;

            case UITween_Effect.BlendMode.Switch:
                rawimage.color = Action.Effect.RecolorGradient.Evaluate(Action.GetActionTime(), Action.getNormalizedRandom());
                break;

            case UITween_Effect.BlendMode.Multiply:
                rawimage.color = color * Action.Effect.RecolorGradient.Evaluate(Action.GetActionTime(), Action.getNormalizedRandom());
                break;

            case UITween_Effect.BlendMode.AlphaOnly:
                auxcolor = color;
                auxcolor.a = Action.Effect.RecolorGradient.Evaluate(Action.GetActionTime(), Action.getNormalizedRandom()).a;
                rawimage.color = auxcolor;
                break;
        }
    }

    void RecolorText(UITween_Action Action)
    {
        switch (Action.Effect.RecolorBlend)
        {
            case UITween_Effect.BlendMode.Add:
                text.color = color + Action.Effect.RecolorGradient.Evaluate(Action.GetActionTime(), Action.getNormalizedRandom());
                break;

            case UITween_Effect.BlendMode.Mix:
                text.color = Color.Lerp(color, Action.Effect.RecolorGradient.Evaluate(Action.GetActionTime(), Action.getNormalizedRandom()), Action.GetActionTime());
                break;

            case UITween_Effect.BlendMode.Switch:
                text.color = Action.Effect.RecolorGradient.Evaluate(Action.GetActionTime(), Action.getNormalizedRandom());
                break;

            case UITween_Effect.BlendMode.Multiply:
                text.color = color * Action.Effect.RecolorGradient.Evaluate(Action.GetActionTime(), Action.getNormalizedRandom());
                break;

            case UITween_Effect.BlendMode.AlphaOnly:
                auxcolor = color;
                auxcolor.a = Action.Effect.RecolorGradient.Evaluate(Action.GetActionTime(), Action.getNormalizedRandom()).a;
                text.color = auxcolor;
                break;
        }
    }

    void RecolorTextM(UITween_Action Action)
    {
        switch (Action.Effect.RecolorBlend)
        {
            case UITween_Effect.BlendMode.Add:
                textM.color = color + Action.Effect.RecolorGradient.Evaluate(Action.GetActionTime(), Action.getNormalizedRandom());
                break;

            case UITween_Effect.BlendMode.Mix:
                textM.color = Color.Lerp(color, Action.Effect.RecolorGradient.Evaluate(Action.GetActionTime(), Action.getNormalizedRandom()), Action.GetActionTime());
                break;

            case UITween_Effect.BlendMode.Switch:
                textM.color = Action.Effect.RecolorGradient.Evaluate(Action.GetActionTime(), Action.getNormalizedRandom());
                break;

            case UITween_Effect.BlendMode.Multiply:
                textM.color = color * Action.Effect.RecolorGradient.Evaluate(Action.GetActionTime(), Action.getNormalizedRandom());
                break;

            case UITween_Effect.BlendMode.AlphaOnly:
                auxcolor = color;
                auxcolor.a = Action.Effect.RecolorGradient.Evaluate(Action.GetActionTime(), Action.getNormalizedRandom()).a;
                textM.color = auxcolor;
                break;
        }
    }

    //----------SCALE------------
    void ActionCanvasAlpha(UITween_Action Action)
    {
        canvasGroup.alpha = Action.Effect.CanvasAlphaCurve.Evaluate(Action.GetActionTime(), Action.getNormalizedRandom());
    }

}
