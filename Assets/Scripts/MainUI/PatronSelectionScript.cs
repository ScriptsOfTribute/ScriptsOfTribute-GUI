using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using TalesOfTribute;
using UnityEngine;

public class PatronSelectionScript : MonoBehaviour
{
    public GameObject MainGame;
    public GameObject self;
    public GameObject[] slots;
    public static List<PatronId> selectedPatrons;
    private int counter = 0;
    void Start()
    {
        selectedPatrons = new List<PatronId>();
    }

    public void PatronClicked(GameObject patron)
    {
        PatronId id = patron.GetComponent<PatronScript>().patronID;
        selectedPatrons.Add(id);
        
        if (selectedPatrons.Count == 2)
            selectedPatrons.Add(PatronId.TREASURY);
        patron.transform.position = slots[counter].transform.position;
        counter++;
        if (selectedPatrons.Count == 5)
        {
            MainGame.SetActive(true);
            self.SetActive(false);
        }
    }

}
