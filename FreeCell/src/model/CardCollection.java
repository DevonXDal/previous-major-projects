package model;

/**
 * The interface CardCollection provides the basic methods that are common for a number of collections used for storing cards.
 *
 * @author Devon X. Dalrymple
 * @version 2020.11.18
 */
public interface CardCollection
{
    /**
     * Adds a card to the collection. It throws an IllegalStateException if the the card can not be added for some reason
     *
     * @param card The card to be added
     * @throws IllegalStateException Thrown when a card can not be added
     */
    public void addCard(Card card) throws IllegalStateException;

    /**
     * Returns the last card stored or null if no cards are stored
     *
     * @return The last card stored or null
     */
    public Card getCard();

    /**
     * Removes and retrieves the last card stored. Throws an IllegalStateException if no card is stored
     *
     * @return The card stored last
     * @throws IllegalStateException Thrown if no card is stored
     */
    public Card removeCard() throws IllegalStateException;

    /**
     * Checks if the card to be placed is allowed to be stored in the collection as a legal move
     *
     * @param card The card to check
     * @return Is the card allowed to be stored?
     */
    public boolean checkLegal(Card card);

    /**
     * Should be used to return the size of the subclass' collection if implemented. Otherwise it throws a an
     * UnsupportedOperationException.
     *
     * @throws UnsupportedOperationException Thrown if not implemented in the subclass
     * @return The size of the collection
     */
    public default int checkSize() throws UnsupportedOperationException
    {
        throw new UnsupportedOperationException("This class does not support the checkSize() method");
    }

}
