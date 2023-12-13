using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartOver : MonoBehaviour
{
    [SerializeField] private string startScene;

    void Update()
    {
        var restart = Input.GetKey(KeyCode.Space);

        if (restart)
        {
            SceneManager.LoadScene(startScene);
        }
    }
}
