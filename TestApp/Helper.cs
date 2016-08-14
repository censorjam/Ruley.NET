//using System.Collections.Generic;
//using System.Collections.Immutable;

//namespace TestApp
//{
//	public class Helper
//	{
//		public static ImmutableDictionary<string, object> ToImmutable(IDictionary<object, object> source)
//		{
//			var o = ImmutableDictionary<string, object>.Empty;

//			foreach (var o1 in source)
//			{
//				if (o1.Value is IDictionary<object, object>)
//				{
//					o = o.SetItem((string)o1.Key, ToImmutable((IDictionary<object, object>)o1.Value));
//				}
//				else
//				{
//					double d;
//					if (double.TryParse((string)o1.Value, out d))
//					{
//						o = o.SetItem((string)o1.Key, d);
//					}
//					else
//					{
//						o = o.SetItem((string)o1.Key, o1.Value);
//					}
//				}
//			}

//			return o;
//		}
//	}
//}