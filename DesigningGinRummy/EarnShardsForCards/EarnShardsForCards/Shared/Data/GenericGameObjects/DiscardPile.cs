using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EarnShardsForCards.Shared.Data.GenericGameObjects
{
    /// <summary>
    /// A DiscardPile represents the real-world pile of cards that grows throughout the play of the game. 
    /// It is accessible from the top and is able to be cleared out. 
    /// It can be used as a source of cards for various games. 
    /// It may be at points empty and at other points full of cards.
    /// </summary>
    /// <typeparam name="T">The type of card that this discard pile contains</typeparam>
    public class DiscardPile<T> where T : Card
    {
        public IList<Card> Cards { get; protected set; }

        public string EmptyImageRepresentation { get; protected set; }

        /// <summary>
        /// Create a discard pile with no cards
        /// </summary>
        public DiscardPile()
        {
            Cards = new List<Card>();
            EmptyImageRepresentation = "img/No-Card.webp";
        }

        /// <summary>
        /// Add a card to the top of this pile.
        /// </summary>
        /// <param name="card">The card to be added</param>
        /// <param name="insertFaceUp">Whether the card should be face up (true by default)</param>
        public void Add(T card, bool insertFaceUp = true)
        {
            if (insertFaceUp)
            {
                card.Show();
            }
            
            Cards.Insert(Cards.Count, card);
        }

        /// <summary>
        /// Remove a card from the top of the pile and return it
        /// </summary>
        /// <returns>The card removed and returned from the top of the pile</returns>
        /// <exception cref="InvalidOperationException">Thrown if the discard pile is empty</exception>
        public T Draw()
        {
            if (Cards.Count == 0)
            {
                throw new InvalidOperationException("Cannot draw from an empty discard pile");
            }

            T card = Cards[Cards.Count - 1] as T;
            Cards.RemoveAt(Cards.Count - 1);
            return card;
        }

        /// <summary>
        /// Returns the current image representation for this pile.
        /// </summary>
        /// <returns>The image representation of this discard pile</returns>
        public string GetImageFilePath()
        {
            if (Cards.Count == 0)
            {
                return EmptyImageRepresentation;
            }

            return Cards[Cards.Count - 1].GetImageFilePath();
        }

        /// <summary>
        /// Returns the top card without removing it from the discard pile
        /// </summary>
        /// <returns>The top card of the discard pile</returns>
        /// <exception cref="InvalidOperationException">Thrown if there are no cards in the discard pile</exception>
        public T ViewTop()
        {
            if (Cards.Count == 0)
            {
                throw new InvalidOperationException("Cannot view the top of an empty discard pile");
            }

            return Cards[Cards.Count - 1] as T;
        }

        /// <summary>
        /// Empties the discard pile of all of its cards but does not return them.
        /// </summary>
        public void Clear()
        {
            Cards.Clear();
        }
    }
}
