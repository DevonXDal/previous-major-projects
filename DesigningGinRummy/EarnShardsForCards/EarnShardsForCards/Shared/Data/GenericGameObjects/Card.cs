using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EarnShardsForCards.Shared.Data.GenericGameObjects
{
    /// <summary>
    /// A general structuring class used to handle cards for various games. 
    /// It includes the base information of the images for each face and which side of the card is shown.
    /// </summary>
    public abstract class Card
    {
        protected bool IsVisible { get; set; }

        protected string FaceUpImagePath {  get;  set; }

        protected string FaceDownImagePath {  get;  set; }

        /// <summary>
        /// Creates an instance of a card with the given face up and face down images.
        /// </summary>
        /// <param name="cardFrontImagePath">The image for the card that will be shown to the user when card's front is shown</param>
        /// <param name="cardBackImagePath">The image for the card that will be shown to the user when the card's back is shown</param>
        public Card(string cardFrontImagePath, string cardBackImagePath)
        {
            IsVisible = false;
            FaceUpImagePath = cardFrontImagePath;
            FaceDownImagePath = cardBackImagePath;
        }

        /// <summary>
        /// Put the card face up.
        /// </summary>
        public void Show()
        {
            IsVisible = true;
        }

        /// <summary>
        /// Put the card face down.
        /// </summary>
        public void Hide()
        {
            IsVisible = false;
        }

        /// <summary>
        /// Returns the image path for the card based on its face up status.
        /// </summary>
        /// <returns>The path to the image to display for the card</returns>
        public string GetImageFilePath()
        {
            if (IsVisible)
            {
                return FaceUpImagePath;
            }
            else
            {
                return FaceDownImagePath;
            }
        }
    }
}
