namespace Ruley.NET
{
    public abstract class Output : Component
    {
        public abstract void Do(Event msg);

        public void Execute(Event msg)
        {
            Do(msg);
        }
    }
}