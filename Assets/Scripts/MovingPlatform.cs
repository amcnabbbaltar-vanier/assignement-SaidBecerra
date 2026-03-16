using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float minY = 1.5f;
    public float maxY = 3.5f;
    public float minZ = -27.5f;
    public float maxZ = -23.5f;
    public float speed = 2f;
    public bool moveOnY = false;
    public bool moveOnZ = false;

    void Update()
    {
        float newY = transform.position.y;
        float newZ = transform.position.z;

        if (moveOnY)
            newY = Mathf.Lerp(minY, maxY, (Mathf.Sin(Time.time * speed) + 1f) / 2f);

        if (moveOnZ)
            newZ = Mathf.Lerp(minZ, maxZ, (Mathf.Sin(Time.time * speed) + 1f) / 2f);

        transform.position = new Vector3(transform.position.x, newY, newZ);
    }
}