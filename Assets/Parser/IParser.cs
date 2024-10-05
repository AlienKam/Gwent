
using Parser.Language;
using Parser.Tokenstools;

namespace Parser
{

    /// <summary>
    /// Interfaz para un parser genérico que puede analizar expresiones de tipo T
    /// </summary>
    /// <typeparam name="T">Tipo de dato que se analiza</typeparam>
    /// <typeparam name="K">Tipo de token que se analiza</typeparam>
    public interface IParser<out T, in K>
        where T : Node
        where K : IToken
    {
        /// <summary>
        /// Analiza una expresión de texto y devuelve un IEnumerable de resultados de tipo T
        /// </summary>
        /// <param name="tokens">Expresión de texto a analizar</param>
        /// <returns>IEnumerable de resultados de tipo T</returns>
        T Parse(K[] tokens);
    }
}