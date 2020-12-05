using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Year2020
{
    public enum Day4Items
    {
        BirthYear,
        IssueYear,
        ExpirationYear,
        Height,
        HairColor,
        EyeColor,
        PassportID,
        CountryID,
        Terminator
    }

    public class Day4Lexer : SimpleLexer<Day4Items>
    {
        private static readonly List<Tuple<Day4Items, string>> NewList = new List<Tuple<Day4Items, string>> {
            Tuple.Create(Day4Items.BirthYear, @"^byr:\S+"),
            Tuple.Create(Day4Items.IssueYear, @"^iyr:\S+"),
            Tuple.Create(Day4Items.ExpirationYear, @"^eyr:\S+"),
            Tuple.Create(Day4Items.Height, @"^hgt:\S+"),
            Tuple.Create(Day4Items.HairColor, @"^hcl:\S+"),
            Tuple.Create(Day4Items.EyeColor, @"^ecl:\S+"),
            Tuple.Create(Day4Items.PassportID, @"^pid:\S+"),
            Tuple.Create(Day4Items.CountryID, @"^cid:\S+"),
            Tuple.Create(Day4Items.Terminator, @""),
        };

        public Day4Lexer() : base(NewList)
        { }
    }

    public class Passport
    {
        public static readonly Regex FourDigitNumberValidator = new Regex(@"^\d\d\d\d$", RegexOptions.Compiled);
        public static readonly Regex NineDigitNumberValidator = new Regex(@"^\d\d\d\d\d\d\d\d\d$", RegexOptions.Compiled);
        public static readonly Regex HeightValidator = new Regex(@"^\d+(?:cm|in)$", RegexOptions.Compiled);
        public static readonly Regex HairColorValidator = new Regex(@"^#[0-9a-f][0-9a-f][0-9a-f][0-9a-f][0-9a-f][0-9a-f]$", RegexOptions.Compiled);
        public static readonly Regex EyeColorValidator = new Regex(@"^(amb|blu|brn|gry|grn|hzl|oth)$", RegexOptions.Compiled);

        public void Reset()
        {
            BirthYear = IssueYear = ExpirationYear = Height =
                HairColor = EyeColor = PassportID = CountryID = null;
        }

        public bool Part1Valid()
        {
            bool isValid = (BirthYear != null && IssueYear != null && ExpirationYear != null &&
                Height != null && HairColor != null && EyeColor != null && PassportID != null);

            return isValid;
        }

        public bool Part2Valid()
        {
            bool isValid = true;

            isValid &= BirthYear != null && FourDigitNumberValidator.IsMatch(BirthYear);
            if (isValid)
            {
                int year = int.Parse(BirthYear);
                isValid = year >= 1920 && year <= 2002;
            }

            isValid &= IssueYear != null && FourDigitNumberValidator.IsMatch(IssueYear);
            if (isValid)
            {
                int year = int.Parse(IssueYear);
                isValid = year >= 2010 && year <= 2020;
            }

            isValid &= ExpirationYear != null && FourDigitNumberValidator.IsMatch(ExpirationYear);
            if (isValid)
            {
                int year = int.Parse(ExpirationYear);
                isValid = year >= 2020 && year <= 2030;
            }

            isValid &= Height != null && HeightValidator.IsMatch(Height);
            if (isValid)
            {
                string type = Height.Substring(Height.Length - 2);
                int height = int.Parse(Height.Substring(0, Height.Length - 2));
                isValid = type == "cm" ? height >= 150 && height <= 193 :
                            type == "in" ? height >= 59 && height <= 76 :
                            false;
            }

            isValid = (isValid && HairColor != null && HairColorValidator.IsMatch(HairColor));

            isValid = (isValid && EyeColor != null && EyeColorValidator.IsMatch(EyeColor));

            isValid = (isValid && PassportID != null && NineDigitNumberValidator.IsMatch(PassportID));

            return isValid;
        }

        public string BirthYear { get; set; }
        public string IssueYear { get; set; }
        public string ExpirationYear { get; set; }
        public string Height { get; set; }
        public string HairColor { get; set; }
        public string EyeColor { get; set; }
        public string PassportID { get; set; }
        public string CountryID { get; set; }
    }

    // https://adventofcode.com/2020/day/4
    public class Day4
    {
        private readonly ITestOutputHelper output;

        public Day4(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Part1()
        {
            var passports = GetPossiblePassports();
            int validPassports = passports.Where(p => p.Part1Valid()).Count();
            Assert.Equal(190, validPassports);
        }

        [Fact]
        public void Part2()
        {
            var passports = GetPossiblePassports();
            int validPassports = passports.Where(p => p.Part2Valid()).Count();
            // Not 140
            Assert.Equal(121, validPassports);
        }

        private IEnumerable<Passport> GetPossiblePassports()
        {
            var stream = InputClient.GetFileStream(2020, 4, "");
            using var reader = new StreamReader(stream);
            var input = reader.ReadLine();
            var lexer = new Day4Lexer();
            var passport = new Passport();
            while (input != null)
            {
                if (input.Trim() == "")
                {
                    yield return passport;
                    passport = new Passport();
                }
                else
                {
                    IEnumerable<Token<Day4Items>> tokens = lexer.Tokenize(input);
                    foreach (var item in tokens)
                    {
                        switch (item.TokenType)
                        {
                            case Day4Items.BirthYear:
                                {
                                    passport.BirthYear = item.Value.Substring(4);
                                    break;
                                }
                            case Day4Items.IssueYear:
                                {
                                    passport.IssueYear = item.Value.Substring(4);
                                    break;
                                }
                            case Day4Items.ExpirationYear:
                                {
                                    passport.ExpirationYear = item.Value.Substring(4);
                                    break;
                                }
                            case Day4Items.Height:
                                {
                                    passport.Height = item.Value.Substring(4);
                                    break;
                                }
                            case Day4Items.HairColor:
                                {
                                    passport.HairColor = item.Value.Substring(4);
                                    break;
                                }
                            case Day4Items.EyeColor:
                                {
                                    passport.EyeColor = item.Value.Substring(4);
                                    break;
                                }
                            case Day4Items.PassportID:
                                {
                                    passport.PassportID = item.Value.Substring(4);
                                    break;
                                }
                            case Day4Items.CountryID:
                                {
                                    passport.CountryID = item.Value.Substring(4);
                                    break;
                                }
                        }
                    }
                }

                input = reader.ReadLine();
            }
        }
    }
}
