using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles Start screen -> Playing -> End screen -> Restart.
/// Attach to an empty GameObject in your game scene and wire up the references.
/// </summary>
public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager Instance { get; private set; }

    [Header("UI Panels (child objects of a Canvas)")]
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject endPanel;

    [Header("Optional")]
    [Tooltip("Shown on the end screen, e.g. 'You Win!' / 'Game Over'. Needs a TMPro or UI Text component.")]
    [SerializeField] private TMPro.TMP_Text endMessageText;

    // Survives scene reloads so Restart skips the start screen and goes straight to play.
    private static bool skipStartScreen = false;

    public bool IsPlaying { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        endPanel.SetActive(false);

        if (skipStartScreen)
        {
            skipStartScreen = false;
            BeginPlay();
        }
        else
        {
            startPanel.SetActive(true);
            IsPlaying = false;
            Time.timeScale = 0f; // freeze the game behind the start screen
        }
    }

    // ---- Hook this to the Play button's OnClick ----
    public void OnPlayClicked()
    {
        BeginPlay();
    }

    // ---- Hook this to the Restart button's OnClick ----
    public void OnRestartClicked()
    {
        Time.timeScale = 1f;
        skipStartScreen = true; // set to false if you want the start screen again after restart
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Call this from your own code when the player wins or loses:
    /// GameFlowManager.Instance.EndGame("Game Over");
    /// </summary>
    public void EndGame(string message = "Game Over")
    {
        if (!IsPlaying) return;

        IsPlaying = false;
        Time.timeScale = 0f;

        if (endMessageText != null) endMessageText.text = message;
        endPanel.SetActive(true);
    }

    /// <summary>
    /// Like EndGame, but waits first (in game time) so a death animation can play.
    /// </summary>
    public void EndGameDelayed(string message, float delay)
    {
        if (!IsPlaying) return;
        StartCoroutine(EndGameAfter(message, delay));
    }

    private System.Collections.IEnumerator EndGameAfter(string message, float delay)
    {
        yield return new WaitForSeconds(delay);
        EndGame(message);
    }

    private void BeginPlay()
    {
        startPanel.SetActive(false);
        endPanel.SetActive(false);
        Time.timeScale = 1f;
        IsPlaying = true;
    }
}
