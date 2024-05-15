namespace FlashCards_Project
{
    public class FlashcardDTO
    {
        public int ID { get; set; }
        public required string FrontText { get; set; }
        public required string BackText { get; set; }

        public int StoredStackId { get; set; } //Must exists in the Stack class
        // public virtual Stack Stack { get; set; } // Which stack is belongs to

        public int FlashcardId { get; set; } //For usage to connect to original flashcarad
    }
}