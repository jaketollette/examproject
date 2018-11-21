namespace ExamProject.Models.Entities
{
    /// <summary>
    /// A like for an idea
    /// </summary>
    public class Like
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the idea identifier.
        /// </summary>
        /// <value>
        /// The idea identifier.
        /// </value>
        public int IdeaId { get; set; }
    }
}