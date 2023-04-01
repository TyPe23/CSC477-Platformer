using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeLevelToEnd : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.CompareTag("Player"))
        {
            SceneManager.LoadScene("EndMenu");
        }
    }
}
