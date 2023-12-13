using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    [SerializeField] private string gameScene;

    void Update()
    {
        var start = Input.GetKey(KeyCode.Space);
        var q = Input.GetKey(KeyCode.Q);

        if (start)
        {
            SceneManager.LoadScene(gameScene);
        }

        if (q)
        {
            Application.Quit();
        }
    }
}
