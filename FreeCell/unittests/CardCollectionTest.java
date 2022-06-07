import model.Card;
import model.CardCollection;
import model.Rank;
import model.Suit;

import static org.junit.Assert.*;

/**
 * The class CardCollectionTest is a test class using the reformed way of testing for interfaces. It tests the concrete
 * classes to ensure they follow the guidelines provided unto them by the interface.
 */
public class CardCollectionTest
{
    private CardCollection testSubject;
    private Class<? extends CardCollection> classBeingTested;

    /**
     * Non-default constructor of the unit test, used to test a specific implementation
     */
    public CardCollectionTest(CardCollection testSubject)
    {
        classBeingTested = testSubject.getClass();
    }

    /**
     * Sets up the object for use in tests, using the declared constructor of the subclass,
     * requires the default constructor to be implemented in the class you wish to test
     *
     * https://stackoverflow.com/questions/6094575/creating-an-instance-using-the-class-name-and-calling-constructor
     */
    public void setUp()
    {
        try
        {
            testSubject = classBeingTested.getDeclaredConstructor().newInstance();
        }
        catch (Exception exception)
        {
            System.out.println("This exception was thrown trying to create a new instance of the provided class" + exception.getMessage());
        }
    }

    /**
     * Tests that the concrete class is empty after creation by a default constructor.
     */
    public void constructorTest()
    {
        setUp();
        assertNull("The default constructor left cards in the card collection", testSubject.getCard());
    }

    /**
     * Tests that a card can be added to the collection and returned correctly and that getting the card does not remove it.
     */
    public void addAndGetCardTest()
    {
        setUp();
        Card card = new Card(Suit.SPADES, Rank.FIVE, 2);
        testSubject.addCard(card);
        Card toTest = testSubject.getCard();
        assertEquals("The card was ruined by adding or retrieving or otherwise stored incorrecly", card, toTest);
        assertNotNull("The get process removed the card as well", testSubject.getCard());
    }

    /**
     * Tests that a card can be added and removed from it and that removal returns the card.
     */
    public void addAndRemoveTest()
    {
        setUp();
        Card card = new Card(Suit.SPADES, Rank.FIVE, 2);
        testSubject.addCard(card);
        Card toTest = testSubject.removeCard();
        assertEquals("The card was ruined by adding or retrieving/removing or otherwise stored incorrecly", card, toTest);

        try
        {
            testSubject.removeCard();
            fail("The remove method did not throw an exception on an empty card collection");
        }
        catch (Exception ignored)
        {

        }
    }

    /**
     * Tests that the size method either throws an UnsupportedOperationException or returns the correct size
     */
    public void checkSizeTest()
    {
        setUp();
        try
        {
            int size = testSubject.checkSize();
            assertEquals("The size was not zero when empty", 0, size);
            testSubject.addCard(new Card(Suit.SPADES, Rank.FIVE, 2));
            assertEquals("The size was not 1 after adding a card", 1, testSubject.checkSize());
        }
        catch (UnsupportedOperationException ignored)
        {

        }
    }
}
