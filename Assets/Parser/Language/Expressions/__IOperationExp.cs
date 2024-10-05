
namespace Parser.Language
{
    public interface IOperationExp<out T>
    {
        public T Execute(IBlockContext blockContext);
    }

    public interface IDoubleOperationExp<out T, K> : IOperationExp<T>
    {
        IOperationExp<K> Left { get; }
        IOperationExp<K> Rigth { get; }
    }

    public interface ISimpleOperationExp<out T, K> : IOperationExp<T>
    {
        IOperationExp<K> Operation { get; }
    }
}