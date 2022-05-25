using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CircuitBreakerDemo.CircuitBreaker
{
    public class CircuitBreakerStateStore : ICircuitBreakerStateStore
    {
        public CircuitBreakerStateEnum State { get; set; }

        public Exception LastException { get; set; }

        public DateTime LastStateChangedDateUtc { get; set; }

        public bool IsClosed { get; set; }

        public void HalfOpen()
        {
            this.State = CircuitBreakerStateEnum.halfopen;
        }

        public void Reset()
        {
            this.State = CircuitBreakerStateEnum.closed;
        }

        public void Trip(Exception ex)
        {
            // Setting the next state.
            State = Enum.GetValues(State.GetType()).Cast<CircuitBreakerStateEnum>().Concat(new[] { default(CircuitBreakerStateEnum) }).SkipWhile(e => !State.Equals(e)).Skip(1).First();
        }
    }
}
