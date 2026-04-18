using UnityEngine;

public class BillboardScript : MonoBehaviour
{
    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
    }

    // LateUpdate is best here so it adjusts AFTER the enemy has moved/rotated this frame
    void LateUpdate()
    {
        if (mainCam != null)
        {
            // Forces the text to perfectly mirror the camera's rotation so it never looks backwards
            transform.LookAt(transform.position + mainCam.transform.rotation * Vector3.forward,
                mainCam.transform.rotation * Vector3.up);
        }
    }
}