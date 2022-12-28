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

    public void PatronClicked(GameObject patron)
    {
        PatronId id = patron.GetComponent<PatronScript>().patronID;
        selectedPatrons.Add(id);
        availablePatrons.Remove(id);
        patron.transform.position = slots[counter].transform.position;
        counter++;
        
        if (selectedPatrons.Count == 1)
        {
            id = UnityRandomBot.Instance.SelectPatron(availablePatrons, 1);
            Debug.Log(id);
            selectedPatrons.Add(id);
            availablePatrons.Remove(id);
            patrons.First(p => p.GetComponent<PatronScript>().patronID == id).transform.position = slots[counter].transform.position;
            counter++;

            selectedPatrons.Add(PatronId.TREASURY);

            id = UnityRandomBot.Instance.SelectPatron(availablePatrons, 2);
            Debug.Log(id);
            selectedPatrons.Add(id);
            availablePatrons.Remove(id);
            patrons.First(p => p.GetComponent<PatronScript>().patronID == id).transform.position = slots[counter].transform.position;
            counter++;
        }
        
        
        if (selectedPatrons.Count == 5)
        {
            MainGame.SetActive(true);
            self.SetActive(false);
        }
    }

}
