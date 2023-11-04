using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMove : MonoBehaviour
{
    [SerializeField] private bool isRepeatedMovement = false;
    [SerializeField] private float moveDuration = 0.3f;
    [SerializeField] private float gridSize = 1f;
    private bool isMoving = false;

    private IEnumerator Move(Vector2 direction)
    {
        isMoving = true;

        Vector2 startPosition = transform.position;
        Vector2 endPosition = startPosition + (direction * gridSize);

        float elapsedTime = 0;
        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float percent = elapsedTime / moveDuration;
            transform.position = Vector2.Lerp(startPosition, endPosition, percent);
            yield return null;
        }

        transform.position = endPosition;

        isMoving = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (!isMoving)
        {
            System.Func<KeyCode, bool> inputFunction;
            if (isRepeatedMovement)
            {
                inputFunction = Input.GetKey;
            }
            else
            {
                inputFunction = Input.GetKeyDown;
            }

            if (inputFunction(KeyCode.UpArrow))
            {
                StartCoroutine(Move(Vector2.up));
            }
            else if (inputFunction(KeyCode.DownArrow))
            {
                StartCoroutine(Move(Vector2.down));
            }
            else if (inputFunction(KeyCode.RightArrow))
            {
                StartCoroutine(Move(Vector2.right));
            }
            else if (inputFunction(KeyCode.LeftArrow))
            {
                StartCoroutine(Move(Vector2.left));
            }
        }
    }
}
