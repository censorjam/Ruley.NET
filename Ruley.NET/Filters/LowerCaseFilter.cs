namespace Ruley.Core.Filters
{
    public class LowerCaseFilter : InlineFilter
    {
        public Property<string> Value { get; set; }
        public Property<string> Destination { get; set; }

        public override Event Apply(Event msg)
        {
            msg.Data[Destination.Get(msg)] = ((string) Value.Get(msg)).ToLower();
            return msg;
        }
    }

    public class UpperCaseFilter : InlineFilter
    {
        public Property<string> Value { get; set; }
        public Property<string> Destination { get; set; }

        public override Event Apply(Event msg)
        {
            msg.Data[Destination.Get(msg)] = ((string)Value.Get(msg)).ToUpper();
            return msg;
        }
    }

    public class MaxLengthFilter : InlineFilter
    {
        public Property<string> Value { get; set; }
        public Property<string> Destination { get; set; }
        public Property<long> Length { get; set; }

        public override Event Apply(Event msg)
        {
            var value = (string) Value.Get(msg);
            var length = Length.Get(msg);

            if (value.Length <= length)
            {
                msg.Data[Destination.Get(msg)] = value;
                return msg;
            }
            else
            {
                msg.Data[Destination.Get(msg)] = value.Substring(0, (int)length - 3) + "...";
            }
            
            return msg;
        }
    }
}
