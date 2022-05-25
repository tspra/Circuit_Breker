using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CircuitBreakerDemo.CircuitBreaker
{
    public class  CircuitBreaker
    {
        private readonly ICircuitBreakerStateStore stateStore =
         CircuitBreakerStateStoreFactory.GetCircuitBreakerStateStore();

        private readonly object halfOpenSyncObject = new object();

        public bool IsClosed { get { return stateStore.IsClosed; } }

        public bool IsOpen { get { return !IsClosed; } }

        public void ExecuteAction(Action action)
        {
           
             if (IsOpen)
            {
                if (stateStore.LastStateChangedDateUtc  < DateTime.UtcNow)
                {
                    bool lockTaken = false;
                    try
                    {
                        Monitor.TryEnter(halfOpenSyncObject, ref lockTaken);
                        if (lockTaken)
                        {
                            stateStore.HalfOpen();
                            action();
                            this.stateStore.Reset();
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        // If there's still an exception, trip the breaker again immediately.
                        this.stateStore.Trip(ex);

                        // Throw the exception so that the caller knows which exception occurred.
                        throw;
                    }
                    finally
                    {
                        if (lockTaken)
                        {
                            Monitor.Exit(halfOpenSyncObject);
                        }
                    }
                }
               
               // throw new CircuitBreakerOpenException(stateStore.LastException);
            }

            // The circuit breaker is Closed, execute the action.
            try
            {
                action();
            }
            catch (Exception ex)
            {
                this.TrackException(ex);
                throw;
            }
        }

        private void TrackException(Exception ex)
        {
            this.stateStore.LastException = ex;
            this.stateStore.Trip(ex);
        }
    }
}
