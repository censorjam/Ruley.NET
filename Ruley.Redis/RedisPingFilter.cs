using Ruley.Core;
using Ruley.Core.Filters;
using Ruley.Core.Inputs;
using StackExchange.Redis;
using System;

namespace Ruley.Redis
{
    public class RedisPingFilter : InlineFilter
    {
        private IServer _server;
        private readonly object _lock = new object();
        public Property<string> Host { get; set; }
        public Property<string> Password { get; set; }
        public Property<long> Port { get; set; }

        public override Event Apply(Event msg)
        {
            string connectionString = Host.Get(msg);
            if (Port != null)
            {
                connectionString += ":" + Port.Get(msg);
            }
            if (Password != null)
            {
                connectionString += ",password=" + Password.Get(msg);
            }

            lock (_lock)
            {
                if (_server == null)
                {
                    try
                    {
                        //todo send unhealthy if this fails
                        var options = ConfigurationOptions.Parse(connectionString);
                        options.AllowAdmin = true;
                        var redis = ConnectionMultiplexer.Connect(options);
                        _server = redis.GetServer(redis.GetEndPoints()[0]);
                    }
                    catch (Exception e)
                    {
                        dynamic m = msg;
                        m.exception = e;
                        m.ping = null;
                        m.redis_ping_ms = null;
                        return msg;
                    }
                }
            }

            try
            {
                var elapsed = _server.Ping();
                dynamic payload = msg;
                payload.exception = null;
                payload.ping = elapsed;
                payload.redis_ping_ms = elapsed.TotalMilliseconds;
                return msg;
            }
            catch (Exception e)
            {
                dynamic m = msg;
                m.exception = e;
                m.ping = null;
                m.pingMs = null;
                return msg;
            }
        }
    }


    public class RedisInfoInput : IntervalInput
    {
        private IServer _server;
        private readonly object _lock = new object();
        public Property<string> Host { get; set; }
        public Property<string> Password { get; set; }
        public Property<long> Port { get; set; }

        public override void OnTick()
        {
            //string connectionString = Host.Get(msg);
            //if (Port != null)
            //{
            //    connectionString += ":" + Port.Get(msg);
            //}
            //if (Password != null)
            //{
            //    connectionString += ",password=" + Password.Get(msg);
            //}

            string connectionString = "ddbrds001:6387,password=DEV_bc7859c63ce32c5f6636717d9068f234bf4095eaeeff86b08d480396648bfe21";

            lock (_lock)
            {
                if (_server == null)
                {
                    //todo send unhealthy if this fails
                    var options = ConfigurationOptions.Parse(connectionString);
                    options.AllowAdmin = true;
                    var redis = ConnectionMultiplexer.Connect(options);
                    _server = redis.GetServer(redis.GetEndPoints()[0]);
                }
            }

            var info = _server.Info();
            

            foreach (var x in info)
            {
                foreach (var k in x)
                {
                    dynamic payload = new Event();
                    payload.property = k.Key;

                    double v;
                    if (double.TryParse(k.Value, out v)) {
                        payload.value = v;
                        OnNext(payload);
                    }
                        
                }
            }
        }
    }
}
