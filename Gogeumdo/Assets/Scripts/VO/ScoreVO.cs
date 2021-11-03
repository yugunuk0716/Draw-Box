using System;

[Serializable]
public class ScoreVO
{
    public string name;
    public int score;

    public ScoreVO()
    {

    }

    public ScoreVO(string name, int score)
    {
        this.name = name;
        this.score = score;
    }
}
