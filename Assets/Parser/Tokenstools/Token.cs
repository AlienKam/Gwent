
namespace Parser.Tokenstools
{

    /// <summary>
    /// Clase que representa un token
    /// </summary>
    public class Token : IToken
    {
        public static readonly Token EOF = new Token("$", TokenType.EOF);

        /// <summary>
        /// Inicializa una nueva instancia de la clase Token con el lexema y tipo de token especificados.
        /// </summary>
        /// <param name="lex">El lexema del token.</param>
        /// <param name="type">El tipo de token.</param>
        public Token(string lex, TokenType type)
        {
            Lex = lex;
            Type = type;
        }

        public string Lex { get; private set; }
        public TokenType Type { get; private set; }

        public override string ToString()
        {
            return $"{Lex} --> {Type}";
        }
    }
}