using System.Collections.Generic;
using NUnit.Framework;
using UGF.RuntimeTools.Runtime.Collections;

namespace UGF.RuntimeTools.Runtime.Tests.Collections
{
    public class TestCollectionsUtility
    {
        public class Data
        {
            public string Name { get; set; }
            public int Value { get; set; }
        }

        [Test]
        public void TryGetReadOnlyList()
        {
            var collection = new List<Data>
            {
                new Data
                {
                    Name = "Name",
                    Value = 10
                }
            };

            bool result1 = collection.TryGet((name: "Name", value: 10), (value, argument) => value.Name == argument.name && value.Value == argument.value, out Data result2);

            Assert.True(result1);
            Assert.NotNull(result2);
            Assert.AreEqual("Name", result2.Name);
            Assert.AreEqual(10, result2.Value);
        }

        [Test]
        public void TryGetDictionary()
        {
            var collection = new Dictionary<string, Data>
            {
                {
                    "Data",
                    new Data
                    {
                        Name = "Name",
                        Value = 10
                    }
                }
            };

            bool result1 = CollectionsUtility.TryGet(collection, (name: "Name", value: 10), (value, argument) => value.Name == argument.name, out Data value);
        }
    }
}
