using UnityEngine;

public class FinalTrap : MonoBehaviour
{
    public float minZ = -48f;
    public float maxZ = -41f;
    public float speed = 2f;

    void Update()
    {
        float newZ = Mathf.Lerp(minZ, maxZ, (Mathf.Sin(Time.time * speed) + 1f) / 2f);
        transform.position = new Vector3(transform.position.x, transform.position.y, newZ);
    }
}