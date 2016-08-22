using Ruley.Core;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Ruley
{
	public class Context
	{
        public DynamicDictionary Parameters { get; private set; }

		public Context(DynamicDictionary parameters)
		{
			Parameters = parameters;
		}

		public Event GetNext()
		{
			return new Event(Parameters);
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

            Stages[Stages.Count - 1].Subscribe(PushNext);
        }
    }
}