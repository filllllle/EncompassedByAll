using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task : MonoBehaviour
{
    [SerializeField]
    private string taskName;

    bool resolved;

    public bool IsResolved { get => resolved; }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    protected void SetAsResolved()
    {
        resolved = true;
    }
}
