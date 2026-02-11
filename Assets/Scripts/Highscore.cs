using UnityEngine;
using System.IO;

public class Highscore : MonoBehaviour
{
    public string filename;
    public string path;
    public int highscore = 0;
    void Start()
    {
        filename = "highscore.txt";
        path = Path.Combine(Application.persistentDataPath, filename);
        highscore = ReadHighscore();
    }

    public void WriteHighscore(int score)
    {
        File.WriteAllText(path, score.ToString());
        Debug.Log("Wrote to file: " + path);
    }

    public int ReadHighscore()
    {
        Debug.Log(path);
        if (File.Exists(path))
        {
            string content = File.ReadAllText(path);
            Debug.Log(content);
            if (int.TryParse(content, out int savedScore))
            {
                return savedScore;
            }
        }
        return 0;
    }
}