using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameScreenResult : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gameResultText;
    [SerializeField] private Canvas gameResultCanvas;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button restartButton;

    private void Start()
    {
        exitButton.onClick.AddListener(GameManager.Instance.ExitGame);
        restartButton.onClick.AddListener(GameManager.Instance.RestartGame);
    }

    private void OnDestroy()
    {
        exitButton.onClick.RemoveListener(GameManager.Instance.ExitGame);
        restartButton.onClick.RemoveListener(GameManager.Instance.RestartGame);
    }

    public void DisplayResult(string message)
    {
        gameResultText.text = message;
        gameResultCanvas.enabled = true;
    }

    public void HideResult()
    {
        gameResultCanvas.enabled = false;
    }
}