using System;
using System.Collections.Generic;

namespace Ruley.NET
{
    public class Context
	{
        public DynamicDictionary Parameters { get; private set; }

        public Context() : this(new DynamicDictionary())
        {
        }

		public Context(DynamicDictionary parameters)
		{
			Parameters = parameters;
		}

		public Event GetNext()
		{
			return new Event();
		}
	}

    public class Process : Pipeline
    {
        
    }

	public class Pipeline : Stage
    {
        public List<Stage> Stages { get; set; }

        public Pipeline()
        {
            Stages = new List<Stage>();
        }

        protected override void Process(Event e)
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

            Stages[Stages.Count - 1].Subscribe(PushNext);
        }
    }
}