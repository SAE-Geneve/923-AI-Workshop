using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealableItem : MonoBehaviour
{

    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private int _prize;
    public int Prize => _prize;

    private void Start()
    {
        // If nothing setup, find it automatically
        if (_meshRenderer == null)
        {
            if(!TryGetComponent<MeshRenderer>(out _meshRenderer))
                Debug.LogError("No component _meshRenderer available.");
        }
    }

    public void StealItem()
    {
        _meshRenderer.enabled = false;
    }

}
