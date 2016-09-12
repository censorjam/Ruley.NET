namespace Ruley.NET
{
    public class TemplateStage : InlineStage
    {
        [Primary]
        public Property<string> Template { get; set; }
        public Property<string> Field { get; set; }

        private Templater _templater = new Templater();

        public override void OnFirst(Event e)
        {
            _templater.Compile(Template.Get(e));
        }

        public override Event Apply(Event e)
        {
            e[Field.Get(e)] = _templater.Apply(new TemplateParameters() {@event = e, @params = Context == null ? null : Context.Parameters});
            return e;
        }
    }
}