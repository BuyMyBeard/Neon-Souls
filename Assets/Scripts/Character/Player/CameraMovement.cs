using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] Transform followTarget;
    [SerializeField] float catchUpSpeed = 1;
    [Header("Pitch clamping")]
    [Range(-90, 90)]
    [SerializeField] float camMinClamp = -90;
    [Range(-90, 90)]
    [SerializeField] float camMaxClamp = 90;

    [Header("Drift behind player")]
    [Tooltip("Speed at which the camera drifts behind the player when moving and not touching the camera")]
    [SerializeField] float driftSpeed = 10;

    [Tooltip("Time before drift fully kicks in")]
    [SerializeField] float easeInTime = 1;

    [Range(0, 5)]
    [Tooltip("Time before camera drift starts kicking in")]
    [SerializeField] float driftTimer = 1;

    [Range(0, 2)]
    [SerializeField] float followTargetVerticalOffset = 0;

    CharacterController characterController;
    InputInterface inputInterface;

    public float CamMinClamp => camMinClamp;
    public float CameraMaxClamp => camMaxClamp;

    [HideInInspector]
    public bool frozen = false;
    float driftTime = 0;
    void Awake()
    {   
        characterController = GetComponentInChildren<CharacterController>();
        inputInterface = GetComponent<InputInterface>();
    }
    void Update()
    {
        if (frozen)
            return;

        followTarget.position = Vector3.Lerp(followTarget.position, characterController.transform.position + Vector3.up * followTargetVerticalOffset, Time.deltaTime * catchUpSpeed);
        float appliedXSens = inputInterface.GamepadActive ? Preferences.ControllerSensitivityX : Preferences.MouseSensitivity;
        float appliedYSens = inputInterface.GamepadActive ? Preferences.ControllerSensivityY : Preferences.MouseSensitivity;
        int invertX;
        int invertY;

        if (inputInterface.GamepadActive)
        {
            invertX = Preferences.ControllerInvertX ? -1 : 1;
            invertY = Preferences.ControllerInvertY ? -1 : 1;
        }
        else
        {

            invertX = Preferences.MouseInvert ? -1 : 1;
            invertY = invertX;
        }

        // Quaternion * Quaternion is the same as applying rotation from second to first
        Quaternion cameraRotation = followTarget.transform.localRotation *= Quaternion.AngleAxis(inputInterface.Look.x * appliedXSens * invertX * Time.deltaTime, Vector3.up);

        cameraRotation *= Quaternion.AngleAxis(-inputInterface.Look.y * appliedYSens * invertY * Time.deltaTime, Vector3.right);

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
        if (movementDirection.magnitude > 0 && inputInterface.Look.magnitude == 0)
        {
            driftTime += Time.deltaTime;
            if (driftTime < driftTimer) return;

            Vector2 camDirection = new Vector2(followTarget.transform.forward.x, followTarget.transform.forward.z);
            Vector2 playerDirection = new Vector2(characterController.velocity.x, characterController.velocity.z).normalized;

            float dot = Vector2.Dot(playerDirection, camDirection);

            float dotFactor = (1 - Mathf.Abs(dot)) * Mathf.Clamp01((driftTime - driftTimer) / easeInTime);
            Quaternion to = Quaternion.LookRotation(movementDirection, Vector3.up);
            followTarget.transform.rotation = Quaternion.Lerp(followTarget.transform.rotation, to, Time.deltaTime * dotFactor * driftSpeed);
        }
        else
            driftTime = 0;
    }
    public void SyncFollowTarget()
    {
        followTarget.position = characterController.transform.position + Vector3.up * followTargetVerticalOffset;
    }
}
