namespace TaskManagement.Domain
{
    public class ReactionType
    {
        public int Id { get; set; }

        public required string Name { get; set; }
        public string? Icon { get; set; }

        public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();
    }
}