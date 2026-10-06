using UnityEngine;
using Gamekit2D;

/// <summary>
/// Added to the player automatically by SpeedBoostPickup. Multiplies maxSpeed for a
/// few seconds, then restores it. Picking up another boost just refreshes the timer.
/// </summary>
public class SpeedBoostEffect : MonoBehaviour
{
    private PlayerCharacter player;
    private float originalSpeed;
    private float timeLeft;
    private bool active;

    public static void Apply(PlayerCharacter target, float multiplier, float duration)
    {
        SpeedBoostEffect effect = target.GetComponent<SpeedBoostEffect>();
        if (effect == null)
            effect = target.gameObject.AddComponent<SpeedBoostEffect>();

        effect.Begin(target, multiplier, duration);
    }

    private void Begin(PlayerCharacter target, float multiplier, float duration)
    {
        player = target;

        // Only remember the original speed the first time, so boosts never stack up.
        if (!active)
            originalSpeed = player.maxSpeed;

        player.maxSpeed = originalSpeed * multiplier;
        timeLeft = duration;
        active = true;
    }

    private void Update()
    {
        if (!active) return;

        timeLeft -= Time.deltaTime; // scaled time, so it pauses with the game
        if (timeLeft <= 0f)
            End();
    }

    private void End()
    {
        player.maxSpeed = originalSpeed;
        active = false;
    }

    private void OnDisable()
    {
        if (active && player != null)
            End();
    }
}
