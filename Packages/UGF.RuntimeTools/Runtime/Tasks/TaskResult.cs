using System;

namespace UGF.RuntimeTools.Runtime.Tasks
{
    public readonly struct TaskResult<TValue>
    {
        public TValue Value { get { return HasValue ? m_value : throw new ArgumentException("Value not specified."); } }
        public bool HasValue { get; }

        public static TaskResult<TValue> Empty { get; } = new TaskResult<TValue>();

        private readonly TValue m_value;

        public TaskResult(TValue value)
        {
            m_value = value;

            HasValue = true;
        }

        public static implicit operator TValue(TaskResult<TValue> result)
        {
            return result.Value;
        }

        public static implicit operator TaskResult<TValue>(TValue value)
        {
            return new TaskResult<TValue>(value);
        }

        public static implicit operator bool(TaskResult<TValue> result)
        {
            return result.HasValue;
        }
    }
}
