using UnityEngine;
using System.IO;

public class Highscore : MonoBehaviour
{
    void Start()
    {
        WriteStringToFile("Hello, World!\nThis is a new line of text.", "highscore.txt");
    }

    public void WriteStringToFile(string content, string fileName)
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);

        File.WriteAllText(path, content);
        Debug.Log("Wrote to file: " + path);
    }
}