using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform player;       // Player Transform
    private float camPosY =4.5f;    // Cam Pos Y
    private float distance = 6f;    // Camera Distance
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>(); // Reference
    }
    private void LateUpdate()
    {
        // Player Follow
        transform.position =
            new Vector3(player.position.x, player.position.y + camPosY, player.position.z - distance);
    }
}
