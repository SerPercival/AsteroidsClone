using UnityEngine;

public class PlayerShipController : MonoBehaviour
{
    public float thrustForce = 5f;
    public float turnSpeed = 180f;

    private float screenLeft, screenRight, screenTop, screenBottom;
    private float objectWidth, objectHeight;

    public GameObject thrusterFlame;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Calculate screen bounds
        Camera cam = Camera.main;
        float camHeight = 2f * cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;
        screenLeft = -camWidth / 2;
        screenRight = camWidth / 2;
        screenBottom = -camHeight / 2;
        screenTop = camHeight / 2;

        // Get ship sprite size (in world units)
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            objectWidth = sr.bounds.extents.x;
            objectHeight = sr.bounds.extents.y;
        }
        else
        {
            objectWidth = objectHeight = 0.5f; // fallback
        }

        // Find the thruster flame object
        if (thrusterFlame == null)
            thrusterFlame = transform.Find("Thruster").gameObject;

    }

    void Update()
    {
        // Rotate left (A or LeftArrow)
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(0, 0, turnSpeed * Time.deltaTime);
        }

        // Rotate right (D or RightArrow)
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(0, 0, -turnSpeed * Time.deltaTime);
        }

        // Thrust forward (W or UpArrow)
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            rb.AddForce(transform.up * thrustForce);
        }

        bool isThrusting = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        // Activate thruster flame if thrusting
        thrusterFlame.SetActive(isThrusting);



        ScreenWrap();

    }

    void ScreenWrap()
    {
        Vector3 pos = transform.position;

        if (pos.x < screenLeft - objectWidth)
            pos.x = screenRight + objectWidth;
        else if (pos.x > screenRight + objectWidth)
            pos.x = screenLeft - objectWidth;

        if (pos.y < screenBottom - objectHeight)
            pos.y = screenTop + objectHeight;
        else if (pos.y > screenTop + objectHeight)
            pos.y = screenBottom - objectHeight;

        transform.position = pos;
    }


}
