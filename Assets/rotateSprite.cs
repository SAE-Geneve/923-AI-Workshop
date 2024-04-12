using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateSprite : MonoBehaviour
{

    [SerializeField] private Transform _target;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_target is not null)
        {
            Quaternion rotation = Quaternion.LookRotation(_target.position - transform.position,  Vector3.forward);
            transform.rotation = rotation;
        }
    }
}
