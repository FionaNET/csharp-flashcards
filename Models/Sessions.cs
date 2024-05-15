using System.ComponentModel.DataAnnotations.Schema;

namespace FlashCards_Project
{
    public class Sessions
    {
        public int ID { get; set; }

        [Column(TypeName = "Date")]
        public required DateTime DateCreated { get; set; }
        public required int Score { get; set; }

        [ForeignKey("Stack")]
        public int StoredStackId { get; set; } //Must exists in the Stack class
        public virtual Stack Stack { get; set; } = null!; // Which stack is belongs to
    }
}