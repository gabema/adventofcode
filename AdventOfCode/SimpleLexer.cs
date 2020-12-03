using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    // https://jack-vanlightly.com/blog/2016/2/3/creating-a-simple-tokenizer-lexer-in-c
    public class SimpleLexer<TT> where TT : Enum
    {
        private List<TokenDefinition<TT>> definitions;
        private Token<TT> terminator;

        public SimpleLexer(IEnumerable<Tuple<TT,string>> definitions)
        {
            this.definitions = definitions.SkipLast(1).Select(d => new TokenDefinition<TT>(d.Item1, d.Item2)).ToList();
            var last = definitions.Last();
            this.terminator = new Token<TT>(last.Item1);
        }

        public IEnumerable<Token<TT>> Tokenize(string lqlText)
        {
            var tokens = new List<Token<TT>>();
            string remainingText = lqlText;

            while (!string.IsNullOrWhiteSpace(remainingText))
            {
                var match = FindMatch(remainingText);
                if (match.IsMatch)
                {
                    yield return new Token<TT>(match.TokenType, match.Value);
                    remainingText = match.RemainingText;
                }
                else
                {
                    remainingText = remainingText.Substring(1);
                }
            }

            yield return terminator;
        }

        private TokenMatch<TT> FindMatch(string lqlText)
        {
            foreach (var tokenDefinition in definitions)
            {
                var match = tokenDefinition.Match(lqlText);
                if (match.IsMatch)
                    return match;
            }

            return new TokenMatch<TT>() { IsMatch = false };
        }
    }

    public class TokenDefinition<TT> where TT : Enum
    {
        private Regex _regex;
        internal readonly TT _returnsToken;

        public TokenDefinition(TT returnsToken, string regexPattern)
        {
            _regex = new Regex(regexPattern, RegexOptions.IgnoreCase);
            _returnsToken = returnsToken;
        }

        public TokenMatch<TT> Match(string inputString)
        {
            var match = _regex.Match(inputString);
            if (match.Success)
            {
                string remainingText = string.Empty;
                if (match.Length != inputString.Length)
                    remainingText = inputString.Substring(match.Length);

                return new TokenMatch<TT>()
                {
                    IsMatch = true,
                    RemainingText = remainingText,
                    TokenType = _returnsToken,
                    Value = match.Value
                };
            }
            else
            {
                return new TokenMatch<TT>() { IsMatch = false };
            }

        }
    }

    public class TokenMatch<TT> where TT : Enum
    {
        public bool IsMatch { get; set; }
        public TT TokenType { get; set; }
        public string Value { get; set; }
        public string RemainingText { get; set; }
    }

    public class Token<TT> where TT : Enum
    {
        public Token(TT tokenType)
        {
            TokenType = tokenType;
            Value = string.Empty;
        }

        public Token(TT tokenType, string value)
        {
            TokenType = tokenType;
            Value = value;
        }

        public TT TokenType { get; set; }
        public string Value { get; set; }
    }
}
