using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public bool isDestroyed;
    public int state; // Reflects to what is in ObstacleState
}

enum ObstacleState {
    Damage = 0,
    Health = 1
}