using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{

    public Image playGameImage;
    public Image creditsImage;
    public Image quitGameImage;

    private int menuPointer = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) menuPointer--;
        if (Input.GetKeyDown(KeyCode.S)) menuPointer++;

        if(menuPointer < 0) menuPointer = 0;
        if(menuPointer > 2) menuPointer = 2;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S)) {
            if(menuPointer == 0) {
                playGameImage.sprite = Resources.Load<Sprite>("MenuActive_PlayGame");
                creditsImage.sprite = Resources.Load<Sprite>("MenuInactive_Credits");
                quitGameImage.sprite = Resources.Load<Sprite>("MenuInactive_Quit");
            } else if(menuPointer == 1) {
                playGameImage.sprite = Resources.Load<Sprite>("MenuInactive_PlayGame");
                creditsImage.sprite = Resources.Load<Sprite>("MenuActive_Credits");
                quitGameImage.sprite = Resources.Load<Sprite>("MenuInactive_Quit");
            } else if(menuPointer == 2) {
                playGameImage.sprite = Resources.Load<Sprite>("MenuInactive_PlayGame");
                creditsImage.sprite = Resources.Load<Sprite>("MenuInactive_Credits");
                quitGameImage.sprite = Resources.Load<Sprite>("MenuActive_Quit");
            }
        } else if (Input.anyKeyDown && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S)) {
            if (menuPointer == 0) GameManager.GotoScene("Intro");
            else if (menuPointer == 1) GameManager.GotoScene("Credits");
            else if (menuPointer == 2) GameManager.QuitGame();
        }
    }
}
