using Serilog;
using BooksFilterApp;
using Microsoft.EntityFrameworkCore;
using BooksFilterApp.DBContext;
using BooksFilterApp.InputCheck;
using BooksFilterApp.Enums;
using BooksFilterApp.CSVDataHandling;
using BooksFilterApp.Filter;

// c:\books.csv
// c:\books_copy.csv
// c:\books_temp.csv

Console.WriteLine("Welcome to \"BOOKS\" app!\n" +
    "Please define how the App will work.\n" +
    "If you want to create books database - choose - \"D\"\n" +
    "If you want to filter books & get console view - choose - \"C\"\n" +
    "If you want to filter books & get a file - choose  - \"F\".");

var userChoiceCheck = new UserChoiceCheck();
var appStrategy = userChoiceCheck.UserChoiceStrategy();

using var dbContext = new BooksDBContext();
await dbContext.Database.EnsureCreatedAsync();

if (appStrategy == ProcessStrategyEnum.Database)
{
    try
    {
        var filePathCheck = new FilePathCheck();
        var bookDataSearchService = new BookDataSearchService(dbContext);
        var fileImportService = new FileImportService(filePathCheck.FilePath, dbContext, bookDataSearchService);
        await fileImportService.ProcessImportFile();
    }
    catch (Exception ex)
    {
        using var log = new LoggerConfiguration()
           .WriteTo.File("logBooks.txt", rollingInterval: RollingInterval.Month)
           .CreateLogger();
        log.Information(ex.Message.ToString());
    }
}
else if (appStrategy == ProcessStrategyEnum.Console)
{
    try
    {
        var filterForSearch = new JsonFileReader();
        var booksFilterService = new BooksFilterService(filterForSearch.GetFilter(), dbContext);
        await booksFilterService.DisplayFilteredBooksToConsoleAsync();
    }
    catch (Exception ex)
    {
        using var log = new LoggerConfiguration()
           .WriteTo.File("logBooks.txt", rollingInterval: RollingInterval.Month)
           .CreateLogger();
        log.Information(ex.Message.ToString());
    }
}
else
{
    try
    {
        var filterForSearch = new JsonFileReader();
        var booksFilterService = new BooksFilterService(filterForSearch.GetFilter(), dbContext);
        await booksFilterService.ExportToFileAsync();
    }
    catch (Exception ex)
    {
        using var log = new LoggerConfiguration()
           .WriteTo.File("logBooks.txt", rollingInterval: RollingInterval.Month)
           .CreateLogger();
        log.Information(ex.Message.ToString());
    }
}


