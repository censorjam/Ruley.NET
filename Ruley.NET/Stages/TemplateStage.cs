namespace Ruley.NET
{
    public class TemplateStage : InlineStage
    {
        [Primary]
        public Property<string> Template { get; set; }
        public Property<string> Destination { get; set; }

        private Templater _templater = new Templater();

        public override void OnFirst(Event e)
        {
            _templater.Compile(Template.Get(e));
        }

        public override Event Apply(Event e)
        {
            e[Destination.Get(e)] = _templater.Apply(new TemplateParameters() {@event = e, @params = Context.Parameters});
            return e;
        }
    }
}