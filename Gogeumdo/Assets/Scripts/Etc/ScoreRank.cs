using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreRank : MonoBehaviour
{
    public Text rankText;
    public Text nameText;
    public Text scoreText;

    public void SetData(int rank, ScoreVO vo) // 등수 표시
    {
        rankText.text = $"{rank}위";
        nameText.text = vo.name;
        scoreText.text = $"{vo.score}점";
    }
}
