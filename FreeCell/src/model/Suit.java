package model;

/**
 * Enumeration for the suits of a card and how to correctly display them to the user
 *
 * @author Devon X. Dalrymple
 * @version 2020.09.09
 */
public enum Suit {
    HEARTS, DIAMONDS, CLUBS, SPADES;

    /*
     * Returns a string formatted correctly of the used suit
     *
     * @return String the String representation of the constant used
     */
    @Override
    public String toString() {
        switch (this)
        {
            case CLUBS:
                return "Clubs";
            case DIAMONDS:
                return "Diamonds";
            case HEARTS:
                return "Hearts";
            case SPADES:
                return "Spades";
            default:
                throw new IllegalArgumentException();
        }

    }
}
