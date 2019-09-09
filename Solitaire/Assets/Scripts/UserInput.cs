using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    //these are the slots
    public GameObject slot1;
    private Solitaire solitaire;


    // Start is called before the first frame update
    void Start()
    {
        solitaire = FindObjectOfType<Solitaire>();
    }

    // Update is called once per frame
    void Update()
    {
        GetMouseClick();
    }

    //this is for clicking in the cards
    void GetMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //save the mouse position in world units
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10));
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit) {
                //this is if it is hit
                //check what it was, e.g. deck or card or empty slot
                //this way we can determine what to do
                if (hit.collider.CompareTag("Deck"))
                {
                    //clicked on the deck
                    Deck();
                }
                else if (hit.collider.CompareTag("Card"))
                {
                    //clicked on the card
                    Card();
                }
                else if (hit.collider.CompareTag("Top"))
                {
                    //clicked on the top card of the deck
                    Top();

                }
                else if (hit.collider.CompareTag("Bottom"))
                {
                    //clicked on the bottom card of the deck
                    Bottom();
                }
            }
        }
    }
    void Deck()
    {
        //deck
        solitaire.dealFromDeck();
    }
    void Card()
    {
        //card
        //if card is face down AND not blocked
        //flip

        //if in deck pile AND not blocked
        //select it

        //if the card is face up
        //if there is no card currently selected
        //select the new card
        //if there is a card selected and not the same card
        //if new card can be stacked
        //stack
        //else select new card

        //else if already card selected and it's the same card

        print("Clicked on the card!");
    }
    void Top()
    {
        //top
        print("Clicked on the top!");
    }
    void Bottom()
    {
        //bottom
        print("Clicked on the bottom!");
    }
}

