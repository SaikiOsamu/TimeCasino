using UnityEngine;

public class RandomCardGenerator : MonoBehaviour
{
    // Arrays to hold card sprites for each suit
    public Sprite[] heartSprites;
    public Sprite[] spadeSprites;
    public Sprite[] clubSprites;
    public Sprite[] diamondSprites;

    // Reference to the SpriteRenderer component on the RandomCard GameObject
    private SpriteRenderer randomCardSpriteRenderer;

    // Variables to control card selection in the editor
    public bool forceCardSelection = false;  // Toggle this to manually select a card
    public int forcedSuitIndex = 0;          // 0: Hearts, 1: Spades, 2: Clubs, 3: Diamonds
    public int forcedCardIndex = 0;          // Index of the card in the chosen suit array

    // Start is called before the first frame update
    void Start()
    {
        // Get the SpriteRenderer component attached to this GameObject
        randomCardSpriteRenderer = GetComponent<SpriteRenderer>();

        // Generate a random or forced card when the game starts
        GenerateRandomCard();
    }

    // Function to generate a random card or force a specific card
    public void GenerateRandomCard()
    {
        Sprite selectedSprite = null;

        // If forceCardSelection is true, use the forced suit and card index
        if (forceCardSelection)
        {
            selectedSprite = GetCardByIndices(forcedSuitIndex, forcedCardIndex);
        }
        else
        {
            // Randomly choose a suit
            int suitIndex = Random.Range(0, 4);
            selectedSprite = GetCardByIndices(suitIndex, Random.Range(0, GetSuitArray(suitIndex).Length));
        }

        // Set the sprite of the SpriteRenderer to the selected one
        randomCardSpriteRenderer.sprite = selectedSprite;

        // Change the name of this GameObject to the name of the selected sprite
        gameObject.name = selectedSprite.name;
    }

    // Function to retrieve the correct sprite based on the suit and card index
    private Sprite GetCardByIndices(int suitIndex, int cardIndex)
    {
        Sprite[] selectedSuitArray = GetSuitArray(suitIndex);
        return selectedSuitArray[cardIndex];
    }

    // Helper function to get the correct suit array based on index
    private Sprite[] GetSuitArray(int suitIndex)
    {
        switch (suitIndex)
        {
            case 0: return heartSprites;
            case 1: return spadeSprites;
            case 2: return clubSprites;
            case 3: return diamondSprites;
            default: return null;
        }
    }
}
