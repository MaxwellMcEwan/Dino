using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private GameObject player;
    private Vector3 playerPos;
    private Camera cam;

    private GameEvents gameEvents;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        gameEvents = FindObjectOfType<GameEvents>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = FindObjectOfType<PlayerController>().gameObject;
        }

        playerPos = player.transform.position;
        transform.position = new Vector3(playerPos.x, playerPos.y, -5);

        cam.orthographicSize = player.transform.localScale.x * 2;
    }
}