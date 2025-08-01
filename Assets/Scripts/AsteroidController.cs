using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    public float minSpeed = 1f;
    public float maxSpeed =3f;
    public float minRotationSpeed = -30f;
    public float maxRotationSpeed = 30f;
    private bool hasSplit = false;

    private float screenLeft, screenRight, screenTop, screenBottom;
    private float objectWidth, objectHeight;

    private Rigidbody2D rb;

    public int asteroidSize = 3; // 3 = big, 2 = medium, 1 = small
    public Sprite bigSprite;
    public Sprite mediumSprite;
    public Sprite smallSprite;
    public GameObject asteroidPrefab; // For spawning new asteroids when splitting

    void ApplySize()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (asteroidSize == 3 && bigSprite != null) sr.sprite = bigSprite;
        else if (asteroidSize == 2 && mediumSprite != null) sr.sprite = mediumSprite;
        else if (asteroidSize == 1 && smallSprite != null) sr.sprite = smallSprite;

        if (sr != null)
        {
            objectWidth = sr.bounds.extents.x;
            objectHeight = sr.bounds.extents.y;
        }
        else
        {
            objectWidth = objectHeight = 0.5f; // fallback
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ApplySize();
        Debug.Log($"Asteroid size: {asteroidSize}, asteroidPrefab: {asteroidPrefab}");

        // Give a random velocity
        float moveAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        float moveSpeed = Random.Range(minSpeed, maxSpeed);
        Vector2 moveDirection = new Vector2(Mathf.Cos(moveAngle), Mathf.Sin(moveAngle));
        rb.linearVelocity = moveDirection * moveSpeed;

        // Give a random rotation speed
        rb.angularVelocity = Random.Range(minRotationSpeed, maxRotationSpeed);

        // Get camera bounds in world space
        Camera cam = Camera.main;
        float camHeight = 2f * cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;
        screenLeft = -camWidth / 2;
        screenRight = camWidth / 2;
        screenBottom = -camHeight / 2;
        screenTop = camHeight / 2;

    }

    void ScreenWrap()
    {
        Vector3 pos = transform.position;
        bool wrapped = false;

        if (pos.x < screenLeft - objectWidth)
        {
            pos.x = screenRight + objectWidth;
            wrapped = true;
        }
        else if (pos.x > screenRight + objectWidth)
        {
            pos.x = screenLeft - objectWidth;
            wrapped = true;
        }

        if (pos.y < screenBottom - objectHeight)
        {
            pos.y = screenTop + objectHeight;
            wrapped = true;
        }
        else if (pos.y > screenTop + objectHeight)
        {
            pos.y = screenBottom - objectHeight;
            wrapped = true;
        }

        if (wrapped)
            transform.position = pos;
    }


    void Update()
    {
        ScreenWrap();
    }


    public void Split()
    {
        if (hasSplit) return;
        hasSplit = true;

        Vector2 originalVelocity = GetComponent<Rigidbody2D>().linearVelocity;
        Debug.Log($"Splitting asteroid of size {asteroidSize}");

        int numToSpawn = 0;
        if (asteroidSize == 3) numToSpawn = 2; // big splits into 2 medium
        else if (asteroidSize == 2) numToSpawn = 3; // medium splits into 3 small

        if (asteroidSize > 1 && asteroidPrefab != null)
        {
            for (int i = 0; i < numToSpawn; i++)
            {
                GameObject newAsteroid = Instantiate(asteroidPrefab, transform.position, Quaternion.identity);
                AsteroidController ac = newAsteroid.GetComponent<AsteroidController>();
                ac.asteroidSize = asteroidSize - 1;
                ac.bigSprite = bigSprite;
                ac.mediumSprite = mediumSprite;
                ac.smallSprite = smallSprite;
                //ac.asteroidPrefab = asteroidPrefab;
                ac.ApplySize(); // Ensure correct sprite and size

                Rigidbody2D rb = newAsteroid.GetComponent<Rigidbody2D>();
                Vector2 newDir = Random.insideUnitCircle.normalized;
                rb.linearVelocity = newDir * originalVelocity.magnitude * 2f;
                Debug.Log($"Spawning asteroid of size {ac.asteroidSize}, asteroidPrefab: {ac.asteroidPrefab}");
            }
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Smallest asteroid destroyed!");
            Destroy(gameObject);
        }
    }



}
