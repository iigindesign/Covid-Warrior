﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
  public GameObject main;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name.Equals("player")) {
          main.GetComponent<ChoiceScript>().newQuestion();
        }
        //TODO: also will want to lock movement and otherwise pause the game
        //have question show now but not before this
    }
}
