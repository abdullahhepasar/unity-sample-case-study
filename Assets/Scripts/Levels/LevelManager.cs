using UnityEngine;
using System;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    private List<LevelObject> levelObjects = new List<LevelObject>();

    private LevelObject Currentlevel;
    private int CurrentLevelId = 0;

    [Serializable]
    private class LevelCache
    {
        public GameObject LevelPrefab;
    }
    private LevelCache levelCache;

    [SerializeField] private Camera MainCamera;

    // Start is called before the first frame update
    void Start()
    {
        PrepareAndStart();
    }

    private void PrepareAndStart()
    {
        ClearCache();

        GetLevelData();

        //Set Current Level Data
        Currentlevel = levelObjects[CurrentLevelId];

        SetPrepareSceneForCurrentLevel();
    }

    /// <summary>
    /// Get All Level Data
    /// </summary>
    private void GetLevelData()
    {
        if (levelObjects.Count > 0)
            return;

        //Get All Level Datas
        LevelObject[] tempLevelObjects = Resources.LoadAll<LevelObject>("Levels") as LevelObject[];

        //Sort Level data by Level Number
        int LevelObjectsLength = tempLevelObjects.Length;
        LevelObject temp;
        for (int write = 0; write < LevelObjectsLength; write++)
        {
            for (int sort = 0; sort < LevelObjectsLength - 1; sort++)
            {
                if (tempLevelObjects[sort].LevelNumber > tempLevelObjects[sort + 1].LevelNumber)
                {
                    temp = tempLevelObjects[sort + 1];
                    tempLevelObjects[sort + 1] = tempLevelObjects[sort];
                    tempLevelObjects[sort] = temp;
                }
            }
        }

        //Add Global List
        for (int i = 0; i < LevelObjectsLength; i++)
        {
            levelObjects.Add(tempLevelObjects[i]);
        }
    }

    /// <summary>
    /// Create Level Data for Scene and Apply other settings
    /// </summary>
    private void SetPrepareSceneForCurrentLevel()
    {
        //Level Prefab
        GameObject tempLevelPrefab = Instantiate(Currentlevel.LevelPrefab);

        //Camera Settings
        MainCamera.backgroundColor = Currentlevel.CameraBackgroundColor;

        //Level Cache
        levelCache = new LevelCache();
        levelCache.LevelPrefab = tempLevelPrefab;
    }

    /// <summary>
    /// Clear previous cache
    /// </summary>
    private void ClearCache()
    {
        if (levelCache == null)
            return;

        DestroyImmediate(levelCache.LevelPrefab);
    }

    #region SET UI

    /// <summary>
    /// It is triggered from ButtonNextLevel in the UI and 
    /// it is provided to go to the next level.
    /// </summary>
    public void SetNextLevel()
    {
        CurrentLevelId++;

        if (CurrentLevelId >= levelObjects.Count)
            CurrentLevelId = 0;

        PrepareAndStart();
    }

    #endregion

}
