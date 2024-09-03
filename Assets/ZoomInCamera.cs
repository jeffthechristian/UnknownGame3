using Cinemachine;
using UnityEngine;
using System.Collections;

public class ZoomInCamera : MonoBehaviour
{
    private PlayerData pd;
    public float zoomedTopRigHeight = 3f;
    public float zoomedTopRigRadius = 2f;
    public float zoomedMiddleRigHeight = 2f;
    public float zoomedMiddleRigRadius = 1.5f;
    public float zoomedBottomRigHeight = 0.5f;
    public float zoomedBottomRigRadius = 0.5f;
    private bool isZoomed = false;

    private void Start()
    {
        pd = GameObject.FindWithTag("Player").GetComponent<PlayerData>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Trigger hit");
            if (pd != null && pd.cinemachineFreelook != null && !isZoomed)
            {
                StartCoroutine(SmoothTransition(
                    zoomedTopRigHeight, zoomedTopRigRadius,
                    zoomedMiddleRigHeight, zoomedMiddleRigRadius,
                    zoomedBottomRigHeight, zoomedBottomRigRadius
                ));
                isZoomed = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (pd != null && pd.cinemachineFreelook != null && isZoomed)
            {
                StartCoroutine(SmoothTransition(
                    pd.normalTopRigHeight, pd.normalTopRigRadius,
                    pd.normalMiddleRigHeight, pd.normalMiddleRigRadius,
                    pd.normalBottomRigHeight, pd.normalBottomRigRadius
                ));
                isZoomed = false;
            }
        }
    }

    private IEnumerator SmoothTransition(float topHeight, float topRadius, float middleHeight, float middleRadius, float bottomHeight, float bottomRadius)
    {
        var orbits = pd.cinemachineFreelook.m_Orbits;
        float elapsedTime = 0f;

        var startTopRig = orbits[0];
        var startMiddleRig = orbits[1];
        var startBottomRig = orbits[2];

        var endTopRig = new CinemachineFreeLook.Orbit(topHeight, topRadius);
        var endMiddleRig = new CinemachineFreeLook.Orbit(middleHeight, middleRadius);
        var endBottomRig = new CinemachineFreeLook.Orbit(bottomHeight, bottomRadius);

        while (elapsedTime < pd.transitionDuration)
        {
            float t = elapsedTime / pd.transitionDuration;
            orbits[0] = new CinemachineFreeLook.Orbit(
                Mathf.Lerp(startTopRig.m_Height, endTopRig.m_Height, t),
                Mathf.Lerp(startTopRig.m_Radius, endTopRig.m_Radius, t)
            );
            orbits[1] = new CinemachineFreeLook.Orbit(
                Mathf.Lerp(startMiddleRig.m_Height, endMiddleRig.m_Height, t),
                Mathf.Lerp(startMiddleRig.m_Radius, endMiddleRig.m_Radius, t)
            );
            orbits[2] = new CinemachineFreeLook.Orbit(
                Mathf.Lerp(startBottomRig.m_Height, endBottomRig.m_Height, t),
                Mathf.Lerp(startBottomRig.m_Radius, endBottomRig.m_Radius, t)
            );

            pd.cinemachineFreelook.m_Orbits = orbits;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        pd.cinemachineFreelook.m_Orbits = new CinemachineFreeLook.Orbit[]
        {
            new CinemachineFreeLook.Orbit(topHeight, topRadius),
            new CinemachineFreeLook.Orbit(middleHeight, middleRadius),
            new CinemachineFreeLook.Orbit(bottomHeight, bottomRadius)
        };
    }
}
