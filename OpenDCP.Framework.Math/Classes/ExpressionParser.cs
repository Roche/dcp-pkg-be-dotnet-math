using OpenDCP.Framework.Math.Classes.AST;
using OpenDCP.Framework.Math.Constants;
using OpenDCP.Framework.Math.Interfaces;
using OpenDCP.Framework.Math.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace OpenDCP.Framework.Math.Classes
{
    public class ExpressionParser : IExpressionParser
    {
        public IExpressionNode Parse(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression))
                throw new ArgumentException("Expression cannot be empty.");

            var tokens = Tokenize(expression);
            int index = 0;

            var ast = ParseLogicalOr(tokens, ref index);

            if (tokens[index].Type != TokenType.EOF)
                throw new Exception($"Unexpected token at end of formula: {tokens[index].Value}");

            return ast;
        }

        private List<Token> Tokenize(string input)
        {
            var tokens = new List<Token>();
            int i = 0;

            while (i < input.Length)
            {
                char ch = input[i];

                if (char.IsWhiteSpace(ch)) { i++; continue; }

                if (ch == '{')
                {
                    int end = input.IndexOf('}', i + 1);
                    if (end == -1) throw new Exception("Mismatched '{' sensor delimiter brackets.");
                    tokens.Add(new Token(TokenType.Sensor, input.Substring(i + 1, end - i - 1)));
                    i = end + 1;
                    continue;
                }

                if (char.IsDigit(ch) || ch == '.')
                {
                    int start = i;
                    bool hasDecimalPoint = ch == '.';
                    i++;

                    while (i < input.Length)
                    {
                        char nextCh = input[i];

                        if (char.IsDigit(nextCh))
                        {
                            i++;
                        }
                        else if (nextCh == '.' && !hasDecimalPoint)
                        {
                            hasDecimalPoint = true;
                            i++;
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (i - start == 1 && hasDecimalPoint)
                    {
                        throw new Exception($"Invalid character in expression: .");
                    }

                    tokens.Add(new Token(TokenType.Number, input.Substring(start, i - start)));
                    continue;
                }

                if (ch == '(') { tokens.Add(new Token(TokenType.LParen, "(")); i++; continue; }
                if (ch == ')') { tokens.Add(new Token(TokenType.RParen, ")")); i++; continue; }
                if (ch == ',') { tokens.Add(new Token(TokenType.Comma, ",")); i++; continue; }

                if (ch == '+' || ch == '-' || ch == '*' || ch == '/' || ch == '=' || ch == '>' || ch == '<' || ch == '!')
                {
                    string op = ch.ToString();

                    if (i + 1 < input.Length && input[i + 1] == '=')
                    {
                        op += "=";
                        i++;
                    }

                    if (op == "!")
                    {
                        throw new Exception("Unexpected character '!' found. Did you mean '!='?");
                    }

                    tokens.Add(new Token(TokenType.Operator, op));
                    i++;
                    continue;
                }

                if (ch == '&' || ch == '|')
                {
                    tokens.Add(new Token(TokenType.Operator, ch.ToString()));
                    i++;
                    continue;
                }

                if (char.IsLetter(ch))
                {
                    var match = Regex.Match(input.Substring(i), @"^[a-zA-Z][a-zA-Z0-9]*");
                    string word = match.Value;

                    if (string.Equals(word, "AND", StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(word, "OR", StringComparison.OrdinalIgnoreCase))
                    {
                        tokens.Add(new Token(TokenType.Operator, word.ToUpper()));
                    }
                    else
                    {
                        tokens.Add(new Token(TokenType.Identifier, word));
                    }
                    i += match.Length;
                    continue;
                }

                throw new Exception($"Invalid character in expression: {ch}");
            }

            tokens.Add(new Token(TokenType.EOF, ""));
            return tokens;
        }

        private IExpressionNode ParseLogicalOr(List<Token> tokens, ref int index)
        {
            var left = ParseLogicalAnd(tokens, ref index);

            while (index < tokens.Count && tokens[index].Type == TokenType.Operator && (tokens[index].Value == "OR" || tokens[index].Value == "|"))
            {
                var op = tokens[index].Value;
                index++;
                var right = ParseLogicalAnd(tokens, ref index);
                left = new BinaryNode(op, left, right);
            }
            return left;
        }

        private IExpressionNode ParseLogicalAnd(List<Token> tokens, ref int index)
        {
            var left = ParseComparison(tokens, ref index);

            while (index < tokens.Count && tokens[index].Type == TokenType.Operator && (tokens[index].Value == "AND" || tokens[index].Value == "&"))
            {
                var op = tokens[index].Value;
                index++;
                var right = ParseComparison(tokens, ref index);
                left = new BinaryNode(op, left, right);
            }
            return left;
        }

        private IExpressionNode ParseComparison(List<Token> tokens, ref int index)
        {
            var left = ParseExpression(tokens, ref index);

            while (index < tokens.Count &&
                tokens[index].Type == TokenType.Operator &&
                  (tokens[index].Value == "="
                  || tokens[index].Value == ">"
                  || tokens[index].Value == "<"
                  || tokens[index].Value == "<="
                  || tokens[index].Value == ">="
                  || tokens[index].Value == "!="))
            {
                var op = tokens[index].Value;
                index++;
                var right = ParseExpression(tokens, ref index);
                left = new BinaryNode(op, left, right);
            }
            return left;
        }

        private IExpressionNode ParseExpression(List<Token> tokens, ref int index)
        {
            var left = ParseTerm(tokens, ref index);

            while (index < tokens.Count && tokens[index].Type == TokenType.Operator && (tokens[index].Value == "+" || tokens[index].Value == "-"))
            {
                var op = tokens[index].Value;
                index++;
                var right = ParseTerm(tokens, ref index);
                left = new BinaryNode(op, left, right);
            }
            return left;
        }

        private IExpressionNode ParseTerm(List<Token> tokens, ref int index)
        {
            var left = ParseUnary(tokens, ref index);

            while (index < tokens.Count && tokens[index].Type == TokenType.Operator && (tokens[index].Value == "*" || tokens[index].Value == "/"))
            {
                var op = tokens[index].Value;
                index++;
                var right = ParseUnary(tokens, ref index);
                left = new BinaryNode(op, left, right);
            }
            return left;
        }

        private IExpressionNode ParseUnary(List<Token> tokens, ref int index)
        {
            if (index < tokens.Count && tokens[index].Type == TokenType.Operator && tokens[index].Value == "-")
            {
                index++;
                var operand = ParseUnary(tokens, ref index);
                return new UnaryMinusNode(operand);
            }

            return ParseFactor(tokens, ref index);
        }

        private IExpressionNode ParseFactor(List<Token> tokens, ref int index)
        {
            if (index >= tokens.Count)
            {
                throw new Exception("Unexpected end of formula expression.");
            }

            var token = tokens[index];

            if (token.Type == TokenType.Number)
            {
                index++;
                return new ConstantNode(FormulaValue.Number(double.Parse(token.Value, CultureInfo.InvariantCulture)));
            }

            if (token.Type == TokenType.Sensor)
            {
                index++;
                return new PlaceholderNode(token.Value);
            }

            if (token.Type == TokenType.Identifier)
            {
                string funcName = token.Value;
                index++;

                if (tokens[index].Type != TokenType.LParen)
                    throw new Exception($"Expected '(' after function: {funcName}");
                index++;

                var args = new List<IExpressionNode>();
                if (tokens[index].Type != TokenType.RParen)
                {
                    args.Add(ParseLogicalOr(tokens, ref index));
                    while (tokens[index].Type == TokenType.Comma)
                    {
                        index++;
                        args.Add(ParseLogicalOr(tokens, ref index));
                    }
                }

                if (tokens[index].Type != TokenType.RParen)
                    throw new Exception($"Missing closing parenthesis for function: {funcName}");
                index++;

                return new FunctionNode(funcName, args);
            }

            if (token.Type == TokenType.LParen)
            {
                index++;
                var node = ParseLogicalOr(tokens, ref index);
                if (tokens[index].Type != TokenType.RParen)
                    throw new Exception("Mismatched parenthesis.");
                index++;
                return node;
            }

            throw new Exception($"Unexpected expression token: {token.Value}");
        }
    }
}
