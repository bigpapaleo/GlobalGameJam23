using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GoToRollaball());
    }

    IEnumerator GoToRollaball()
    {
        yield return new WaitForSeconds(5);
        GameManager.GotoScene("Minigame_Rollaball");
    }
}
