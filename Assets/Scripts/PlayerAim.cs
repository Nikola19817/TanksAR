using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    // AIM STATS
    [SerializeField]
    public float brzinaCevi = 1f;
    [SerializeField]
    public float brzinaKupole = 25f;

    // OBJECT REFERENCES
    private Joystick joystick;
    private GameObject head;
    private GameObject barrel;
    private float desiredAngle;

    private void Start()
    {
        GameObject temp = GameObject.Find("Aim");
        if (temp != null)
        {
            joystick = temp.GetComponent<Joystick>();
        }
        head = this.transform.GetChild(0).gameObject;
        barrel = head.transform.GetChild(0).GetChild(0).gameObject;
    }

    private void FixedUpdate()
    {
        // pomeranje cevi (vertikalno)
        float vertical = joystick.Vertical;
        if (vertical >= 0.25f || vertical <= -0.25f)
        {
            desiredAngle += vertical * brzinaCevi * 10;
            desiredAngle = Mathf.Clamp(desiredAngle, 0.0f, 90.0f);
            barrel.transform.localEulerAngles = new Vector3(-desiredAngle, barrel.transform.localEulerAngles.y, barrel.transform.localEulerAngles.z);
        }

        // pomeranje kupole (horiznotalno)
        float horizontal = joystick.Horizontal;
        if (horizontal >= 0.25f || horizontal <= -0.25f)
        {
            head.transform.Rotate(new Vector3(0, horizontal * brzinaKupole * Time.deltaTime, 0));
        }
    }
}