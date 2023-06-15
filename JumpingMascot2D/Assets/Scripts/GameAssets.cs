using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    public static GameAssets _i;

    public static GameAssets i
    {
        get
        {
            if (_i == null) _i = Instantiate(Resources.Load<GameAssets>("GameAssets"));
            return _i;
        }

    }

    public Transform pfCube;
    public Transform pfScorePopup;
    public Transform pfReward;
    public float distanceSpawnMin = 1.5f;
    public float distanceSpawnMax = 3f;
    public float percentSpawnMin = 0f;
    public float percentSpawnMax = 1f;
    public int incScoreAmount = 5;
}
