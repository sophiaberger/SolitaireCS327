using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Solitaire : MonoBehaviour
{

    public Sprite[] cardFaces;
    public GameObject cardPreFab;
    public GameObject deckButton;
    public GameObject[] bottomPos;
    public GameObject[] topPos;
    



    public static string[] suits = {"Clubs", "Hearts", "Diamonds", "Spades"};
    public static string[] values = { "Ace", "2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Queen", "King" };
    public List<string>[] bottoms;
    public List<string>[] tops;
    public List<string> GroupOfThreeOnDisplays = new List<string>();
    public List<List<string>> deckGroupsOfThree = new List<List<string>>();

    private List<string> bottom0 = new List<string>();
    private List<string> bottom1 = new List<string>();
    private List<string> bottom2 = new List<string>();
    private List<string> bottom3 = new List<string>();
    private List<string> bottom4 = new List<string>();
    private List<string> bottom5 = new List<string>();
    private List<string> bottom6 = new List<string>();

    //has to do with keeping track of the groups of three that have been dealt
    private int GroupOfThree;
    private int GroupOfThreeRemainder;

    public List<string> deck;
    public List<string> discardPile = new List<string>();

    private int deckLocation;
    // Start is called before the first frame update
    void Start()
    {
        bottoms = new List<string>[] { bottom0, bottom1, bottom2, bottom3, bottom4, bottom5, bottom6 };
        playCards();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playCards()
    {
        deck = GenerateDeck();
        Shuffle(deck);
        foreach(string card in deck)
        {
            print(card);
        }
        SolitaireSort();
        StartCoroutine(DealCards());
        SortDeckIntoGroupsOfThree();
    }


    public static List<string> GenerateDeck()
    {
        List<string> newDeck = new List<string>();
        //nested foreach loops to create all possible card values
        foreach (string s in suits)
        {
            foreach(string v in values)
            {
                newDeck.Add(v + " of "  + s);
            }
        }

        return newDeck;
    }

    //of type T, takes in a list of type T
    void Shuffle<T>(List<T> list)
    {
        //randomly reorder
        System.Random random = new System.Random();
        //take the length of the list
        int n = list.Count;
        //this is essentially each time choosing some value in the list, swapping it with a different position value
        //each time, n is getting smaller, so the number that need to be swapped needs to be smaller
        //this guarantees the deck is shuffled n times total
        //a smarter implementation may exist but tbh i don't know the rules and this tutorial seems to

        while (n > 1)
        {
            //random number within the range of n
            int k = random.Next(n);
            n--;
            //this is to switch the positions in the list
            T temp = list[k];
            list[k] = list[n];
            list[n] = temp;
        }
    }

    IEnumerator DealCards()
    {
        for (int i = 0; i < 7; i++)
        {
            float yOffset = 0f;
            float zOffset = 0.03f;
            foreach (string card in bottoms[i])
            {
                //this is a handy trick so they don't all appear at once!
                //i modified the time from the tutorial because it was too fast in my opinion
                //i like how this one looks more like cards stacking
                yield return new WaitForSeconds(0.1f);     
                //instantiate a new card, last stuff has to do with position and rotation
                GameObject newCard = Instantiate(cardPreFab, new Vector3(bottomPos[i].transform.position.x, bottomPos[i].transform.position.y - yOffset, bottomPos[i].transform.position.z - zOffset), Quaternion.identity, bottomPos[i].transform);
                newCard.name = card;
                //this is basically saying, only make the very top card value be visible upon initialization
                if (card == bottoms[i][bottoms[i].Count - 1]){
                    newCard.GetComponent<Selectable>().faceUp = true;
                }
                
                yOffset = yOffset + 0.3f;
                zOffset = zOffset + 0.03f;
                //adding to it when the cards are first dealt in this method call
                discardPile.Add(card);
            }
        }

        //catching accidental duplicates
        foreach (string card in discardPile)
        {
            if (deck.Contains(card))
            {
                deck.Remove(card);
            }
        }
        discardPile.Clear();
    }

    void SolitaireSort()
    {
        for (int i = 0; i < 7; i++)
        {
            //this is so that there is 1 more card for each one than the previous
            //as per the rules of solitaire

            for (int j = i; j<7; j++)
            {
                bottoms[j].Add(deck.Last<string>());
                deck.RemoveAt(deck.Count - 1);
            }
        }
    }

    void SortDeckIntoGroupsOfThree()
    {
        GroupOfThree = deck.Count / 3;
        GroupOfThreeRemainder = deck.Count % 3;
        deckGroupsOfThree.Clear();
        //will be used to create temporary list of the three cards
        //to add to larger list
        int modifier = 0;
        for (int i = 0; i < GroupOfThree; i++)
        {
            List<string> myGroupOfThree = new List<string>();
            for (int j = 0; j <3; j++)
            {
                myGroupOfThree.Add(deck[j + modifier]);

            }
            deckGroupsOfThree.Add(myGroupOfThree);
            modifier = modifier + 3;
            //in other words, the index of the deck is based on the modifier, so this way each time
            //the deck skips the previous group of three
        }
        //if there is a remainder at all
        if (GroupOfThreeRemainder != 0)
        {
            List<string> Remainders = new List<string>();
            modifier = 0;
            for (int k = 0; k < GroupOfThreeRemainder; k++)
            {
                Remainders.Add(deck[deck.Count - GroupOfThreeRemainder + modifier]);
                modifier++;
            }
            deckGroupsOfThree.Add(Remainders);
            GroupOfThree++;
        }
        deckLocation = 0;
    }

    public void dealFromDeck()
    {

        foreach (Transform child in deckButton.transform)
        {
            if (child.CompareTag("Card"))
            {
                deck.Remove(child.name);
                discardPile.Add(child.name);
                Destroy(child.gameObject);
            }
        }
        if (deckLocation < GroupOfThree)
        {
            GroupOfThreeOnDisplays.Clear();
            //this specifically controls where they are placed and the value -3.5 is the one that is the most accurate
            float xOffset = -3.5f;
            //this is so only the top card can be selected
            float zOffset = -0.2f;

            foreach(string card in deckGroupsOfThree[deckLocation])
            {
                //to make the cards smaller
   
                GameObject newTopCard = Instantiate(cardPreFab, new Vector3(deckButton.transform.position.x + xOffset, deckButton.transform.position.y, deckButton.transform.position.z + zOffset), Quaternion.identity, deckButton.transform);
                //increase by this much each time
                xOffset = xOffset + 0.5f;
                zOffset = zOffset - 0.2f;
                newTopCard.name = card;
                GroupOfThreeOnDisplays.Add(card);
                //for the life of me I don't know why this doesn't work when you remove the below line of code
                //but for some reason the scales will not be correct
                newTopCard.transform.localScale = new Vector3(1f, 1f, 1);
                newTopCard.GetComponent<Selectable>().faceUp = true;
            }
            deckLocation++;
        }
        else
        {
            //restack the top deck
            RestackTopDeck();
        }
    }

    void RestackTopDeck()
    {
        //to keep track of which cards need to be restacked, list of strings
        foreach (string card in discardPile)
        {
            deck.Add(card);
        }
        discardPile.Clear();
        SortDeckIntoGroupsOfThree();
        //need to add any cards on display before they are cleared to the discard pile
    }
}
