using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EarnShardsForCards.Test.Models
{
    /// <summary>
    /// Tests the implementation of the PlayingCardDeck class to ensure that it is working as expected.
    /// 
    /// Axioms:
    /// I.	(deck.new()).Count() == 52
    /// II.deck.new ()
    /// deck.Shuffle()
    /// check the difference between the cards
    /// adjacentCardsCount <= 26 (Ensure that cards are shuffled well and do not have cards in sequential order too frequently or of just differing suits)
    /// III.deck.new ()
    /// card = deck.Draw()
    /// card.Rank == Rank.Ace
    /// card.Suit == Suit.Diamonds
    /// Repeat to verify all 13 ranks appear for each suit
    /// deck.Count () == 0
    /// IV.deck.new ()
    /// deck.Draw()
    /// deck.Count() == 51
    /// deck.Draw()
    /// deck.Count() == 50
    /// V.deck.new (cards)
    /// deck.Count() == cards.Count()
    /// card = deck.Draw()
    /// card.Show()
    /// card.Rank == [rank of the last card in the list passed]
    /// card.Suit == [suit of the last card in the list passed]
    /// Repeat to ensure the cards were inserted into the correct order from the passed list into the deck. 
    /// Such that the first card is at the bottom of the deck and the last card is at the top of the decks
    /// </summary>
    public class PlayingCardDeckTest
    {
        [Fact]
        public void ConstructorTest()
        {
            
        }
    }
}
