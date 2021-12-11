using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class InputClient
    {
        public static Stream GetFileStream(int year, int day, string variant = "")
        {
            var directory = new DirectoryInfo(typeof(InputClient).Assembly.Location);
            var inputFile = new FileInfo(Path.Combine(directory.Parent.FullName, $"Year{year}\\Day{day}{variant}.txt"));
            return inputFile.OpenRead();
        }

        public static async Task<Stream> GetWebStream(int year, int day)
        {
            using HttpClient httpClient = new HttpClient();
            var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, new Uri($"https://adventofcode.com/{year}/day/{day}/input")));
            return await response.Content.ReadAsStreamAsync();
        }

        public static StreamReader GetFileStreamReader(int year, int day, string variant = "") => new StreamReader(GetFileStream(year, day, variant));

        /// <summary>
        ///  https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex?view=netcore-3.1
        /// </summary>
        /// <param name="year"></param>
        /// <param name="day"></param>
        /// <param name="variant"></param>
        /// <param name="regEx"></param>
        /// <returns></returns>
        public static IEnumerable<GroupCollection> GetRegularExpressionRows(int year, int day, string variant, string regEx)
        {
            using StreamReader reader = GetFileStreamReader(year, day, variant);
            string line;

            Regex rx = new Regex(regEx, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            while ((line = reader.ReadLine()) != null)
            {
                foreach(Match match in rx.Matches(line))
                {
                    yield return match.Groups;
                }
            }
        }

        public static IEnumerable<int> GetIntInput(int year, int day, string variant = "")
        {
            using StreamReader reader = GetFileStreamReader(year, day, variant);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                yield return int.Parse(line);
            }
        }
    }
}
