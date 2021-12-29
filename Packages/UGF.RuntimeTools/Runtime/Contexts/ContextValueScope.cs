using System;

namespace UGF.RuntimeTools.Runtime.Contexts
{
    public readonly struct ContextValueScope : IDisposable
    {
        public IContext Context { get; }
        public object Value { get; }

        public ContextValueScope(IContext context, object value)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Value = value ?? throw new ArgumentNullException(nameof(value));

            Context.Add(Value);
        }

        public void Dispose()
        {
            Context.Remove(Value);
        }
    }
}
