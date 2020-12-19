using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 0.05f;

    [SerializeField]
    private float rotationSpeed = 3f;

    [SerializeField]
    private float rotationAngleDiffrence = 3f;

    // OBJECT REFERENCES
    private Transform cam;
    private Joystick joystick;
    private Vector3 MoveVector;

    // MOVEMENT VARIABLE
    private bool isRotating = false;

    private void Start()
    {
        joystick = (GameObject.Find("Move") as GameObject).GetComponent<Joystick>();
        cam = GameObject.Find("ARCamera").transform;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }
    private void Update()
    {
        MoveVector = InputDirection();
    }
    private void MovePlayer()
    {

        RotatePlayer();
        if (!isRotating && MoveVector.magnitude != 0)
            transform.position += transform.forward * Time.deltaTime * moveSpeed;
    }

    private void RotatePlayer()
    {
        if (joystick.Vertical != 0 || joystick.Horizontal != 0)
        {
            Vector3 camRelative = cam.TransformDirection(MoveVector);
            camRelative.y = transform.up.z;
            var q = Quaternion.LookRotation(camRelative);
            this.transform.rotation = Quaternion.RotateTowards(transform.rotation, q, rotationSpeed * Time.deltaTime);
            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
           
            Vector3 objectForward = q * Vector3.forward;
           
            float angle = transform.rotation.eulerAngles.y - q.eulerAngles.y;
            if (Mathf.Abs(angle) < rotationAngleDiffrence)
                isRotating = false;
            else
                isRotating = true;
        }
        else
        {
            isRotating = false;
        }
    }

    // Gets the movement input
    private Vector3 InputDirection()
    {
        Vector3 dir = new Vector3();
        dir.z = joystick.Vertical;
        dir.x = joystick.Horizontal;

        if (dir.magnitude > 1)
        {
            dir.Normalize();
        }
        return dir;
    }
        
}