package model;

/**
 * The class FreeCell is a code implementation of a free cell for the game Free Cell Solitaire. It provides operations
 * to add and remove cards from the free cell and check for a legal move.
 *
 * @author Devon X. Dalrymple
 * @version 2020.11.08
 */
public class FreeCell implements CardCollection
{
    private Card card;

    public FreeCell()
    {
        card = null;
    }

    /**
     * Adds a card to the collection. It throws an IllegalStateException if the the card can not be added for some reason
     *
     * @param card The card to be added
     * @throws IllegalStateException Thrown when a card can not be added
     */
    @Override
    public void addCard(Card card) throws IllegalStateException
    {
        if (this.card == null)
        {
            this.card = card;
        }
        else
        {
            throw new IllegalStateException("The free cell is already in use");
        }
    }

    /**
     * Returns the last card stored or null if no cards are stored
     *
     * @return The last card stored or null
     */
    @Override
    public Card getCard()
    {
        return card;
    }
    /**
     * Removes and retrieves the last card stored. Throws an IllegalStateException if no card is stored
     *
     * @return The card stored last
     * @throws IllegalStateException Thrown if no card is stored
     */
    @Override
    public Card removeCard() throws IllegalStateException
    {
        if (card == null)
        {
            throw new IllegalStateException("There is no card to remove");
        }
        Card card = this.card;
        this.card = null;
        return card;
    }

    /**
     * Checks if the card to be placed is allowed to be stored in the collection as a legal move
     *
     * A card is legal to move to a free cell if the free cell does not contain another card.
     *
     * @param card The card to check
     * @return Is the card allowed to be stored?
     */
    @Override
    public boolean checkLegal(Card card)
    {
        if (this.card == null)
        {
            return true;
        }
        return false;
    }
}
