using StarterAssets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private GameScreenResult gameScreenResult;
    [SerializeField] private ThirdPersonController thirdPersonController;
    [SerializeField] private Transform player;
    public Transform Player { get => player; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Start()
    {
        SetCursorState(false);
        Time.timeScale = 1;
    }

    public void ShowWin() => ShowGameResult("You Escaped the Guards!");

    public void ShowLose() => ShowGameResult("You've Been Caught!");

    private void ShowGameResult(string message)
    {
        gameScreenResult.DisplayResult(message);
        Time.timeScale = 0;
        SetCursorState(true);
        PlayerControllerStatus(false);
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        gameScreenResult.HideResult();
        SetCursorState(false);
        PlayerControllerStatus(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void SetCursorState(bool isVisible)
    {
        Cursor.visible = isVisible;
        Cursor.lockState = isVisible ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private void PlayerControllerStatus(bool status)
    {
        if (thirdPersonController)
        {
            thirdPersonController.enabled = status;
        }
    }
}