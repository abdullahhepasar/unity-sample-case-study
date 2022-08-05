


/*
 * 
 * A persistent type of asset, not living in any scene, is needed for a system.
 * It needs to be created inside the editor in the right-click context menu ("Create/Random/FloatCollection").
 * Item's inspector should show an array of floats, just like a regular behaviour's inspector.
 * There should be a button in the inspector that says "Generate" and it should populate the array shown, with random values between 0 and 1.
 * The generated values should persist between editor sessions, scene loads, etc. So make sure of that.
 * 
 * 
 * Code from UnityEditor namespace is not allowed in the build, but RandomFloatCollection class should make it to the build. 
 * Use preprocessor definitions to handle that. What would you do if for some reason you weren't allowed to use preprocessor definitions?
 * 
 * Continue implementing the class however you wish.
 * 
 * 
 */

using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "FloatCollection", menuName = "Random/FloatCollection")]
public class RandomFloatCollection : ScriptableObject
{
    public float[] floatValues = new float[10];

    public void SetRandom()
    {
        int Length = floatValues.Length;
        for (int i = 0; i < Length; i++)
        {
            floatValues[i] = (Random.Range(0, 2) == 0) ? 0 : 1;
        }
    }
}


//NOTE:
//If preprocessor definitions were not allowed,
//I would create a new folder named "Editor" within the project.
//And I would create the inherited class from Editor as a different file and move it into this folder.

#if UNITY_EDITOR
[CustomEditor(typeof(RandomFloatCollection))]
public class EditorRandomFloatCollection : Editor
{
    RandomFloatCollection randomFloatCollection;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        randomFloatCollection = (RandomFloatCollection)target;


        GUI.color = Color.white;
        DrawDefaultInspector();

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();

        GUI.color = Color.green;
        if (GUILayout.Button("Generate", GUILayout.Width(200f), GUILayout.Height(50f)))
            randomFloatCollection.SetRandom();

        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
        if (GUI.changed && !EditorApplication.isPlaying)
        {
            EditorUtility.SetDirty(randomFloatCollection);
        }
    }
}
#endif