using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(UITween_Effect))]
[CanEditMultipleObjects]
public class UITween_Effect_Editor : Editor
{
    UITween_Effect Effect;

    //Serialized Propriets
    SerializedProperty SwipeCurve;
    SerializedProperty MoveCurve_X;
    SerializedProperty MoveCurve_Y;
    SerializedProperty ScaleCurve_X;
    SerializedProperty ScaleCurve_Y;
    SerializedProperty RotationCurve_X;
    SerializedProperty RotationCurve_Y;
    SerializedProperty RotationCurve_Z;
    SerializedProperty OffsetMoveTowardsCurve_X;
    SerializedProperty OffsetMoveTowardsCurve_Y;
    SerializedProperty MoveTowardsCurve_X;
    SerializedProperty MoveTowardsCurve_Y;
    SerializedProperty RecolorGradient;
    SerializedProperty swipeFrom;
    SerializedProperty RecolorBlend;
    SerializedProperty CanvasAlphaCurve;

    private void OnEnable()
    {
        SwipeCurve = serializedObject.FindProperty("SwipeCurve");
        MoveCurve_X = serializedObject.FindProperty("MoveCurve_X");
        MoveCurve_Y = serializedObject.FindProperty("MoveCurve_Y");
        ScaleCurve_X = serializedObject.FindProperty("ScaleCurve_X");
        ScaleCurve_Y = serializedObject.FindProperty("ScaleCurve_Y");
        RotationCurve_X = serializedObject.FindProperty("RotationCurve_X");
        RotationCurve_Y = serializedObject.FindProperty("RotationCurve_Y");
        RotationCurve_Z = serializedObject.FindProperty("RotationCurve_Z");
        OffsetMoveTowardsCurve_X = serializedObject.FindProperty("OffsetMoveTowardsCurve_X");
        OffsetMoveTowardsCurve_Y = serializedObject.FindProperty("OffsetMoveTowardsCurve_Y");
        MoveTowardsCurve_X = serializedObject.FindProperty("MoveTowardsCurve_X");
        MoveTowardsCurve_Y = serializedObject.FindProperty("MoveTowardsCurve_Y");
        RecolorGradient = serializedObject.FindProperty("RecolorGradient");
        swipeFrom = serializedObject.FindProperty("swipeFrom");
        RecolorBlend = serializedObject.FindProperty("RecolorBlend");
        CanvasAlphaCurve = serializedObject.FindProperty("CanvasAlphaCurve");
    }

    public override void OnInspectorGUI()
    {
        Effect = (UITween_Effect)target;
        serializedObject.Update();
        EditorUtility.SetDirty(target);


        //--Swipe---------------------------------------------------------

        EditorGUILayout.BeginHorizontal();        
        Effect.Swipe = EditorGUILayout.ToggleLeft("Swipe", Effect.Swipe);
        EditorGUI.BeginDisabledGroup(!Effect.Swipe);
        //EditorGUILayout.Foldout(Effect.Swipe,"",false);
        EditorGUILayout.EndHorizontal();

        GUIStyle GroupsStyle = new GUIStyle(GUI.skin.label);
        GUIStyle AxisStyle = new GUIStyle(GUI.skin.label);
        GroupsStyle.margin = new RectOffset(30, 0, 0, 0);
        AxisStyle.margin = new RectOffset(50, 0, 0, 0);


        EditorGUILayout.BeginVertical(GroupsStyle);
        if (Effect.Swipe || true)
        {
            EditorGUILayout.PropertyField(swipeFrom, new GUIContent("Swipe From"));
            EditorGUILayout.PropertyField(SwipeCurve, new GUIContent("Swipe Curve"));
        }
        EditorGUILayout.EndVertical();
        EditorGUI.EndDisabledGroup();
        GUILayout.Space(10);

        //--Move---------------------------------------------------------

        EditorGUILayout.BeginHorizontal();
        Effect.Move = EditorGUILayout.ToggleLeft("Move", Effect.Move);
        EditorGUI.BeginDisabledGroup(!Effect.Move);
        //EditorGUILayout.Foldout(Effect.Move, "", false);
        EditorGUILayout.EndHorizontal();

        

        EditorGUILayout.BeginVertical(GroupsStyle);
        if (Effect.Move || true)
        {
            Effect.OffSetPosition = EditorGUILayout.Vector3Field("Add Position", Effect.OffSetPosition);
            Effect.XYMove = EditorGUILayout.ToggleLeft("2D Control", Effect.XYMove);
            if (!Effect.XYMove)
            {
                EditorGUILayout.PropertyField(MoveCurve_X, new GUIContent("Move Curve"));
            }
            else
            {
                EditorGUILayout.PropertyField(MoveCurve_X, new GUIContent("Move Curve X"));
                EditorGUILayout.PropertyField(MoveCurve_Y, new GUIContent("Move Curve Y"));
            }

        }
        EditorGUILayout.EndVertical();
        EditorGUI.EndDisabledGroup();
        GUILayout.Space(10);

        //--Rotate---------------------------------------------------------

        EditorGUILayout.BeginHorizontal();
        Effect.Rotate = EditorGUILayout.ToggleLeft("Rotate", Effect.Rotate);
        EditorGUI.BeginDisabledGroup(!Effect.Rotate);
        //EditorGUILayout.Foldout(Effect.Rotate, "", false);
        EditorGUILayout.EndHorizontal();

        

        EditorGUILayout.BeginVertical(GroupsStyle);
        if (Effect.Rotate || true)
        {
            Effect.EulerAngles = EditorGUILayout.Vector3Field("Angles", Effect.EulerAngles);
            Effect.XYZRotation = EditorGUILayout.ToggleLeft("3D Control", Effect.XYZRotation);
            if (!Effect.XYZRotation)
            {
                EditorGUILayout.PropertyField(RotationCurve_X, new GUIContent("Rotation Curve"));
            }
            else
            {
                EditorGUILayout.PropertyField(RotationCurve_X, new GUIContent("Rotation Curve X"));
                EditorGUILayout.PropertyField(RotationCurve_Y, new GUIContent("Rotation Curve Y"));
                EditorGUILayout.PropertyField(RotationCurve_Z, new GUIContent("Rotation Curve Z"));
            }

        }
        EditorGUILayout.EndVertical();
        EditorGUI.EndDisabledGroup();
        GUILayout.Space(10);

        //--Scale---------------------------------------------------------
        

        EditorGUILayout.BeginHorizontal();
        Effect.Scale = EditorGUILayout.ToggleLeft("Scale", Effect.Scale);
        EditorGUI.BeginDisabledGroup(!Effect.Scale);
        //EditorGUILayout.Foldout(Effect.Scale, "", false);
        EditorGUILayout.EndHorizontal();

        

        EditorGUILayout.BeginVertical(GroupsStyle);
        if (Effect.Scale || true)
        {
            Effect.ScaleMultiplier = EditorGUILayout.FloatField("Multiplier", Effect.ScaleMultiplier);
            Effect.XYScale = EditorGUILayout.ToggleLeft("3D Control", Effect.XYScale);
            if (!Effect.XYScale)
            {
                EditorGUILayout.PropertyField(ScaleCurve_X, new GUIContent("Scale Curve"));
            }
            else
            {
                EditorGUILayout.PropertyField(ScaleCurve_X, new GUIContent("Scale Curve X"));
                EditorGUILayout.PropertyField(ScaleCurve_Y, new GUIContent("Scale Curve Y"));
            }
        }
        EditorGUILayout.EndVertical();
        EditorGUI.EndDisabledGroup();
        GUILayout.Space(10);

        //--Recolor---------------------------------------------------------

        EditorGUILayout.BeginHorizontal();
        Effect.Recolor = EditorGUILayout.ToggleLeft("Recolor", Effect.Recolor);
        EditorGUI.BeginDisabledGroup(!Effect.Recolor);
        EditorGUILayout.Foldout(Effect.Recolor, "", false);
        EditorGUILayout.EndHorizontal();

        

        EditorGUILayout.BeginVertical(GroupsStyle);
        if (Effect.Recolor || true)
        {
            EditorGUILayout.PropertyField(RecolorBlend, new GUIContent("Blend Mode"));
            EditorGUILayout.PropertyField(RecolorGradient, new GUIContent("Swipe Curve"));
        }
        EditorGUILayout.EndVertical();
        EditorGUI.EndDisabledGroup();
        GUILayout.Space(10);

        //--MoveTowards---------------------------------------------------------

        EditorGUILayout.BeginHorizontal();
        Effect.MoveTowards = EditorGUILayout.ToggleLeft("Move Towards", Effect.MoveTowards);
        EditorGUI.BeginDisabledGroup(!Effect.MoveTowards);
        //EditorGUILayout.Foldout(Effect.MoveTowards, "", false);
        EditorGUILayout.EndHorizontal();        

        EditorGUILayout.BeginVertical(GroupsStyle);
        if (Effect.MoveTowards || true)
        {
            Effect.ScaleMultiplier = EditorGUILayout.FloatField("Multiplier", Effect.ScaleMultiplier);

            EditorGUILayout.PropertyField(OffsetMoveTowardsCurve_X, new GUIContent("Offset X"));
            EditorGUILayout.PropertyField(OffsetMoveTowardsCurve_Y, new GUIContent("Offset Y"));

            Effect.XYMoveTowards = EditorGUILayout.ToggleLeft("3D Control", Effect.XYMoveTowards);
            if (!Effect.XYMoveTowards)
            {
                EditorGUILayout.PropertyField(MoveTowardsCurve_X, new GUIContent("Move Curve"));
            }
            else
            {
                EditorGUILayout.PropertyField(MoveTowardsCurve_X, new GUIContent("Move Curve X"));
                EditorGUILayout.PropertyField(MoveTowardsCurve_Y, new GUIContent("Move Curve Y"));
            }
        }
        EditorGUILayout.EndVertical();
        EditorGUI.EndDisabledGroup();
        GUILayout.Space(10);

        //--Canvas-Alpha---------------------------------------------------------
        EditorGUILayout.BeginHorizontal();
        Effect.CanvasAlpha = EditorGUILayout.ToggleLeft("Canvas Alpha", Effect.CanvasAlpha);
        EditorGUI.BeginDisabledGroup(!Effect.CanvasAlpha);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginVertical(GroupsStyle);
        if (Effect.CanvasAlpha || true)
        {
            EditorGUILayout.PropertyField(CanvasAlphaCurve, new GUIContent("Alpha Curve"));            
        }

        serializedObject.ApplyModifiedProperties();
    }
}
