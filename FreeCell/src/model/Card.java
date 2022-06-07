package model;

/**
 * Sets up the base methods that a playing card would use. It does not have support for jokers.
 *
 * @author Devon X. Dalrymple
 * @version 2020.09.14
 */
public class Card
{

    private Suit suit;
    private Rank rank;
    private int value;
    private boolean isVisible;

    public Card(Suit suit, Rank rank, int value)
    {
        this.suit = suit;
        this.rank = rank;
        this.value = value;
        this.isVisible = true;
    }

    /**
     * Accessor method for the card's suit.
     *
     * @return suit The suit of the card (Diamonds, Hearts, Clubs, and Spades)
     */
    public Suit getSuit()
    {
        return suit;
    }

    /**
     * Accessor method for the card's rank.
     *
     * @return rank A String with the name of the rank (1-10, Jack, Queen, and King)
     */
    public Rank getRank()
    {
        return rank;
    }

    /**
     * Returns a value associated with a rank.
     *
     * @return value The value associated with that card's rank
     */
    public int getValue()
    {
        return value;
    }

    /**
     * Returns the visibility of the card
     *
     * @return value The value associated with that card's rank
     */
    public boolean getVisibility()
    {
        return isVisible;
    }

    /**
     * Toggles the visibility of the card
     */
    public void toggleVisibility()
    {
        isVisible = !isVisible;
    }

    /**
     * Gets the image of the card (In console ready form)
     *
     * @return The string image of the card
     */
    public String getImage()
    {
        return toString();
    }

    /*
     * toString of the card's name
     *
     * @return name The name of the card
     */
    @Override
    public String toString()
    {
        return (rank + " of " + suit);
    }

}
