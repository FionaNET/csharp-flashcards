using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FlashCards_Project
{
    public class Stack
    {
        public int StackId { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(100)]
        [Index(IsUnique = true)]
        public required string StackName { get; set; }

        public string Description { get; set; } = string.Empty;

        public ICollection<Flashcard>? Flashcards { get; set; }
        public ICollection<Sessions>? Sessions { get; set; }
    }

}