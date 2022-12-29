using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using TalesOfTribute;
using UnityEngine;

public class PatronSelectionScript : MonoBehaviour
{
    public GameObject MainGame;
    public GameObject self;
    public GameObject[] slots;
    public GameObject[] patrons;
    public static List<PatronId> selectedPatrons;
    public static List<PatronId> availablePatrons;
    private int counter = 0;
    void Start()
    {
        selectedPatrons = new List<PatronId>();
        availablePatrons = patrons.Select(p => p.GetComponent<PatronScript>().patronID).ToList();
    }

    private void Update()
    {
        if (selectedPatrons.Count == 2)
        {
            selectedPatrons.Add(PatronId.TREASURY);
        }
        else if(selectedPatrons.Count == 5)
        {
            MainGame.SetActive(true);
            self.SetActive(false);
        }
        if (TalesOfTributeAI.Instance.botID == PlayerEnum.PLAYER1)
        {
            if (selectedPatrons.Count == 0)
            {
                var id = TalesOfTributeAI.Instance.SelectPatron(availablePatrons, 1);
                selectedPatrons.Add(id);
                availablePatrons.Remove(id);
                patrons.First(p => p.GetComponent<PatronScript>().patronID == id).transform.position = slots[counter].transform.position;
                counter++;
            }
            else if (selectedPatrons.Count == 4)
            {
                var id = TalesOfTributeAI.Instance.SelectPatron(availablePatrons, 4);
                selectedPatrons.Add(id);
                availablePatrons.Remove(id);
                patrons.First(p => p.GetComponent<PatronScript>().patronID == id).transform.position = slots[counter].transform.position;
                counter++;
            }
        }
        else if (TalesOfTributeAI.Instance.botID == PlayerEnum.PLAYER2)
        {
            if (selectedPatrons.Count == 1)
            {
                var id = TalesOfTributeAI.Instance.SelectPatron(availablePatrons, 2);
                selectedPatrons.Add(id);
                availablePatrons.Remove(id);
                patrons.First(p => p.GetComponent<PatronScript>().patronID == id).transform.position = slots[counter].transform.position;
                counter++;
            }
            else if (selectedPatrons.Count == 2)
            {
                selectedPatrons.Add(PatronId.TREASURY);
            }
            else if (selectedPatrons.Count == 3)
            {
                var id = TalesOfTributeAI.Instance.SelectPatron(availablePatrons, 3);
                selectedPatrons.Add(id);
                availablePatrons.Remove(id);
                patrons.First(p => p.GetComponent<PatronScript>().patronID == id).transform.position = slots[counter].transform.position;
                counter++;
            }
        }
    }

    public void PatronClicked(GameObject patron)
    {
        PatronId id = patron.GetComponent<PatronScript>().patronID;
        selectedPatrons.Add(id);
        availablePatrons.Remove(id);
        patron.transform.position = slots[counter].transform.position;
        counter++;
    }

}
