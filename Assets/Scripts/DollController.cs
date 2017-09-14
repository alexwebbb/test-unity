using UnityEngine;

public class DollController : MonoBehaviour {

    public float walkSpeed = 2;
    public float runSpeed = 6;

    public float turnSmoothTime = 0.2f;
    public float speedSmoothTime = 0.1f;

    // refs for smooth damp
    float currentTurnVelocity;
    float currentSpeedVelocity;
    float currentSpeed;

    Animator animator;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector2 inputRaw = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 inputDir = inputRaw.normalized;
        float inputState = ((inputDir.sqrMagnitude > 0.1f) ? 1 : 0);

        if (inputDir != Vector2.zero) {
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg;

            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref currentTurnVelocity, turnSmoothTime);
        }

        bool running = Input.GetKey(KeyCode.LeftShift);

        float targetSpeed = ((running) ? runSpeed : walkSpeed) * inputState;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref currentSpeedVelocity, speedSmoothTime);

        transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World);

        float animationSpeedPercent = ((running) ? 1f : 0.5f) * inputState;
        animator.SetFloat("speedPercent", animationSpeedPercent, speedSmoothTime, Time.deltaTime);
    }
}
