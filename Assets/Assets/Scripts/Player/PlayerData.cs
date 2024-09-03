using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [Header("Movement")]
    public CharacterController characterController;
    public AnimationMovement animationMovement;
    public float walkSpeed;
    public float runSpeed;
    public float acceleration;
    public float deceleration;
    public float gravity;
    public float jumpHeight;
    public float minSpeedThreshold;
    public float rotationSlowdownFactor;
    public float rotationThreshold;

    public float fallThreshold;
    public float fallTimeThreshold;
    public float fallStartTime;

    public bool isFalling;
    public bool isStandingUp;

    public Vector3 velocity;
    public Vector3 currentVelocity;
    public Vector3 targetVelocity;

    public bool forward;
    public bool left;
    public bool right;
    public bool backward;
    public bool run;
    public bool jump;

    public float verticalVelocity;

    public PlayerCameraAlignment playerCameraAlignment;

    [Header("Animations")]
    public Animator animator;

    public float velocityZ;
    public float velocityX;
    public float acceleration2;
    public float deceleration2;
    public float maximumWalkingVelocity;
    public float maximumRunningVelocity;
    public int VelocityZHash;
    public int VelocityXHash;

    [Header("Camera")]
    public Camera cinemachineCamera;
    public float rotationSpeed = 10f;

    public PlayerMovement playerMovement;
    public Quaternion lastRotation;
    public float rotationSpeedMeasured;
}
