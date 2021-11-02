using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreRank : MonoBehaviour
{
    public Text rankText;
    public Text nameText;
    public Text scoreText;

    public void SetData(int rank, ScoreVO vo)
    {
        rankText.text = $"{rank}À§";
        nameText.text = vo.name;
        scoreText.text = $"{vo.score}Á¡";
    }
}
