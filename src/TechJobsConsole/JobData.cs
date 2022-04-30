using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace TechJobsConsole
{
    class JobData
    {
        static List<Dictionary<string, string>> AllJobs = new List<Dictionary<string, string>>();//list of dictionaries 
        static bool IsDataLoaded = false;
       

        public static List<Dictionary<string, string>> FindAll()
        {
            LoadData();
            List<Dictionary<string, string>> AllJobsCopy = new List<Dictionary<string, string>>(AllJobs);
            return AllJobsCopy;
        }

        /*
         * Returns a list of all values contained in a given column,
         * without duplicates. 
         */
        public static List<string> FindAll(string column)//value is the column data
        {
            LoadData();

            List<string> values = new List<string>();

            foreach (Dictionary<string, string> job in AllJobs)
            {
                string aValue = job[column];

                if (!values.Contains(aValue))
                {
                    values.Add(aValue);
                }
            }
            return values;
        }

        public static List<Dictionary<string, string>> FindByColumnAndValue(string column, string value)
        {
            // load data, if not already loaded
            LoadData();

            List<Dictionary<string, string>> jobs = new List<Dictionary<string, string>>();

            foreach (Dictionary<string, string> row in AllJobs)
            {
                string aValue = row[column];
                string loweraValue = aValue.ToLower();

                if (loweraValue.Contains(value.ToLower()))
                {
                    jobs.Add(row);
                }
            }

            return jobs;
        }

        /*
         * Load and parse data from job_data.csv
         */
        private static void LoadData()
        {

            if (IsDataLoaded)
            {
                return;
            }

            List<string[]> rows = new List<string[]>();//new list corresponding to info in row of CSV

            using (StreamReader reader = File.OpenText("job_data.csv"))//imports CSV data
            {
                while (reader.Peek() >= 0)//keeps reading the CSV file until there is no more data (i.e returning -1)
                {
                    string line = reader.ReadLine();//reads a line and returns the next line. null if end is reached
                    string[] rowArrray = CSVRowToStringArray(line);//Takes the string and turns it into an array seperted by commas
                    if (rowArrray.Length > 0)
                    {
                        rows.Add(rowArrray);
                    }
                }
            }

            string[] headers = rows[0];//grabs the headers of name,employer,location,position type,core competency
            rows.Remove(headers); //removes the headers from the csv data. Just a list of jobs

            // Parse each row array into a more friendly Dictionary
            foreach (string[] row in rows)
            {
                Dictionary<string, string> rowDict = new Dictionary<string, string>();

                for (int i = 0; i < headers.Length; i++)
                {
                    rowDict.Add(headers[i], row[i]);
                }
                AllJobs.Add(rowDict);
            }

            IsDataLoaded = true;
        }

        /*
         * Parse a single line of a CSV file into a string array
         */
        private static string[] CSVRowToStringArray(string row, char fieldSeparator = ',', char stringSeparator = '\"')
        {
            bool isBetweenQuotes = false;
            StringBuilder valueBuilder = new StringBuilder();
            List<string> rowValues = new List<string>();

            // Loop through the row string one char at a time
            foreach (char c in row.ToCharArray())
            {
                if ((c == fieldSeparator && !isBetweenQuotes))
                {
                    rowValues.Add(valueBuilder.ToString());
                    valueBuilder.Clear();
                }
                else
                {
                    if (c == stringSeparator)
                    {
                        isBetweenQuotes = !isBetweenQuotes;
                    }
                    else
                    {
                        valueBuilder.Append(c);
                    }
                }
            }

            // Add the final value
            rowValues.Add(valueBuilder.ToString());
            valueBuilder.Clear();

            return rowValues.ToArray();
        }
        public static List<Dictionary<string, string>> FindByValue(string searchTerm)
        {

            // load data, if not already loaded
            LoadData();
            List<Dictionary<string, string>> jobs = new List<Dictionary<string, string>>();

            foreach (Dictionary<string, string> job in AllJobs)
            {
                foreach (KeyValuePair<string, string> kvp in job)
                {
                    //string key = kvp.Key;
                    string value = kvp.Value;
                    string lowerValue = value.ToLower();

                    if (lowerValue.Contains(searchTerm.ToLower()))
                    {
                        jobs.Add(job);
                    }
                }
            }

            return jobs;
        }
    }
}
       
    
