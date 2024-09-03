using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class AnimationMovement : MonoBehaviour
{
    private PlayerData pd;

    private void Start()
    {
        pd = GetComponent<PlayerData>();
        pd.VelocityZHash = Animator.StringToHash("MovementZ_NoWeapon");
        pd.VelocityXHash = Animator.StringToHash("MovementX_NoWeapon");
    }

    private void Update()
    {
        pd.forward = Input.GetKey(KeyCode.W);
        pd.left = Input.GetKey(KeyCode.A);
        pd.right = Input.GetKey(KeyCode.D);
        pd.backward = Input.GetKey(KeyCode.S);
        pd.run = Input.GetKey(KeyCode.LeftShift);

        float currentMaxVelocity = pd.run ? pd.maximumRunningVelocity : pd.maximumWalkingVelocity;

        ChangeVelocity(pd.forward, pd.left, pd.right, pd.backward, pd.run, currentMaxVelocity);
        ResetVelocity(pd.forward, pd.left, pd.right, pd.backward, pd.run, currentMaxVelocity);

        // Animation
        pd.animator.SetFloat(pd.VelocityXHash, pd.velocityX);
        pd.animator.SetFloat(pd.VelocityZHash, pd.velocityZ);
    }

    private void ResetVelocity(bool forward, bool left, bool right, bool backward, bool run, float currentMaxVelocity)
    {
        // Clamping VelocityZ when releasing the run button
        if (pd.velocityZ > currentMaxVelocity)
        {
            pd.velocityZ -= Time.deltaTime * pd.deceleration2;
            if (pd.velocityZ < currentMaxVelocity)
            {
                pd.velocityZ = currentMaxVelocity;
            }
        }
        else if (pd.velocityZ < -currentMaxVelocity)
        {
            pd.velocityZ += Time.deltaTime * pd.deceleration2;
            if (pd.velocityZ > -currentMaxVelocity)
            {
                pd.velocityZ = -currentMaxVelocity;
            }
        }

        // Clamping VelocityX when releasing the run button
        if (pd.velocityX > currentMaxVelocity)
        {
            pd.velocityX -= Time.deltaTime * pd.deceleration2;
            if (pd.velocityX < currentMaxVelocity)
            {
                pd.velocityX = currentMaxVelocity;
            }
        }
        else if (pd.velocityX < -currentMaxVelocity)
        {
            pd.velocityX += Time.deltaTime * pd.deceleration2;
            if (pd.velocityX > -currentMaxVelocity)
            {
                pd.velocityX = -currentMaxVelocity;
            }
        }
    }

    private void ChangeVelocity(bool forward, bool left, bool right, bool backward, bool run, float currentMaxVelocity)
    {
        // acceleration2
        if (forward && pd.velocityZ < currentMaxVelocity)
        {
            pd.velocityZ += Time.deltaTime * pd.acceleration2;
        }
        if (left && pd.velocityX > -currentMaxVelocity)
        {
            pd.velocityX -= Time.deltaTime * pd.acceleration2;
        }
        if (right && pd.velocityX < currentMaxVelocity)
        {
            pd.velocityX += Time.deltaTime * pd.acceleration2;
        }
        if (backward && pd.velocityZ > -currentMaxVelocity)
        {
            pd.velocityZ -= Time.deltaTime * pd.acceleration2;
        }

        // deceleration2 for Z-axis (forward/backward)
        if (!forward && !backward && pd.velocityZ > 0.0f)
        {
            pd.velocityZ -= Time.deltaTime * pd.deceleration2;
        }

        if (!forward && !backward && pd.velocityZ < 0.0f)
        {
            pd.velocityZ += Time.deltaTime * pd.deceleration2;
        }

        // deceleration2 for X-axis (left/right)
        if (!left && !right && pd.velocityX < 0.0f)
        {
            pd.velocityX += Time.deltaTime * pd.deceleration2;
        }

        if (!left && !right && pd.velocityX > 0.0f)
        {
            pd.velocityX -= Time.deltaTime * pd.deceleration2;
        }

        if (!left && !right && pd.velocityX != 0.0f && pd.velocityX > -0.05f && pd.velocityX < 0.05f)
        {
            pd.velocityX = 0.0f;
        }
    }

    public void Falling(bool falling)
    {
        if (falling)
        {
            pd.animator.SetTrigger("Falling");
        } else
        {
            pd.animator.SetTrigger("StandingUp");
        }
    }

    public void Jump()
    {
        pd.animator.SetTrigger("Jump");
    }
}
