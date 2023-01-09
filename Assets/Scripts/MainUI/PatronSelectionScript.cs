using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Threading.Tasks;
using TalesOfTribute;
using TalesOfTribute.Board.Cards;
using TalesOfTribute.Board;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PatronSelectionScript : MonoBehaviour
{
    public GameObject MainGame;
    public GameObject self;
    public GameObject[] slots;
    public GameObject[] patrons;
    public static List<PatronId> selectedPatrons;
    public static List<PatronId> availablePatrons;
    private int counter = 0;
    public GameObject EndGameUI;
    public GameObject ConfirmButton;
    void Start()
    {
        selectedPatrons = new List<PatronId>();
        availablePatrons = patrons.Select(p => p.GetComponent<PatronScript>().patronID).ToList();
        if (TalesOfTributeAI.Instance.botID == PlayerEnum.PLAYER1)
        {
            AIPickPatron();
        }
        
    }

    private void Update()
    {
        if (selectedPatrons.Count == 5)
        {
            ConfirmButton.GetComponent<Button>().interactable = true;
        }

    }

    public async void PatronClicked(GameObject patron)
    {
        if (selectedPatrons.Count >= 5)
        {
            return;
        }
        PatronId id = patron.GetComponent<PatronScript>().patronID;
        selectedPatrons.Add(id);
        availablePatrons.Remove(id);
        patron.transform.position = slots[counter].transform.position;
        slots[counter].transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("Player pick");
        counter++;

        if (TalesOfTributeAI.Instance.botID == PlayerEnum.PLAYER2 && selectedPatrons.Count == 1)
        {
            AIPickPatron();
            selectedPatrons.Add(PatronId.TREASURY);
            await Task.Delay(100);
            AIPickPatron();
        }
        else if (TalesOfTributeAI.Instance.botID == PlayerEnum.PLAYER1 && selectedPatrons.Count == 4)
        {
            AIPickPatron();
        }
        else if (TalesOfTributeAI.Instance.botID == PlayerEnum.PLAYER1 && selectedPatrons.Count == 2)
        {
            selectedPatrons.Add(PatronId.TREASURY);
        }
    }

    public async void AIPickPatron()
    {
        var id = await TalesOfTributeAI.Instance.SelectPatron(availablePatrons, counter);
        if (id != PatronId.TREASURY)
        {
            selectedPatrons.Add(id);
            availablePatrons.Remove(id);
            patrons.First(p => p.GetComponent<PatronScript>().patronID == id).transform.position = slots[counter].transform.position;
            slots[counter].transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("AI pick");
            counter++;
        }
        else
        {
            EndGameUI.SetActive(true);
            EndGameUI.GetComponent<EndGameUI>().SetUp(new EndGameState(PlayerScript.Instance.playerID, GameEndReason.PATRON_SELECTION_TIMEOUT));
            this.enabled = false;
            return;
        }
    }

    public void ProceedGame()
    {
        MainGame.SetActive(true);
        self.SetActive(false);
    }

}
