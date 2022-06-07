import model.*;
import org.junit.Before;
import org.junit.Test;
import static org.junit.Assert.*;

/**
 * The class CardColumnTest tests the CardColumn class to ensure that it functions as it should.
 *
 * @author Devon X. Dalrymple
 * @version 2020.11.18
 */
public class CardColumnTest
{
    CardColumn cc;

    /**
     * Creates a empty card column
     */
    @Before
    public void createEmptyCC()
    {
        cc = new CardColumn();
    }

    /**
     * Tests the card column using the tests from the interface
     */
    @Test
    public void interfaceTest()
    {
        CardCollectionTest interfaceTester = new CardCollectionTest(new CardColumn());
        interfaceTester.constructorTest();
        interfaceTester.addAndGetCardTest();
        interfaceTester.addAndRemoveTest();
        interfaceTester.checkSizeTest();
    }

    /**
     * Tests the card column to see if an array of the appropriate number of strings is returned from grabbing the card
     * images.
     */
    @Test
    public void getCardImagesTest()
    {
        for (int i = 0; i < 4; i++)
        {
            cc.addCard(new GraphicalCard("", Suit.SPADES, Rank.FIVE, 1));
        }

        String[] images = cc.getCardImages();
        assertEquals(4, images.length);
    }

    /**
     * Tests to see if the card column returns the right boolean response as to if a card moved there would be legal
     */
    @Test
    public void checkLegal()
    {
        cc.addCard(new Card(Suit.SPADES, Rank.FIVE, 5));

        assertFalse("Random card can be added", cc.checkLegal(new Card(Suit.CLUBS, Rank.NINE, 9)));
        assertFalse("Card of wrong suit can be added", cc.checkLegal(new Card(Suit.CLUBS, Rank.FOUR, 4)));
        assertFalse("Card of wrond rank can be added", cc.checkLegal(new Card(Suit.HEARTS, Rank.NINE, 9)));
        assertFalse("Same card can be added", cc.checkLegal(new Card(Suit.SPADES, Rank.FIVE, 5)));
        assertTrue("Correct Card 1 could not be added", cc.checkLegal(new Card(Suit.DIAMONDS, Rank.FOUR, 4)));
        assertTrue("Correct Card 2 could not be added", cc.checkLegal(new Card(Suit.HEARTS, Rank.FOUR, 4)));

        System.out.println("Tests for legal side 1 passed");

        cc.addCard(new Card(Suit.DIAMONDS, Rank.TWO, 2));

        assertFalse("Random card can be added", cc.checkLegal(new Card(Suit.HEARTS, Rank.NINE, 9)));
        assertFalse("Card of wrong suit can be added", cc.checkLegal(new Card(Suit.DIAMONDS, Rank.ACE, 1)));
        assertFalse("Card of wrond rank can be added", cc.checkLegal(new Card(Suit.HEARTS, Rank.NINE, 9)));
        assertFalse("Same card can be added", cc.checkLegal(new Card(Suit.DIAMONDS, Rank.TWO, 2)));
        assertTrue("Correct Card 1 could not be added", cc.checkLegal(new Card(Suit.CLUBS, Rank.ACE, 1)));
        assertTrue("Correct Card 2 could not be added", cc.checkLegal(new Card(Suit.SPADES, Rank.ACE, 1)));

    }
}
