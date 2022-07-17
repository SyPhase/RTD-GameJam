using UnityEngine;

public class PortalTeleporter : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform receiver; // other portal collider trigger
    [SerializeField] Rigidbody playerRB;

    [Tooltip("Set 1 if left portal, set -1 for right")]
    [SerializeField] float invertPoral = 1f;

    bool playerIsOverlapping = false;

    void Update()
    {
        if (playerIsOverlapping)
        {
            Vector3 portalToPlayer = player.position - transform.position;

            float playerVelX = playerRB.velocity.x; // negative means moving left, positive moving right

            if (playerVelX * invertPoral < 0) // moving to the direction desired
            {
                float rotationDiff = -Quaternion.Angle(transform.rotation, player.rotation);
                rotationDiff += 180;

                Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToPlayer;
                player.position = receiver.position + positionOffset;

                playerIsOverlapping = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerIsOverlapping = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerIsOverlapping = false;
        }
    }
}
