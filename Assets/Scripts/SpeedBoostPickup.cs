using UnityEngine;
using Gamekit2D;

/// <summary>
/// Put on a pickup object that has a Collider2D with "Is Trigger" ticked.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class SpeedBoostPickup : MonoBehaviour
{
    [Tooltip("1.5 = 50% faster")]
    public float speedMultiplier = 1.5f;
    public float duration = 5f;

    [Header("Optional")]
    public AudioClip pickupSound;

    private bool collected = false;

    private void Reset()
    {
        // Makes sure the collider is a trigger when you first add this script.
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected) return;

        PlayerCharacter player = other.GetComponentInParent<PlayerCharacter>();
        if (player == null) return;

        collected = true;
        SpeedBoostEffect.Apply(player, speedMultiplier, duration);

        if (pickupSound != null)
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);

        Destroy(gameObject);
    }
}
