
namespace Parser.Tokenstools
{

    /// <summary>
    /// Enum que representa los tipos de tokens
    /// </summary>
    public enum TokenType
    {
        /// <summary>
        /// EOF, representa el final de la secuencia de tokens
        /// </summary>
        EOF,

        /// <summary>
        /// Operador de suma
        /// </summary>
        Plus,

        /// <summary>
        /// Operador de resta
        /// </summary>
        Minus,

        /// <summary>
        /// Operador de multiplicación
        /// </summary>
        Multiply,

        /// <summary>
        /// Operador de división
        /// </summary>
        Divide,

        /// <summary>
        /// Paréntesis izquierdo, se utiliza para agrupar expresiones
        /// </summary>
        OpenParenthesis,

        /// <summary>
        /// Paréntesis derecho, se utiliza para agrupar expresiones
        /// </summary>
        CloseParenthesis,

        /// <summary>
        /// Número, puede ser un entero o un flotante
        /// </summary>
        Number,

        /// <summary>
        /// Identificador, se utiliza para representar una variable o un nombre de función
        /// </summary>
        Identifier,

        /// <summary>
        /// DefType, se utiliza para definir el tipo de una variable
        /// </summary>
        DefType,
        /// <summary>
        /// False, se utiliza para representar el valor booleano falso
        /// </summary>
        False,
        /// <summary>
        /// True, se utiliza para representar el valor booleano verdadero
        /// </summary>
        True,
        /// <summary>
        /// If, se utiliza para representar una sentencia de decisión
        /// </summary>
        If,
        /// <summary>
        /// Else, se utiliza para representar una sentencia de decisión
        /// </summary>
        Else,
        /// <summary>
        /// For, se utiliza para representar una sentencia de bucle
        /// </summary>
        For,
        /// <summary>
        /// While, se utiliza para representar una sentencia de bucle
        /// </summary>
        While,
        /// <summary>
        /// Do, se utiliza para representar una sentencia de bucle
        /// </summary>
        Do,
        /// <summary>
        /// Switch, se utiliza para representar una sentencia de selección
        /// </summary>
        Switch,
        /// <summary>
        /// Comma, se utiliza para separar argumentos en una función
        /// </summary>
        Comma,
        /// <summary>
        /// Semicolon, se utiliza para separar sentencias
        /// </summary>
        Semicolon,
        /// <summary>
        /// Colon, se utiliza para separar el nombre de una variable de su tipo
        /// </summary>
        Colon,
        /// <summary>
        /// LessThan, se utiliza para representar el operador relacional 'menor que'
        /// </summary>
        LessThan,
        /// <summary>
        /// GreaterThan, se utiliza para representar el operador relacional 'mayor que'
        /// </summary>
        GreaterThan,
        /// <summary>
        /// Equal, se utiliza para representar el operador relacional 'igual'
        /// </summary>
        Equal,
        /// <summary>
        /// OpenKey, se utiliza para representar la apertura de un bloque de código
        /// </summary>
        OpenKey,
        /// <summary>
        /// CloseKey, se utiliza para representar el cierre de un bloque de código
        /// </summary>
        CloseKey,
        /// <summary>
        /// Dot, se utiliza para representar el acceso a un atributo de una variable
        /// </summary>
        Dot,
        /// <summary>
        /// EqualEqual, se utiliza para representar la comparación de igualdad entre dos valores
        /// </summary>
        EqualEqual,
        /// <summary>
        /// NotEqual, se utiliza para representar la comparación de desigualdad entre dos valores
        /// </summary>
        NotEqual,
        /// <summary>
        /// LessThanEqual, se utiliza para representar la comparación de menor o igual que entre dos valores
        /// </summary>
        LessThanEqual,
        /// <summary>
        /// GreaterThanEqual, se utiliza para representar la comparación de mayor o igual que entre dos valores
        /// </summary>
        GreaterThanEqual,
        /// <summary>
        /// Or, se utiliza para representar el operador lógico 'o'
        /// </summary>
        Or,
        /// <summary>
        /// Not, se utiliza para representar el operador lógico 'no'
        /// </summary>
        Not,
        /// <summary>
        /// And, se utiliza para representar el operador lógico 'y'
        /// </summary>
        And,
        /// <summary>
        /// Concat, se utiliza para representar la concatenación de cadenas
        /// </summary>
        Concat,
        /// <summary>
        /// ConcatEsp, se utiliza para representar la concatenación de cadenas que incluyen espacios
        /// </summary>
        ConcatEsp,
        /// <summary>
        /// Pow, se utiliza para representar la potenciación entre dos valores
        /// </summary>
        Pow,
        /// <summary>
        /// OpenBracket, se utiliza para representar la apertura de un array
        /// </summary>
        OpenBracket,
        /// <summary>
        /// CloseBracket, se utiliza para representar el cierre de un array
        /// </summary>
        CloseBracket,
        /// <summary>
        /// Lambda, se utiliza para representar una función lambda
        /// </summary>
        Lambda,
        /// <summary>
        /// PlusEqual, se utiliza para representar la asignación de suma (a += b)
        /// </summary>
        PlusEqual,
        /// <summary>
        /// MinusEqual, se utiliza para representar la asignación de resta (a -= b)
        /// </summary>
        MinusEqual,
        /// <summary>
        /// MultiplyEqual, se utiliza para representar la asignación de multiplicación (a *= b)
        /// </summary>
        MultiplyEqual,
        /// <summary>
        /// DivideEqual, se utiliza para representar la asignación de división (a /= b)
        /// </summary>
        DivideEqual,
        /// <summary>
        /// PowEqual, se utiliza para representar la asignación de potenciación (a ^= b)
        /// </summary>
        PowEqual,
        Comment,
        Marks,
        String
    }
}