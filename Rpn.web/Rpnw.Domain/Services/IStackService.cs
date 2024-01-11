using Rpnw.Domain.Model.Rpn;

public interface IStackService
{
    Guid CreateStack();
    void DeleteStack(Guid stackId);
    void ClearStack(Guid stackId);
    IEnumerable<IRpnElement> GetStackElements(Guid stackId);
    IEnumerable<Guid> GetAllStacks();
    void AddOperator(Guid stackId, int operatorId);
    public void UndoOperator(Guid stackId);
    void AddOperand(Guid stackId, decimal value);

}
