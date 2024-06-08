namespace ITService.UI.Services
{
    public interface IMessageBus
    {
        IDisposable RegisterHandler<T>(Action<T> handler);
        void Send<T>(T message);
    }
    public class MessageBusService : IMessageBus
    {
        private class Subscription<T> : IDisposable
        {
            private WeakReference<MessageBusService> _messageBus;
            public Action<T> Handler { get; }
            public Subscription(MessageBusService messageBus, Action<T> handler)
            {
                this.Handler = handler;
                _messageBus = new(messageBus);
            }

            public void Dispose()
            {
                if(!_messageBus.TryGetTarget(out var bus))
                    return;
                var @lock = bus._lock;
                @lock.EnterWriteLock();
                var messageType = typeof(T);
                try
                {
                    if(!bus._subscriptions.TryGetValue(messageType, out var references))
                        return;
                    var updatedReferences = references.Where(r => r.IsAlive).ToList();
                    WeakReference? currentReference = null;
                    foreach (var reference in updatedReferences)
                    {
                        if (ReferenceEquals(reference.Target, this))
                        {
                            currentReference = reference;
                            break;
                        }
                    }
                    if(currentReference is null)
                        return;

                    updatedReferences.Remove(currentReference);
                    bus._subscriptions[messageType] = updatedReferences;
                }
                finally
                {
                    @lock.ExitWriteLock();
                }
            }
        }

        private readonly Dictionary<Type, IEnumerable<WeakReference>> _subscriptions = new();
        private readonly ReaderWriterLockSlim _lock = new();

        public IDisposable RegisterHandler<T>(Action<T> handler)
        {
            var subscription = new Subscription<T>(this, handler);
            
            _lock.EnterReadLock();
            try
            {
                var subscriptionReference = new WeakReference(subscription);
                var messageType = typeof(T);
                _subscriptions[messageType] = _subscriptions.TryGetValue(messageType, out var subscriptions)
                    ? subscriptions.Append(subscriptionReference)
                    : new[] { subscriptionReference };
            }
            finally
            {
                _lock.ExitReadLock();
            }
            return subscription;
        }

        private IEnumerable<Action<T>>? GetHanlers<T>()
        {
            var handlers = new List<Action<T>>();
            var messageType = typeof(T);
            var isDeadRefExist = false;

            _lock.EnterReadLock();
            try
            {
                if (!_subscriptions.TryGetValue(messageType, out var references))
                    return null;
                foreach (var reference in references)
                    if (reference.Target is Subscription<T> { Handler: var handler })
                        handlers.Add(handler);
                    else
                        isDeadRefExist = true;
            }
            finally
            {
                _lock.ExitReadLock();
            }

            if (!isDeadRefExist) return handlers;

            _lock.EnterWriteLock();
            try
            {
                if(_subscriptions.TryGetValue(messageType, out var references))
                    if (references.Where(r => r.IsAlive).ToArray() is { Length: > 0 } newReferences)
                        _subscriptions[messageType] = newReferences;
                    else
                        _subscriptions.Remove(messageType);

            }
            finally
            {
                _lock.ExitWriteLock();
            }

            return handlers;
        }
        public void Send<T>(T message)
        {
            if(GetHanlers<T>() is not {} handlers)
                return;
            foreach (var handler in handlers)
                handler(message);
        }
    }
}
