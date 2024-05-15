using System.ComponentModel.DataAnnotations.Schema;

namespace FlashCards_Project
{
    public class Flashcard
    {
        public int FlashcardId { get; set; }
        public required string FrontText { get; set; }
        public required string BackText { get; set; }

        [ForeignKey("Stack")]
        public int StoredStackId { get; set; } //Must exists in the Stack class
        public virtual Stack Stack { get; set; } = null!;// Which stack is belongs to, required to store the stack ID
    }

}