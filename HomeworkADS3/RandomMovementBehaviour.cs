using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Random Movement")]
public class RandomMovementBehavior : FlockBehaviour
{
    public override Vector2 CalculateMove(Drone agent, List<Transform> context, Flock flock)
    {
        // Random movement logic
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
    }
}

