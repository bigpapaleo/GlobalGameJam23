using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeartRateMonitor : MonoBehaviour
{

    public bool countdownActive;

    private float timer = 0;
    public int timeForThisMinigame = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(countdownActive) UpdateTimer();

        var livesText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        livesText.text = "" + GameManager.Lives;

        var blackoutBar = gameObject.GetComponentInChildren<RawImage>();
        blackoutBar.transform.localScale = new Vector3( timer / timeForThisMinigame * 1.0f, 1.0f, 1.0f);
    }

    public void UpdateTimer() {
        timer += Time.deltaTime;
        float seconds = timeForThisMinigame - (timer % 60);
        int iSeconds = (int)seconds;
        GameManager.SetTime("Time Remaining: " + iSeconds);
        if(seconds < 0) {
            GameManager.MinigameDeath();
        }
    }
}
