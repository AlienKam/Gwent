
using System.Collections.Generic;
using Parser.Tokenstools;

namespace Parser
{

    public static class Storage
    {
        public static Dictionary<string, TokenType> Tokens = new() {
        // Booleanos
        {"false", TokenType.False},
        {"true", TokenType.True},

        // Palabras clave
        {"if", TokenType.If},
        {"else", TokenType.Else},
        {"for", TokenType.For},
        {"while", TokenType.While},
        {"do", TokenType.Do},
        {"switch", TokenType.Switch},

        // Separadores
        {",", TokenType.Comma},
        {";", TokenType.Semicolon},
        {":", TokenType.Colon},
        {".", TokenType.Dot},
        {"=", TokenType.Equal},

        // Boolean operators
        {"<", TokenType.LessThan},
        {">", TokenType.GreaterThan},
        {"==", TokenType.EqualEqual},
        {"!=", TokenType.NotEqual},
        {"<=", TokenType.LessThanEqual},
        {">=", TokenType.GreaterThanEqual},
        {"||", TokenType.Or},
        {"!", TokenType.Not},
        {"&&", TokenType.And},

        // Concatenation operators
        {"@", TokenType.Concat},
        {"@@", TokenType.ConcatEsp},

        // Arithmetic operators
        {"+", TokenType.Plus},
        {"-", TokenType.Minus},
        {"*", TokenType.Multiply},
        {"/", TokenType.Divide},
        {"^", TokenType.Pow},
        {"+=", TokenType.PlusEqual},
        {"-=", TokenType.MinusEqual},
        {"*=", TokenType.MultiplyEqual},
        {"/=", TokenType.DivideEqual},
        {"^=", TokenType.PowEqual},

        // Symbols
        {"(", TokenType.OpenParenthesis},
        {")", TokenType.CloseParenthesis},
        {"[", TokenType.OpenBracket},
        {"]", TokenType.CloseBracket},
        {"{", TokenType.OpenKey},
        {"}", TokenType.CloseKey},
        {"=>", TokenType.Lambda},
        {"\"", TokenType.Marks},

        //Coments
        {"//", TokenType.Comment},
    };
    }
}