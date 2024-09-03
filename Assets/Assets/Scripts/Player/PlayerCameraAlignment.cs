using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerCameraAlignment : MonoBehaviour
{
    PlayerData pd;

    private void Start()
    {
        pd = GetComponent<PlayerData>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pd.lastRotation = transform.rotation;
    }

    private void Update()
    {
        if (pd.playerMovement != null && pd.playerMovement.IsMoving())
        {
            if (pd.cinemachineCamera != null)
            {
                Vector3 cameraForward = pd.cinemachineCamera.transform.forward;
                cameraForward.y = 0;
                Quaternion targetRotation = Quaternion.LookRotation(cameraForward);

                float angle = Quaternion.Angle(transform.rotation, targetRotation);
                pd.rotationSpeedMeasured = angle / Time.deltaTime;

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, pd.rotationSpeed * Time.deltaTime * 10f);
                pd.lastRotation = transform.rotation;
            }
        }
    }

    public float GetRotationSpeed()
    {
        return pd.rotationSpeedMeasured;
    }
}
