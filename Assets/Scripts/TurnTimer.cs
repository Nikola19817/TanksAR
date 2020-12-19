using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnTimer : MonoBehaviour
{
    [SerializeField]
    public float turnTime = 20f;

    // TIMER VARIABLES 
    float timer;
    bool isActivated = true;

    void Start()
    {
        ResetTimer();
    }
    void Update()
    {
        if(isActivated)
            timer -= Time.deltaTime;
        this.gameObject.GetComponent<Text>().text = timer.ToString();
        if (timer <= 10f)
            this.gameObject.GetComponent<Text>().color = Color.red;
        else
            this.gameObject.GetComponent<Text>().color = Color.white;
        if (timer <= 0)
        {
            GameObject.Find("UI").GetComponent<GameController>().EndTurn();
            ResetTimer();
        }
    }
    public void ResetTimer()
    {
        timer = turnTime;
        this.gameObject.GetComponent<Text>().text = (Convert.ToInt32(timer)).ToString();
        isActivated = true;
    }
    public void ActivateTimer()
    {
        isActivated = true;
    }
    public void DeactivateTimer()
    {
        isActivated = false;
    }
    // Returns the current status of the timer on/off
    public bool GetIsActivated()
    {
        return isActivated;
    }
}
