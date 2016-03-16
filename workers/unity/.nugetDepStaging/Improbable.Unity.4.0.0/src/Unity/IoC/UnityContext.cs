using System;
using UnityEngine;

public class ContextMessage
{
    public static readonly string ADDED = "OnAdded";
}

public class UnityContext<T> : MonoBehaviour where T : IContextRoot, new()
{
    private T _applicationRoot;

    private void Awake()
    {
        Debug.Log("UnityContext Awaked");

        _applicationRoot = new T();
    }

    //
    // Defining OnEnable as fix for UnityEngine execution order bug
    //
    private void OnEnable()
    {
        Debug.Log("UnityContext Enabled");
    }

    private void OnAdded(MonoBehaviour component)
    {
        if (_applicationRoot == null || _applicationRoot.container == null)
        {
            throw new Exception("Container not initialized correctly, possible script execution order problem");
        }

        _applicationRoot.container.Inject(component);
    }
}