using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class MoveBotButton : MonoBehaviour
{ 
    public GameObject BoardManager;
    private bool isRunningThread = false;

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
        Debug.Log("clicked");
        StartCoroutine(BoardManager.GetComponent<GameManager>().PlayBotMove());
    }

    public void PlayAllMoves()
    {
        StartCoroutine(BoardManager.GetComponent<GameManager>().PlayBotAllTurnMoves());  
    }
}
