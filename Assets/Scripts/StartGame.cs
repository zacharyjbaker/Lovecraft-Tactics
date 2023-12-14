using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    [SerializeField] private string gameScene;
    [SerializeField] private AudioClip select;
    [SerializeField] private GameObject title;
    private bool hasStarted = false;

    void Update()
    {
        var start = Input.GetKey(KeyCode.Space);
        var q = Input.GetKey(KeyCode.Q);

        IEnumerator Fade(float length, GameObject obj1)
        {
            while (obj1.GetComponent<SpriteRenderer>().color.a > 0) 
            {
                obj1.GetComponent<SpriteRenderer>().color = new Color(obj1.GetComponent<SpriteRenderer>().color.r, obj1.GetComponent<SpriteRenderer>().color.g, obj1.GetComponent<SpriteRenderer>().color.b, obj1.GetComponent<SpriteRenderer>().color.a - (2.5f * Time.deltaTime / length));
                this.GetComponent<TextMeshProUGUI>().color = new Color(this.GetComponent<TextMeshProUGUI>().color.r, this.GetComponent<TextMeshProUGUI>().color.g, this.GetComponent<TextMeshProUGUI>().color.b, this.GetComponent<TextMeshProUGUI>().color.a - (2.5f * Time.deltaTime / length));
                yield return null;
            }
            yield return new WaitForSeconds(0.8f);
            SceneManager.LoadScene(gameScene);
        }

        if (start)
        {
            if (!hasStarted)
            {
                hasStarted = true;
                GetComponent<AudioSource>().PlayOneShot(select);
                StartCoroutine(Fade(5, title));
            }
        }

        if (q)
        {
            Application.Quit();
        }
    }
}
