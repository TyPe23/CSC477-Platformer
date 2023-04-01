using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeLevel1 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.CompareTag("Player"))
        {
            SceneManager.LoadScene("Level 1");
        }
    }
}
