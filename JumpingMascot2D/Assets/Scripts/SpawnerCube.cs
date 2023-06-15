using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerCube : MonoBehaviour
{
    public static Transform SpawnCube(Vector3 position)
    {
        Transform transformCube = Instantiate(GameAssets.i.pfCube, position, Quaternion.identity);
        Transform transformCubeStand = transformCube.GetChild(0);
        return transformCubeStand;
    }
}
