namespace Localwire.AlgoToolkit.Graphs
{
    public interface IGraph<TKey> where TKey : struct
    {
        bool AddEdge(TKey firstNodeKey, TKey secondNodeKey);
        bool RemoveEdge(TKey firstNodeKey, TKey secondNodeKey);
        bool ClearNodes();
        bool HasNode(TKey nodeKey);
        bool AddEdgeWithNodes(TKey firstNodeKey, TKey secondNodeKey);
    }
}