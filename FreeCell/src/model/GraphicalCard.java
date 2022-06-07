package model;

/**
 * Concrete class implementation of AbstractPlayingCard meant for the card game war.
 *
 * @author Devon X. Dalrymple
 * @version 2020.11.08
 */
public class GraphicalCard extends Card
{
    private CardImage cardImage;

    /**
     * Creates a card with its suit and rank. The rank must have only the first capitalized and the first letter must be capitalized. This may lead to a IllegalArgumentException otherwise.
     *
     * @param frontImage The card's front image
     * @param suit The suit of the card
     * @param rank The rank of the capitalized which should be formatted like "3" or "Ace". Expect an IllegalArgumentException if not specified correctly
     * @param value The value of the card
     */
    public GraphicalCard(String frontImage, Suit suit, Rank rank, int value)
    {
        super(suit, rank, value);
        cardImage = new CardImage(frontImage);
    }

    /**
     * Returns the image filepath string appropriate to this card's current visibility
     *
     * @return The filepath to the image
     */
    @Override
    public String getImage()
    {
        return cardImage.getImage();
    }

    /*
     * The class CardImage handles the filepaths for the card's related images.
     *
     * @author Devon X. Dalrymple
     * @version 2020.11.08
     */
    private class CardImage
    {
        private String frontImage;
        private String backImage;

        /*
         * Takes a
         */
        private CardImage(String frontImage)
        {
            backImage = null;

            this.frontImage = frontImage;
        }

        /*
         * Returns the image filepath string associated with the card (either the back or the front)
         *
         * @return The filepath string of the image
         */
        private String getImage()
        {
            if (getVisibility())
            {
                return frontImage;
            }
            return backImage;
        }
    }

}
