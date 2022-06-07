package model;

/**
 * The class Table is the access point for a controller to access the components of FreeCell. It does not know anything
 * about the controller that accesses it.
 *
 * @author Devon X. Dalrymple
 * @version 2020.11.17
 */
public class Table
{
    private FreeCell[] freeCells;
    private Foundation[] foundations; //Hearts, Diamonds, Clubs, Spades is the order of the foundations stored
    private Tableau tableau;

    /**
     * The default constructor for table creates the components used on a table for playing FreeCell.
     */
    public Table()
    {
        freeCells = new FreeCell[]{new FreeCell(), new FreeCell(), new FreeCell(), new FreeCell()};
        foundations = new Foundation[]{new Foundation(Suit.HEARTS), new Foundation(Suit.DIAMONDS), new Foundation(Suit.CLUBS), new Foundation(Suit.SPADES)};
        tableau = new Tableau();
    }

    /**
     * Getter for the tableau
     *
     * @return The tableau used in FreeCell
     */
    public Tableau getTableau()
    {
        return tableau;
    }

    /**
     * Getter for a foundation
     *
     * Acceptable values are:
     * 1. Hearts Foundation
     * 2. Diamonds Foundation
     * 3. Clubs Foundation
     * 4. Spades Foundation
     *
     * @param foundation The foundation to select with the acceptable values
     * @return The selected foundation
     */
    public Foundation getFoundation(int foundation)
    {
        return foundations[foundation-1];
    }

    /**
     * Getter for a free cell
     *
     * Use the range of 1-4 to specify a freecell
     *
     * @param freeCell The free cell to select with the acceptable range
     * @return The selected free cell
     */
    public FreeCell getFreeCell(int freeCell)
    {
        return freeCells[freeCell-1];
    }
}
