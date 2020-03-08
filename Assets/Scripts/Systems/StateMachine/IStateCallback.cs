public interface IStateCallback<T>
{
    void OnChangeState(T state);
}
