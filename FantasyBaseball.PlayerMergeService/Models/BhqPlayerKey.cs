using System;
using FantasyBaseball.CommonModels.Enums;

namespace FantasyBaseball.PlayerMergeService.Models
{
    /// <summary>A simple object for matching up players. Using type to distinguish two way players.</summary>
    public sealed class BhqPlayerKey
    {
        /// <summary>Create a new immutable instance of the key.</summary>
        /// <param name="id">The player's ID.</param>
        /// <param name="type">The player's type (B for Batter or P for Pitcher).</param>
        public BhqPlayerKey(int id, PlayerType type)
        {
            Id = id;
            Type = type;
        }

        /// <summary>The player's ID.</summary>
        public int Id { get; private set; }

        /// <summary>The player's type (Batter or Pitcher).</summary>
        public PlayerType Type { get; private set; }

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified object is equal to the current object; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj) => Equals(obj as BhqPlayerKey);
        
        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode() => HashCode.Combine(Id, Type);

        private bool Equals(BhqPlayerKey obj) => obj != null && Id == obj.Id && Type == obj.Type;
    }
}