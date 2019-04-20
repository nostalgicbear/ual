using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Interactable : MonoBehaviour
{

    private HandController activeHand = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetObjectPickedUp()
    {
        Rigidbody r = GetComponent<Rigidbody>();
        r.useGravity = false;
        r.isKinematic = true;
    }

    public void SetObjectDropped()
    {
        Rigidbody r = GetComponent<Rigidbody>();
        r.useGravity = true;
        r.isKinematic = false;
    }
}
