using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class FightingGame : MonoBehaviour
{
    public TMP_Text text;
    int bufferLength = 32;
    List<int> directionalBuffer = new List<int>();
    int chosenGesture;

    List<string> gestures;
    double timeLimit;

    public MeshRenderer dfmesh;
    public MeshRenderer dmesh;
    public MeshRenderer fdmesh;

    public SpriteRenderer player;
    public SpriteRenderer punchingBag;

    public Sprite punchingBagNormal;
    public Sprite punchingBagHit;

    public Sprite hadokenPose;
    public Sprite tatsuJump;
    public Sprite[] tatsuFrames;
    public SpriteRenderer fireball;
    public Sprite idlePose;

    public GameObject downForward;
    public GameObject forward;

    public AudioSource Tatsu;
    public AudioSource Fireball;
    public AudioSource Hit;

    public Material[] ArrowMaterials;

    bool timeFreeze = false;

    public GameObject heartRateMonitor;

    // Start is called before the first frame update
    void Start()
    {
        timeFreeze = false;
        // timeLimit = 10;
        chosenGesture = UnityEngine.Random.Range(0, 2);
        for (int i = 0; i < bufferLength; i++)
        {
            directionalBuffer.Add(0);
        }
        gestures = new List<string>() { "26", "24", };
        // text.text = gestures[chosenGesture] + "+#";
        print("I want a " + gestures[chosenGesture]);
        PlaceArrows();

    }

    int meshSelected = 0;

    // Update is called once per frame
    void Update()
    {
        if (!timeFreeze)
        {
            var heartRateMonitorScr = heartRateMonitor.GetComponent<HeartRateMonitor>();
            heartRateMonitorScr.UpdateTimer();
            /*
            timeLimit -= Time.deltaTime;
            GameManager.SetTime(Math.Ceiling(timeLimit).ToString());
            if (timeLimit <= 0)
            {
                GameManager.MinigameDeath();
                timeLimit = 1;
            }
            */
        }

        if (UnityEngine.Random.Range(0, 40) == 1)
            meshSelected = (meshSelected + 1) % ArrowMaterials.Length;
        dfmesh.material = ArrowMaterials[meshSelected];
        dmesh.material = ArrowMaterials[meshSelected];
        fdmesh.material = ArrowMaterials[meshSelected];
        

        if(Input.GetKeyDown(KeyCode.Space))
        {
            print("CHosengest " + chosenGesture);
            
            
            if (MatchGesture(gestures[chosenGesture]))
            {
                StartCoroutine(GameWon());
            }
            else
            {
                print("oof");
            }
        }
    }

    IEnumerator GameWon()
    {
        StartCoroutine(AnimatePlayer());
        timeFreeze = true;
        if (gestures[chosenGesture] == "26")
        {
            Fireball.Play();
            yield return new WaitForSeconds(2);
        }
        else
        {
            Tatsu.Play();
            yield return new WaitForSeconds(2);
        }
        GameManager.MinigameComplete();

    }

    IEnumerator AnimatePlayer()
    {
        yield return new WaitForSeconds(.1f);
        if (gestures[chosenGesture] == "26")
        {
            player.sprite = hadokenPose;
            StartCoroutine(PlayFireball());
            yield return new WaitForSeconds(.8f);
            player.sprite = idlePose;
        }
        else if (gestures[chosenGesture] == "24")
        {
            player.sprite = tatsuJump;
            for (int i = 0; i < 25; i++)
            {
                yield return new WaitForSeconds(.1f);
                player.sprite = tatsuFrames[i % tatsuFrames.Length];

                player.transform.position = new Vector3(player.transform.position.x + .4f, player.transform.position.y, player.transform.position.z);
                if (player.transform.position.x > punchingBag.transform.position.x - 1)
                {
                    player.transform.position = new Vector3(player.transform.position.x -.2f, player.transform.position.y, player.transform.position.z);
                    punchingBag.transform.position = new Vector3(player.transform.position.x + 1f, player.transform.position.y, player.transform.position.z); ;
                    punchingBag.sprite = i % 2 == 1 ? punchingBagHit : punchingBagNormal;
                }
            }
        }
    }

    IEnumerator PlayFireball()
    {
        yield return new WaitForSeconds(.1f);
        fireball.gameObject.SetActive(true);
        for (int i = 0; i < 18; i++)
        {
            fireball.transform.position = new Vector3(fireball.transform.position.x + .6f, fireball.transform.position.y, fireball.transform.position.z);
            if (fireball.transform.position.x > punchingBag.transform.position.x)
            {
                fireball.gameObject.SetActive(false);
                punchingBag.sprite = punchingBagHit;
                for(int j = 0; j < 5; i++)
                {
                    yield return new WaitForSeconds(.1f);
                    punchingBag.transform.position = new Vector3(punchingBag.transform.position.x + .1f, punchingBag.transform.position.y, punchingBag.transform.position.z);
                }
                fireball.sprite = punchingBagNormal;
                break;
            }
            yield return new WaitForSeconds(.1f);
        }
    }

    private void PlaceArrows()
    {
        if (gestures[chosenGesture] == "26")
        {
            downForward.transform.localPosition = new Vector3(0.2105238f, 0.008796209f, 0.212771f);
            downForward.transform.localRotation = Quaternion.Euler(new Vector3(0, -45, 0));

            forward.transform.localPosition = new Vector3(0.1465743f, 0.008796209f, 0.2286169f);
            forward.transform.localRotation = Quaternion.Euler(0, -90, 0);
        }
        else if (gestures[chosenGesture] == "24")
        {
            print("Placing23");
            downForward.transform.localPosition = new Vector3(0.185238f, 0.008796209f, 0.212771f);
            downForward.transform.localRotation = Quaternion.Euler(new Vector3(0, 45, 0));

            
            forward.transform.localRotation = Quaternion.Euler(0, 90, 0);

        }
    }

    // so the buffer isn't messed up
    private void FixedUpdate()
    {
        int verticalness = 0;
        int horizontalness = 0;
        if (Input.GetKey(KeyCode.W)) { verticalness++; }
        if (Input.GetKey(KeyCode.S)) { verticalness--; }
        if (Input.GetKey(KeyCode.D)) { horizontalness++; }
        if (Input.GetKey(KeyCode.A)) { horizontalness--; }
        // get horizontal and vertical
        int directionalInput = parseInput(horizontalness, verticalness);
        AddInput(directionalInput);
        
    }

    private void AddInput(int directionalInput)
    {
        for (int i = directionalBuffer.Count - 1; i > 0 ; i--)
        {
            directionalBuffer[i] = directionalBuffer[i - 1];
        }

        directionalBuffer[0] = directionalInput;
    }

    int parseInput(int horizontal, int vertical)
    {
        // gets numpad no
        if (horizontal == 1)
        {
            return 6;
        }
        else if (horizontal == -1)
        {
            return 4;
        }
        else if (vertical == -1)
        {
            return 2;
        }
        return 5;
    }


    bool MatchGesture(string gesture)
    {

        // only doing special moves
        if (gesture.Length <= 1)
        {
            return false;
        }


        // first, we want to reverse the gesture.
        char[] gestureArray = gesture.Reverse().ToArray();
        int curGestureArrIndex = 0;
        foreach(int i in directionalBuffer)
        {
            print("dirbuff " + i);
        }

        print("input:" + gesture);
        for (int i = 0; i < directionalBuffer.Count; i++)
        {
            // get a char
            char checkedDir = directionalBuffer[i].ToString().ToArray()[0];
            if (curGestureArrIndex >= gestureArray.Length)
            {
                return true;
            }
            else if (checkedDir == gestureArray[curGestureArrIndex])
            {
                print("incing");
                curGestureArrIndex++;
            }
            else if (curGestureArrIndex > 0 && checkedDir == gestureArray[curGestureArrIndex - 1])
            {
                print("0 indexed");
                // do nothing
            }
            else if (curGestureArrIndex == 0 || checkedDir == 5)
            {
                print("CD " + checkedDir);
                print("Gestchar " + gestureArray[curGestureArrIndex]);
                print("ignoring 5o r the 0");
                // also do nothing. I just want to know for my brain these bases are covered.
            }
            else
            {
                return false;
            }
        }

        return false;
    }

}
