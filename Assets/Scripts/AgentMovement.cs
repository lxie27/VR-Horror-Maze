using UnityEngine;
using System.Collections;

public class AgentMovement : MonoBehaviour
{
    //agent base movespeed
    public float moveSpeed;
    
    //constants for speed modifiers
    private float sprintSpeed = 2.5f;
    private float crouchSpeed = .25f;
    private float turnSpeed = 25.0f;

    //rigidbody of the agent
    public Rigidbody rb;
    //camera attached to this agent
    public Transform cameraTransform;

    void Start()
    {
        //pressing escape unlocks the cursor for pausing
        //Cursor.lockState = CursorLockMode.Locked;
        //cursorLocked = true;
        rb = GetComponent<Rigidbody>();

    }


    void Update()
    {
        //movement
        float translation = Input.GetAxis("Oculus_GearVR_LThumbstickY") * moveSpeed;
        float strafe = Input.GetAxis("Oculus_GearVR_LThumbstickX") * moveSpeed;

        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0;
        camRight.y = 0;
        camForward = camForward.normalized;
        camRight = camRight.normalized;

        transform.position += (camForward * translation + camRight * strafe) * Time.deltaTime;

        //float horiTurn = Input.GetAxis("Oculus_GearVR_RThumbstickX") * turnSpeed * Time.deltaTime;
        //float vertTurn = Input.GetAxis("Oculus_GearVR_RThumbstickY") * turnSpeed * Time.deltaTime;
        //transform.Rotate(vertTurn, horiTurn, 0.0f);

        if (Input.GetKeyDown(KeyCode.R))
        {
            Application.LoadLevel(0);
        }
    }
}
