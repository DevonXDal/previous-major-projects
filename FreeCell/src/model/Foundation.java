package model;

import java.util.EmptyStackException;
import java.util.Stack;

/**
 * The class Foundation represents a foundation in the game, FreeCell Solitaire
 *
 * @author Devon X. Dalrymple
 * @version 2020-12-10
 */
public class Foundation implements CardCollection
{
    private Suit assignedSuit;
    private Stack<Card> cards;

    /**
     * Creates a foundation and assigns it the the Diamonds suit
     */
    public Foundation()
    {
        assignedSuit = Suit.DIAMONDS;
        cards = new Stack<>();
    }

    /**
     * Creates a foundation and assigns it the suit it should accept
     *
     * @param suit The suit of cards that this foundation will allow and hold
     */
    public Foundation(Suit suit)
    {
        assignedSuit = suit;
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
     * Checks if the card to be placed is allowed to be stored in the collection as a legal move.
     *
     * A card is legal to move to a foundation if it is of the foundation's suit and one of the following:
     * <ul>
     *     <li>
     *         It is an ace and the foundation is currently empty (aces low).
     *     </li>
     *     <li>
     *         It is a rank between 2-King and is one higher than the one at the top of the foundation.
     *     </li>
     * </ul>
     *
     * @param card The card to check
     * @return Is the card allowed to be stored?
     */
    @Override
    public boolean checkLegal(Card card)
    {
        if (cards.isEmpty())
        {
            if (card.getSuit() == assignedSuit)
            {
                return card.getValue() == 1;
            }
        }
        else
        {
            if (card.getSuit() == assignedSuit)
            {
                if (card.getValue() == (cards.peek().getValue() + 1))
                {
                    return true;
                }
            }
        }

        return false;
    }

    /**
     * Returns the number of cards in the collection
     *
     * @return The number of cards in this collection
     */
    @Override
    public int checkSize()
    {
        return cards.size();
    }
}
