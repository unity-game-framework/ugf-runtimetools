using NUnit.Framework;
using UGF.RuntimeTools.Runtime.Contexts;

namespace UGF.RuntimeTools.Runtime.Tests.Contexts
{
    public class TestContext
    {
        [Test]
        public void AddAndRemove()
        {
            int counterAdded = 0;
            int counterRemoved = 0;
            int counterCleared = 0;
            var context = new Context();

            context.Added += (context1, value) => counterAdded++;
            context.Removed += (context1, value) => counterRemoved++;
            context.Cleared += context1 => counterCleared++;

            Assert.IsEmpty(context);
            Assert.AreEqual(0, counterAdded);
            Assert.AreEqual(0, counterRemoved);
            Assert.AreEqual(0, counterCleared);

            context.Add("0");
            context.Add("1");

            Assert.IsNotEmpty(context);
            Assert.True(context.Contains("0"));
            Assert.True(context.Contains("1"));
            Assert.AreEqual(2, counterAdded);
            Assert.AreEqual(0, counterRemoved);
            Assert.AreEqual(0, counterCleared);

            string result0 = context.Get<string>();
            string result1 = context.Get<string, string>("1", (arguments, value) => value == arguments);

            Assert.NotNull(result0);
            Assert.NotNull(result1);
            Assert.AreEqual("0", result0);
            Assert.AreEqual("1", result1);

            context.Remove("0");

            Assert.IsNotEmpty(context);
            Assert.False(context.Contains("0"));
            Assert.True(context.Contains("1"));
            Assert.AreEqual(2, counterAdded);
            Assert.AreEqual(1, counterRemoved);
            Assert.AreEqual(0, counterCleared);

            string result2 = context.Get<string>();

            Assert.NotNull(result2);
            Assert.AreEqual("1", result2);

            context.Clear();

            Assert.IsEmpty(context);
            Assert.False(context.Contains("0"));
            Assert.False(context.Contains("1"));
            Assert.AreEqual(2, counterAdded);
            Assert.AreEqual(1, counterRemoved);
            Assert.AreEqual(1, counterCleared);
        }
    }
}
