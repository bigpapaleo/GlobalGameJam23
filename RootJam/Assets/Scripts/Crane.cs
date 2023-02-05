using UnityEngine;
using System;
using static UnityEngine.Random;
using UnityEngine.UI;

public class Crane : MonoBehaviour
{

    bool clawActivated = false;
    int autoMoveState = 0;
    float startY = 920f;
    float targetX = 300f;
    float toyGravity = 0f;

    int toyRandomPositionX = -1;
    int clawRandomPositionX = -1;

    private int clawSpeed = 20;

    public GameObject heartRateMonitor;

    // Start is called before the first frame update
    void Start()
    {
        toyRandomPositionX = Range(500, 1520);
        clawRandomPositionX = Range(300, 1520);

        while(Math.Abs(toyRandomPositionX-clawRandomPositionX) < 400) {
            toyRandomPositionX = Range(500, 1520);
            clawRandomPositionX = Range(300, 1520);
        }

        var toy = GameObject.Find("Toy");
        toy.transform.position =
            new Vector3(toyRandomPositionX, toy.transform.position.y, toy.transform.position.z);
        gameObject.transform.position =
            new Vector3(clawRandomPositionX, gameObject.transform.position.y, gameObject.transform.position.z);

        GameManager.SetTime("3");
    }

    // Update is called once per frame
    void Update()
    {
        var heartRateMonitorScr = heartRateMonitor.GetComponent<HeartRateMonitor>();
        if(!clawActivated || autoMoveState == 0) heartRateMonitorScr.UpdateTimer();
    }

    void FixedUpdate() {
        if(!clawActivated) {
            if (Input.GetKey(KeyCode.A) && gameObject.transform.position.x > 300)
            {
                gameObject.transform.Translate(-1*clawSpeed, 0, 0);
            } else if (Input.GetKey(KeyCode.D) && gameObject.transform.position.x < 1520)
            {
                gameObject.transform.Translate(clawSpeed, 0, 0);
            } else if (Input.GetKey(KeyCode.Space))
            {
                Debug.Log(gameObject.transform.position.y);
                clawActivated = true;
            }
        } else {
            if(autoMoveState == 0) gameObject.transform.Translate(0, -1*clawSpeed, 0);
            if(autoMoveState == 1) gameObject.transform.Translate(0, clawSpeed, 0);
            if(autoMoveState == 2) gameObject.transform.Translate(-1*clawSpeed, 0, 0);

            if(autoMoveState == 0 && gameObject.transform.position.y < -0.0) {
                // Try again
                GameManager.MinigameDeath();
                clawActivated = false;
                gameObject.transform.position =
                    new Vector3(gameObject.transform.position.x, startY, gameObject.transform.position.z);
            }

            if(autoMoveState == 1 && gameObject.transform.position.y >= startY) {
                autoMoveState = 2;
            } else if(autoMoveState == 2 && gameObject.transform.position.x <= targetX) {
                autoMoveState = 3;
            }

            if(autoMoveState > 0 && autoMoveState < 3) {
                var toy = GameObject.Find("Toy");
                toy.transform.position =
                    new Vector3(gameObject.transform.position.x+10, gameObject.transform.position.y-200, gameObject.transform.position.z);
            } else if(autoMoveState == 3) {
                toyGravity -= 1f;
                var toy = GameObject.Find("Toy");
                toy.transform.Translate(0, toyGravity, 0);

                if(toy.transform.position.y < 0) {
                    GameManager.MinigameComplete();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.name == "Toy")
        {
            autoMoveState = 1;
            gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Crane/ClawClosed");
        }
    }
}
