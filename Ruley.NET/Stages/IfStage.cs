namespace Ruley
{
    public class PassThroughStage : InlineStage
    {
        public override Event Apply(Event x)
        {
            return x;
        }
    }

    public class BlockingStage : InlineStage
    {
        public override Event Apply(Event x)
        {
            return null;
        }
    }

    public class WhereStage : IfStage
    {
        
    }

    public class IfStage : Stage
    {
        [Primary]
        public Property<bool> Value { get; set; }
        public Stage Then { get; set; }
        public Stage Else { get; set; }

        public IfStage()
        {
            Then = new PassThroughStage();
            Else = new BlockingStage();
        }

        public override void Start()
        {
            Then.NextStage = this.NextStage;
            this.NextStage = null;     
            
            Then.Start();
            Else.Start();   
        }

        public override void Next(Event x)
        {
            var match = Value.Get(x);
            if (match)
            {
                Logger.Debug("True");
                Then.Next(x);
            }
            else
            {
                Logger.Debug("False");
                Else.Next(x);
            }
        }
    }
}