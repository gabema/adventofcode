using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    // https://jack-vanlightly.com/blog/2016/2/3/creating-a-simple-tokenizer-lexer-in-c
    public class SimpleLexer2<TT> where TT : Enum
    {
        private List<TokenDefinition2<TT>> definitions;
        private Token2<TT> terminator;

        public SimpleLexer2(IEnumerable<Tuple<TT,string>> definitions)
        {
            this.definitions = definitions.SkipLast(1).Select(d => new TokenDefinition2<TT>(d.Item1, d.Item2)).ToList();
            var last = definitions.Last();
            this.terminator = new Token2<TT>(last.Item1);
        }

        public IEnumerable<Token2<TT>> Tokenize(string lqlText)
        {
            var tokens = new List<Token2<TT>>();
            string remainingText = lqlText;

            while (!string.IsNullOrWhiteSpace(remainingText))
            {
                var match = FindMatch(remainingText);
                if (match.IsMatch)
                {
                    yield return new Token2<TT>(match.TokenType, match.Value);
                    remainingText = match.RemainingText;
                }
                else
                {
                    remainingText = remainingText.Substring(1);
                }
            }

            yield return terminator;
        }

        private TokenMatch2<TT> FindMatch(string lqlText)
        {
            foreach (var tokenDefinition in definitions)
            {
                var match = tokenDefinition.Match(lqlText);
                if (match.IsMatch)
                    return match;
            }

            return new TokenMatch2<TT>() { IsMatch = false };
        }
    }

    public class TokenDefinition2<TT> where TT : Enum
    {
        private Regex _regex;
        internal readonly TT _returnsToken;

        public TokenDefinition2(TT returnsToken, string regexPattern)
        {
            _regex = new Regex(regexPattern, RegexOptions.IgnoreCase);
            _returnsToken = returnsToken;
        }

        public TokenMatch2<TT> Match(string inputString)
        {
            var match = _regex.Match(inputString);
            if (match.Success)
            {
                string remainingText = string.Empty;
                if (match.Length != inputString.Length)
                    remainingText = inputString.Substring(match.Length);

                return new TokenMatch2<TT>()
                {
                    IsMatch = true,
                    RemainingText = remainingText,
                    TokenType = _returnsToken,
                    Value = match
                };
            }
            else
            {
                return new TokenMatch2<TT>() { IsMatch = false };
            }

        }
    }

    public class TokenMatch2<TT> where TT : Enum
    {
        public bool IsMatch { get; set; }
        public TT TokenType { get; set; }
        public Match Value { get; set; }
        public string RemainingText { get; set; }
    }

    public class Token2<TT> where TT : Enum
    {
        public Token2(TT tokenType)
        {
            TokenType = tokenType;
            Value = default;
        }

        public Token2(TT tokenType, Match match)
        {
            TokenType = tokenType;
            Value = match;
        }

        public TT TokenType { get; set; }
        public Match Value { get; set; }
    }
}
