package model;

/**
 * ADT class for use with card games that use playing cards. It was not built to deal with jokers.
 *
 * @author Devon X. Dalrymple
 * @version 2020.09.09
 */
public interface DeckADT
{

    /**
     * Adds a card into the deck
     *
     * @param card The card to be added
     */
    public abstract void add(Card card);

    /**
     * Deal the top card from the deck and return it.
     *
     * @return card The card from the top of the deck
     */
    public abstract Card drawTop();

    /**
     * Getter for the size of the deck
     *
     * @return size The size of the deck
     */
    public int getSize();

    /**
     * Shuffles the cards left in the deck.
     */
    public abstract void shuffle();


}
