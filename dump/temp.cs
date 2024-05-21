using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    public Transform targetPosition; // Target position to move towards
    public float moveDuration = 2f; // Duration of the movement

    void Start()
    {
        // Start the movement coroutine
        StartCoroutine(MoveObject(transform, targetPosition.position, moveDuration));
    }

    IEnumerator MoveObject(Transform objectToMove, Vector3 targetPosition, float duration)
    {
        float elapsedTime = 0f;
        Vector3 startingPosition = objectToMove.position;

        while (elapsedTime < duration)
        {
            // Calculate the interpolation factor (0 to 1)
            float t = Mathf.Clamp01(elapsedTime / duration);

            // Move the object towards the target position using Lerp
            objectToMove.position = Vector3.Lerp(startingPosition, targetPosition, t);

            // Update the elapsed time
            elapsedTime += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Ensure the object reaches the exact target position
        objectToMove.position = targetPosition;
    }
}