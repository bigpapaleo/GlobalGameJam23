using System;
using TMPro;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class Intermezzo : MonoBehaviour
{
    public TMP_Text controlsText;
    public TMP_Text timeremainingText;
    double totalTime = 3;
    public AudioSource threeSound;
    public AudioSource twoSound;
    public AudioSource oneSound;
    private bool threeSoundPlayed;
    private bool twoSoundPlayed;
    private bool oneSoundPlayed;

    private bool countdown = false;

    public VideoPlayer prizePlayer;
    public VideoPlayer cameraPlayer;
    public VideoPlayer gamesPlayer;

    public GameObject hideythings;
    public GameObject wButton;
    public GameObject sButton;
    public GameObject aButton;
    public GameObject dButton;
    public GameObject spaceButton;

    private void Awake()
    {
        totalTime = 3;
        countdown = false;

        VideoPlayer player = FindCorrectVideoPlayer();
        player.Play();
        hideythings.SetActive(false);
        player.loopPointReached += (VideoPlayer player) => countdown = true;
    }

    public VideoPlayer FindCorrectVideoPlayer()
    {
        switch (GameManager.GetCurrentMinigame().Scene)
        {
            case "Minigame_Focus":
            case "Minigame_Phone":
                print("Plaing camera");
                return cameraPlayer;
            case "Minigame_RPG":
            case "Minigame_SFScene":
                print("Plaing game");
                return gamesPlayer;
            case "Minigame_Crane":
            default:
                print("Plaing cramne");
                return prizePlayer;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (!countdown) return;
        hideythings.SetActive(true);
        var thisMinigameControls = GameManager.GetCurrentMinigame().UsedButtons.ToUpper();
        if (!thisMinigameControls.Contains('W')) wButton.SetActive(false);
        if (!thisMinigameControls.Contains('S')) sButton.SetActive(false);
        if (!thisMinigameControls.Contains('A')) aButton.SetActive(false);
        if (!thisMinigameControls.Contains('D')) dButton.SetActive(false);
        if (!thisMinigameControls.Contains(' ')) spaceButton.SetActive(false);

        totalTime -= Time.deltaTime;
        GameManager.SetTime(Math.Ceiling(totalTime).ToString());
        timeremainingText.text = GameManager.TimerText;

        if (totalTime < 0)
        {
            GameManager.IntermezzoComplete();
        }
        else if (totalTime <= 3)
        {
            GameManager.SetTime(Math.Ceiling(totalTime).ToString());
            if(totalTime < 3 && !threeSoundPlayed) {
                threeSound.Play();
                threeSoundPlayed = true;
            }
            if(totalTime < 2 && !twoSoundPlayed) {
                twoSound.Play();
                twoSoundPlayed = true;
            }
            if(totalTime < 1 && !oneSoundPlayed) {
                oneSound.Play();
                oneSoundPlayed = true;
            }
        }
        else
        {
            GameManager.SetTime(string.Empty);
        }
    }
}
