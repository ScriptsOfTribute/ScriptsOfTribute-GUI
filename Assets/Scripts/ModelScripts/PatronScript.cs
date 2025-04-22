    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptsOfTribute;

//ALLOW LONG LINES
public class PatronScript : MonoBehaviour
{
    public PatronId patronID;

    public string GetName()
    {
        string toReturn = "";
        switch (patronID)
        {
            case PatronId.ANSEI:
                toReturn = @"Ansei Frandar Hunding";
                break;
            case PatronId.DUKE_OF_CROWS:
                toReturn = @"Duke of Crows";
                break;
            case PatronId.HLAALU:
                toReturn = @"Grandmaster Hlaalu";
                break;
            case PatronId.RAJHIN:
                toReturn = @"Rajhin, The Purring Liar";
                break;
            case PatronId.RED_EAGLE:
                toReturn = @"Red Eagle, King of the Reach";
                break;
            case PatronId.ORGNUM:
                toReturn = @"Sorcerer-King Orgnum";
                break;
            case PatronId.PSIJIC:
                toReturn = @"Psijic Loremaster";
                break;
            case PatronId.PELIN:
                toReturn = @"Saint Pelin";
                break;
            case PatronId.SAINT_ALESSIA:
                toReturn = @"Saint Alessia";
                break;
            case PatronId.TREASURY:
                break;
        }

        return toReturn;
    }

    public string GetFavoredDescription()
    {
        string toReturn = "";
        switch (patronID)
        {
            case PatronId.ANSEI:
                toReturn = @"<color=""green"">Favored<color=""white"">
<size=70%>Gain 1 Coin at the start of your turn<size=100%>";
                break;
            case PatronId.DUKE_OF_CROWS:
                toReturn = @"<color=""green"">Favored<color=""white"">
<size=70%>Unusable. No benefits<size=100%>";
                break;
            case PatronId.HLAALU:
                toReturn = @"<color=""green"">Favored<color=""white"">
<size=70%><color=""grey"">Sacrifice 1 card in play with cost > 1: <color=""white"">Gain <color=#F4D35E><i>Cost - 1</i><color=""white""> prestige. <size=100%>";
                break;
            case PatronId.RAJHIN:
                toReturn = @"<color=""green"">Favored<color=""white"">
<size=70%><color=""grey"">Pay 3 Coin: <color=""white"">Create 1 Bewilderment card and place it in your opponent's cooldown<size=100%>";
                break;
            case PatronId.RED_EAGLE:
                toReturn = @"<color=""green"">Favored<color=""white"">
<size=70%><color=""grey"">Pay 2 Power: <color=""white"">Draw 1 card<size=100%>";
                break;
            case PatronId.ORGNUM:
                toReturn = @"<color=""green"">Favored<color=""white"">
<size=70%><color=""grey"">Pay 3 Coin: <color=""white"">Gain 1 Power for every 4 cards you own, rounded down. Create 1 Summerset Sacking Card and place it in your cooldown<size=100%>";
                break;
            case PatronId.PSIJIC:
                toReturn = @"<color=""green"">Favored<color=""white"">
<size=70%><color=""grey"">Opponent has 1 agent active, Pay 4 Coin: <color=""white"">Place 1 of your opponent's active agents into their cooldown<size=100%>";
                break;
            case PatronId.PELIN:
                toReturn = @"<color=""green"">Favored<color=""white"">
<size=70%><color=""grey"">Pay 2 Power. Have 1 agent card in your cooldown: <color=""white"">Return up to 1 agent from your cooldown to the top of your draw<size=100%>";
                break;
            case PatronId.SAINT_ALESSIA:
                toReturn = @"<color=""green"">Favored<color=""white"">
<size=70%><color=""grey"">Pay 4 Coin: <color=""white"">Create 1 Chainbreaker Sergeant card and place it in your cooldown pile.<size=100%>";
                break;

            case PatronId.TREASURY:
                break;
        }

        return toReturn;
    }

    public string GetNeutralDescription()
    {
        string toReturn = "";
        switch (patronID)
        {
            case PatronId.ANSEI:
                toReturn = @"<color=""white"">Neutral<color=""white"">
<size=70%><color=""grey"">Pay 2 Power: <color=""white"">Gain 1 Coin. This patron now <color=""green"">favors<color=""white""> you<size=100%>";
                break;
            case PatronId.DUKE_OF_CROWS:
                toReturn = @"<color=""white"">Neutral<color=""white"">
<size=70%><color=""grey"">Pay all your Coin: <color=""white"">Gain <color=#F4D35E><i>Coin - 1</i><color=""white""> amount of Power.
This patron now <color=""green"">favors<color=""white""> you<size=100%>";
                break;
            case PatronId.HLAALU:
                toReturn = @"<color=""white"">Neutral<color=""white"">
<size=70%><color=""grey"">Sacrifice 1 card in play with cost > 1: <color=""white"">Gain <color=#F4D35E><i>Cost - 1</i><color=""white""> prestige.  This patron now <color=""green"">favors<color=""white""> you<size=100%>";
                break;
            case PatronId.RAJHIN:
                toReturn = @"<color=""white"">Neutral<color=""white"">
<size=70%><color=""grey"">Pay 3 Coin: <color=""white"">Create 1 Bewilderment card and place it in your opponent's cooldown.  This patron now <color=""green"">favors<color=""white""> you<size=100%>";
                break;
            case PatronId.RED_EAGLE:
                toReturn = @"<color=""white"">Neutral<color=""white"">
<size=70%><color=""grey"">Pay 2 Power: <color=""white"">Draw 1 card.  This patron now <color=""green"">favors<color=""white""> you<size=100%>";
                break;
            case PatronId.ORGNUM:
                toReturn = @"<color=""white"">Neutral<color=""white"">
<size=70%><color=""grey"">Pay 3 Coin: <color=""white"">Gain 1 Power for every 6 cards you own, rounded down.  This patron now <color=""green"">favors<color=""white""> you<size=100%>";
                break;
            case PatronId.PSIJIC:
                toReturn = @"<color=""white"">Neutral<color=""white"">
<size=70%><color=""grey"">Opponent has 1 agent active, Pay 4 Coin: <color=""white"">Place 1 of your opponent's active agents into their cooldown.  This patron now <color=""green"">favors<color=""white""> you<size=100%>";
                break;
            case PatronId.PELIN:
                toReturn = @"<color=""white"">Neutral<color=""white"">
<size=70%><color=""grey"">Pay 2 Power. Have 1 agent card in your cooldown: <color=""white"">Return up to 1 agent from your cooldown to the top of your draw. This patron now <color=""green"">favors<color=""white""> you<size=100%>";
                break;
            case PatronId.SAINT_ALESSIA:
                toReturn = @"<color=""white"">Neutral<color=""white"">
<size=70%><color=""grey"">Pay 4 Coin: <color=""white"">Create 1 Soldier of the Empire card and place it in your cooldown pile. This patron now <color=""green"">favors<color=""white""> you<size=100%>";
                break;

            case PatronId.TREASURY:
                break;
        }

        return toReturn;
    }

    public string GetUnFavoredDescription()
    {
        string toReturn = "";
        switch (patronID)
        {
            case PatronId.ANSEI:
                toReturn = @"<color=""red"">Unfavored<color=""white"">
<size=70%><color=""grey"">Pay 2 Power: <color=""white"">Gain 1 Coin. This patron now <color=""green"">favors<color=""white""> you<size=100%>";
                break;
            case PatronId.DUKE_OF_CROWS:
                toReturn = @"<color=""red"">Unfavored<color=""white"">
<size=70%><color=""grey"">Pay all your Coin: <color=""white"">Gain <color=#F4D35E><i>Coin - 1</i><color=""white""> amount of Power.
This patron is now <color=""white"">neutral<color=""white""><size=100%>";
                break;
            case PatronId.HLAALU:
                toReturn = @"<color=""red"">Unfavored<color=""white"">
<size=70%><color=""grey"">Sacrifice 1 card in play with cost > 1: <color=""white"">Gain <color=#F4D35E><i>Cost - 1</i><color=""white""> prestige.  This patron is now Neutral";
                break;
            case PatronId.RAJHIN:
                toReturn = @"<color=""red"">Unfavored<color=""white"">
<size=70%><color=""grey"">Pay 3 Coin: <color=""white"">Create 1 Bewilderment card and place it in your opponent's cooldown.  This patron is now Neutral";
                break;
            case PatronId.RED_EAGLE:
                toReturn = @"<color=""red"">Unfavored<color=""white"">
<size=70%><color=""grey"">Pay 2 Power: <color=""white"">Draw 1 card.  This patron is now Neutral";
                break;
            case PatronId.ORGNUM:
                toReturn = @"<color=""red"">Unfavored<color=""white"">
<size=70%><color=""grey"">Pay 3 Coin: <color=""white"">Gain 2 Power.  This patron is now Neutral";
                break;
            case PatronId.PSIJIC:
                toReturn = @"<color=""red"">Unfavored<color=""white"">
<size=70%><color=""grey"">Opponent has 1 agent active, Pay 4 Coin: <color=""white"">Place 1 of your opponent's active agents into their cooldown.  This patron is now Neutral";
                break;
            case PatronId.PELIN:
                toReturn = @"<color=""red"">Unfavored<color=""white"">
<size=70%><color=""grey"">Pay 2 Power. Have 1 agent card in your cooldown: <color=""white"">Return up to 1 agent from your cooldown to the top of your draw. This patron is now Neutral";
                break;
            case PatronId.SAINT_ALESSIA:
                toReturn = @"<color=""red"">Unfavored<color=""white"">
<size=70%><color=""grey"">Pay 4 Coin: <color=""white"">Gain 2 Power. This patron is now Neutral";
                break;
            case PatronId.TREASURY:
                break;
        }

        return toReturn;
    }



}
