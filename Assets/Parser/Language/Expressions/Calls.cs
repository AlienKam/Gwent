
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Parser;

namespace Parser.Language
{

    class CallVar<T> : IOperationExp<T>
    {
        public string name { get; protected set; }

        public CallVar(string name)
        {
            this.name = name;
        }

        public T Execute(IBlockContext blockContext)
        {
            (VarType type, object value) value;
            bool existvar = blockContext.LocalVar.TryGetValue(name, out value) || blockContext.GlobalVar.TryGetValue(name, out value);

            if (!existvar)
            {
                throw new Exception();
            }

            switch (value.type)
            {
                case VarType.Int:
                    if (typeof(T) != typeof(int))
                    {
                        throw new Exception();
                    }
                    return (T)value.value;
                case VarType.Bool:
                    if (typeof(T) != typeof(bool))
                    {
                        throw new Exception();
                    }
                    return (T)value.value;
                case VarType.Str:
                    if (typeof(T) != typeof(string))
                    {
                        throw new Exception();
                    }
                    return (T)value.value;
                case VarType.Card:
                    if (typeof(T) is not IContextCard)
                    {
                        throw new Exception();
                    }
                    return (T)value.value;
                default:
                    throw new Exception();
            }
        }
    }

    class Propiety<T> : IOperationExp<T>
    {
        public string name { get; protected set; }
        public IOperationExp<IContextCard> exp { get; protected set; }

        public Propiety(IOperationExp<IContextCard> exp, string name)
        {
            this.name = name;
            this.exp = exp;
        }

        public T Execute(IBlockContext blockContext)
        {
            IContextCard card = exp.Execute(blockContext);
            object value;
            switch (name)
            {
                case "Type":
                    if (typeof(T) != typeof(string))
                    {
                        throw new Exception("");
                    }

                    value = card.Type;
                    return (T)value;

                case "Name":
                    if (typeof(T) != typeof(string))
                    {
                        throw new Exception("");
                    }
                    value = card.Name;
                    return (T)value;
                case "Faction":
                    if (typeof(T) != typeof(string))
                    {
                        throw new Exception("");
                    }
                    value = card.Name;
                    return (T)value;

                case "Power":
                    if (typeof(T) != typeof(int))
                    {
                        throw new Exception("");
                    }
                    value = card.Power;
                    return (T)value;

                case "Range":
                    if (typeof(T) != typeof(string[]))
                    {
                        throw new Exception("");
                    }
                    value = card.Range;
                    return (T)value;

                default:
                    throw new Exception("");
            }
        }
    }

    public class CallMethods : IOperationExp<List<IContextCard>>
    {
        public static Dictionary<string, TypeList> MethodsConvert = new Dictionary<string, TypeList>()
        {
            {"Field", TypeList.Field },
            {"Graveyard", TypeList.Graveyard },
            {"Deck", TypeList.Deck },
            {"Hand", TypeList.Hand },
            {"FieldOfPlayer", TypeList.Field },
            {"GraveyardOfPlayer", TypeList.Graveyard },
            {"DeckOfPlayer", TypeList.Deck },
            {"HandOfPlayer", TypeList.Hand }
        };
        public enum TypeList
        {
            Field,
            Graveyard,
            Deck,
            Hand
        }

        public TypeList method { get; protected set; }
        public IOperationExp<double>? player { get; protected set; }

        public CallMethods(string method, IOperationExp<double>? player = null)
        {
            this.method = MethodsConvert[method];
            this.player = player;
        }

        public List<IContextCard> Execute(IBlockContext blockContext)
        {
            int calledPlayer = player != null ? (int)player.Execute(blockContext) : blockContext.context.TriggerPlayer;
            switch (method)
            {
                case TypeList.Field:
                    return blockContext.context.FieldOfPlayer(calledPlayer);
                case TypeList.Graveyard:
                    return blockContext.context.GraveyardOfPlayer(calledPlayer);
                case TypeList.Deck:
                    return blockContext.context.DeckOfPlayer(calledPlayer);
                case TypeList.Hand:
                    return blockContext.context.HandOfPlayer(calledPlayer);
                default:
                    throw new Exception("");
            }
        }
    }

    public class PopMethod : IOperationExp<IContextCard>
    {
        public CallMethods callList { get; protected set; }
        public string method { get; protected set; }

        public PopMethod(CallMethods callList, string method)
        {
            this.callList = callList;
            this.method = method;
        }

        public IContextCard Execute(IBlockContext blockContext)
        {
            List<IContextCard> cards = callList.Execute(blockContext);
            IContextCard card = cards[cards.Count - 1];
            cards.RemoveAt(cards.Count - 1);
            return card;
        }
    }

    public class IndexMethod : IOperationExp<IContextCard>
    {
        public CallMethods callList { get; protected set; }
        public IOperationExp<double> index { get; protected set; }

        public IndexMethod(CallMethods callList, IOperationExp<double> index)
        {
            this.callList = callList;
            this.index = index;
        }

        public IContextCard Execute(IBlockContext blockContext)
        {
            List<IContextCard> cards = callList.Execute(blockContext);
            return cards[(int)index.Execute(blockContext)];
        }
    }

    public class FindMethod : IOperationExp<IEnumerable<IContextCard>>
    {
        public FindMethod(CallMethods call, string name, IOperationExp<bool> operation)
        {
            Call = call;
            Name = name;
            Operation = operation;
        }

        public CallMethods Call { get; }
        public string Name { get; }
        public IOperationExp<bool> Operation { get; }

        public IEnumerable<IContextCard> Execute(IBlockContext blockContext)
        {
            return Call.Execute(blockContext).Where(x =>
            {
                blockContext.LocalVar[Name] = (VarType.Card, x);
                return Operation.Execute(blockContext);
            });
        }
    }

    public class TargetExp : IOperationExp<IEnumerable<IContextCard>>
    {
        public IEnumerable<IContextCard> Execute(IBlockContext blockContext)
        {
            return blockContext.targets;
        }
    }
}