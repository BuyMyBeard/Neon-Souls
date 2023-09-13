using UnityEngine;

public class PlaceholderMovementScript : MonoBehaviour
{
    [SerializeField] Transform followTarget;
    public float mouseSensitivity = 1f;
    Quaternion nextRotation;
    

    // Update is called once per frame
    void Update()
    {
        Debug.Log(PlayerInputs.LookDelta.ToString());
        Vector3 nextRotation = new Vector3(PlayerInputs.LookDelta.y * mouseSensitivity, -PlayerInputs.LookDelta.x * mouseSensitivity, 0);
        //followTarget.transform.
    }
}
