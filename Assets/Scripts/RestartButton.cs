using UnityEngine;

public class RestartButton : MonoBehaviour
{
    public Board board;
    public void PlayerClick()
    {
        board.Restart();
    }
}
