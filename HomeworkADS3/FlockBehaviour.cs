using System.Collections.Generic;
using UnityEngine;

public abstract class FlockBehaviour : ScriptableObject
{
    public abstract Vector2 CalculateMove(Drone agent, List<Transform> context, Flock flock);
}


