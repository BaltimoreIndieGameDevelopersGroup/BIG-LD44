using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public bool isDestroyed;
    public int state; // Reflects to what is in ObstacleState

    public string specialCase;

    public AudioClip collisionSound;
}

enum ObstacleState {
    Death = -1,
    Damage = 0,
    Health = 1
}