using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Focus : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite picture;
    public Sprite white;

    public SpriteRenderer sprite;
    public SpriteRenderer cameraColor;
    public AudioClip cameraScan;
    public AudioClip cameraClick;
    float curVal;
    float sendVal;
    private AudioSource source;
    bool won = false;

    public GameObject heartRateMonitor;
    public GameObject trianglePointer;
    public float trianglePointerLeftX;
    public float trianglePointerRightX;

    void Awake()
    {
        won = false;
        source = GetComponent<AudioSource>();
        source.clip = cameraScan;
        source.Stop();
        sprite.sprite = picture;
        curVal = UnityEngine.Random.Range(0,2) * 60 - 30;
        sendVal = curVal;
    }

    // -2 to 2 is the good range
    // red and green for in focus/out of focus

    private void Update()
    {
        var heartRateMonitorScr = heartRateMonitor.GetComponent<HeartRateMonitor>();
        if(!won) heartRateMonitorScr.UpdateTimer();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (sendVal <= 4)
            {
                won = true;
                StartCoroutine(GameWon());
            }
            else
            {
                print("Lost");
                GameManager.MinigameDeath();
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        float leftChange = 0;
        if (Input.GetKey(KeyCode.A)) { leftChange += .5f; }
        if (Input.GetKey(KeyCode.D)) { leftChange -= .5f; }
        curVal += leftChange;
        if (leftChange == 0)
        {
            source.Pause();
        }
        else if (!source.isPlaying)
        {
            source.Play();
        }

        trianglePointer.transform.position = new Vector3(
            trianglePointer.transform.position.x + leftChange/10,
            trianglePointer.transform.position.y,
            trianglePointer.transform.position.z);

        curVal = Math.Max(Math.Min(curVal, 30), -30);
         sendVal= Mathf.Min(30, Math.Abs(curVal));
        sprite.material.SetFloat("radius", sendVal);

        cameraColor.color = sendVal <= 2 ? Color.green : Color.red;


    }

    IEnumerator GameWon()
    {
        source.Stop();
        yield return new WaitForSeconds(.2f);
        print("Gwon");
        source.PlayOneShot(cameraClick);
        yield return new WaitForSeconds(.5f);
        sprite.sprite = white;
        GameManager.MinigameComplete();
        yield return new WaitForSeconds(.2f);
        sprite.sprite = picture;
        yield return new WaitForSeconds(.5f);

    }
}
