using UnityEngine;
using UnityEngine.UI;

public class MoveBotButton : MonoBehaviour
{ 
    public GameObject BoardManager;

    public void Update()
    {
        if (GameManager.Board.CurrentPlayerId != PlayerScript.Instance.playerID && !TalesOfTributeAI.Instance.isMoving)
            GetComponent<Button>().interactable = true;
        else
        {
            GetComponent<Button>().interactable = false;
        }
    }
    public void PlayMove()
    {
        BoardManager.GetComponent<GameManager>().PlayBotMove();
    }

    public void PlayAllMoves()
    {
        BoardManager.GetComponent<GameManager>().PlayBotAllTurnMoves();
    }
}
