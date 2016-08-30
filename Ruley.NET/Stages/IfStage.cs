using System;

namespace Ruley.NET
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

        public override void OnFirst(Event e)
        {
            Then.Subscribe(PushNext);
            Else.Subscribe(PushNext);

            Then.Start();
            Else.Start();
        }

        protected override void Process(Event x)
        {
            var match = Value.Get(x);
            if (match)
            {
                Logger.Debug("True");
                Then.OnNext(x);
            }
            else
            {
                Logger.Debug("False");
                Else.OnNext(x);
            }
        }
    }
}