import model.GraphicalCard;
import model.Rank;
import model.Suit;
import org.junit.Test;

import static org.junit.Assert.*;

/**
 * Tests the concrete class model.Card for the correct values to be returned
 *
 * @author Devon X. Dalrymple
 * @version 2020.09.09
 */
public class GraphicalCardTest
{
    /**
     * Tests the card's constructor to ensure values are assigned and an exception is thrown when it should
     */
    @Test
    public void constructorTest()
    {
        GraphicalCard playingCard = new GraphicalCard("", Suit.CLUBS, Rank.QUEEN, 12);

        assertEquals(Suit.CLUBS, playingCard.getSuit());
        assertEquals(Rank.QUEEN, playingCard.getRank());
        assertEquals(12, playingCard.getValue());
    }

    /**
     * Tests the toString method to ensure the right name of a card is returned
     */
    @Test
    public void toStringTest()
    {
        assertEquals("Ace of Spades", (new GraphicalCard("", Suit.SPADES, Rank.ACE, 1)).toString());
        assertEquals("2 of Hearts", (new GraphicalCard("", Suit.HEARTS, Rank.TWO, 1)).toString());
        assertEquals("Jack of Clubs", (new GraphicalCard("", Suit.CLUBS, Rank.JACK, 1)).toString());
        assertEquals("8 of Diamonds", (new GraphicalCard("", Suit.DIAMONDS, Rank.EIGHT, 1)).toString());
    }
}
