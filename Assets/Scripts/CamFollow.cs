using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Reference to the player Transform
    public float minXClamp = 0f;
    public float maxXClamp = 0f;
    public float minYClamp = 0f;
    public float maxYClamp = 0f;
    public float smoothTime = 0.3f; // Smoothing time for camera movement

    private Vector3 velocity = Vector3.zero;

    private void LateUpdate()
    {


        if (minXClamp <= 0f) minXClamp = -7.29f;
        if (maxXClamp <= 0f) maxXClamp = 41f;
        if (minYClamp <= 0f) minYClamp = -10;
        if (maxYClamp <= 0f) maxYClamp = 50; 
        if (smoothTime <= 0f) smoothTime = 0.3f;
        if (player != null)
        {
            // Get the player's position
            Vector3 playerPos = player.position;

            // Adjust minYClamp based on player's x position
            if (playerPos.x < 28)
            {
                minYClamp = playerPos.y;
            }

            // Clamp the player's position within the specified bounds
            float clampedX = Mathf.Clamp(playerPos.x, minXClamp, maxXClamp);
            float clampedY = Mathf.Clamp((playerPos.y+1), minYClamp, maxYClamp);

            // Target position for the camera
            Vector3 targetPos = new Vector3(clampedX, clampedY, transform.position.z);

            // Smoothly move the camera towards the target position
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
        }
    }
}
