using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Alert : MonoBehaviour
{
    private Image alertImage;
    [SerializeField] private Enemy enemy;
    [SerializeField] private ConeDetector coneDetector;
    private float alertTimer;
    private Coroutine fillAlertCoroutine;
    private void OnEnable()
    {
        coneDetector.OnPlayerDetected += OnPlayerFound;
    }

    private void OnDisable()
    {
        coneDetector.OnPlayerDetected -= OnPlayerFound;
    }

    private void Awake()
    {
        if (alertImage == null)
            alertImage = GetComponent<Image>();

        alertTimer = enemy.GetDetectTimer();
    }

    private void OnPlayerFound(bool playerDetected)
    {
        if (playerDetected)
        {
            if (fillAlertCoroutine != null)
                StopCoroutine(fillAlertCoroutine);

            fillAlertCoroutine = StartCoroutine(FillAlert());
        }
        else
        {
            if (fillAlertCoroutine != null)
            {
                StopCoroutine(fillAlertCoroutine);
                fillAlertCoroutine = null;
            }

            alertImage.fillAmount = 0;
        }
    }

    private IEnumerator FillAlert()
    {
        float elapsedTime = 0f;
        while (elapsedTime < alertTimer)
        {
            alertImage.fillAmount = elapsedTime / alertTimer;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        alertImage.fillAmount = 1f;
    }
}