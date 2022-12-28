using System.Collections;
using System.Collections.Generic;
using TalesOfTribute;
using UnityEngine;

public class EffectUIScript : MonoBehaviour
{
    private EffectType _left;
    private EffectType _right;
    public GameObject Left;
    public GameObject Right;

    public GameObject EffectCardPrefab;
    public Choice<Effect> effectChoice;
    private bool _completed;
    private PlayResult endResult;


    private void Start()
    {
        _completed = false;
    }

    public bool GetCompletedStatus()
    {
        return _completed;
    }

    public void SetUpChoices(Choice<Effect> choice)
    {
        effectChoice = choice;
        _completed = false;
        var e = Instantiate(EffectCardPrefab, Left.transform);
        e.GetComponent<EffectButtonUIScript>().SetUpEffectInfo(choice.PossibleChoices[0]);

        e = Instantiate(EffectCardPrefab, Right.transform);
        e.GetComponent<EffectButtonUIScript>().SetUpEffectInfo(choice.PossibleChoices[1]);
    }

    void CleanUpChoices()
    {
        if (Left.transform.childCount > 0)
            Destroy(Left.transform.GetChild(0).gameObject);
        if (Right.transform.childCount > 0)
            Destroy(Right.transform.GetChild(0).gameObject);
    }

    public void MakeChoice(Effect effect)
    {
        GameManager.Board.MakeChoice<Effect>(effect);
        CleanUpChoices();
        _completed = true;
    }


}
