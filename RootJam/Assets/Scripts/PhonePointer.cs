using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PhonePointer : MonoBehaviour
{
    // Start is called before the first frame update
    private int gridPositionX = 0;
    private int gridPositionY = 0;

    private string correctNumber = "7668";
    private string playerNumber = "";

    public AudioSource phoneBeep1;
    public AudioSource phoneBeep2;
    public AudioSource phoneBeep3;

    private bool thisMinigameDead;

    public TextMeshProUGUI phoneSoFar;
    
    public GameObject heartRateMonitor;
    private bool beatThisGame;

    // Start is called before the first frame update
    void Start()
    {
        gridPositionX = Random.Range(0, 3);
        gridPositionY = Random.Range(0, 4);
    }

    // Update is called once per frame
    void Update()
    {
        var heartRateMonitorScr = heartRateMonitor.GetComponent<HeartRateMonitor>();
        if(!beatThisGame) heartRateMonitorScr.UpdateTimer();

        /*
        if (Input.GetKey(KeyCode.F))
        {
            gameObject.transform.Translate(-2, 0, 0);
        } else if (Input.GetKey(KeyCode.H))
        {
            gameObject.transform.Translate(2, 0, 0);
        } else if (Input.GetKey(KeyCode.T))
        {
            gameObject.transform.Translate(0, 2, 0);
        } else if (Input.GetKey(KeyCode.G))
        {
            gameObject.transform.Translate(0, -2, 0);
        }else if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("X:" + gameObject.transform.position.x);
            Debug.Log("Y:" + gameObject.transform.position.y);
        }
        */

        if (Input.GetKeyDown(KeyCode.A)) gridPositionX--;
        if (Input.GetKeyDown(KeyCode.D)) gridPositionX++;
        if (Input.GetKeyDown(KeyCode.W)) gridPositionY--;
        if (Input.GetKeyDown(KeyCode.S)) gridPositionY++;

        if(gridPositionX < 0) gridPositionX = 0;
        if(gridPositionX > 2) gridPositionX = 2;
        if(gridPositionY < 0) gridPositionY = 0;
        if(gridPositionY > 3) gridPositionY = 3;

        var finalPositionX = 0;
        if(gridPositionX == 0) finalPositionX = 750;
        if(gridPositionX == 1) finalPositionX = 960;
        if(gridPositionX == 2) finalPositionX = 1170;

        var finalPositionY = 0;
        if(gridPositionY == 0) finalPositionY = 709;
        if(gridPositionY == 1) finalPositionY = 521;
        if(gridPositionY == 2) finalPositionY = 351;
        if(gridPositionY == 3) finalPositionY = 137;
        
        gameObject.transform.position = new Vector3(finalPositionX, finalPositionY, 0);

        if(Input.GetKeyDown(KeyCode.Space)) {
            AddNumberToString();
            PlayPhoneBeep();
            if(CheckForCorrectAction()) {
                GameManager.MinigameComplete();
                beatThisGame = true;
            } else if(playerNumber.Length > correctNumber.Length) {
                // Try again
                GameManager.MinigameDeath();
                playerNumber = "";
                var textCmp = phoneSoFar.GetComponent<TMP_Text>();
                textCmp.text = playerNumber;
            }
        }
    }

    private bool CheckForCorrectAction()
    {
        return correctNumber.Equals(playerNumber);
    }

    private void AddNumberToString() {
        string appendMe = "";
        var myPosition = GetPositionValue();
        if(myPosition == 0) appendMe = "1";
        if(myPosition == 1) appendMe = "2";
        if(myPosition == 2) appendMe = "3";
        if(myPosition == 3) appendMe = "4";
        if(myPosition == 4) appendMe = "5";
        if(myPosition == 5) appendMe = "6";
        if(myPosition == 6) appendMe = "7";
        if(myPosition == 7) appendMe = "8";
        if(myPosition == 8) appendMe = "9";
        if(myPosition == 9) appendMe = "*";
        if(myPosition == 10) appendMe = "0";
        if(myPosition == 11) appendMe = "#";
        playerNumber = playerNumber + appendMe;

        var textCmp = phoneSoFar.GetComponent<TMP_Text>();
        textCmp.text = playerNumber;
    }

    private void PlayPhoneBeep() {
        var myPosition = GetPositionValue();
        if(myPosition == 0) phoneBeep1.Play();
        if(myPosition == 1) phoneBeep2.Play();
        if(myPosition == 2) phoneBeep3.Play();
        if(myPosition == 3) phoneBeep1.Play();
        if(myPosition == 4) phoneBeep2.Play();
        if(myPosition == 5) phoneBeep3.Play();
        if(myPosition == 6) phoneBeep1.Play();
        if(myPosition == 7) phoneBeep2.Play();
        if(myPosition == 8) phoneBeep3.Play();
        if(myPosition == 9) phoneBeep1.Play();
        if(myPosition == 10) phoneBeep2.Play();
        if(myPosition == 11) phoneBeep3.Play();     
    }

    private int GetPositionValue() {
        return (3*gridPositionY) + gridPositionX;
    }
}
