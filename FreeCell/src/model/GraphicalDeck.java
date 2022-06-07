package model;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.Random;

/**
 * Concrete class of the DeckADT interface that is built for graphical card games.
 *
 * @author Devon X. Dalrymple
 * @version 2020-12-10
 */
public class GraphicalDeck implements DeckADT
{
    private Card[] cards;
    private int last; //-1 means no cards are there to use

    /**
     * Default constructor for a deck that creates a list of cards and shuffles them
     */
    public GraphicalDeck() {
        super();
        cards = new GraphicalCard[52];
        last = -1; //Last index is current -1 because there is no cards

        fill();
        shuffle();

    }

    /**
     * Creates a deck from cards provided to the constructor then it shuffles them.
     * Still allows 52 cards
     *
     * @param cards An array of model.Card that is to be used instead of 52 cards.
     */
    public GraphicalDeck(Card[] cards) {
        super();
        this.cards = Arrays.copyOf(cards, 52);
        last = (cards.length - 1);
        shuffle();
    }

    /**
     * Getter for the size of the deck
     *
     * @return
     */
    public int getSize()
    {
        return (last + 1);
    }

    /**
     * Adds a card to the bottom of the deck if the deck does not have it
     *
     * @param card The card to be added if the deck does not already have it
     */
    @Override
    public void add(Card card) {

        if (last == cards.length-1)
        {
            return; //Full deck
        }
        if (!(cards[0] == null))
        {
            for (int i = 0; i <= last; i++)
            {
                if (cards[i].toString().equals(card.toString()))
                {
                    return;
                }
            }
        }

        if (last >= 0)
        {
            int i = last;
            while (i > -1)
            {
                cards[i+1] = cards[i];
                i--;
            }
        }

        cards[0] = card;
        last++;
    }

    /**
     * Deals the top card of the deck.
     *
     * @return card The card removed from the deck
     */
    @Override
    public Card drawTop()
    {
        if (last == -1 ) //Empty Array
        {
            return null;
        }

        Card toReturn = cards[last];
        last--;
        return toReturn;
    }

    /*
     * Fills the deck full of cards, clears the cards that are already inside of it. Gives the deck card
     * values of aces high.
     *
     * Throws an IllegalStateException if something strange occurs with the value
     */
    private void fill() throws IllegalStateException {
        last = -1; //Say the array is empty

        for (Rank rank : Rank.values())
        {
            for (Suit suit : Suit.values())
            {
                int value;
                switch (rank)
                {
                    case ACE:
                        value = 1;
                        break;
                    case TWO:
                        value = 2;
                        break;
                    case THREE:
                        value = 3;
                        break;
                    case FOUR:
                        value = 4;
                        break;
                    case FIVE:
                        value = 5;
                        break;
                    case SIX:
                        value = 6;
                        break;
                    case SEVEN:
                        value = 7;
                        break;
                    case EIGHT:
                        value = 8;
                        break;
                    case NINE:
                        value = 9;
                        break;
                    case TEN:
                        value = 10;
                        break;
                    case JACK:
                        value = 11;
                        break;
                    case QUEEN:
                        value = 12;
                        break;
                    case KING:
                        value = 13;
                        break;
                    default:
                        throw new IllegalStateException("Unexpected value: " + rank);
                }

                add(new GraphicalCard("/normalcards/" + rank.toString().toLowerCase() + "_of_" + suit.toString().toLowerCase() + ".png", suit, rank, value));
            }
        }
    }

    /**
     * Shuffles the cards in the deck
     * https://www.journaldev.com/32661/shuffle-array-java - for the for loop.
     */
    @Override
    public void shuffle()
    {
        if (last == -1)
        {
            return; //Empty array successfully shuffled
        }
        Random random = new Random();

        for (int i = 0; i <= last; i++)
        {
            int swapIndex = random.nextInt(last);
            Card temp = cards[swapIndex];
            cards[swapIndex] = cards[i];
            cards[i] = temp;
        }
    }

    /**
     * Retrieves 1-4 aces from the deck.
     *
     * @param number The number of aces to retrieve
     * @return The aces
     */
    public Card[] retrieveAces(int number)
    {
        if (number < 1 || number > 4)
        {
            return null;
        }

        Card[] acesGrabbed = new Card[number];
        ArrayList<Card> list = new ArrayList<>(Arrays.asList(cards));
        for (int i = 0, card = 0; i < number && card < list.size(); card++) //Grab the aces
        {
            if (list.get(card).getRank() == Rank.ACE)
            {
                acesGrabbed[i++] = list.remove(card--);
            }
        }
        last = list.size() -1;
        cards = list.toArray(new Card[52]);


        shuffle();
        return acesGrabbed;
    }
}
