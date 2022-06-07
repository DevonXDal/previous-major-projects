using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EarnShardsForCards.Shared.Data.Enumerations;
using EarnShardsForCards.Shared.Data.GenericGameObjects;
using FluentAssertions;
using Xunit;
using static EarnShardsForCards.Test.TestHelpers.GeneralTestingAssister;

namespace EarnShardsForCards.Test.Models
{
    /// <summary>
    /// Tests the PlayingCard class to ensure that it conforms to that specified in the design document.
    /// 
    /// Axioms:
    /// I.	(card.new(rank, suit, value)).Rank == rank
    /// II.	(card.new (rank, suit, value)).Suit == suit
    /// III. card.new (rank, suit, value)
    /// card.Show()
    /// card.GetImageFilePath() == “~/img/PlayingCard/{rank-{suit}.webp”
    /// IV. card.new (rank, suit, value)
    /// card.GetImageFilePath() == “~/ img / PlayingCard / Back.webp”
    /// V. card.new (rank, suit, value)
    /// card.Value == value
    /// VI.	card.new(rank, suit, value)
    /// card.Equals({another card with the same rank and suit}) == true
    /// card.Equals({ another card with a different rank}) == false
    /// card.Equals({ another card with a different suit}) == false
    /// card.Equals({ another card with both a different rank and a different suit}) == false
    /// </summary>
    public class PlayingCardTest
    {
        public PlayingCardTest()
        {
            
        }

        /// <summary>
        /// Tests the card's constructor to ensure that its inital state is configured correctly
        /// 
        /// Axioms:
        /// I.	(card.new(rank, suit, value)).Rank == rank
        /// II.	(card.new (rank, suit, value)).Suit == suit
        /// IV.card.new (rank, suit, value)
        /// card.GetImageFilePath() == “img/PlayingCard/Back.webp”
        /// V. card.new (rank, suit, value)
        /// card.Value == value
        /// 
        /// </summary>
        [Fact]
        public void ConstructorTest()
        {
            RunFunctionUsingEachPlayingCard((PlayingCard card, Rank rank, Suit suit, int value) =>
            {
                // Assert
                card.Rank.Should().Be(rank);
                card.Suit.Should().Be(suit);
                card.Value.Should().Be(value);
                card.GetImageFilePath().Should().Be("img/PlayingCard/Back.webp");
            });    
        }

        /// <summary>
        /// Tests the card's image to ensure that it matches the expected image for that visibility modifier.
        /// 
        /// Axioms:
        /// IV.card.new (rank, suit, value)
        /// card.GetImageFilePath() == “~/ img / PlayingCard / Back.webp”
        /// III. card.new (rank, suit, value)
        /// card.Show()
        /// card.GetImageFilePath() == “~/img/PlayingCard/{rank}-{suit}.webp
        /// </summary>
        [Fact]
        public void ShowCorrectImageBasedOnVisibillityTest()
        {
            RunFunctionUsingEachPlayingCard((PlayingCard card, Rank rank, Suit suit, int value) =>
            {
                // Act & Assert
                card.GetImageFilePath().Should().Be("img/PlayingCard/Back.webp");

                card.Show();
                card.GetImageFilePath().Should().Be($"img/PlayingCard/{rank}-{suit}.webp");
            });
        }

        /// <summary>
        /// Tests that a card shows as equal to another whenever the rank and suit are the same. The value does not matter for equality checks.
        /// 
        /// VI.	card.new(rank, suit, value)
        /// card.Equals({another card with the same rank and suit}) == true
        /// card.Equals({ another card with a different rank}) == false
        /// card.Equals({ another card with a different suit}) == false
        /// card.Equals({ another card with both a different rank and a different suit}) == false
        /// </summary>
        [Fact]
        public void EqualityTest()
        {
            RunFunctionUsingEachPlayingCard((PlayingCard card, Rank rank, Suit suit, int value) =>
            {
                Rank differentRank = Enum.GetValues<Rank>().First(e => e != rank);
                Suit differentSuit = Enum.GetValues<Suit>().First(e => e != suit);

                // Act & Assert
                card.Equals(new PlayingCard(rank, suit, value)).Should().BeTrue(); // Same card

                card.Equals(new PlayingCard(differentRank, suit, value)).Should().BeFalse(); // Different rank
                card.Equals(new PlayingCard(rank, differentSuit, value)).Should().BeFalse(); // Different suit
                card.Equals(new PlayingCard(differentRank, differentSuit, value)).Should().BeFalse(); // Different rank and suit
            });
        } 
    }
}
