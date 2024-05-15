using Microsoft.Data.SqlClient;
using System.Data;
using NLog;
using NLog.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Connections;
using System;
using System.Security.Policy;
using System.Data.Entity;
using AutoMapper;


namespace FlashCards_Project
{
    public class Program
    {
        private static readonly ILogger logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
        private static readonly MapperConfiguration config = new(cfg => cfg.CreateMap<Flashcard, FlashcardDTO>());

        private static bool mainMenuSelected = true;
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetSection("ConnectionStrings")["DefaultConnection"] ?? throw new ArgumentNullException("Couldn't find a connection string");
            logger.Info("Connection url found for sql:" + connectionString);

            using FlashcardsDbContext context = new(connectionString);
            Mapper mapper = new(config);
            DAL dal = new(context, mapper, logger);
            MainMenu menu = new(dal);
            try
            {
                while (true)
                {
                    if (mainMenuSelected)
                    {
                        MainMenu.DisplayMenuOption();
                        int option = Console.ReadKey(intercept: true).KeyChar - '0';
                        menu.SwitchMenuOption(option, out mainMenuSelected);
                    }
                    else
                    {
                        // Secondary menu for showing stack options only
                        MainMenu.DisplayStackMenuOptions();
                        int option = Console.ReadKey(intercept: true).KeyChar - '0';
                        menu.SwitchStackMenuOption(option, out mainMenuSelected);
                    }
                }
            }
            catch (Exception e)
            {
                logger.Error(e, e.Message);
                LogManager.Shutdown();
            }
        }

    }
}