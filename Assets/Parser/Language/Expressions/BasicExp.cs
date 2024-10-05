
namespace Parser.Language
{

    /// <summary>
    /// Clase abstracta que representa una expresión booleana
    /// </summary>
    public abstract class BooleanExp : IOperationExp<bool>
    {
        /// <summary>
        /// Ejecuta la expresión booleana y devuelve el resultado
        /// </summary>
        /// <returns>El resultado de la expresión booleana</returns>
        public abstract bool Execute(IBlockContext blockContext);
    }

    /// <summary>
    /// Clase que representa un valor booleano
    /// </summary>
    public class Bool : BooleanExp
    {
        /// <summary>
        /// El valor booleano
        /// </summary>
        protected bool value;

        /// <summary>
        /// Constructor que recibe el valor booleano
        /// </summary>
        /// <param name="value">El valor booleano</param>
        public Bool(bool value) : base()
        {
            this.value = value;
        }

        /// <summary>
        /// Ejecuta la expresión booleana y devuelve el resultado
        /// </summary>
        /// <returns>El resultado de la expresión booleana</returns>
        public override bool Execute(IBlockContext blockContext) { return value; }
    }

    /// <summary>
    /// Clase abstracta que representa una expresión aritmética
    /// </summary>
    public abstract class ArithmeticExp : IOperationExp<double>
    {
        /// <summary>
        /// Ejecuta la expresión aritmética y devuelve el resultado
        /// </summary>
        /// <returns>El resultado de la expresión aritmética</returns>
        public abstract double Execute(IBlockContext blockContext);
    }

    /// <summary>
    /// Clase que representa un número
    /// </summary>
    public class Number : ArithmeticExp
    {
        /// <summary>
        /// El valor del número
        /// </summary>
        protected double value;

        /// <summary>
        /// Constructor que recibe el valor del número
        /// </summary>
        /// <param name="value">El valor del número</param>
        public Number(double value) : base()
        {
            this.value = value;
        }

        /// <summary>
        /// Ejecuta la expresión aritmética y devuelve el resultado
        /// </summary>
        /// <returns>El resultado de la expresión aritmética</returns>
        public override double Execute(IBlockContext blockContext) { return value; }
    }

    /// <summary>
    /// Clase abstracta que representa una expresión de cadena
    /// </summary>
    public abstract class StringExp : IOperationExp<string>
    {
        /// <summary>
        /// Ejecuta la expresión de cadena y devuelve el resultado
        /// </summary>
        /// <returns>El resultado de la expresión de cadena</returns>
        public abstract string Execute(IBlockContext blockContext);
    }

    /// <summary>
    /// Clase que representa un valor de cadena
    /// </summary>
    public class StringValue : StringExp
    {
        /// <summary>
        /// El valor de la cadena
        /// </summary>
        protected string value;

        /// <summary>
        /// Constructor que recibe el valor de la cadena
        /// </summary>
        /// <param name="value">El valor de la cadena</param>
        public StringValue(string value) : base()
        {
            this.value = value;
        }

        /// <summary>
        /// Ejecuta la expresión de cadena y devuelve el resultado
        /// </summary>
        /// <returns>El resultado de la expresión de cadena</returns>
        public override string Execute(IBlockContext blockContext) { return value; }
    }

    /// <summary>
    /// Clase que representa una variable o una expresión que puede ser de diferentes tipos
    /// </summary>
    public class ValueExp : IOperationExp<(VarType type, object value)>
    {
        /// <summary>
        /// El tipo de la variable o expresión
        /// </summary>
        public VarType Type { get; private set; }

        /// <summary>
        /// El valor de la variable o expresión
        /// </summary>
        public IOperationExp<object> Value { get; private set; }

        /// <summary>
        /// Constructor que recibe una expresión num rica
        /// </summary>
        /// <param name="exp">La expresión num rica</param>
        public ValueExp(IOperationExp<double> exp)
        {
            Value = exp.Convert();
            Type = VarType.Int;
        }

        /// <summary>
        /// Constructor que recibe una expresión booleana
        /// </summary>
        /// <param name="exp">La expresión booleana</param>
        public ValueExp(IOperationExp<bool> exp)
        {
            Value = exp.Convert();
            Type = VarType.Bool;
        }

        /// <summary>
        /// Constructor que recibe una expresión de cadena
        /// </summary>
        /// <param name="exp">La expresión de cadena</param>
        public ValueExp(IOperationExp<string> exp)
        {
            Value = exp.Convert();
            Type = VarType.Str;
        }

        public ValueExp(IOperationExp<IContextCard> exp)
        {
            Value = exp.Convert();
            Type = VarType.Card;
        }

        /// <summary>
        /// Ejecuta la variable o expresión y devuelve el resultado
        /// </summary>
        /// <returns>El resultado de la variable o expresión</returns>
        public (VarType type, object value) Execute(IBlockContext blockContext)
        {
            return (Type, Value);
        }
    }
}