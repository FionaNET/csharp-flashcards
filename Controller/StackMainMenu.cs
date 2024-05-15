using System.Text.RegularExpressions;

namespace FlashCards_Project
{
    public partial class MainMenu
    {
        public enum StackMenuOptions
        {
            Back = 0,
            ViewFlashcardsInStack = 1,
            CreateFlashcard = 2,
            UpdateFlashcardInStack = 3,
            DeleteFlashcard = 4,
        }

        public static void DisplayStackMenuOptions()
        {
            Console.WriteLine("Select an option:");
            foreach (StackMenuOptions option in Enum.GetValues(typeof(StackMenuOptions)))
            {
                var formattedText = formattedRegex().Replace(option.ToString(), " $1");
                Console.WriteLine($"[{(int)option}]: {formattedText}");
            }
        }

        public void SwitchStackMenuOption(int option, out bool mainMenuSelected)
        {
            mainMenuSelected = false;
            switch ((StackMenuOptions)option)
            {
                case StackMenuOptions.Back:
                    mainMenuSelected = true;
                    break;

                case StackMenuOptions.CreateFlashcard:
                    Console.WriteLine("Front of flashcard text: ");
                    string? frontText = Console.ReadLine();
                    Console.WriteLine("Back of flashcard text: ");
                    string? backText = Console.ReadLine();
                    dal.DisplayAllStacks();
                    Console.WriteLine("Stack ID to add to: ");
                    int stackIdToAddTo = Console.ReadKey().KeyChar - '0';
                    bool success = dal.CreateFlashcard(frontText, backText, stackIdToAddTo);
                    if (!success)
                    {
                        Console.WriteLine("Failed to create.");
                        logger.Info($"Input details: FrontText: {frontText}, BackText: {backText}, StoredStackId: {stackIdToAddTo}, Stack valid?: {dal.CheckStackValid(stackIdToAddTo)}");
                    }
                    break;

                case StackMenuOptions.UpdateFlashcardInStack:
                    Console.WriteLine("Update existing flashcard in a stack. Select stack by Stack ID:  ");
                    var stackIdToUpdate = Console.ReadKey().KeyChar - '0';
                    var flashcardsInStackToUpdate = dal.GetFlashcardsInStack(stackIdToUpdate);
                    DAL.DisplayFlashcardsText(flashcardsInStackToUpdate, true);
                    Console.WriteLine("Flashcard ID to update: ");
                    int flashcardIdToUpdate = Console.ReadKey().KeyChar - '0';
                    dal.UpdateFlashcardInStack(flashcardsInStackToUpdate, flashcardIdToUpdate);
                    break;

                case StackMenuOptions.ViewFlashcardsInStack:
                    Console.WriteLine("Stack ID to retrieve flashcards for: ");
                    int stackIdToRetrieveFlashcardsFor = Console.ReadKey().KeyChar - '0';
                    var flashcardsInStack = dal.GetFlashcardsInStack(stackIdToRetrieveFlashcardsFor);
                    Console.WriteLine(flashcardsInStack.Count);
                    DAL.DisplayFlashcardsText(flashcardsInStack, true);
                    break;

                case StackMenuOptions.DeleteFlashcard:
                    Console.WriteLine("Delete existing flashcard in a stack. Select stack by Stack ID:  ");
                    int stackIdToDeleteFrom = Console.ReadKey().KeyChar - '0';
                    var flashcardsToDeleteFromStack = dal.GetFlashcardsInStack(stackIdToDeleteFrom);
                    DAL.DisplayFlashcardsText(flashcardsToDeleteFromStack, true);
                    Console.WriteLine("Flashcard ID to delete: ");
                    int flashcardIdToDelete = Console.ReadKey().KeyChar - '0';
                    dal.DeleteFlashcardInStack(flashcardsToDeleteFromStack, flashcardIdToDelete);
                    break;

                default:
                    Console.WriteLine("Option out of range");
                    break;
            }
            if (mainMenuSelected) return;
            Console.WriteLine("\nPress to continue...");
            Console.ReadKey(intercept: true);
        }
    }
}