using UnityEngine;
using UnityEngine.Video;

public class IntroVideo: MonoBehaviour
{

    VideoPlayer player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<VideoPlayer>();
        player.loopPointReached += (VideoPlayer player) => GameManager.StartGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.StartGame();
        }
    }
}
