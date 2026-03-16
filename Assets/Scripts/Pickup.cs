using UnityEngine;

public class Pickup : MonoBehaviour
{
    public float hoverHeight = 0.3f;
    public float hoverSpeed = 2f;
    public float rotateSpeed = 90f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * hoverSpeed) * hoverHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }
}