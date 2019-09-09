using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateSprites : MonoBehaviour
{

    public Sprite cardFace;
    public Sprite cardBack;


    private SpriteRenderer spriteRenderer;
    private Selectable selectable;
    private Solitaire solitaire;

    //allows us to use the arrays created from previous code

    // Start is called before the first frame update
    void Start()
    {
        List<string> deck = Solitaire.GenerateDeck();
        solitaire = FindObjectOfType<Solitaire>();

        //loop to find the card in the deck
        int i = 0;
        foreach (string card in deck)
        {
            if (this.name == card)
            {
                cardFace = solitaire.cardFaces[i];
                break; //this means we found the correct card
            }
            i++;
            spriteRenderer = GetComponent<SpriteRenderer>();
            selectable = GetComponent<Selectable>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (selectable.faceUp == true)
        {
            spriteRenderer.sprite = cardFace;
        }
        //if you don't show face up, the default is showing the card back
        else
        {
            spriteRenderer.sprite = cardBack;
        }
    }
}
