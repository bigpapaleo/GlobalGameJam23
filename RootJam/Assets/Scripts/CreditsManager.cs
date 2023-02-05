using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsManager : MonoBehaviour
{
    int waitTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        waitTimer++;
        if (Input.anyKey && waitTimer > 5) {
            GameManager.GotoScene("MainMenu");
        }
    }
}
