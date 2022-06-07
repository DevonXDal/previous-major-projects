import model.*;
import org.junit.Before;
import org.junit.Test;

import static org.junit.Assert.*;

/**
 * The class FreeCellTest tests the implementation of the FreeCell class.
 *
 * @author Devon X. Dalrymple
 * @version 2020.11.18
 */
public class FreeCellTest
{
    FreeCell freeCell;

    /**
     * Creates a empty card column
     */
    @Before
    public void createEmptyCC()
    {
        freeCell = new FreeCell();
    }

    /**
     * Tests the card column using the tests from the interface
     */
    @Test
    public void interfaceTest()
    {
        CardCollectionTest interfaceTester = new CardCollectionTest(freeCell);
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
        assertTrue("Free cell did not allow a card into an empty slot", freeCell.checkLegal(new Card(Suit.DIAMONDS, Rank.ACE, 1)));
        freeCell.addCard(new Card(Suit.DIAMONDS, Rank.ACE, 1));
        assertFalse("Free cell allowed a card in a taken slot", freeCell.checkLegal(new Card(Suit.DIAMONDS, Rank.KING, 2)));
    }
}
