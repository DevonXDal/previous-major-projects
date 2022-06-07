package model;

import java.util.EmptyStackException;
import java.util.Iterator;
import java.util.Stack;

/**
 * The class CardColumn represents a column of cards in the tableau.
 *
 * @author Devon X. Dalrymple
 * @version 2020.12.10
 */
public class CardColumn implements CardCollection
{
    private Stack<Card> cards;

    /**
     * Creates an empty card column
     */
    public CardColumn()
    {
        cards = new Stack<>();
    }

    /**
     * Adds a card to the collection. It throws an IllegalStateException if the the card can not be added for some reason
     *
     * @param card The card to be added
     */
    @Override
    public void addCard(Card card)
    {
        cards.push(card);
    }

    /**
     * Returns the last card stored or null if no cards are stored
     *
     * @return The last card stored or null
     */
    @Override
    public Card getCard()
    {
        try
        {
            return cards.peek();
        }
        catch (EmptyStackException ignored)
        {
            return null;
        }

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
        if (cards.size() == 0)
        {
            throw new IllegalStateException("No card to remove");
        }
        return cards.pop();
    }

    /**
     * Checks if the card to be placed is allowed to be stored in the collection as a legal move
     *
     * A card is legal to go into a card column if its rank is one less then the rank of the card at the top of the card column
     * (usually displays at the bottom instead of the top) and its suit is of opposite polar suit color.
     * Hearts and Diamonds are one polar color.
     * Spades and clubs are of the other polar color.
     *
     * @param card The card to check
     * @return Is the card allowed to be stored?
     */
    @Override
    public boolean checkLegal(Card card)
    {
        try
        {
            Card topCard = cards.peek();
            if (topCard.getSuit() == Suit.CLUBS || topCard.getSuit() == Suit.SPADES)
            {
                return (card.getSuit() == Suit.DIAMONDS && card.getValue() == (topCard.getValue() - 1) || card.getSuit() == Suit.HEARTS && card.getValue() == (topCard.getValue() - 1));
            }
            else
            {
                return (card.getSuit() == Suit.CLUBS && card.getValue() == (topCard.getValue() - 1) || card.getSuit() == Suit.SPADES && card.getValue() == (topCard.getValue() - 1));
            }
        }
        catch (EmptyStackException ignored)
        {
            return true;
        }
    }

    /**
     * Returns an array of images used with the cards in string format
     *
     * @return The images used by the cards
     */
    public String[] getCardImages()
    {
        Iterator<Card> it = cards.iterator();
        String[] images = new String[cards.size()];
        int i = 0;
        while (it.hasNext())
        {
            images[i++] = it.next().getImage();
        }
        return images;
    }
}
