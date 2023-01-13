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
    private bool _AIselecting;
    private bool _updatePerformed;
    void Start()
    {
        selectedPatrons = new List<PatronId>();
        availablePatrons = patrons.Select(p => p.GetComponent<PatronScript>().patronID).ToList();
        _AIselecting = false;
        if (TalesOfTributeAI.Instance.botID == PlayerEnum.PLAYER1)
        {
            _AIselecting = true;
            AIPickPatron();
            _AIselecting = false;
        }
        _updatePerformed = false;
    }

    private void Update()
    {
        if (selectedPatrons.Count == 4 && !_updatePerformed)
        {
            ConfirmButton.GetComponent<Button>().interactable = true;
            foreach(var patron in patrons)
            {
                if (!selectedPatrons.Contains(patron.GetComponent<PatronScript>().patronID))
                {
                    patron.GetComponent<Button>().interactable = false;
                }
            }
            _updatePerformed = true;
        }

    }

    public void PatronClicked(GameObject patron)
    {
        if (selectedPatrons.Count >= 4 || _AIselecting)
        {
            return;
        }
        PatronId id = patron.GetComponent<PatronScript>().patronID;
        if (selectedPatrons.Contains(id))
        {
            return;
        }
        selectedPatrons.Add(id);
        availablePatrons.Remove(id);
        patron.transform.position = slots[counter].transform.position;
        slots[counter].transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("Player pick");
        counter++;
        patron.GetComponent<Button>().enabled = false;

        if (TalesOfTributeAI.Instance.botID == PlayerEnum.PLAYER2 && selectedPatrons.Count == 1)
        {
            _AIselecting = true;
            AIPickPatron();
            AIPickPatron();
            _AIselecting = false;
        }
        else if (TalesOfTributeAI.Instance.botID == PlayerEnum.PLAYER1 && selectedPatrons.Count == 3)
        {
            _AIselecting = true;
            AIPickPatron();
            _AIselecting = false;
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
            patrons.First(p => p.GetComponent<PatronScript>().patronID == id).GetComponent<Button>().enabled = false;
            slots[counter].transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("AI pick");
            counter++;
        }
        else
        {
            EndGameUI.SetActive(true);
            StartCoroutine(EndGameUI.GetComponent<EndGameUI>().SetUp(new EndGameState(PlayerScript.Instance.playerID, GameEndReason.PATRON_SELECTION_TIMEOUT)));
            this.enabled = false;
            return;
        }
    }

    public void ProceedGame()
    {
        selectedPatrons.Insert(2, PatronId.TREASURY);
        MainGame.SetActive(true);
        self.SetActive(false);
    }

}
