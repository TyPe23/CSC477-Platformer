using UnityEngine;

public class ChangeQuitEditor : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.CompareTag("Player"))
        {
            Application.Quit();
        }
    }
}
