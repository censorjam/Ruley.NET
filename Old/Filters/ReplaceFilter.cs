namespace Ruley.NET
{
    class ReplaceFilter : InlineFilter
    {
        public Property<Event> Data { get; set; } 

        public override Event Apply(Event msg)
        {
            return msg;
        }
    }
}
