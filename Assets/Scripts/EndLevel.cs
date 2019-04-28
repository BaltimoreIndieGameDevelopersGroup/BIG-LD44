using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel : MonoBehaviour
{
    GameEvents gameEvents;

    void Awake()
    {
        gameEvents = GameObject.Find("GameManager").GetComponent<GameEvents>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
            gameEvents.LevelComplete();
    }
}
