using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Hand : MonoBehaviour
{
    //Animation
    Animator animator;

    public float animationSpeed;

    private float gripTarget;
    private float triggerTarget;

    private float gripCurrent;
    private float triggerCurrent;

    private string animatorGripParam = "Grip";
    private string animatorTriggerParam = "Trigger";

    //Physics Movment
    [SerializeField] private GameObject followObject;
    [SerializeField] private float followSpeed = 30f;
    [SerializeField] private float rotateSpeed = 100f;

    [SerializeField] private Vector3 posOffset;
    [SerializeField] private Vector3 rotOffset;

    private Transform _followTarget;
    private Rigidbody _body;

    // Start is called before the first frame update
    void Start()
    {
        //Animation
        animator = GetComponent<Animator>();

        //Physics Movment
        _followTarget = followObject.transform;

        _body = GetComponent<Rigidbody>();
        _body.collisionDetectionMode = CollisionDetectionMode.Continuous;
        _body.interpolation = RigidbodyInterpolation.Interpolate;
        _body.mass = 20f;

        //Teleport hands
        _body.position = _followTarget.position;
        _body.rotation = _followTarget.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        AnimateHand();

        PhysicsMove();
    }

    private void PhysicsMove()
    {
        //Position
        //var posWithOffset = _followTarget.position + posOffset;
        var posWithOffset = _followTarget.TransformPoint(posOffset);

        var distance = Vector3.Distance(posWithOffset, transform.position);
        _body.velocity = (posWithOffset - transform.position).normalized * (followSpeed * distance * Time.deltaTime);

        //Rotation
        var rotWithOffset = _followTarget.rotation * Quaternion.Euler(rotOffset);

        var q = rotWithOffset * Quaternion.Inverse(_body.rotation);
        q.ToAngleAxis(out float angle, out Vector3 axis);

        if(Mathf.Abs(axis.magnitude) != Mathf.Infinity)
        {
            if(angle > 180f)
            {
                angle -= 360f;
            }
            _body.angularVelocity = axis * (angle * Mathf.Deg2Rad * rotateSpeed * Time.deltaTime);
        }
    }

    internal void SetGrip(float v)
    {
        gripTarget = v;
    }

    internal void SetTrigger(float v)
    {
        triggerTarget = v;
    }

    void AnimateHand()
    {
        if(gripCurrent != gripTarget)
        {
            gripCurrent = Mathf.MoveTowards(gripCurrent, gripTarget, Time.deltaTime * animationSpeed);
            animator.SetFloat(animatorGripParam, gripCurrent);
        }

        if (triggerCurrent != triggerTarget)
        {
            triggerCurrent = Mathf.MoveTowards(triggerCurrent, triggerTarget, Time.deltaTime * animationSpeed);
            animator.SetFloat(animatorTriggerParam, triggerCurrent);
        }
    }
}
