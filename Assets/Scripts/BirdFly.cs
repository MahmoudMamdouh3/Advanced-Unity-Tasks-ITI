using UnityEngine;

public class BirdFly : MonoBehaviour
{
    public float speed = 2f;
    public float width = 5f;  // How far left and right it goes
    public float height = 1f; // Slight bobbing up and down
    private Vector3 startPos;

    void Start() { startPos = transform.position; }

    void Update()
    {
        // PingPong moves the value back and forth between 0 and width
        float xOffset = Mathf.PingPong(Time.time * speed, width) - (width / 2f);
        float yOffset = Mathf.Sin(Time.time * speed) * height;

        // Apply movement relative to the starting position
        transform.position = startPos + (transform.right * xOffset) + (Vector3.up * yOffset);
    }
}