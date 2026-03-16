using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
void LateUpdate()
{
    transform.position = new Vector3(target.position.x + 10f, target.position.y + 3f, target.position.z + 3f);
}
}