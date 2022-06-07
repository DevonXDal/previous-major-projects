import model.Suit;
import org.junit.Test;

import static org.junit.Assert.assertEquals;
/**
 * Tests the toString method of model.Suit. That is basically it
 *
 * @author Devon X. Dalrymple
 * @version 2020.09.09
 */
public class SuitTest {

    /**
     * Tests the toString method for the correct value returns
     */
    @Test
    public void toStringTest()
    {
        Suit suit = Suit.CLUBS;
        assertEquals(suit.toString(), "Clubs");

        suit = Suit.DIAMONDS;
        assertEquals(suit.toString(), "Diamonds");

        suit = Suit.HEARTS;
        assertEquals(suit.toString(), "Hearts");

        suit = Suit.SPADES;
        assertEquals(suit.toString(), "Spades");
    }
}
