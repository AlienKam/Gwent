
namespace Parser.Language
{

    /// <summary>
    /// Clase que se encarga de hacer conversiones entre tipos de expresiones
    /// </summary>
    public static class OperationExtensions
    {
        /// <summary>
        /// Clase que se encarga de hacer conversiones entre tipos de expresiones
        /// </summary>
        /// <typeparam name="T">El tipo de dato que se va a convertir</typeparam>
        public class BaseOperationExp<T> : IOperationExp<object>
        {
            /// <summary>
            /// La expresión que se va a convertir
            /// </summary>
            public IOperationExp<T> Exp { get; private set; }

            /// <summary>
            /// Constructor que recibe la expresión a convertir
            /// </summary>
            /// <param name="exp">La expresión a convertir</param>
            public BaseOperationExp(IOperationExp<T> exp)
            {
                Exp = exp;
            }

            /// <summary>
            /// Ejecuta la conversi n y devuelve el resultado
            /// </summary>
            /// <returns>El resultado de la conversi n</returns>
            public object Execute(IBlockContext blockContext)
            {
                // Se ejecuta la expresión original y se devuelve el resultado
                // como un objeto, ya que no se sabe el tipo exacto al que se
                // est  convirtiendo
                return Exp.Execute(blockContext)!;
            }
        }

        /// <summary>
        /// M todo que se encarga de convertir una expresión de un tipo a otro
        /// </summary>
        /// <typeparam name="T">El tipo de dato que se va a convertir</typeparam>
        /// <param name="operation">La expresión a convertir</param>
        /// <returns>La expresión convertida</returns>
        public static IOperationExp<object> Convert<T>(this IOperationExp<T> operation)
        {
            // Se crea una nueva instancia de la clase BaseOperationExp y se
            // devuelve como resultado
            return new BaseOperationExp<T>(operation);
        }
    }
}