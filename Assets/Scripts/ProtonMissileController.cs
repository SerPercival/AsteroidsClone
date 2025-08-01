using UnityEngine;

public class ProtonMissileController : MonoBehaviour
{
    public float speed = 8f;
    private float screenLeft, screenRight, screenTop, screenBottom;
    private float width, height;

    void Start()
    {
        // Camera bounds
        Camera cam = Camera.main;
        float camHeight = 2f * cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;
        screenLeft = -camWidth / 2;
        screenRight = camWidth / 2;
        screenBottom = -camHeight / 2;
        screenTop = camHeight / 2;

        // Get size
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            width = sr.bounds.extents.x;
            height = sr.bounds.extents.y;
        }
        else
        {
            width = height = 0.2f;
        }
    }

    void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;

        // Destroy if off-screen
        if (transform.position.x < screenLeft - width || transform.position.x > screenRight + width ||
            transform.position.y < screenBottom - height || transform.position.y > screenTop + height)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Asteroid"))
        {
            // Tell the asteroid to split
            other.GetComponent<AsteroidController>().Split();
            Destroy(gameObject); // Destroy missile

        }
    }

}
