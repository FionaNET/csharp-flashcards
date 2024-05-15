namespace FlashCards_Project
{
    public partial class DAL
    {
        public void DisplayAllSessions(int year = 2024, string mode = "count")
        {
            var data = _context.Sessions.Where(s => s.DateCreated.Year == year).ToArray().GroupBy(s => s.Stack);
            dynamic pivot_data;
            string header;
            if (mode == "count")
            {
                header = $"Number of study sessions this year: {year}";
                pivot_data = data.Select(g => new
                {
                    Stack = g.Key.StackName,
                    January = g.Where(s => s.DateCreated.Month == 1).Count(),
                    February = g.Where(s => s.DateCreated.Month == 2).Count(),
                    March = g.Where(s => s.DateCreated.Month == 3).Count(),
                    April = g.Where(s => s.DateCreated.Month == 4).Count(),
                    May = g.Where(s => s.DateCreated.Month == 5).Count(),
                    June = g.Where(s => s.DateCreated.Month == 6).Count(),
                    July = g.Where(s => s.DateCreated.Month == 7).Count(),
                    August = g.Where(s => s.DateCreated.Month == 8).Count(),
                    September = g.Where(s => s.DateCreated.Month == 9).Count(),
                    October = g.Where(s => s.DateCreated.Month == 10).Count(),
                    November = g.Where(s => s.DateCreated.Month == 11).Count(),
                    December = g.Where(s => s.DateCreated.Month == 12).Count()
                });

            }
            else
            {
                header = $"Average score of your study sessions this year: {year}";
                pivot_data = data.Select(g => new
                {
                    Stack = g.Key.StackName,
                    January = g.Where(s => s.DateCreated.Month == 1).Average(s => s.Score),
                    February = g.Where(s => s.DateCreated.Month == 2).Average(s => s.Score),
                    March = g.Where(s => s.DateCreated.Month == 3).Average(s => s.Score),
                    April = g.Where(s => s.DateCreated.Month == 4).Average(s => s.Score),
                    May = g.Where(s => s.DateCreated.Month == 5).Average(s => s.Score),
                    June = g.Where(s => s.DateCreated.Month == 6).Average(s => s.Score),
                    July = g.Where(s => s.DateCreated.Month == 7).Average(s => s.Score),
                    August = g.Where(s => s.DateCreated.Month == 8).Average(s => s.Score),
                    September = g.Where(s => s.DateCreated.Month == 9).Average(s => s.Score),
                    October = g.Where(s => s.DateCreated.Month == 10).Average(s => s.Score),
                    November = g.Where(s => s.DateCreated.Month == 11).Average(s => s.Score),
                    December = g.Where(s => s.DateCreated.Month == 12).Average(s => s.Score),
                });
            }

            Console.WriteLine($"--------------------{header,-4}--------------------");
            Console.WriteLine("Stack   | Jan | Feb | Mar | Apr | May | Jun | Jul | Aug | Sep | Oct | Nov | Dec");

            foreach (var item in pivot_data)
            {
                Console.WriteLine($"{item.Stack,-8}| {item.January,-3} | {item.February,-3} | {item.March,-3} | {item.April,-3} | {item.May,-3} | {item.June,-3} | {item.July,-3} | {item.August,-3} | {item.September,-3} | {item.October,-3} | {item.November,-3} | {item.December,-3}");
            }

        }

        public void CreateStudySession(string? stack)
        {
            Stack? stack_for_session;
            if (int.TryParse(stack, out int stackNumberToDelete))
            {
                stack_for_session = GetStack(stackNumberToDelete);
            }
            else
            {
                stack_for_session = GetStack(stack);
            }
            if (stack_for_session == null) { Console.WriteLine("Failed to retrieve stack"); return; }
            int score = DisplayFlashcardsText(GetFlashcardsInStack(stack_for_session.StackId));
            if (score != -1)
            {
                var session = new Sessions() { Score = score, Stack = stack_for_session, StoredStackId = stack_for_session.StackId, DateCreated = DateTime.Now };
                _context.Sessions.Add(session);
                _context.SaveChanges();
            }
        }
    }
}