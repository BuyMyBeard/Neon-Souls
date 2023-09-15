using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    
    [SerializeField] Transform followTarget;
    [SerializeField] float camMinClamp = -90;
    [SerializeField] float camMaxClamp = 90;
    [SerializeField] float driftBehindPlayerSpeed = 10;
    public float mouseSensitivity = .1f;
    public float controllerSensitivity = 1f;
    public float timeCameraInactive = 0;
    public CharacterController playerMovement;

    void Awake()
    {
        playerMovement = GetComponentInChildren<CharacterController>();
    }
    void Update()
    {
        // TODO: Must be a better way to do this, this will do for now though
        // float appliedSensitivity = PlayerInputs.ActiveLookControl.device == null || PlayerInputs.ActiveLookControl.device is Gamepad ? controllerSensitivity : mouseSensitivity;
        float appliedSensitivity = mouseSensitivity;

        // Quaternion * Quaternion is the same as applying rotation from second to first
        Quaternion cameraRotation = followTarget.transform.rotation *= Quaternion.AngleAxis(PlayerInputs.LookDelta.x * appliedSensitivity, Vector3.up);

        cameraRotation *= Quaternion.AngleAxis(-PlayerInputs.LookDelta.y * appliedSensitivity, Vector3.right);

        Vector3 cameraAngles = cameraRotation.eulerAngles;

        cameraAngles.z = 0;

        // convert [0,360[ degrees to ]-180:180] degrees to avoid looping at 0 and allowing negative angles.
        // 0 degrees is direction of original angle when camera looks at player
        cameraAngles.x = cameraAngles.x > 180 ? cameraAngles.x - 360 : cameraAngles.x;
        cameraAngles.x = Mathf.Clamp(cameraAngles.x, camMinClamp, camMaxClamp);

        followTarget.transform.localEulerAngles = new Vector3(cameraAngles.x, cameraAngles.y, 0);

        // Quaternion y = Quaternion.Euler(angles.x, angles.y, 0);
        // rotationTarget = y;
        // followTarget.transform.rotation = Quaternion.Lerp(transform.rotation, y, Time.deltaTime * lerpSpeed);
        
        // Moves Camera behind the player when no look input is given
        Vector3 movementDirection = playerMovement.velocity;
        movementDirection.y = 0;
        if (movementDirection.magnitude > 0 && PlayerInputs.LookDelta.magnitude == 0)
        {
            Quaternion to = Quaternion.LookRotation(movementDirection, Vector3.up);
            followTarget.transform.rotation = Quaternion.RotateTowards(followTarget.transform.rotation, to, Time.deltaTime * driftBehindPlayerSpeed);
        }
    }
}
