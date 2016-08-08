using System;

namespace Ruley.Core.Inputs
{
    public class TestInput : IntervalInput
    {
        private int _number = 1;
        private Random r = new Random();

        public override void OnTick()
        {
            string json;
            if (r.NextDouble() > 0.5)
            {
                json = "{ 'appName':'finscoreservice', 'host':'L1APDEV001', 'o': { 'value': 1000 } }";
            }
            else
            {
                json = "{ 'appName':'sportscore', 'host':'L1APDEV001', 'o': { 'value': 1000 } }";
            }

            dynamic x = FromJson(json);
            _number = _number + 10;
            x.o.value = _number;
            OnNext(x);
        }
    }
}