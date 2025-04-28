using BooksFilterApp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksFilterApp.InputCheck
{
    public class UserChoiceCheck
    {
        public UserChoiceCheck()
        {
        }

        public ProcessStrategyEnum UserChoiceStrategy()
        {
            string? userChoice = GetUserChoice();

            if (userChoice.Equals("c", StringComparison.InvariantCultureIgnoreCase))
            {
                return ProcessStrategyEnum.Console;
            }

            if (userChoice.Equals("d", StringComparison.InvariantCultureIgnoreCase))
            {
                return ProcessStrategyEnum.Database;
            }

            return ProcessStrategyEnum.File;
        }

        private string? GetUserChoice()
        {
            string? choice = string.Empty;
            bool isCorrectChoice = false;

            do
            {
                Console.Write("Your choice: ");
                string? userChoiceStrategy = Console.ReadLine();

                if (userChoiceStrategy.Equals("c", StringComparison.InvariantCultureIgnoreCase)
                    || userChoiceStrategy.Equals("f", StringComparison.InvariantCultureIgnoreCase)
                    || userChoiceStrategy.Equals("d", StringComparison.InvariantCultureIgnoreCase))
                {
                    isCorrectChoice = true;
                    choice = userChoiceStrategy;
                }
                else
                {
                    Console.WriteLine("Please give the correct letter!");
                }
            }
            while (!isCorrectChoice);

            return choice;
        }
    }
}
