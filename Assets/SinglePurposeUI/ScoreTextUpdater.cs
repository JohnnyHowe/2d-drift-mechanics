using UnityEngine;
using UnityEngine.UI;

public class ScoreTextUpdater : MonoBehaviour
{
    public Text text;
    public ScoreHandler scoreHandler;

    void Update()
    {
        text.text = scoreHandler.GetScore().ToString();
    }
}
