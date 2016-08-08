using System.Dynamic;

namespace Ruley.Core.Outputs
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