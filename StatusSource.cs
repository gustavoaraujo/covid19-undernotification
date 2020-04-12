using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Covid19_UnderNotification
{
    public class StatusSource
    {
        private readonly double DeathExpectedPercentage = 4;
        private readonly string Country = "brazil";
        private readonly string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "\\result.json");

        private static StatusSource instance;

        public static StatusSource Instance 
        { 
            get 
            {
                if(instance == null)
                    instance = new StatusSource();

                return instance;
            } 
        }
        private StatusSource() {}

        public void Calculate() 
        {
            var confirmedCases = GetCasesByDate("confirmed");
            var deathCases = GetCasesByDate("deaths");
            var expectedCases = new List<AnalysisResult>();

            foreach(var date in confirmedCases.Keys) 
            {
                var actualCases = confirmedCases[date];
                
                if(confirmedCases[date] == 0)
                    continue;
                
                var expected = deathCases[date] * 100 / DeathExpectedPercentage;

                var analysisResult = new AnalysisResult 
                {
                    ActualCases = actualCases,
                    Date = date,
                    ExpectedCases = expected,
                    Difference = expected - actualCases
                };

                Console.WriteLine($"Date: {analysisResult.Date:dd/MM/yyyy} - Actual cases: {analysisResult.ActualCases} - Expected cases: {analysisResult.ExpectedCases} - Difference: {analysisResult.Difference} ");

                expectedCases.Add(analysisResult);
            }

            var json = JsonConvert.SerializeObject(expectedCases);

            File.WriteAllText(FilePath, json);
        }

        public Dictionary<DateTime, int> GetCasesByDate(string status) 
        {
            var result = new Dictionary<DateTime, int>();

            var textResponse = Request.DoRequest($"https://api.covid19api.com/country/{Country}/status/{status}?from=2020-02-25T00:00:00Z&to=2020-{DateTime.Now.Month}-{DateTime.Now.Day}T00:00:00Z");
            var objResponse = JsonConvert.DeserializeObject<List<ApiOutput>>(textResponse);

            foreach(var obj in objResponse) 
            {
                result.Add(obj.Date, obj.Cases);
            }

            return result;
        }
    }

    public class AnalysisResult 
    {
        public DateTime Date { get; set; }
        public int ActualCases { get; set; }
        public double ExpectedCases { get; set; }
        public double Difference { get; set; }
    }
}