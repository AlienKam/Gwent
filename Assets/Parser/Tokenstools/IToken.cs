
namespace Parser.Tokenstools
{

    public interface IToken
    {
        /// <summary>
        /// El lexema del token
        /// </summary>
        public string Lex { get; }

        /// <summary>
        /// El tipo de token
        /// </summary>
        public TokenType Type { get; }
    }
}