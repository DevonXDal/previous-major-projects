package model;

/**
 * The class Tableau represents a tableau used in a solitaire game. It provides access to the card columns used and
 * ways to access the images used in the cards.
 *
 * @author Devon X. Dalrymple
 * @version 2020.11.17
 */
public class Tableau
{
    private CardColumn[] cardColumns;

    /**
     * Default constructor for the tableau that sets up its fields.
     */
    public Tableau()
    {
        cardColumns = new CardColumn[8];
        for (int i = 0; i < cardColumns.length; i++)
        {
            cardColumns[i] = new CardColumn();
        }
    }

    /**
     * Grabs a card column using a range from 1-8
     *
     * @param option The option from the range 1-8 to return
     * @return The card column selected
     */
    public CardColumn getCardColumn(int option)
    {
        return cardColumns[option-1];
    }

    /**
     * Grabs the paths for the card images in each card column and returns them as a 2D String array
     *
     * @return The card image paths on the tableau
     */
    public String[][] getCardImages()
    {
        return new String[][]{cardColumns[0].getCardImages(), cardColumns[1].getCardImages(), cardColumns[2].getCardImages(),
                                cardColumns[3].getCardImages(), cardColumns[4].getCardImages(), cardColumns[5].getCardImages(),
                                cardColumns[6].getCardImages(), cardColumns[7].getCardImages()};
    }
}
