using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksFilterApp.Filter
{
    public class JsonFileReader
    {
        private Filter _filter = new Filter();
        private ILogger _log;

        public JsonFileReader()
        {
            _log = new LoggerConfiguration()
                .WriteTo.File("logBooks.txt", rollingInterval: RollingInterval.Month)
                .CreateLogger();
            _filter = Read("filter.json");
        }

        private Filter Read(string filePath)
        {
            Filter filter = new Filter();

            try
            {
                string jsonText = File.ReadAllText(filePath);
                filter = JsonConvert.DeserializeObject<Filter>(jsonText);
                _log.Information("Json file read successfully.");
            }
            catch (Exception ex)
            {
                _log.Information($"Error reading json file: {ex.Message}");
            }

            return filter;
        }

        public Filter GetFilter()
        {
            return _filter;
        }
    }
}
