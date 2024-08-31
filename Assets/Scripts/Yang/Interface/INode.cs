public interface INode
{
    public enum ENodeState
    {
        ENS_Running,
        ENS_Success,
        ENS_Failure,
    }

    public ENodeState Evaluate();
}
