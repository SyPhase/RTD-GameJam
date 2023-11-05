using UnityEngine;

public class PortalTeleporter : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform receiver; // other portal collider trigger

    [Tooltip("Set 1 if left portal, set -1 for right")]
    [SerializeField] float invertPortal = 1f;

    float portalRadius = 0f;
    float playerRadius = 0f;

    void Start()
    {
        portalRadius = GetComponent<Collider>().bounds.size.x / 2;
        playerRadius = player.GetComponentInChildren<Collider>().bounds.size.x / 2;
    }

    void OnTriggerEnter(Collider other)
    {
        float xPos = receiver.position.x;
        if (xPos < 0)
        {
            xPos += 0.01f + portalRadius + playerRadius;
        }
        else
        {
            xPos -= 0.01f + portalRadius + playerRadius;
        }

        other.attachedRigidbody.transform.position = new Vector3(xPos, other.transform.position.y, 0f);

        /*
        Vector3 portalToPlayer = player.position - transform.position;

        float playerVelX = playerRB.velocity.x; // negative means moving left, positive moving right

        if (playerVelX * invertPortal < 0) // moving to the direction desired
        {
            float rotationDiff = -Quaternion.Angle(transform.rotation, player.rotation);
            rotationDiff += 180;

            Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToPlayer;
            player.position = receiver.position + positionOffset;
        }
        */
    }
}