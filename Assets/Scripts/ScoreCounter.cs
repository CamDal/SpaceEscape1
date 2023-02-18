using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour
{
    public Text scoreText;
    public float score = 0f;
    public float pointIncreasedPerSecond = 100f;
    // Update is called once per frame
    void FixedUpdate()
    {
        scoreText.text = "Score: " + ((int)score).ToString();
        score += pointIncreasedPerSecond * Time.fixedDeltaTime;
    }
}
