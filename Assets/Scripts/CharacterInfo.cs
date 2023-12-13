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
    public OverlayTile standingOnTile;

    public OverlayTile getCharTile()
    {
        return standingOnTile;
    }

    void Start()
    {
        hitPoints = startingHitPoints;
        gameOverScene = "GameOver";
    }

    public void TakeHit(int damage)
    {
        if (hitPoints - damage <= 0)
        {
            Debug.Log("Game Over");
            SceneManager.LoadScene(gameOverScene);
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
        for (int i = 0; i < length; i++)
        {
            transform.localPosition += new Vector3(0.06f, 0, 0);
            yield return new WaitForSeconds(0.01f);
            transform.localPosition -= new Vector3(0.06f, 0, 0);
            yield return new WaitForSeconds(0.01f);
        }
    }
}