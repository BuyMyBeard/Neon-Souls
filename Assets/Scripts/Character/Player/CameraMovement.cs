using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] Transform followTarget;
    [Header("Pitch clamping")]
    [Range(-90, 90)]
    [SerializeField] float camMinClamp = -90;
    [Range(-90, 90)]
    [SerializeField] float camMaxClamp = 90;

    [Header("Drift behind player")]
    [Tooltip("Speed at which the camera drifts behind the player when moving and not touching the camera")]
    [SerializeField] float driftSpeed = 10;

    [Range(-1, 0)]
    [Tooltip("Dot product between player and camera which doesn't make camera drift. Used when running towards camera")]
    [SerializeField] float driftDeadZone = -.9f;

    [Header("Sensitivity")]
    [Range(.01f, 5)]
    public float mouseSensitivity = .1f;
    [Range(.01f, 5)]
    public float controllerSensitivity = 1f;

    [Range(0, 2)]
    [SerializeField] float verticalOffset = 1;

    CharacterController characterController;
    PlayerController playerController;
    void Awake()
    {   
        characterController = GetComponentInChildren<CharacterController>();
        playerController = GetComponent<PlayerController>();
    }
    void Update()
    {
        followTarget.position = characterController.transform.position + new Vector3(0, verticalOffset, 0);
        float appliedSensitivity = playerController.GamepadActive ? controllerSensitivity : mouseSensitivity;

        // Quaternion * Quaternion is the same as applying rotation from second to first
        Quaternion cameraRotation = followTarget.transform.rotation *= Quaternion.AngleAxis(playerController.Look.x * appliedSensitivity, Vector3.up);

        cameraRotation *= Quaternion.AngleAxis(-playerController.Look.y * appliedSensitivity, Vector3.right);

        Vector3 cameraAngles = cameraRotation.eulerAngles;

        cameraAngles.z = 0;

        // convert [0,360[ degrees to ]-180:180] degrees to avoid looping at 0 and allowing negative angles.
        // 0 degrees is direction of original angle when camera looks at player
        cameraAngles.x = cameraAngles.x > 180 ? cameraAngles.x - 360 : cameraAngles.x;
        cameraAngles.x = Mathf.Clamp(cameraAngles.x, camMinClamp, camMaxClamp);

        followTarget.transform.localEulerAngles = new Vector3(cameraAngles.x, cameraAngles.y, 0);

        ApplyCameraDrift();
    }

    private void ApplyCameraDrift()
    {
        // Moves Camera behind the player when no look input is given
        Vector3 movementDirection = characterController.velocity;
        movementDirection.y = 0;
        if (movementDirection.magnitude > 0 && playerController.Look.magnitude == 0)
        {

            Vector2 camDirection = new Vector2(followTarget.transform.forward.x, followTarget.transform.forward.z);
            Vector2 playerDirection = new Vector2(characterController.velocity.x, characterController.velocity.z).normalized;

            float dot = Vector2.Dot(playerDirection, camDirection);

            /// if player walks towards camera, camera doesn't try to drift behind player
            if (dot > driftDeadZone)
            {
                float dotFactor = 1 - Mathf.Abs(dot);
                Quaternion to = Quaternion.LookRotation(movementDirection, Vector3.up);
                followTarget.transform.rotation = Quaternion.Lerp(followTarget.transform.rotation, to, Time.deltaTime * dotFactor * driftSpeed);
            }
        }
    }
}
