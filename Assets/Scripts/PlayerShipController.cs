using System.Collections.Generic;
using UnityEngine;

public class PlayerShipController : MonoBehaviour
{
    public float thrustForce = 50f; // Increased for FixedUpdate timing
    public float turnSpeed = 180f;

    private float screenLeft, screenRight, screenTop, screenBottom;
    private float objectWidth, objectHeight;

    public GameObject thrusterFlame;
    private AudioSource engineAudio;

    private Rigidbody2D rb;

    public GameObject missilePrefab;
    public float missileCooldown = 0.2f; // seconds between shots
    public int maxMissiles = 4; // limit number of missiles on screen

    private float lastFireTime = -999f;
    private List<GameObject> activeMissiles = new List<GameObject>();


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        engineAudio = GetComponent<AudioSource>();

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
        // Only handle non-physics logic here
        bool isThrusting = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        thrusterFlame.SetActive(isThrusting);

        if (isThrusting && !engineAudio.isPlaying)
        {
            engineAudio.Play();
        }
        else if (!isThrusting && engineAudio.isPlaying)
        {
            engineAudio.Stop();
        }

        CleanupMissiles();

        bool canShoot = activeMissiles.Count < maxMissiles && Time.time - lastFireTime > missileCooldown;

        if (canShoot && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.LeftControl)))
        {
            // Fire!
            GameObject missile = Instantiate(
                missilePrefab,
                transform.position + transform.up * 0.5f, // nose of the ship
                transform.rotation
            );
            activeMissiles.Add(missile);
            lastFireTime = Time.time;
            // Optionally, play shoot sound here
        }

        ScreenWrap();
    }

    void FixedUpdate()
    {
        bool rotated = false;
        // Rotate left (A or LeftArrow)
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(0, 0, turnSpeed * Time.fixedDeltaTime); // smooth turn speed
            rotated = true;
        }

        // Rotate right (D or RightArrow)
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(0, 0, -turnSpeed * Time.fixedDeltaTime); // smooth turn speed
            rotated = true;
        }

        // If player is rotating, null out any angular velocity from physics
        if (rotated)
        {
            rb.angularVelocity = 0f;
        }

        // Thrust forward (W or UpArrow)
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            rb.AddForce(transform.up * thrustForce);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Prevent asteroids from imparting spin to the ship
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            rb.angularVelocity = 0f;
        }
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
    void CleanupMissiles()
    {
        // Remove destroyed missiles from the list
        activeMissiles.RemoveAll(missile => missile == null);
    }


}
