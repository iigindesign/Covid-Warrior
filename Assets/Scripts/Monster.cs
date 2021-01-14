﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Monster : MonoBehaviour
{
    public GameObject main;
    public GameObject virtualCam;
    public Button attack;

    public int health = 10;

    private GameObject player;

    void Awake() {
      player = GameObject.Find("player");
      attack.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
      if (health <= 0) {
        virtualCam.SetActive(false);
        player.GetComponent<Player>().updateUser();
        player.GetComponent<PlayerMovement>().setBattle(false);
        attack.interactable = false;
        attack.gameObject.SetActive(false);
        Destroy(this.gameObject);
      }
    }

    void FixedUpdate() {
      Vector3 euler = transform.eulerAngles;
      if (euler.z > 180) euler.z = euler.z - 360;
      euler.z = Mathf.Clamp(euler.z, -10, 10);
      transform.eulerAngles = euler;

      if (GetComponent<Renderer>().isVisible) {
        float dist = Vector3.Distance(player.transform.position, transform.position);
        if (dist <= 10.0f) {
          attack.interactable = true;
          attack.GetComponentInChildren<TextMeshProUGUI>().text = "Attack";
        }
        else {
          attack.interactable = false;
          attack.GetComponentInChildren<TextMeshProUGUI>().text = "Move closer to attack";
        }
      }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name.Equals("player")) {
          main.GetComponent<ChoiceScript>().newQuestion();
          PlayerMovement.gameIsPaused = true;
        }
    }

    void OnBecameVisible() {
      //note that this also triggers if you have editor window open and you can see the monster
      //but it should be fine for actual gameplay
      virtualCam.SetActive(true);
      attack.gameObject.SetActive(true);
      GetComponent<MonsterMove>().enabled = true;
    }
}
