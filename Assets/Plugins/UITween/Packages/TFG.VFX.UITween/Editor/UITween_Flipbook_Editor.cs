using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UITween_FlipBook))]
public class UITween_FlipBook_Editor : Editor
{
    // draw lines between a chosen game object
    // and a selection of added game objects

    void OnSceneGUI()
    {
        // get the chosen game object
        UITween_FlipBook t = target as UITween_FlipBook;

        Handles.BeginGUI();
        {
            GUIStyle boxstyle = new GUIStyle("box");

            GUILayout.BeginArea(new Rect(10, 10, 200,70), boxstyle);
            {
                if (GUILayout.Button("Play"))
                {
                    t.Play();
                }

                if (GUILayout.Button("Stop"))
                {
                    t.Stop();
                }
            }
            GUILayout.EndArea();
        }
        Handles.EndGUI();

       
    }
}