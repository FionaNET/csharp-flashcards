using AutoMapper;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using NLog;
namespace FlashCards_Project
{
    public partial class DAL
    {
        protected readonly FlashcardsDbContext _context;
        protected readonly Mapper _mapper;
        public readonly ILogger? logger;

        public DAL(FlashcardsDbContext context, Mapper mapper, ILogger? logger)
        {
            _context = context;
            _mapper = mapper;
            this.logger = logger;
        }

        public void DisplayAllStacks()
        {
            var stacks = _context.Stacks.Include(s => s.Flashcards).OrderBy(s => s.StackName).ToList();

            foreach (var stack in stacks)
            {
                var count = stack.Flashcards?.Count ?? 0;
                Console.WriteLine($"\n--------------------------------\n" +
                    $"#{stack.StackId}\n" +
                    $"{stack.StackName}\n" +
                    $"{stack.Description}\n" +
                    $"Flashcards in this stack: {count}\n");
                Console.WriteLine("--------------------------------");
            }
        }


        public Stack? GetStack(int ID)
        {
            return _context.Stacks.Find(ID);
        }

        public Stack? GetStack(string? name)
        {
            return _context.Stacks.FirstOrDefault(s => s.StackName == name);
        }

        public void CreateStack(string? newStackName, string? newStackDescription)
        {
            if (!string.IsNullOrEmpty(newStackName))
            {
                var newStack = new Stack() { StackName = newStackName, Description = newStackDescription ?? "" };
                _context.Stacks.Add(newStack);
                _context.SaveChanges();
            }
            else
            {
                Console.WriteLine("Cannot have an empty name for a stack. Failed to create a new stack.");
                logger?.Info($"Input details: StackName: {newStackName}");
            };
        }

        public void UpdateStack(int ID)
        {
            var stack = _context.Stacks.Find(ID);
            if (stack == null) { Console.WriteLine("Failed to retrieve stack"); return; }

            UpdateStackProperties(stack);
        }

        public void UpdateStack(string? name)
        {
            var stack = _context.Stacks.FirstOrDefault(s => s.StackName == name);
            if (stack == null) { Console.WriteLine("Failed to retrieve stack"); return; }

            UpdateStackProperties(stack);
        }

        private void UpdateStackProperties(Stack stack)
        {
            Console.WriteLine($"Current name: {stack.StackName}. Enter new text: <Leave empty to keep>");
            string? input = Console.ReadLine();
            if (!string.IsNullOrEmpty(input))
            {
                stack.StackName = input;
            }

            Console.WriteLine($"Current description: {stack.Description}. Enter new text: <Leave empty to keep>");
            string? input_back = Console.ReadLine();
            if (!string.IsNullOrEmpty(input_back))
            {
                stack.Description = input_back;
            }

            _context.SaveChanges();
            Console.WriteLine("Updated successfully. ");
        }


        public void DeleteStack(int ID)
        {
            DeleteStack(_context.Stacks.Find(ID));
        }

        public void DeleteStack(string? name)
        {
            if (name == null) { Console.WriteLine("Failed to retrieve stack"); return; };
            var stack = _context.Stacks.FirstOrDefault(s => s.StackName == name);
            DeleteStack(stack);
        }

        private void DeleteStack(Stack? stack)
        {
            if (stack == null) { Console.WriteLine("Failed to retrieve stack"); return; };

            _context.Stacks.Remove(stack);
            _context.SaveChanges();
            Console.WriteLine("Deleted successfully. ");
        }

        public bool CheckStackValid(int ID)
        {
            return _context.Stacks.Find(ID) != null;
        }
    }
}