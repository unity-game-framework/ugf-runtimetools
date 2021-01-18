using System.Collections.Generic;
using NUnit.Framework;
using UGF.RuntimeTools.Runtime.Collections;
using Unity.PerformanceTesting;

namespace UGF.RuntimeTools.Runtime.Tests.Collections
{
    public class TestCollectionsPerformance
    {
        private readonly List<Data> m_list = new List<Data>
        {
            new Data
            {
                Name = "Name1",
                Value = 11
            },
            new Data
            {
                Name = "Name2",
                Value = 11
            },
            new Data
            {
                Name = "Name3",
                Value = 11
            }
        };

        public class Data
        {
            public string Name { get; set; }
            public int Value { get; set; }
        }

        [Test, Performance]
        public void List()
        {
            Measure.Method(ListMethod).WarmupCount(1).MeasurementCount(100).GC().Run();
        }

        [Test, Performance]
        public void ListDirty()
        {
            Measure.Method(ListDirtyMethod).WarmupCount(1).MeasurementCount(100).GC().Run();
        }

        private void ListMethod()
        {
            var arg = (name: "Name1", value: 11);

            m_list.Get(arg, (value, argument) => value.Name == argument.name && value.Value == argument.value);
        }

        private void ListDirtyMethod()
        {
            var arg = (name: "Name1", value: 11);

            m_list.Find(x => x.Name == arg.name && x.Value == arg.value);
        }
    }
}
