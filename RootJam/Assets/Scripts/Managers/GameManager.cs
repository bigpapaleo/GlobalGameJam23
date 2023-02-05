using Assets.Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static int Lives { get; private set; } = 3;
    public static string TimerText { get; private set; } = string.Empty;
    private static int minigamesCompleted = 0;

    private static bool MinigameLockout = false;


    public static AudioSource MinigameWin;
    public static AudioSource MinigameLoss;
    public static AudioSource BigGameWin;

    public AudioSource minigameWin;
    public AudioSource minigameLoss;
    public AudioSource bigGameWin;

    public TMP_Text livesField;
    public TMP_Text timeField;

    public static GameObject me;

    public const string MAIN_MENU = "MainMenu";
    public const string INTERMEZZO_SCENE = "InBetween";
    static List<Minigame> minigames;

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
        bool dead = false;
        foreach (GameManager otherManagers in FindObjectsOfType<GameManager>())
        {
            if (otherManagers.gameObject != gameObject)
            {
                dead = true;
                Destroy(gameObject);
            }
        }

        if (!dead)
        {
            MinigameWin = minigameWin;
            MinigameLoss = minigameLoss;
            me = gameObject;
            BigGameWin = bigGameWin;
        }
    }


    public static void QuitGame()
    {
        Application.Quit();
    }

    public static void StartGame()
    {
        minigames = new List<Minigame>()
        {
            new Minigame(){ Scene = "Minigame_Focus", UsedButtons = "ad "},
            new Minigame(){ Scene = "Minigame_RPG", UsedButtons = "wasd "},
            new Minigame(){ Scene = "Minigame_SFScene", UsedButtons = "asd " },
            new Minigame(){ Scene = "Minigame_Crane", UsedButtons = "ad "},
            new Minigame(){ Scene = "Minigame_Phone", UsedButtons = "wasd "},
        };

        minigames = minigames.OrderBy(item => Random.Range(int.MinValue, int.MaxValue)).ToList();
        Lives = 3;

        //scramble the list
        print("starting the game");
        minigamesCompleted = 0;
        GotoScene(INTERMEZZO_SCENE);
    }

    public static Minigame GetCurrentMinigame()
    {
        if (minigames== null)
        {
            return new Minigame() { Scene = "", UsedButtons = "wasd " };
        }
        // when you win, go to the main menu
        if (minigamesCompleted >= minigames.Count)
            return new Minigame() { Scene = MAIN_MENU, UsedButtons = string.Empty };

        return minigames[minigamesCompleted];
    }


    public static void MinigameComplete()
    {
        if (MinigameLockout) return;
        me.GetComponent<GameManager>().StartCoroutine(MinigameCompleteAsync());
    }
    
    public static IEnumerator MinigameCompleteAsync()
    {
        MinigameLockout = true;
        print("You did the thing");
        minigamesCompleted++;
        if (minigamesCompleted >= minigames.Count)
        {
            GotoScene("YouWin");
            minigamesCompleted = 0;
            MinigameLockout = false;
            SetTime(string.Empty);
        }
        else
        {
            MinigameWin.Play();
            yield return new WaitForSeconds(1);
            GotoScene(INTERMEZZO_SCENE);
        }
        MinigameLockout = false;
    }

    public static void GotoScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public static void IntermezzoComplete()
    {
        print("itermezzing");
        GotoScene(GetCurrentMinigame().Scene);
    }

    public static void MinigameDeath()
    {
        if (MinigameLockout) return;
        print("ur ded");
        Lives--;
        MinigameLoss.Play();
        if (Lives <= 0)
        {
            minigamesCompleted = 0;
            GotoScene("GoToHell");
        }
    }

    public static void SetTime(string time)
    {
        if (MinigameLockout) return;
        TimerText = time;
    }


    // Update is called once per frame
    void Update()
    {
        var currentScene = SceneManager.GetActiveScene();
        var currentSceneName = currentScene.name;
        timeField.text = "";
        livesField.text = "";

        if (Input.GetKeyDown(KeyCode.Escape)) QuitGame();
    }
}
