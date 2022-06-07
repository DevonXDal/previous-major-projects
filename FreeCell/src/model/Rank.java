package model;

/**
 * The enumeration Rank stored the valid ranks for a set of playing cards
 *
 * @author Devon X. Dalrymple
 * @version 2020.11.08
 */
public enum Rank
{
    ACE, TWO, THREE, FOUR, FIVE, SIX, SEVEN, EIGHT, NINE, TEN, JACK, QUEEN, KING;

    /*
     * Returns a string formatted correctly of the used RANK
     *
     * @return String the String representation of the constant used
     */
    @Override
    public String toString() {
        switch (this)
        {
            case ACE:
                return "Ace";
            case TWO:
                return "2";
            case THREE:
                return "3";
            case FOUR:
                return "4";
            case FIVE:
                return "5";
            case SIX:
                return "6";
            case SEVEN:
                return "7";
            case EIGHT:
                return "8";
            case NINE:
                return "9";
            case TEN:
                return "10";
            case JACK:
                return "Jack";
            case QUEEN:
                return "Queen";
            case KING:
                return "King";
            default:
                throw new IllegalArgumentException();
        }

    }
}
