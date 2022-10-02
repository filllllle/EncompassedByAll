using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoints : MonoBehaviour
{
    [SerializeField]
    Transform[] positions;

    public Transform[] SpawnPositions { get => positions; }

    private void Reset()
    {
        positions = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            positions[i] = transform.GetChild(i);
        }
    }

    private void OnValidate()
    {
        
    }
}
