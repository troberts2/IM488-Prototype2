using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dashing : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform playerCam;
    private Rigidbody rb;
    private PlayerMovement pm;

    [Header("Dashing")]
    public float dashForce;
    public float dashUpwardForce;
    public float maxDashYSpeed;
    public float dashDuration;

    [Header("CameraEffects")]
    public Camera cam;
    public float dashFov;

    [Header("Settings")]
    public bool useCameraForward = true;
    public bool allowAllDirections = true;
    public bool disableGravity = false;
    public bool resetVel = true;

    [Header("Cooldown")]
    public float dashCd;
    private float dashCdTimer;

    [Header("Input")]
    public KeyCode dashKey = KeyCode.E;
    private DefaultInputActions playerInput;
    private InputAction dash;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
    }
    private void Awake() {
        playerInput = new DefaultInputActions();
        dash = playerInput.Player.Dash;
    }
    private void OnEnable() {

        dash.Enable();
    }
    private void OnDisable() {
        dash.Disable();
    }

    private void Update()
    {
        if(dash.IsPressed()){
            Dash();
        }

        if (dashCdTimer > 0)
            dashCdTimer -= Time.deltaTime;
    }

    private void Dash()
    {
        if (dashCdTimer > 0 || pm.state == PlayerMovement.MovementState.attacking || pm.state == PlayerMovement.MovementState.drinking) return;
        else dashCdTimer = dashCd;

        pm.dashing = true;
        pm.maxYSpeed = maxDashYSpeed;
        pm.iFrames = true;

        //cam.DoFov(dashFov);

        Transform forwardT;

        if (useCameraForward)
            forwardT = playerCam; /// where you're looking
        else
            forwardT = orientation; /// where you're facing (no up or down)

        Vector3 direction = GetDirection(forwardT);

        Vector3 forceToApply = direction * dashForce + orientation.up * dashUpwardForce;

        if (disableGravity)
            rb.useGravity = false;

        delayedForceToApply = forceToApply;
        Invoke(nameof(DelayedDashForce), 0.025f);

        Invoke(nameof(ResetDash), dashDuration);
    }

    private Vector3 delayedForceToApply;
    private void DelayedDashForce()
    {
        if (resetVel)
            rb.velocity = Vector3.zero;

        rb.AddForce(delayedForceToApply, ForceMode.Impulse);
    }

    private void ResetDash()
    {
        pm.dashing = false;
        pm.iFrames = false;
        pm.maxYSpeed = 0;

        //cam.DoFov(85f);

        if (disableGravity)
            rb.useGravity = true;
    }

    private Vector3 GetDirection(Transform forwardT)
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3();

        if (allowAllDirections)
            direction = forwardT.forward * verticalInput + forwardT.right * horizontalInput;
        else
            direction = forwardT.forward;

        if (verticalInput == 0 && horizontalInput == 0)
            direction = forwardT.forward;

        return direction.normalized;
    }
}