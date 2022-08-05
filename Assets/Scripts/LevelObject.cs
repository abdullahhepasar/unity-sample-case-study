/*
 * 
 * Implement a scriptable object that can be created editor right-click context menu. The object should have the following properties:
 * Level number
 * Camera background color
 * Level prefab (You can use cubes, spheres, etc. placed randomly)
 * Win condition (Use an enum, "Clear all area" or "Clear specific area")
 * Write a level manager that loads the created level objects and initiates the level
 * Make the system testable in the scene LevelTest
 * 
 * */

using UnityEngine;

public enum WinCondition
{
    None,
    ClearAllArea,
    ClearSpesificArea
}

[CreateAssetMenu(fileName = "Level", menuName = "Proje/LevelObject")]
public class LevelObject : ScriptableObject
{
    public int LevelNumber = 0;
    public Color CameraBackgroundColor = Color.white;
    public GameObject LevelPrefab; //Note: If there are more levels, the Level name can be taken as "string" and it would be appropriate to apply "Resources.Load" for RAM optimization. 
    public WinCondition winCondition = WinCondition.None;
}






