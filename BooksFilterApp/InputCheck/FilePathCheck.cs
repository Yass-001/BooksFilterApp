using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksFilterApp.InputCheck
{
    public class FilePathCheck
    {
        private string? _filePath = string.Empty;

        public FilePathCheck()
        {
            CheckUserInputFilePath();
            FilePath = _filePath;
        }

        public string FilePath { get; set; } = string.Empty;

        private void CheckUserInputFilePath()
        {
            string? userFilePath = string.Empty;

            do
            {
                bool isCorrectFilePath = false;
                Console.WriteLine("Enter the path to the CSV file:");

                while (!isCorrectFilePath)
                {
                    userFilePath = Console.ReadLine();

                    if (string.IsNullOrEmpty(userFilePath))
                    {
                        Console.WriteLine("String is null or empty. Enter the correct path.");
                    }
                    else if (!File.Exists(userFilePath))
                    {
                        Console.WriteLine("File not found. Please provide a valid file path.");
                    }
                    else
                    {
                        isCorrectFilePath = true;
                    }
                }

                _filePath = userFilePath;
            }
            while (false);
        }
    }
}
