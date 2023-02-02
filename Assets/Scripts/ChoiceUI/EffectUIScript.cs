using ScriptsOfTribute;
using ScriptsOfTribute.Board.Cards;
using ScriptsOfTribute.Serializers;
using TMPro;
using UnityEngine;

public class EffectUIScript : MonoBehaviour
{
    private EffectType _left;
    private EffectType _right;
    public GameObject Left;
    public GameObject Right;
    public GameObject Panel;
    public TextMeshProUGUI TooltipText;

    public GameObject EffectCardPrefab;
    public SerializedChoice effectChoice;
    private bool _completed;
    private PlayResult endResult;


    private void Start()
    {
        _completed = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Panel.SetActive(!Panel.activeSelf);
            if (Panel.activeSelf)
            {
                SetUpChoices(effectChoice);
                TooltipText.SetText("[Space] Minimize");
            }
            else
            {
                CleanUpChoices();
                TooltipText.SetText("[Space] Show");
            }
        }
    }

    public bool GetCompletedStatus()
    {
        return _completed;
    }

    public void SetUpChoices(SerializedChoice choice)
    {
        effectChoice = choice;
        _completed = false;
        var e = Instantiate(EffectCardPrefab, Left.transform);
        e.GetComponent<EffectButtonUIScript>().SetUpEffectInfo(choice.PossibleEffects[0]);

        e = Instantiate(EffectCardPrefab, Right.transform);
        e.GetComponent<EffectButtonUIScript>().SetUpEffectInfo(choice.PossibleEffects[1]);
    }

    void CleanUpChoices()
    {
        if (Left.transform.childCount > 0)
            Destroy(Left.transform.GetChild(0).gameObject);
        if (Right.transform.childCount > 0)
            Destroy(Right.transform.GetChild(0).gameObject);
    }

    public void MakeChoice(UniqueEffect effect)
    {
        GameManager.Board.MakeChoice(effect);
        CleanUpChoices();
        _completed = true;
    }


}
