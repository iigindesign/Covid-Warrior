﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Treasure_Chest : MonoBehaviour
{
    public GameObject canvas;
    public GameObject promptBckgd;
    public GameObject promptText;
    public GameObject factBckgd;
    public GameObject factText;

    void OnTriggerEnter2D(Collider2D col) {
      if (col.gameObject.name.Equals("player")) {
        promptBckgd.SetActive(true);
        promptText.SetActive(true);
        canvas.SetActive(true);
      }
    }

    /*
    private Canvas fun_fact;
    private bool fun_fact_active = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E)) {
            fun_fact_active = !fun_fact_active;
            fun_fact.transform.SetActive(fun_fact);
        }
    }*/
}

// want window to close with a different key
// one key to open chest, another key to close pop up
