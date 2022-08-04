using NUnit.Framework;
using UGF.RuntimeTools.Runtime.Contexts;
using UGF.RuntimeTools.Runtime.Validation;

namespace UGF.RuntimeTools.Runtime.Tests.Validation
{
    public class TestValidateUtility
    {
        private class Target
        {
            [Validate]
            public object Field;
            [Validate]
            public object Field2;

            [Validate]
            public object Property { get; set; }

            [Validate]
            public object Property2 { get; set; }
        }

        [Test]
        public void ValidateWithExceptionSingle()
        {
        }

        [Test]
        public void ValidateWithExceptionAll()
        {
        }

        [Test]
        public void ValidateWithReportSingle()
        {
        }

        [Test]
        public void ValidateWithReportAll()
        {
        }

        [Test]
        public void ValidateFieldsSingle()
        {
        }

        [Test]
        public void ValidateFieldsAll()
        {
        }

        [Test]
        public void ValidatePropertiesSingle()
        {
        }

        [Test]
        public void ValidatePropertiesAll()
        {
            var target = new Target
            {
                Field = null,
                Field2 = null,
                Property = new object(),
                Property2 = new object()
            };

            var target2 = new Target
            {
                Field = null,
                Field2 = null,
                Property = null,
                Property2 = null
            };

            bool result = ValidateUtility.ValidateProperties(target, new Context(), out ValidateReport report);
            bool result2 = ValidateUtility.ValidateProperties(target2, new Context(), out ValidateReport report2);

            Assert.True(result);
            Assert.False(report.HasResults);
            Assert.False(result2);
            Assert.True(report2.HasResults);
            Assert.AreEqual(2, report2.Results.Count);
            Assert.AreEqual("Property", report2.Results[0].Member.Name);
            Assert.AreEqual("Property2", report2.Results[1].Member.Name);
        }
    }
}
