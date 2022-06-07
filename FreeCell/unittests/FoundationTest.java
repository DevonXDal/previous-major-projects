import model.*;
import org.junit.Before;
import org.junit.Test;

import static org.junit.Assert.*;
import static org.junit.Assert.assertTrue;

/**
 * The class FoundationTest tests the Foundation class
 *
 * @author Devon X. Dalrymple
 * @version 2020.11.18
 */
public class FoundationTest
{
    Foundation foundation;

    /**
     * Creates a empty card column
     */
    @Before
    public void createEmptyCC()
    {
        foundation = new Foundation(Suit.DIAMONDS);
    }

    /**
     * Tests the card column using the tests from the interface
     */
    @Test
    public void interfaceTest()
    {
        CardCollectionTest interfaceTester = new CardCollectionTest(foundation);
        interfaceTester.constructorTest();
        interfaceTester.addAndGetCardTest();
        interfaceTester.addAndRemoveTest();
        interfaceTester.checkSizeTest();
    }

    /**
     * Tests to see if the card column returns the right boolean response as to if a card moved there would be legal
     */
    @Test
    public void checkLegalTest()
    {
        foundation.addCard(new Card(Suit.DIAMONDS, Rank.ACE, 1));
        foundation.addCard(new Card(Suit.DIAMONDS, Rank.TWO, 2));

        assertFalse("Different suit can be added", foundation.checkLegal(new Card(Suit.CLUBS, Rank.THREE, 3)));
        assertFalse("Different rank can be added", foundation.checkLegal(new Card(Suit.DIAMONDS, Rank.SIX, 6)));
        assertTrue("Correct card could not be added", foundation.checkLegal(new Card(Suit.DIAMONDS, Rank.THREE, 3)));
    }
}
