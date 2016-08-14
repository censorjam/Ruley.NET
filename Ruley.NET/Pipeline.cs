using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;

namespace Ruley.Core
{
    public class Pipeline : Stage
    {
        public List<Stage> Stages { get; set; }

        public Pipeline()
        {
            Stages = new List<Stage>();
        }

        public static Pipeline FromDynamic(dynamic def)
        {
            var chain = new Pipeline();

            Stage prev = null;
            foreach (var f in def)
            {
                Stage stage = StageBuilder.Resolve(f);
                chain.Stages.Add(stage);

                if (prev != null)
                {
                    prev.;
                }

                prev = stage;
            }
            return chain;
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