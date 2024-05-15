using System.Data.Entity.Migrations;

namespace FlashCards_Project
{
    public partial class DAL
    {
        private static List<FlashcardDTO> UpdateFlashcardDTOIds(List<FlashcardDTO> flashcards)
        {
            var stackId = 1; // Start with ID 1
            foreach (var flashcard in flashcards)
            {
                flashcard.ID = stackId++;
            }
            return flashcards;
        }

        public List<FlashcardDTO> GetFlashcardsInStack(int ID)
        {

            var stack = _context.Stacks.Find(ID);
            var fcs = _context.Flashcards.Where(f => f.StoredStackId == ID).ToList();
            List<FlashcardDTO> fc_dto = new();
            foreach (var flashcard in fcs)
            {
                var fc = _mapper.Map<FlashcardDTO>(flashcard);
                fc_dto.Add(fc);
            }
            return fc_dto;
        }

        public static int DisplayFlashcardsText(List<FlashcardDTO> flashcards, bool showBack = false)
        {
            List<FlashcardDTO> fc = UpdateFlashcardDTOIds(flashcards);
            int fc_count = fc.Count;
            if (fc_count == 0)
            {
                Console.WriteLine("No flashcards in stack!");
                return -1;
            }
            int score = 0;
            foreach (var flashcard in fc)
            {
                Console.WriteLine($"\n--------------------------------\n" +
                                    $"#{flashcard.ID}/{fc_count}\n" +
                                    $"{flashcard.FrontText}\n");

                if (!showBack)
                {
                    Console.WriteLine("Press any key to flip.");
                    Console.ReadKey();

                }
                Console.WriteLine(flashcard.BackText);
                if (!showBack)
                {
                    Console.WriteLine("Mark as correct with 'o' or wrong with 'x'");
                    char correct = Console.ReadKey().KeyChar;
                    if (correct == 'o')
                    {
                        score++;
                    }
                }
                Console.WriteLine("--------------------------------");
            }
            if (!showBack)
            {
                Console.WriteLine($"{score}/{fc_count}");
            }
            return score;
        }

        public bool CreateFlashcard(string? frontText, string? backText, int stackIdToAddTo)
        {
            if (!string.IsNullOrEmpty(frontText) && !string.IsNullOrEmpty(backText) && CheckStackValid(stackIdToAddTo))
            {
                var flashcardToAdd = new Flashcard() { FrontText = frontText, BackText = backText, StoredStackId = stackIdToAddTo };
                _context.Flashcards.Add(flashcardToAdd);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public void DeleteFlashcardInStack(List<FlashcardDTO> allCards, int flashcardId)
        {
            var flashcardToDelete = allCards.Find(s => s.ID == flashcardId);
            if (flashcardToDelete != null)
            {
                var fc = _context.Flashcards.Find(flashcardToDelete.FlashcardId);
                if (fc != null)
                {
                    _context.Flashcards.Remove(fc);
                    _context.SaveChanges();
                    Console.WriteLine("Flashcard deleted successfully");
                }

            }
        }
        public void UpdateFlashcardInStack(List<FlashcardDTO> allCards, int flashcardId)
        {
            var flashcardToDelete = allCards.Find(s => s.ID == flashcardId);
            if (flashcardToDelete != null)
            {
                var fc = _context.Flashcards.Find(flashcardToDelete.FlashcardId);
                if (fc != null)
                {
                    Console.WriteLine("Current front: " + fc.FrontText + ". Enter new text: <Leave empty to keep>");
                    string? input = Console.ReadLine();
                    Console.WriteLine("Current back: " + fc.BackText + ". Enter new text: <Leave empty to keep>");
                    string? input_back = Console.ReadLine();
                    if (!string.IsNullOrEmpty(input))
                    {
                        fc.FrontText = input;
                    }
                    if (!string.IsNullOrEmpty(input_back))
                    {
                        fc.BackText = input_back;
                    }

                    _context.Flashcards.AddOrUpdate(fc);
                    _context.SaveChanges();
                    Console.WriteLine("Flashcard updated successfully");
                }
            }
        }

    }
}