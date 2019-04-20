using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.Events;
using System;
using DG.Tweening;

[RequireComponent(typeof(FixedJoint))]
public class FishermansTale_HandController : MonoBehaviour
{

    public SteamVR_Action_Boolean extendHandButton;
    public SteamVR_Action_Boolean grabTrigger;

    private float extendedDistance = 3.0f;
    [SerializeField]
    private bool atExtendedDistance = false;

    public float d;
    public Vector3 currentF;
    public Vector3 currentT;

    private SteamVR_Behaviour_Pose pose;
    private FixedJoint joint;

    private GameObject currentInteractable;
    private bool pickedUp = false;

    public UnityEvent OnHover;
    public UnityEvent OnHoverEnd;
    public UnityEvent OnPickUp;
    public UnityEvent OnDrop;

    public Transform handModel;

    private Vector3 curentExtendedV;

    private void Awake()
    {
        pose = transform.parent.GetComponent<SteamVR_Behaviour_Pose>();
        joint = GetComponent<FixedJoint>();

    }


    // Update is called once per frame
    void Update()
    {
        ListenForPickupButtonPress();

        UpdateHandPosition();

        if (joint.connectedBody)
        {
            currentF = joint.currentForce;
            currentT = joint.currentTorque;
        }

    }


    void UpdateHandPosition()
    {
        if (atExtendedDistance)
        {
            Vector3 v = transform.parent.TransformPoint(Vector3.forward * 0.4f);
           // handModel.transform.position = v;
            handModel.DOMove(v, 0.20f);
           // curentExtendedV = v;
        }
        else
        {
            //handModel.transform.position = transform.parent.position;
            handModel.DOMove(transform.parent.position, 0.10f);
        }
    }

    void ListenForPickupButtonPress()
    {

        if (extendHandButton.GetStateDown(pose.inputSource))
        {
            //Debug.Log("Pressed: " + extendHandButton.fullPath + " on " + pose.inputSource);
            atExtendedDistance = !atExtendedDistance;
        }

        if (extendHandButton.GetStateUp(pose.inputSource))
        {
            //Debug.Log("Released: " + extendHandButton.fullPath + " on " + pose.inputSource);
        }



        if (grabTrigger.GetStateDown(pose.inputSource))
        {
            PickUpObject();
        }

        if (grabTrigger.GetStateUp(pose.inputSource))
        {
            DropObject();
        }

        // Debug.LogError("Axis value is : " + single.axis);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Interactable>() && currentInteractable == null)
        {
            OnHover.Invoke();
            currentInteractable = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Interactable>() && pickedUp == false)
        {
            OnHoverEnd.Invoke();
            currentInteractable = null;
        }
    }

    void PickUpObject()
    {
        if (!currentInteractable)
        {
            return;
        }

        currentInteractable.transform.position = transform.position;
        joint.connectedBody = currentInteractable.GetComponent<Rigidbody>();
        pickedUp = true;
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
        pickedUp = false;

    }

}
