using EarnShardsForCards.Shared.Data.Enumerations;
using EarnShardsForCards.Shared.Data.GenericGameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EarnShardsForCards.Test.TestHelpers
{
    /// <summary>
    /// Provides various methods useful for testing functionality.
    /// </summary>
    public class GeneralTestingAssister
    {
        /// <summary>
        /// Returns a random string of the specified length.
        /// </summary>
        /// <param name="length">The length of the string to return.</param>
        /// <returns>A random string of the specified length.</returns>
        public static string GetRandomString(int length)
        {
            var random = new Random();
            var result = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                result.Append((char)random.Next(97, 122));
            }
            return result.ToString();
        }

        /// <summary>
        /// Takes a function and invokes it using the card generated, along with the rank, suit, and value used.
        /// </summary>
        /// <param name="action">The function to invoke</param>
        public static void RunFunctionUsingEachPlayingCard(Action<PlayingCard, Rank, Suit, int> action)
        {
            // Arrange
            foreach (Rank rank in Enum.GetValues<Rank>())
            {
                foreach (Suit suit in Enum.GetValues<Suit>())
                {
                    int value = (int)rank + (int)suit;
                    PlayingCard card = new PlayingCard(rank, suit, value);

                    // Act
                    action.Invoke(card, rank, suit, value);
                }
            }
        }
    }
}
