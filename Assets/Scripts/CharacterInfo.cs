using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterInfo : MonoBehaviour
{
    [SerializeField] private float startingHitPoints = 0f;
    [SerializeField] public float hitPoints = 0f;
    [SerializeField] private string gameOverScene;
    [SerializeField] private GameObject turnManager;
    [SerializeField] AudioClip death;
    public OverlayTile standingOnTile;
    private bool notDead = true;

    public OverlayTile getCharTile()
    {
        return standingOnTile;
    }

    void Start()
    {
        hitPoints = startingHitPoints;
        turnManager = GameObject.Find("TurnManager");
        gameOverScene = "GameOver";
    }

    IEnumerator Fade(float length, GameObject obj1)
    {
        while (obj1.GetComponent<SpriteRenderer>().color.a > 0)
        {
            obj1.GetComponent<SpriteRenderer>().color = new Color(obj1.GetComponent<SpriteRenderer>().color.r, obj1.GetComponent<SpriteRenderer>().color.g, obj1.GetComponent<SpriteRenderer>().color.b, obj1.GetComponent<SpriteRenderer>().color.a - (2.5f * Time.deltaTime / length));
            yield return null;
        }
        yield return new WaitForSeconds(0.8f);
        SceneManager.LoadScene(gameOverScene);
    }

    public void TakeHit(int damage)
    {
        if (hitPoints - damage <= 0 && notDead)
        {
            notDead = false;
            //SceneManager.LoadScene(gameOverScene);
            Debug.Log("Game Over");
            StartCoroutine(Shake(128));
            turnManager.GetComponent<TurnManager>().isEnemyTurn = false;
            turnManager.GetComponent<TurnManager>().hitPointsText.SetText("HP: DEATH");
            turnManager.GetComponent<TurnManager>().hitPointsTextShadow.SetText("HP: DEATH");
            GetComponent<AudioSource>().PlayOneShot(death);
            StartCoroutine(Fade(7, this.gameObject));
        }
        hitPoints -= damage;
        StartCoroutine(Shake(12));
        Debug.Log("Remaining HP" + hitPoints);

        if (hitPoints <= (startingHitPoints * 0.25f)) { this.GetComponent<SpriteRenderer>().color = new Color(1f, 0.5f, 0.5f, 1f); }
        else if (hitPoints <= (startingHitPoints * 0.50f)) { this.GetComponent<SpriteRenderer>().color = new Color(1f, 0.65f, 0.65f, 1f); }
        else if (hitPoints <= (startingHitPoints * 0.75f)) { this.GetComponent<SpriteRenderer>().color = new Color(1f, 0.8f, 0.8f, 1f); }

    }

    IEnumerator Shake(int length)
    {
        yield return new WaitForSeconds(0.8f);
        for (int i = 0; i < length; i++)
        {
            transform.localPosition += new Vector3(0.06f, 0, 0);
            yield return new WaitForSeconds(0.01f);
            transform.localPosition -= new Vector3(0.06f, 0, 0);
            yield return new WaitForSeconds(0.01f);
        }
    }
}