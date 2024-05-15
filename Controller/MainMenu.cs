using System.Text.RegularExpressions;
using NLog;

namespace FlashCards_Project
{
    public partial class MainMenu
    {
        readonly DAL dal;

        public MainMenu(DAL dal)
        {
            this.dal = dal;
        }
        public enum MenuOption
        {
            Exit = 0,
            CreateNewStack = 1,
            UpdateStack = 2,
            ViewAllStacks = 3,
            CreateStudySession = 4,
            ViewAllSessions = 5,
            DeleteStack = 6,
            FlashcardsMenu = 7,
        }

        [GeneratedRegex("(\\B[A-Z])")]
        private static partial Regex formattedRegex();
        public static void DisplayMenuOption()
        {
            Console.WriteLine("Select an option:");
            foreach (MenuOption option in Enum.GetValues(typeof(MenuOption)))
            {
                var formattedText = formattedRegex().Replace(option.ToString(), " $1");
                Console.WriteLine($"[{(int)option}]: {formattedText}");
            }
        }

        public void SwitchMenuOption(int option, out bool mainMenuSelected)
        {
            mainMenuSelected = true; //default true
            switch ((MenuOption)option)
            {
                case MenuOption.Exit:
                    Environment.Exit(0);
                    break;

                case MenuOption.CreateNewStack:
                    Console.WriteLine("Name of new stack: ");
                    string? newStackName = Console.ReadLine();
                    Console.WriteLine("Provide a description of the stack: ");
                    string? newStackDescription = Console.ReadLine();
                    dal.CreateStack(newStackName, newStackDescription);
                    break;

                case MenuOption.UpdateStack:
                    Console.WriteLine("Stack ID or name to delete: ");
                    string? stackIdOrNameToUpdate = Console.ReadLine();
                    int stackNumberToUpdate;
                    if (int.TryParse(stackIdOrNameToUpdate, out stackNumberToUpdate))
                    {
                        dal.UpdateStack(stackNumberToUpdate);
                    }
                    else
                    {
                        dal.UpdateStack(stackIdOrNameToUpdate);
                    }
                    break;

                case MenuOption.ViewAllStacks:
                    dal.DisplayAllStacks();
                    break;

                case MenuOption.CreateStudySession:
                    Console.WriteLine("Stack to study: ");
                    string? stack = Console.ReadLine();
                    dal.CreateStudySession(stack);
                    break;

                case MenuOption.ViewAllSessions:
                    dal.DisplayAllSessions();
                    break;

                case MenuOption.DeleteStack:
                    Console.WriteLine("Stack ID or name to delete: ");
                    string? stackIdOrNameToDelete = Console.ReadLine();
                    int stackNumberToDelete;
                    if (int.TryParse(stackIdOrNameToDelete, out stackNumberToDelete))
                    {
                        dal.DeleteStack(stackNumberToDelete);
                    }
                    else
                    {
                        dal.DeleteStack(stackIdOrNameToDelete);
                    }
                    break;

                case MenuOption.FlashcardsMenu:
                    mainMenuSelected = false;
                    break;

                default:
                    Console.WriteLine("Option out of range");
                    break;
            }
            if (!mainMenuSelected) return;
            Console.WriteLine("\nPress to continue...");
            Console.ReadKey(intercept: true);
        }
    }
}