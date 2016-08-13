namespace Ruley
{ 
    public abstract class Stage : Component
    {
        public virtual Stage NextStage { get; set; }

        public virtual void Start()
        {
            
        }

        public virtual void Next(Event e)
        {
        }

        public void OnNext(Event e)
        {
            if (NextStage != null)
                NextStage.Next(e);
        }
    }
}