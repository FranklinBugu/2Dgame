using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    public Transform player; // drag Ellen here in the Inspector

    private Collider2D finishBox;
    private bool won = false;

    private void Start()
    {
        finishBox = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (!won && finishBox.OverlapPoint(player.position))
        {
            won = true;
            // Shows the end screen (and freezes the game) with Restart button
            GameFlowManager.Instance.EndGame("YOU WIN!");
        }
    }
}
