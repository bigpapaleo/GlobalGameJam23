using UnityEngine;
using UnityEngine.Video;

public class GameEndVideo : MonoBehaviour
{
    VideoPlayer player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<VideoPlayer>();
        player.loopPointReached += (VideoPlayer player) => GameManager.GotoScene(GameManager.MAIN_MENU);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.GotoScene(GameManager.MAIN_MENU);
        }
    }
}
