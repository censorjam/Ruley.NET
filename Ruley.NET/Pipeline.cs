using Ruley.Core;
using System.Collections.Generic;

namespace Ruley
{
	public class Context
	{
		public Event GetNextEvent()
		{
			return new Event();
		}
	}

	public class Pipeline : Stage
    {
        public List<Stage> Stages { get; set; }

        public Pipeline()
        {
            Stages = new List<Stage>();
        }

		

        public override void OnNext(Event e)
        {
            Stages[0].OnNext(e);
        }

        public override void Start()
        {
            //reverse order?
            foreach (var stage in Stages)
            {
                stage.Start();
            }
        }
    }
}