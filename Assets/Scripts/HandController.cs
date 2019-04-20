using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.Events;

[RequireComponent(typeof(SteamVR_Behaviour_Pose))]
[RequireComponent(typeof(FixedJoint))]
public class HandController : MonoBehaviour
{

    public SteamVR_Action_Boolean pickUpButton;
    public SteamVR_Action_Single single;

    private float extendedDistance = 3.0f;
    private bool atExtendedDistance = false;

    private SteamVR_Behaviour_Pose pose;
    private FixedJoint joint;

    private GameObject currentInteractable;

    public UnityEvent OnHover;
    public UnityEvent OnHoverEnd;
    public UnityEvent OnPickUp;
    public UnityEvent OnDrop;

    public Transform model;

    private void Awake()
    {
        pose = GetComponent<SteamVR_Behaviour_Pose>();
        joint = GetComponent<FixedJoint>();

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        ListenForPickupButtonPress();

        if (atExtendedDistance)
        {
            Vector3 v = transform.TransformPoint(Vector3.forward * 1.2f);
            model.transform.position = v;
        }
        else
        {
            model.transform.position = transform.position;
        }



    }

    void ListenForPickupButtonPress()
    {

        if(pickUpButton.GetStateDown(pose.inputSource))
        {
            atExtendedDistance = true;
            Debug.Log("Pressed: " + pickUpButton.fullPath + " on " + pose.inputSource);
            PickUpObject();
        }

        if (pickUpButton.GetStateUp(pose.inputSource))
        {
            atExtendedDistance = false;
            Debug.Log("Released: " + pickUpButton.fullPath + " on " + pose.inputSource);
            DropObject();
        }

        if (single.GetAxis(pose.inputSource) == 1)
        {
            atExtendedDistance = true;
        }
        else {
            atExtendedDistance = false;
        }

       // Debug.LogError("Axis value is : " + single.axis);

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Interactable>())
        {
            OnHover.Invoke();
            currentInteractable = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Interactable>())
        {
            OnHoverEnd.Invoke();
            currentInteractable = null;
        }
    }

    void PickUpObject()
    {
        if(!currentInteractable)
        {
            return;
        }

        currentInteractable.transform.position = transform.position;
        joint.connectedBody = currentInteractable.GetComponent<Rigidbody>();
    }

    void DropObject()
    {
        if (!currentInteractable)
        {
            return;
        }

        currentInteractable.GetComponent<Rigidbody>().velocity = pose.GetVelocity();
        currentInteractable.GetComponent<Rigidbody>().angularVelocity = pose.GetAngularVelocity();
        joint.connectedBody = null;
    }
}
