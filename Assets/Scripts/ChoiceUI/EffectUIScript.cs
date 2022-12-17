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
    public Choice<EffectType> effectChoice;
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

    public void SetUpChoices(Choice<EffectType> choice)
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

    public void MakeChoice(EffectType choice)
    {
        var result = effectChoice.Choose(choice);
        _completed = true;
        CleanUpChoices();
        endResult = result;
        Debug.Log(result);
        if (result is Success)
        {
            _completed = true;
        }
        else if (result is Failure failure)
        {
            Debug.Log(failure.Reason);
            _completed = true;
        }
        else if (result is Choice<Card>)
        {
            Debug.Log("chosen");
            _completed = true;
        }
        else if (result is Choice<EffectType> effectChoice)
        {
            SetUpChoices(effectChoice);
        }
    }

    public PlayResult GetResult()
    {
        return endResult;
    }

}
