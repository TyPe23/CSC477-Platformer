using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeQuitEditor : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.CompareTag("Player"))
        {

            // Closes editor. Be sure to save!
            UnityEditor.EditorApplication.isPlaying = false;
            // Closes .exe (which we don't have yet) 
            //Application.Quit();
        }
    }
}
