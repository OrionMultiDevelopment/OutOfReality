using UnityEngine;

public class PlayerProgram : MonoBehaviour
{
    public CharacterController controller;
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float gravity = -9.81f;
    public Camera playerCamera;
    public float lookSensitivity = 2f;
    public float maxRunTime = 5f;
    private float runTimer;
    private float fovWalk = 60f;
    private float fovRun = 80f;
    private Vector3 velocity;
    private bool isRunning = false;
    public float cooldownTime = 5f;
    private bool isCooldown = false;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        runTimer = maxRunTime;
        playerCamera.fieldOfView = fovWalk;
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;

        if (Input.GetKey(KeyCode.LeftShift) && runTimer > 0 && !isCooldown)
        {
            isRunning = true;
            runTimer -= Time.deltaTime;
            controller.Move(move * runSpeed * Time.deltaTime);
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, fovRun, Time.deltaTime * 8f);
        }
        else
        {
            isRunning = false;
            controller.Move(move * walkSpeed * Time.deltaTime);
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, fovWalk, Time.deltaTime * 8f);

            if (runTimer <= 0 && !isCooldown)
            {
                isCooldown = true;
                Invoke("EndCooldown", cooldownTime);
            }

            if (!isCooldown)
            {
                runTimer += Time.deltaTime;
            }
        }

        runTimer = Mathf.Clamp(runTimer, 0, maxRunTime);

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = 0f;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity;
        transform.Rotate(Vector3.up * mouseX);
        playerCamera.transform.Rotate(Vector3.left * mouseY);

        if (Input.GetKeyDown(KeyCode.Escape))
            Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
    }

    void EndCooldown()
    {
        isCooldown = false;
        runTimer = maxRunTime;
    }
}
