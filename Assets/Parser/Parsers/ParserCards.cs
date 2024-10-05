using Parser.Language;
using Parser.Tokenstools;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Dicciona;

namespace Parser
{
    public class ParserCards : BaseParser, IParser<Node, IToken>
    {
        private Dictionary<string, VarType> global;

        public ParserCards()
        {
            global = new();
        }

        public new virtual Definitions Parse(IToken[] tokens)
        {
            return Start(tokens);
        }
        public Definitions Start(IToken[] tokens)
        {
            var definitions = new Definitions();
            for (int i = 0; tokens[i].Type != TokenType.EOF; i++)
            {
                Def(tokens, definitions, ref i);
            }
            return definitions;
        }

        public void Def(IToken[] tokens, Definitions definitions, ref int index)
        {
            if (tokens[index].Type != TokenType.Identifier)
            {
                throw new Exception();
            }

            if (tokens[index].Lex == "effect")
            {
                if (tokens[++index].Type != TokenType.Colon || tokens[++index].Type != TokenType.OpenKey)
                {
                    throw new Exception();
                }
                EffectDef def = Effect(tokens, ref index);

                if (tokens[++index].Type != TokenType.CloseKey)
                {
                    throw new Exception();
                }
                definitions.AddEffectDef(def.Name, def);
            }
            else if (tokens[index].Lex == "card")
            {
                if (tokens[++index].Type != TokenType.Colon || tokens[++index].Type != TokenType.OpenKey)
                {
                    throw new Exception();
                }
                Card def = Card(tokens, ref index);
                if (tokens[++index].Type != TokenType.CloseKey)
                {
                    throw new Exception();
                }
                definitions.AddCardDef(def.Name, def);
            }
            else
            {
                throw new Exception();
            }
        }

        #region Definisión de efectos
        public EffectDef Effect(IToken[] tokens, ref int index)
        {
            string name = Name(tokens, ref index);

            if (tokens[++index].Type != TokenType.Comma)
            {
                throw new Exception();
            }

            Params[] @params = ParamsDef(tokens, ref index);

            if (tokens[++index].Type != TokenType.Comma)
            {
                throw new Exception();
            }

            var action = ActionDef(tokens, ref index, @params);

            return new EffectDef(name, @params, action);
        }

        public string Name(IToken[] tokens, ref int index)
        {
            if (tokens[++index].Type != TokenType.Identifier || tokens[index].Lex != "Name")
            {
                throw new Exception();
            }

            if (tokens[++index].Type != TokenType.Colon)
            {
                throw new Exception();
            }

            if (tokens[++index].Type != TokenType.Marks)
            {
                throw new Exception();
            }

            if (tokens[++index].Type != TokenType.String)
            {
                throw new Exception();
            }

            string name = tokens[index].Lex;

            if (tokens[++index].Type != TokenType.Marks)
            {
                throw new Exception();
            }

            return name;
        }

        public Params[] ParamsDef(IToken[] tokens, ref int index)
        {
            List<Params> @params = new List<Params>();
            if (tokens[++index].Lex != "Params")
            {
                throw new Exception();
            }

            if (tokens[++index].Type != TokenType.Colon)
            {
                throw new Exception();
            }

            if (tokens[++index].Type != TokenType.OpenKey)
            {
                throw new Exception();
            }

            do
            {
                if (tokens[index + 1].Type == TokenType.CloseKey || tokens[index + 1].Type == TokenType.EOF)
                {
                    index++;
                    break;
                }

                Params param = ParamDef(tokens, ref index);
                @params.Add(param);
            } while (tokens[++index].Type == TokenType.Comma);

            if (tokens[index].Type != TokenType.CloseKey)
            {
                throw new Exception();
            }

            return @params.ToArray();
        }

        public Params ParamDef(IToken[] tokens, ref int index)
        {
            if (tokens[++index].Type != TokenType.Identifier)
            {
                throw new Exception();
            }

            string name = tokens[index].Lex;

            if (tokens[++index].Type != TokenType.Colon)
            {
                throw new Exception();
            }

            VarType type = DeclarationVarType(tokens, ref index);

            return new Params(name, type);
        }

        public VarType DeclarationVarType(IToken[] tokens, ref int index)
        {
            var type = tokens[++index];

            switch (type.Lex)
            {
                case "Number":
                    return VarType.Int;
                case "Bool":
                    return VarType.Bool;
                case "String":
                    return VarType.Str;
                default:
                    throw new Exception();
            }
        }

        public Action<IEnumerable<IContextCard>, IContext, InputParams[]> ActionDef(IToken[] tokens, ref int index, Params[] @params)
        {
            if (tokens[++index].Lex != "Action")
            {
                throw new Exception("");
            }

            if (tokens[++index].Type != TokenType.Colon)
            {
                throw new Exception("");
            }

            if (tokens[++index].Type != TokenType.OpenParenthesis)
            {
                throw new Exception("");
            }
            if (tokens[++index].Lex.ToLower() != "targets")
            {
                throw new Exception("");
            }
            if (tokens[++index].Type != TokenType.Comma)
            {
                throw new Exception("");
            }
            if (tokens[++index].Lex.ToLower() != "context")
            {
                throw new Exception("");
            }
            if (tokens[++index].Type != TokenType.CloseParenthesis)
            {
                throw new Exception("");
            }
            if (tokens[++index].Type != TokenType.Lambda)
            {
                throw new Exception("");
            }

            global = new();
            BlockExp block = BlockExpDef(tokens, ref index);
            Action<IEnumerable<IContextCard>, IContext, InputParams[]> action = (cards, contex, inputParams) =>
            {
                var dict = new Dictionary<string, (VarType, object)>(
                    inputParams.Select(param => new KeyValuePair<string, (VarType, object)>
                        (param.Name, (param.ParamsType, param.Value)))
                );
                block.Execute(new BlockContext(cards, contex, dict));
            };
            return action;
        }
        #endregion

        #region Expresiones de lineas
        public BlockExp BlockExpDef(IToken[] tokens, ref int index)
        {
            List<LineExp> lines = new List<LineExp>();
            if (tokens[++index].Type != TokenType.OpenKey)
            {
                throw new Exception();
            }

            do
            {
                if (tokens[index + 1].Type == TokenType.CloseKey || tokens[index + 1].Type == TokenType.EOF)
                {
                    index++;
                    break;
                }

                LineExp line = LineExps(tokens, ref index);
                lines.Add(line);
            } while (tokens[index + 1].Type != TokenType.EOF);

            if (tokens[index].Type != TokenType.CloseKey)
            {
                throw new Exception();
            }

            return new BlockExp(lines);
        }

        public LineExp LineExps(IToken[] tokens, ref int index)
        {
            IToken token = tokens[++index];
            if (token.Lex == "for")
            {
                return ForEachExp(tokens, ref index);
            }
            if (token.Lex == "if")
            {
                return IF(tokens, ref index);
            }
            else if (token.Lex == "while")
            {
                return WhileExp(tokens, ref index);
            }

            int lastIndex = --index;

            try
            {
                VoidMethodDef voidMethodDef = VoidMethodDef(tokens, ref index);
                return voidMethodDef;
            }
            catch (Exception)
            {
                index = lastIndex;
            }

            try
            {
                IndexDecl indexDecl = IndexDeclExp(tokens, ref index);
                return indexDecl;
            }
            catch (Exception)
            {
                index = lastIndex;
            }

            DeclarationVarExp declarationVar = DeVar(tokens, ref index);

            if (tokens[++index].Type != TokenType.Semicolon)
            {
                throw new Exception();
            }
            return declarationVar;
        }

        public IndexDecl IndexDeclExp(IToken[] tokens, ref int index)
        {
            CallMethods call = CallMethodsExp(tokens, ref index);
            if (tokens[++index].Type != TokenType.OpenBracket)
            {
                throw new Exception();
            }
            IOperationExp<double> exp = AritmExp(tokens, ref index);
            if (tokens[++index].Type != TokenType.CloseBracket)
            {
                throw new Exception();
            }

            string name = "";
            ValueExp valueExp;
            if (tokens[index + 1].Type == TokenType.Dot)
            {
                index += 2;
                name = tokens[index].Lex;
                if (tokens[++index].Type != TokenType.Equal)
                {
                    throw new Exception();
                }
                valueExp = ValueExp(tokens, ref index);
            }
            else
            {
                if (tokens[++index].Type != TokenType.Equal)
                {
                    throw new Exception();
                }
                IOperationExp<IContextCard> card = CardExp(tokens, ref index);
                valueExp = new ValueExp(card);
            }

            if (tokens[++index].Type != TokenType.Semicolon)
            {
                throw new Exception();
            }

            return new IndexDecl(call, exp, name, valueExp);
        }

        public VoidMethodDef VoidMethodDef(IToken[] tokens, ref int index)
        {
            CallMethods callMethods = CallMethodsExp(tokens, ref index);

            if (tokens[++index].Type != TokenType.Dot)
            {
                throw new Exception();
            }
            if (tokens[++index].Type != TokenType.Identifier)
            {
                throw new Exception();
            }

            string name = tokens[index].Lex;

            if (tokens[++index].Type != TokenType.OpenParenthesis)
            {
                throw new Exception();
            }

            CallVar<IContextCard> call = new CallVar<IContextCard>(tokens[++index].Lex);

            if (tokens[++index].Type != TokenType.CloseParenthesis)
            {
                throw new Exception();
            }

            if (tokens[++index].Type != TokenType.Semicolon)
            {
                throw new Exception();
            }

            return new VoidMethodDef(callMethods, name, call);
        }

        public CallMethods CallMethodsExp(IToken[] tokens, ref int index)
        {
            if (tokens[++index].Lex.ToLower() != "context")
            {
                throw new Exception();
            }
            if (tokens[++index].Type != TokenType.Dot)
            {
                throw new Exception();
            }
            if (tokens[++index].Type != TokenType.Identifier)
            {
                throw new Exception();
            }

            string name = tokens[index].Lex;
            IOperationExp<double> operation = null;

            if (!CallMethods.MethodsConvert.ContainsKey(name))
            {
                throw new Exception();
            }
            if (!Enum.GetNames(typeof(CallMethods.TypeList)).Contains(name))
            {
                operation = AritmExp(tokens, ref index);
            }
            return new CallMethods(name, operation);
        }

        public ForExp ForEachExp(IToken[] tokens, ref int index)
        {
            if (tokens[++index].Type != TokenType.Identifier)
            {
                throw new Exception();
            }

            string name = tokens[index].Lex;

            if (tokens[++index].Lex != "in")
            {
                throw new Exception();
            }

            if (tokens[++index].Type != TokenType.Identifier)
            {
                throw new Exception("");
            }

            string nameList = tokens[index].Lex.ToLower();
            IOperationExp<IEnumerable<IContextCard>> list;

            if (nameList == "targets")
            {
                list = new TargetExp();
            }
            else
            {
                int lastIndex = --index;
                try
                {
                    list = FindMethodExp(tokens, ref index);
                }
                catch (Exception)
                {
                    index = lastIndex;
                    list = CallMethodsExp(tokens, ref index);
                }
            }
            BlockExp block = BlockExpDef(tokens, ref index);
            return new ForExp(name, list, block);

        }

        public IOperationExp<IEnumerable<IContextCard>> FindMethodExp(IToken[] tokens, ref int index)
        {
            CallMethods call = CallMethodsExp(tokens, ref index);
            if (tokens[++index].Type != TokenType.Dot)
            {
                throw new Exception();
            }
            if (tokens[++index].Lex != "Find")
            {
                throw new Exception();
            }

            if (tokens[++index].Type != TokenType.OpenParenthesis)
            {
                throw new Exception();
            }
            (var name, var exp) = PredicateExp(tokens, ref index);
            if (tokens[++index].Type != TokenType.CloseParenthesis)
            {
                throw new Exception();
            }

            return new FindMethod(call, name, exp);
        }

        private (string name, IOperationExp<bool> exp) PredicateExp(IToken[] tokens, ref int index)
        {
            if (tokens[++index].Type != TokenType.OpenParenthesis)
            {
                index--;
            }
            if (tokens[++index].Type != TokenType.Identifier)
            {
                throw new Exception();
            }
            string name = tokens[index].Lex;
            global.Add(name, VarType.Card);

            if (tokens[++index].Type != TokenType.CloseParenthesis)
            {
                index--;
            }
            if (tokens[++index].Type != TokenType.Lambda)
            {
                throw new Exception();
            }

            IOperationExp<bool> exp = BoolExp(tokens, ref index);
            global.Remove(name);
            return (name, exp);
        }

        public DeclarationVarExp DeVar(IToken[] tokens, ref int index)
        {
            if (tokens[++index].Type != TokenType.Identifier)
            {
                throw new Exception("");
            }

            string namevar = tokens[index].Lex;

            if (tokens[++index].Type != TokenType.Equal)
            {
                throw new Exception("");
            }

            int lastIndex = index;
            ValueExp valueExp;
            try
            {
                IOperationExp<IContextCard> card = CardExp(tokens, ref index);
                valueExp = new ValueExp(card);
                global[namevar] = valueExp.Type;
                return new DeclarationVarExp(namevar, valueExp);
            }
            catch (Exception)
            {
                index = lastIndex;
            }
            valueExp = ValueExp(tokens, ref index);
            global[namevar] = valueExp.Type;
            return new DeclarationVarExp(namevar, valueExp);
        }

        public ValueExp ValueExp(IToken[] tokens, ref int index)
        {
            int lastIndex = index;
            try
            {
                IOperationExp<bool> valueOp = BoolExp(tokens, ref index);
                return new ValueExp(valueOp);
            }
            catch (Exception)
            {
                index = lastIndex;
            }

            try
            {
                IOperationExp<double> valueOp = AritmExp(tokens, ref index);
                return new ValueExp(valueOp);
            }
            catch (Exception)
            {
                index = lastIndex;
            }

            try
            {
                IOperationExp<string> valueOp = StringExp(tokens, ref index);
                return new ValueExp(valueOp);
            }
            catch (Exception) { }

            throw new Exception();
        }

        public Conditional IF(IToken[] tokens, ref int index)
        {
            if (tokens[++index].Type != TokenType.OpenParenthesis)
            {
                throw new Exception();
            }

            IOperationExp<bool> boolean = BoolExp(tokens, ref index);

            if (tokens[++index].Type != TokenType.CloseParenthesis)
            {
                throw new Exception();
            }

            BlockExp ifTrue = BlockExpDef(tokens, ref index);
            BlockExp ifFalse = default;

            var lastIndex = index;

            if (tokens[++index].Lex == "else")
            {
                ifFalse = BlockExpDef(tokens, ref index);
            }
            else
            {
                index = lastIndex;
            }

            return new Conditional(boolean, ifTrue, ifFalse);
        }

        public WhileExp WhileExp(IToken[] tokens, ref int index)
        {
            if (tokens[++index].Type != TokenType.OpenParenthesis)
            {
                throw new Exception();
            }

            IOperationExp<bool> boolean = BoolExp(tokens, ref index);

            if (tokens[++index].Type != TokenType.CloseParenthesis)
            {
                throw new Exception();
            }

            BlockExp block = BlockExpDef(tokens, ref index);
            return new WhileExp(boolean, block);
        }
        #endregion

        #region Expresiones Booleanas
        public IOperationExp<bool> BoolExp(IToken[] tokens, ref int index)
        {
            return LogicExp(tokens, ref index);
        }

        //Verifica si hay && o || 
        public IOperationExp<bool> LogicExp(IToken[] tokens, ref int index)
        {
            IOperationExp<bool> left = SimpleBoolExp(tokens, ref index);
            int lastIndex = index;
            IToken tokenlog = tokens[++index];
            IOperationExp<bool> rigth;
            try
            {
                rigth = LogicExp(tokens, ref index);
            }
            catch (Exception)
            {
                index = lastIndex;
                return left;
            }

            switch (tokenlog.Type)
            {
                case TokenType.And:
                    return new AndExp(left, rigth);
                case TokenType.Or:
                    return new OrExp(left, rigth);
                default:
                    throw new Exception();
            }
        }

        //Para comparadores 
        public IOperationExp<bool> ComparerExp(IToken[] tokens, ref int index)
        {
            int lastIndex = index;
            try
            {
                return ComparerAritmExp(tokens, ref index);
            }
            catch (Exception)
            {
                index = lastIndex;
            }

            try
            {
                return ComparerStringExp(tokens, ref index);
            }
            catch (Exception)
            {
                index = lastIndex;
            }

            throw new Exception();
        }

        private IOperationExp<bool> ComparerStringExp(IToken[] tokens, ref int index)
        {
            IOperationExp<string> left = StringExp(tokens, ref index);
            IToken token = tokens[++index];
            IOperationExp<string> right = StringExp(tokens, ref index);
            switch (token.Type)
            {
                case TokenType.EqualEqual:
                    return new EqualsExp<string>(left, right);
                case TokenType.NotEqual:
                    return new DistExp<string>(left, right);
                case TokenType.GreaterThan:
                    return new GreaterExp<string>(left, right);
                case TokenType.LessThan:
                    return new LessExp<string>(left, right);
                case TokenType.GreaterThanEqual:
                    return new GreaterEqualExp<string>(left, right);
                case TokenType.LessThanEqual:
                    return new LessEqualExp<string>(left, right);
                default:
                    throw new Exception();
            }
        }

        public IOperationExp<bool> ComparerAritmExp(IToken[] tokens, ref int index)
        {
            IOperationExp<double> left = AritmExp(tokens, ref index);
            IToken token = tokens[++index];
            IOperationExp<double> right = AritmExp(tokens, ref index);
            switch (token.Type)
            {
                case TokenType.EqualEqual:
                    return new EqualsExp<double>(left, right);
                case TokenType.NotEqual:
                    return new DistExp<double>(left, right);
                case TokenType.GreaterThan:
                    return new GreaterExp<double>(left, right);
                case TokenType.LessThan:
                    return new LessExp<double>(left, right);
                case TokenType.GreaterThanEqual:
                    return new GreaterEqualExp<double>(left, right);
                case TokenType.LessThanEqual:
                    return new LessEqualExp<double>(left, right);
                default:
                    throw new Exception();
            }
        }

        public IOperationExp<bool> SimpleBoolExp(IToken[] tokens, ref int index)
        {
            IToken token = tokens[++index];

            switch (token.Type)
            {
                case TokenType.True:
                case TokenType.False:
                    index--;
                    return Bool(tokens, ref index);

                case TokenType.Not:
                    return new NotExp(BoolExp(tokens, ref index));

                case TokenType.OpenParenthesis:
                    IOperationExp<bool> boolExp = BoolExp(tokens, ref index);

                    if (tokens[++index].Type != TokenType.CloseParenthesis) throw new Exception();
                    return boolExp;

                case TokenType.Identifier:
                    if (!global.ContainsKey(token.Lex) || global[token.Lex] != VarType.Bool)
                    {
                        index--;
                        return ComparerExp(tokens, ref index);
                    }
                    return new CallVar<bool>(token.Lex);

                default:
                    index--;
                    return ComparerExp(tokens, ref index);
            }
        }

        public IOperationExp<bool> Bool(IToken[] tokens, ref int index)
        {
            if (tokens[++index].Type != TokenType.True && tokens[index].Type != TokenType.False)
            {
                throw new Exception();
            }

            bool result;
            if (!bool.TryParse(tokens[index].Lex, out result))
            {
                throw new Exception();
            }
            return new Bool(result);
        }
        #endregion

        #region Expresiones Aritméticas
        public IOperationExp<double> AritmExp(IToken[] tokens, ref int index) // Por ahora
        {
            return PlusMinus(tokens, ref index);
        }

        public IOperationExp<double> PlusMinus(IToken[] tokens, ref int index)
        {
            IOperationExp<double> left = MultDiv(tokens, ref index);
            int lastIndex = index;
            IToken token = tokens[++index];
            IOperationExp<double> rigth;

            try
            {
                rigth = PlusMinus(tokens, ref index);
            }
            catch (Exception)
            {
                index = lastIndex;
                return left;
            }

            switch (token.Type)
            {
                case TokenType.Plus:
                    return new PlusExp(left, rigth);

                case TokenType.Minus:
                    return new MinusExp(left, rigth);

                default:
                    index = lastIndex;
                    return left;
            }
        }

        public IOperationExp<double> MultDiv(IToken[] tokens, ref int index)
        {
            IOperationExp<double> left = AritmSimpleExp(tokens, ref index);
            int lastIndex = index;
            IToken token = tokens[++index];
            IOperationExp<double> rigth;

            try
            {
                rigth = MultDiv(tokens, ref index);
            }
            catch (Exception)
            {
                index = lastIndex;
                return left;
            }

            switch (token.Type)
            {
                case TokenType.Multiply:
                    return new MultiplyExp(left, rigth);

                case TokenType.Divide:
                    return new DivideExp(left, rigth);

                default:
                    index = lastIndex;
                    return left;
            }
        }

        public IOperationExp<double> AritmSimpleExp(IToken[] tokens, ref int index)
        {
            IToken token = tokens[++index];

            switch (token.Type)
            {
                case TokenType.Identifier:
                    if (tokens[index + 1].Type == TokenType.Dot)
                    {
                        index--;
                        return AttributeCardExp<double>(tokens, ref index);
                    }
                    index++;
                    if (!global.ContainsKey(token.Lex) || global[token.Lex] != VarType.Int)
                        throw new Exception();
                    return new CallVar<double>(token.Lex);

                case TokenType.Number:
                    double result;
                    if (!double.TryParse(tokens[index].Lex, out result))
                    {
                        throw new Exception();
                    }
                    return new Number(result);

                case TokenType.OpenParenthesis:
                    IOperationExp<double> aritmExp = AritmExp(tokens, ref index);

                    if (tokens[++index].Type != TokenType.CloseParenthesis)
                        throw new Exception();

                    return aritmExp;

                default:
                    throw new Exception();
            }
        }

        public IOperationExp<T> AttributeCardExp<T>(IToken[] tokens, ref int index)
        {
            IToken token = tokens[index + 1];
            IOperationExp<IContextCard> card = CardExp(tokens, ref index);

            if (tokens[++index].Type != TokenType.Dot)
            {
                throw new Exception();
            }
            if (tokens[++index].Type != TokenType.Identifier)
            {
                throw new Exception();
            }
            string name = tokens[index].Lex;
            return new Propiety<T>(card, name);
        }


        public IOperationExp<IContextCard> CardExp(IToken[] tokens, ref int index)
        {
            IToken token = tokens[++index];
            if (tokens[index].Type != TokenType.Identifier)
            {
                throw new Exception();
            }

            int lastIndex = index;
            IOperationExp<IContextCard> card;

            try
            {
                try
                {
                    index--;
                    card = IndexMethodExp(tokens, ref index);
                }
                catch (Exception)
                {
                    index = lastIndex - 1;
                    card = PopMethodExp(tokens, ref index);
                }
            }
            catch (Exception)
            {
                index = lastIndex;
                string cardName = token.Lex;
                card = new CallVar<IContextCard>(cardName);
            }
            return card;
        }

        public IOperationExp<IContextCard> PopMethodExp(IToken[] tokens, ref int index)
        {
            CallMethods call = CallMethodsExp(tokens, ref index);

            if (tokens[++index].Type != TokenType.Dot)
            {
                throw new Exception();
            }
            if (tokens[++index].Type != TokenType.Identifier)
            {
                throw new Exception();
            }

            string name = tokens[index].Lex;

            if (tokens[++index].Type != TokenType.OpenParenthesis)
            {
                throw new Exception();
            }
            if (tokens[++index].Type != TokenType.CloseParenthesis)
            {
                throw new Exception();
            }
            return new PopMethod(call, name);
        }

        public IOperationExp<IContextCard> IndexMethodExp(IToken[] tokens, ref int index)
        {
            CallMethods call = CallMethodsExp(tokens, ref index);

            if (tokens[++index].Type != TokenType.OpenBracket)
            {
                throw new Exception();
            }
            IOperationExp<double> exp = AritmExp(tokens, ref index);
            if (tokens[++index].Type != TokenType.CloseBracket)
            {
                throw new Exception();
            }

            return new IndexMethod(call, exp);
        }
        #endregion

        #region Expresiones de Cadenas
        public IOperationExp<string> StringExp(IToken[] tokens, ref int index)
        {
            return StringConcatExp(tokens, ref index);
        }

        public IOperationExp<string> StringConcatExp(IToken[] tokens, ref int index)
        {
            IOperationExp<string> left = StringSimple(tokens, ref index);
            IToken token = tokens[++index];
            int lastIndex = index;
            IOperationExp<string> rigth;
            try
            {
                rigth = StringConcatExp(tokens, ref index);
            }
            catch (Exception)
            {
                index = lastIndex;
                return left;
            }

            switch (token.Type)
            {
                case TokenType.Concat:
                    return new ConcatExp(left, rigth);

                case TokenType.ConcatEsp:
                    return new ConcatExp2(left, rigth);

                default:
                    index = lastIndex - 1;
                    return left;
            }
        }

        public IOperationExp<string> StringSimple(IToken[] tokens, ref int index)
        {
            IToken token = tokens[++index];
            if (token.Type == TokenType.Identifier)
            {
                if (tokens[index + 1].Type == TokenType.Dot)
                {
                    index--;
                    return AttributeCardExp<string>(tokens, ref index);
                }
                index++;
                if (!global.ContainsKey(token.Lex) || global[token.Lex] != VarType.Str)
                    throw new Exception();
                return new CallVar<string>(token.Lex);
            }
            else if (token.Type != TokenType.Marks)
            {
                throw new Exception();
            }
            if (tokens[++index].Type != TokenType.String)
            {
                throw new Exception();
            }

            string result = tokens[index].Lex;

            if (tokens[++index].Type != TokenType.Marks)
            {
                throw new Exception();
            }
            return new StringValue(result);
        }
        #endregion

        #region Definisión de Cartas
        public Card Card(IToken[] tokens, ref int index)
        {

            string type = TypeCard(tokens, ref index);
            if (tokens[++index].Type != TokenType.Comma)
            {
                throw new Exception();
            }

            string name = NameCard(tokens, ref index);
            if (tokens[++index].Type != TokenType.Comma)
            {
                throw new Exception();
            }

            string faccion = Faccion(tokens, ref index);
            if (tokens[++index].Type != TokenType.Comma)
            {
                throw new Exception();
            }

            int power = Power(tokens, ref index);
            if (tokens[++index].Type != TokenType.Comma)
            {
                throw new Exception();
            }

            CardClassification[] cardClassifications = Classifications(tokens, ref index);
            if (tokens[++index].Type != TokenType.Comma)
            {
                throw new Exception();
            }

            var onAction = OnActivationList(tokens, ref index);

            Card card = new Card(name, type, faccion, power, cardClassifications, onAction);
            return card;
        }

        public IEnumerable<IOnActivation> OnActivationList(IToken[] tokens, ref int index)
        {
            List<IOnActivation> onsList = new List<IOnActivation>();
            if (tokens[++index].Lex != "OnActivation")
            {
                throw new Exception("");
            }
            if (tokens[++index].Type != TokenType.Colon)
            {
                throw new Exception();
            }
            if (tokens[++index].Type != TokenType.OpenBracket)
            {
                throw new Exception();
            }

            do
            {
                if (tokens[index + 1].Type == TokenType.CloseBracket || tokens[index + 1].Type == TokenType.EOF)
                {
                    index++;
                    break;
                }

                IOnActivation onAction = OnActivation(tokens, ref index);
                onsList.Add(onAction);
            } while (tokens[++index].Type == TokenType.Comma);

            if (tokens[++index].Type != TokenType.CloseBracket)
            {
                throw new Exception();
            }

            return onsList;
        }

        public IOnActivation OnActivation(IToken[] tokens, ref int index)
        {
            if (tokens[++index].Type != TokenType.OpenKey)
            {
                throw new Exception();
            }

            ICallEffect effect = CallEffectExp(tokens, ref index);

            if (tokens[++index].Type != TokenType.Comma)
            {
                throw new Exception();
            }

            ISelector selector = SelectorExp(tokens, ref index);

            if (tokens[++index].Type != TokenType.Comma)
            {
                index--;
                return new OnActivation(default, effect, selector);
            }

            IActionEffect postAction = PostAction(tokens, ref index);

            if (tokens[++index].Type != TokenType.CloseKey)
            {
                throw new Exception();
            }

            return new OnActivation(postAction, effect, selector);
        }

        public ICallEffect CallEffectExp(IToken[] tokens, ref int index)
        {
            if (tokens[++index].Lex.ToLower() != "effect")
            {
                throw new Exception();
            }
            if (tokens[++index].Type != TokenType.Colon)
            {
                throw new Exception();
            }

            string name;

            if (tokens[++index].Type != TokenType.OpenKey)
            {
                index--;
                name = SimpleName(tokens, ref index);
                return new CallEffect(name, new IInputParams[0]);
            }

            name = NameCard(tokens, ref index);

            if (tokens[++index].Type != TokenType.Comma)
            {
                throw new Exception();
            }

            IInputParams[] inputParams = InputParamsExp(tokens, ref index);

            return new CallEffect(name, inputParams);
        }

        private string SimpleName(IToken[] tokens, ref int index)
        {
            if (tokens[++index].Type != TokenType.Marks)
            {
                throw new Exception();
            }

            if (tokens[++index].Type != TokenType.String)
            {
                throw new Exception();
            }

            string name = tokens[index].Lex;

            if (tokens[++index].Type != TokenType.Marks)
            {
                throw new Exception();
            }
            return name;
        }

        public IInputParams[] InputParamsExp(IToken[] tokens, ref int index)
        {
            List<IInputParams> inputParams = new List<IInputParams>();
            do
            {
                if (tokens[index + 1].Type == TokenType.CloseKey || tokens[index + 1].Type == TokenType.EOF)
                {
                    index++;
                    break;
                }

                IInputParams param = InputParamsDef(tokens, ref index);
                inputParams.Add(param);
            } while (tokens[++index].Type == TokenType.Comma);

            return inputParams.ToArray();
        }

        public IInputParams InputParamsDef(IToken[] tokens, ref int index)
        {
            if (tokens[++index].Type != TokenType.Identifier)
            {
                throw new Exception();
            }

            string name = tokens[index].Lex;

            if (tokens[++index].Type != TokenType.Colon)
            {
                throw new Exception();
            }

            int lastIndex = index;

            try
            {
                IOperationExp<bool> valueOp = BoolExp(tokens, ref index);
                return new InputParams(name, new ValueExp(valueOp));
            }
            catch (Exception)
            {
                index = lastIndex;
            }

            try
            {
                IOperationExp<double> valueOp = AritmExp(tokens, ref index);
                return new InputParams(name, new ValueExp(valueOp));
            }
            catch (Exception)
            {
                index = lastIndex;
            }

            try
            {
                IOperationExp<string> valueOp = StringExp(tokens, ref index);
                return new InputParams(name, new ValueExp(valueOp));
            }
            catch (Exception) { }

            throw new Exception();
        }

        public ISelector SelectorExp(IToken[] tokens, ref int index)
        {
            if (tokens[++index].Lex.ToLower() != "selector")
            {
                throw new Exception();
            }
            if (tokens[++index].Type != TokenType.Colon)
            {
                throw new Exception();
            }

            if (tokens[++index].Type != TokenType.OpenKey)
            {
                throw new Exception();
            }

            if (tokens[++index].Lex.ToLower() != "source")
            {
                throw new Exception();
            }
            if (tokens[++index].Type != TokenType.Colon)
            {
                throw new Exception();
            }

            string name = SimpleName(tokens, ref index);

            if (tokens[++index].Type != TokenType.Comma)
            {
                throw new Exception();
            }

            if (tokens[++index].Lex.ToLower() != "single")
            {
                throw new Exception();
            }
            if (tokens[++index].Type != TokenType.Colon)
            {
                throw new Exception();
            }
            if (tokens[++index].Type != TokenType.True && tokens[index].Type != TokenType.False)
            {
                throw new Exception();
            }

            bool single;
            if (!bool.TryParse(tokens[index].Lex, out single))
            {
                throw new Exception();
            }

            if (tokens[++index].Type != TokenType.Comma)
            {
                throw new Exception();
            }

            if (tokens[++index].Lex.ToLower() != "predicate")
            {
                throw new Exception();
            }
            if (tokens[++index].Type != TokenType.Colon)
            {
                throw new Exception();
            }

            (var nameExp, var exp) = PredicateExp(tokens, ref index);

            Func<IContextCard, bool> predicate = new Func<IContextCard, bool>((IContextCard card) =>
            {
                var contextDic = new Dictionary<string, (VarType, object)>() { { nameExp, (VarType.Card, card) } };
                BlockContext context = new BlockContext(default, default, contextDic);
                return exp.Execute(context);
            });

            return new Selector(Enum.Parse<Source>(name), single, predicate);
        }

        public IActionEffect PostAction(IToken[] tokens, ref int index)
        {
            if (tokens[++index].Type != TokenType.OpenKey)
            {
                throw new Exception();
            }

            ICallEffect effect = CallEffectExp(tokens, ref index);

            if (tokens[++index].Type != TokenType.Comma)
            {
                throw new Exception();
            }

            ISelector selector = SelectorExp(tokens, ref index);

            if (tokens[++index].Type != TokenType.CloseKey)
            {
                throw new Exception();
            }

            return new ActionEffect(effect, selector);
        }

        public string TypeCard(IToken[] tokens, ref int index)
        {
            if (tokens[++index].Lex != "Type")
            {
                throw new Exception();
            }

            if (tokens[++index].Type != TokenType.Colon)
            {
                throw new Exception();
            }

            if (tokens[++index].Type != TokenType.Marks)
            {
                throw new Exception();
            }

            if (tokens[++index].Type != TokenType.String)
            {
                throw new Exception();
            }

            string type = tokens[index].Lex;

            if (tokens[++index].Type != TokenType.Marks)
            {
                throw new Exception();
            }
            return type;
        }

        public string NameCard(IToken[] tokens, ref int index)
        {
            if (tokens[++index].Lex != "Name")
            {
                throw new Exception();
            }

            if (tokens[++index].Type != TokenType.Colon)
            {
                throw new Exception();
            }
            return SimpleName(tokens, ref index);
        }

        public string Faccion(IToken[] tokens, ref int index)
        {
            if (tokens[++index].Lex != "Faction")
            {
                throw new Exception();
            }

            if (tokens[++index].Type != TokenType.Colon)
            {
                throw new Exception();
            }

            if (tokens[++index].Type != TokenType.Marks)
            {
                throw new Exception();
            }

            if (tokens[++index].Type != TokenType.String)
            {
                throw new Exception();
            }

            string faccion = tokens[index].Lex;

            if (tokens[++index].Type != TokenType.Marks)
            {
                throw new Exception();
            }
            return faccion;
        }

        public int Power(IToken[] tokens, ref int index)
        {
            if (tokens[++index].Lex != "Power")
            {
                throw new Exception();
            }

            if (tokens[++index].Type != TokenType.Colon)
            {
                throw new Exception();
            }

            if (tokens[++index].Type != TokenType.Number)
            {
                throw new Exception();
            }
            int power = int.Parse(tokens[index].Lex);

            return power;
        }

        public CardClassification[] Classifications(IToken[] tokens, ref int index)
        {
            List<CardClassification> classifications = new List<CardClassification>();
            if (tokens[++index].Lex != "Range")
            {
                throw new Exception();
            }

            if (tokens[++index].Type != TokenType.Colon)
            {
                throw new Exception();
            }

            if (tokens[++index].Type != TokenType.OpenBracket)
            {
                throw new Exception();
            }

            do
            {
                CardClassification classification = Classification(tokens, ref index);
                classifications.Add(classification);
            } while (tokens[++index].Type == TokenType.Comma && tokens[index + 1].Type != TokenType.EOF);

            if (tokens[index].Type != TokenType.CloseBracket)
            {
                throw new Exception();
            }

            return classifications.ToArray();
        }

        public CardClassification Classification(IToken[] tokens, ref int index)
        {
            CardClassification classification;
            if (tokens[++index].Type != TokenType.Marks)
            {
                throw new Exception();
            }

            switch (tokens[++index].Lex)
            {
                case "Melee":
                    classification = CardClassification.Melee;
                    break;
                case "Ranged":
                    classification = CardClassification.LongRange;
                    break;
                case "Siege":
                    classification = CardClassification.Siege;
                    break;
                default:
                    throw new Exception();
            }

            if (tokens[++index].Type != TokenType.Marks)
            {
                throw new Exception();
            }
            return classification;
        }
        #endregion

    }
}
