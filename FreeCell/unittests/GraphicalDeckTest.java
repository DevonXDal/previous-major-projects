import model.*;
import org.junit.Test;

import java.util.ArrayList;

import static org.junit.Assert.*;

/**
 * Tests the concrete class model.Deck for correct values and someone tests model.DeckADT which it extends.
 *
 * @author Devon X. Dalrymple
 * @version 2020.09.09
 */
public class GraphicalDeckTest
{
    /**
     * Tests the default constructor to ensure the deck is set up with cards
     */
    @Test
    public void defaultConstructorTest()
    {
        assertEquals(52, (new GraphicalDeck()).getSize());
    }

    /**
     * Tests the constructor that requires a preset of cards to use
     */
    @Test
    public void overloadedConstructorTest()
    {
        Card[] array = {new GraphicalCard("", Suit.SPADES, Rank.FIVE, 1), new GraphicalCard("", Suit.DIAMONDS, Rank.ACE, 1)};
        GraphicalDeck wd = new GraphicalDeck(array);
        Card testCard = wd.drawTop();

        assertTrue(testCard.getRank() == Rank.FIVE || testCard.getRank() == Rank.ACE && testCard.getSuit() == Suit.SPADES || testCard.getSuit() == Suit.DIAMONDS);
        assertEquals(1, wd.getSize());
    }

    /**
     * Tests the add method to ensure it adds cards correctly and no duplicates get added
     */
    @Test
    public void addTest()
    {
        GraphicalDeck deck = new GraphicalDeck();
        ArrayList<Card> aL = new ArrayList<>();

        aL.add(deck.drawTop());
        while (aL.get(0).getSuit() == Suit.CLUBS && aL.get(0).getRank() == Rank.FIVE)
        {
            deck.add(aL.remove(0));
            deck.shuffle();
            aL.add(deck.drawTop());
        }
        aL.add(new GraphicalCard("", Suit.CLUBS, Rank.FIVE, 1));
        assertEquals(51, deck.getSize());

        for (Card card : aL)
        {
            deck.add(card);
        }
        assertEquals(52, deck.getSize());
    }

    /**
     * Tests the shuffle method to ensure shuffling works.
     */
    @Test
    public void shuffleTest()
    {
        GraphicalDeck deck = new GraphicalDeck();
        Card[] array = {deck.drawTop(), deck.drawTop(), deck.drawTop(), deck.drawTop(), deck.drawTop(), deck.drawTop(), deck.drawTop(), deck.drawTop(),deck.drawTop()};
        deck = new GraphicalDeck(array);
        Card[] shuffled = {deck.drawTop(), deck.drawTop(), deck.drawTop(), deck.drawTop(), deck.drawTop(), deck.drawTop(), deck.drawTop(), deck.drawTop(),deck.drawTop()};



        int i = 0;
        boolean passed = false;


        for (Card card : shuffled)
        {
            if (!(card.getRank().equals(array[i].getRank())) || !(card.getSuit() == array[i].getSuit()))
            {
                passed = true;
                break;
            }
            i++;
        }

        if (!(passed))
        {
            fail("The deck was not shuffled");
        }
    }
}
