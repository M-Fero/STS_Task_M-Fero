using UnityEngine;

public class EscapePoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if (!collider.gameObject.CompareTag("Player")) return;
        Debug.Log("Collision Detected with Player!");

        GameManager.Instance.ShowWin();
    }
}
