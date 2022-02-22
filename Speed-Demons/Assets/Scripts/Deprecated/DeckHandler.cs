using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckHandler : MonoBehaviour
{
    public int[] deck;
    public int[] hand;
    //deckpos tracks what we will draw, but also how many cards are in discard
    public int deckpos;
    public int decksize;
    public Card card;
    // Start is called before the first frame update
    void Start()
    {
        //create lists
        deck = new int[20];
        hand = new int[3];
        decksize = 0;

        FillDeck();
        Shuffle();
    }
    //In tying these to buttons, it is signficantly easier to make them three seperate similar functions.
    public void PlayCardLeft()
    {
        card.Action(hand[0]);
        hand[0]=0;
        FillHand();
    }

    public void PlayCardMid()
    {
        card.Action(hand[1]);
        hand[1]=0;
        FillHand();
    }

    public void PlayCardRight()
    {
        card.Action(hand[2]);
        hand[2]=0;
        FillHand();
    }

    public void Shuffle()
    {
        deckpos = 0;
        for (int i = 0; i < decksize - 1; i ++)
        {
            int rnd = Random.Range(i, decksize);
            int tempGO = deck[rnd];
            deck[rnd] = deck[i];
            deck[i]=tempGO;
        }
        for (int x =0; x < 3; x++)
        {
            hand[x] = 0;
        }
        FillHand();
    }

    void FillDeck()
    {
        //1 is currently move
        //2 is currently attack any adjacent enemy
        for (int x =0; x < 6; x++)
        {
            deck[x]=1;
            decksize++;
        }
        for (int x=0; x <3; x++)
        {
            deck[x+6]=3;
            decksize++;
        }
        deck[9]=2;
    }

    void FillHand()
    {
        for (int x =0; x < 3; x++)
        {
            if (hand[x]==0)
            {
                hand[x] = deck[deckpos];
                card.DisplayCard(hand[x],x);
                deckpos++;
            }
        }
    }
}
