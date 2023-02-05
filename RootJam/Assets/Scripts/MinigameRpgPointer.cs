using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Random;
using TMPro;

public class MinigameRpgPointer : MonoBehaviour
{

    private int gridPositionX = 0;
    private int gridPositionY = 0;

    private int chooseThisValue = -1;
    public GameObject heartRateMonitor;

    public AudioSource menuSound;

    public TextMeshProUGUI textHint;
    private bool beatThisGame = false;

    // Start is called before the first frame update
    void Start()
    {
        chooseThisValue = Range(0, 6);

        var textCmp = textHint.GetComponent<TMP_Text>();
        if(chooseThisValue == 0) textCmp.text = "Choose \"Item!\"";
        if(chooseThisValue == 1) textCmp.text = "Choose \"Jump!\"";
        if(chooseThisValue == 2) textCmp.text = "Choose \"Run!\"";
        if(chooseThisValue == 3) textCmp.text = "Choose \"Kiss!\"";
        if(chooseThisValue == 4) textCmp.text = "Choose \"Defend!\"";
        if(chooseThisValue == 5) textCmp.text = "Choose \"Attack!\"";

        while(GetPositionValue() == chooseThisValue) {
            gridPositionX = Random.Range(0, 3);
            gridPositionY = Random.Range(0, 2);
        }
    }

    // Update is called once per frame
    void Update()
    {
        var heartRateMonitorScr = heartRateMonitor.GetComponent<HeartRateMonitor>();
        if(!beatThisGame) heartRateMonitorScr.UpdateTimer();

        if (Input.GetKeyDown(KeyCode.A)) gridPositionX--;
        if (Input.GetKeyDown(KeyCode.D)) gridPositionX++;
        if (Input.GetKeyDown(KeyCode.W)) gridPositionY--;
        if (Input.GetKeyDown(KeyCode.S)) gridPositionY++;

        if (Input.anyKeyDown)
        {
            menuSound.Play();
        }

        if(gridPositionX < 0) gridPositionX = 0;
        if(gridPositionX > 2) gridPositionX = 2;
        if(gridPositionY < 0) gridPositionY = 0;
        if(gridPositionY > 1) gridPositionY = 1;

        var finalPositionX = 0;
        if(gridPositionX == 0) finalPositionX = 89;
        if(gridPositionX == 1) finalPositionX = 361;
        if(gridPositionX == 2) finalPositionX = 627;

        var finalPositionY = 0;
        if(gridPositionY == 0) finalPositionY = 154;
        if(gridPositionY == 1) finalPositionY = 90;
        
        gameObject.transform.position = new Vector3(finalPositionX, finalPositionY, 0);

        if(Input.GetKeyDown(KeyCode.Space)) {
            if(CheckForCorrectAction()) {
                GameManager.MinigameComplete();
                beatThisGame = true;
            } else {
                GameManager.MinigameDeath();
            }
        }
    }

    private bool CheckForCorrectAction()
    {
        return GetPositionValue() == chooseThisValue;
    }

    private int GetPositionValue() {
        return (3*gridPositionY) + gridPositionX;
    }
}

