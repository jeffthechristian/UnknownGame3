using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerData pd;

    private void Start()
    {
        pd = GetComponent<PlayerData>();
    }

    private void Update()
    {
        HandleInput();
        CalculateMovement();
        ApplyGravity();

        Vector3 move = pd.currentVelocity + pd.velocity;
        pd.characterController.Move(move * Time.deltaTime);

        HandleFallingAnimation();
    }

    private void HandleInput()
    {
        pd.forward = Input.GetKey(KeyCode.W);
        pd.left = Input.GetKey(KeyCode.A);
        pd.right = Input.GetKey(KeyCode.D);
        pd.backward = Input.GetKey(KeyCode.S);
        pd.run = Input.GetKey(KeyCode.LeftShift);
        pd.jump = Input.GetKeyDown(KeyCode.Space);
    }

    private void CalculateMovement()
    {
        if (!pd.isStandingUp)
        {
            float currentMaxVelocity = pd.run ? pd.runSpeed : pd.walkSpeed;

            Vector3 move = Vector3.zero;
            if (pd.forward) move += transform.forward;
            if (pd.left) move += -transform.right;
            if (pd.right) move += transform.right;
            if (pd.backward) move += -transform.forward;
            move = move.normalized;

            pd.targetVelocity = move * currentMaxVelocity;

            float rotationSpeed = pd.playerCameraAlignment.GetRotationSpeed();
            float speedMultiplier = 1f;
            if (rotationSpeed > pd.rotationThreshold)
            {
                speedMultiplier = 1 - Mathf.Clamp01((rotationSpeed - pd.rotationThreshold) / pd.rotationThreshold * pd.rotationSlowdownFactor);
            }

            if (pd.targetVelocity.magnitude > 0)
            {
                pd.currentVelocity = Vector3.Lerp(pd.currentVelocity, pd.targetVelocity * speedMultiplier, pd.acceleration * Time.deltaTime);
            }
            else
            {
                if (pd.currentVelocity.magnitude > pd.minSpeedThreshold)
                {
                    pd.currentVelocity = Vector3.Lerp(pd.currentVelocity, Vector3.zero, pd.deceleration * Time.deltaTime);
                }
                else
                {
                    pd.currentVelocity = Vector3.zero;
                }
            }
        }
        else
        {
            pd.currentVelocity = Vector3.zero;
        }
    }

    private void ApplyGravity()
    {
        if (pd.characterController.isGrounded)
        {
            if (pd.jump && !pd.isStandingUp)
            {
                pd.animationMovement.Jump();
                pd.verticalVelocity = Mathf.Sqrt(pd.jumpHeight * -2f * pd.gravity);
                StartCoroutine(AdjustCharacterControllerCenter());
                pd.jump = false;
            }
            else
            {
                pd.verticalVelocity = -2f;
            }
        }
        else
        {
            pd.verticalVelocity += pd.gravity * Time.deltaTime;
        }

        pd.velocity = new Vector3(0, pd.verticalVelocity, 0);
    }

    private IEnumerator AdjustCharacterControllerCenter()
    {
        float startCenterY = 0.98f;
        float endCenterY = 1.5f;
        float duration = 0.4f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            pd.characterController.center = new Vector3(pd.characterController.center.x, Mathf.Lerp(startCenterY, endCenterY, elapsedTime / duration), pd.characterController.center.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        pd.characterController.center = new Vector3(pd.characterController.center.x, endCenterY, pd.characterController.center.z);

        yield return new WaitForSeconds(0.1f);

        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            pd.characterController.center = new Vector3(pd.characterController.center.x, Mathf.Lerp(endCenterY, startCenterY, elapsedTime / duration), pd.characterController.center.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        pd.characterController.center = new Vector3(pd.characterController.center.x, startCenterY, pd.characterController.center.z);
    }

    private void HandleFallingAnimation()
    {
        if (!pd.characterController.isGrounded && pd.verticalVelocity < -pd.fallThreshold)
        {
            if (Time.time - pd.fallStartTime > pd.fallTimeThreshold)
            {
                if (!pd.isFalling)
                {
                    pd.animationMovement.Falling(true);
                    pd.isFalling = true;
                    pd.isStandingUp = true;
                }
            }
        }
        else
        {
            if (pd.isFalling)
            {
                pd.animationMovement.Falling(false);
                pd.isFalling = false;
                StartCoroutine(StandingUp(11f));
            }
            pd.fallStartTime = Time.time;
        }
    }

    private IEnumerator StandingUp(float duration)
    {
        pd.isStandingUp = true;
        yield return new WaitForSeconds(duration);
        pd.isStandingUp = false;
    }

    public bool IsMoving()
    {
        return (pd.forward || pd.left || pd.right || pd.backward) && !pd.isFalling && !pd.isStandingUp;
    }
}
