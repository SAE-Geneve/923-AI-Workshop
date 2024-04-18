using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Pathfinder))]
public class PathfinderEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Pathfinder pathfinder = (Pathfinder)target;

        if (GUILayout.Button("A Star Reset"))
            pathfinder.FindPath();

        if (GUILayout.Button("A Star Iteration"))
            pathfinder.FindPathIteration();

        if (GUILayout.Button("Start A-Star !"))
            pathfinder.StartFinding();

        if (GUILayout.Button("Stop A-Star !"))
            pathfinder.StopFinding();

    }
}
