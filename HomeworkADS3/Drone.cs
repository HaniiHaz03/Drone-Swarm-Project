using UnityEngine;

public class Drone : MonoBehaviour
{
    public float speed = 2f;
    private Vector2 targetPosition;

    void Start()
    {
        GetNewTargetPosition();
    }

    void Update()
    {
        MoveToTarget();
    }

    void GetNewTargetPosition()
    {
        targetPosition = new Vector2(
            Random.Range(-10f, 10f),
            Random.Range(-5f, 5f)
        );
    }

    void MoveToTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            GetNewTargetPosition();
        }
    }
}
