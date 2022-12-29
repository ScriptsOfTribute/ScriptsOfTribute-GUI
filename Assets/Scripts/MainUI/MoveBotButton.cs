using UnityEngine;
using UnityEngine.UI;

public class MoveBotButton : MonoBehaviour
{ 
    public GameObject BoardManager;

    public void Update()
    {
        if (GameManager.Board.CurrentPlayerId != PlayerScript.Instance.playerID)
            GetComponent<Button>().interactable = true;
        else
        {
            GetComponent<Button>().interactable = false;
        }
    }
    public void OnButtonClick()
    {
        StartCoroutine(BoardManager.GetComponent<GameManager>().PlayBotMove());
    }
}
