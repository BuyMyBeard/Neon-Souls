using UnityEngine;

public class PlaceholderMovementScript : MonoBehaviour
{
    [SerializeField] Transform followTarget;
    [SerializeField] float camMinClamp = -90;
    [SerializeField] float camMaxClamp = 90;
    [SerializeField] float lerpSpeed = .5f;
    public float mouseSensitivity = 1f;
    Quaternion rotationTarget;

    // Update is called once per frame
    void Update()
    {
        Quaternion cameraRotation = followTarget.transform.rotation *= Quaternion.AngleAxis(PlayerInputs.LookDelta.x , Vector3.up);
        cameraRotation *= Quaternion.AngleAxis(-PlayerInputs.LookDelta.y, Vector3.right);

        Vector3 cameraAngles = cameraRotation.eulerAngles;
        cameraAngles.z = 0;

        cameraAngles.x = cameraAngles.x > 180 ? cameraAngles.x - 360 : cameraAngles.x;
        cameraAngles.x = Mathf.Clamp(cameraAngles.x, camMinClamp, camMaxClamp);

        followTarget.transform.localEulerAngles = new Vector3(cameraAngles.x, cameraAngles.y, 0);

        // Quaternion y = Quaternion.Euler(angles.x, angles.y, 0);
        // rotationTarget = y;
        // followTarget.transform.rotation = Quaternion.Lerp(transform.rotation, y, Time.deltaTime * lerpSpeed);
    }
}
