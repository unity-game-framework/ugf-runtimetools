using System;
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

            [Validate]
            public Target2 Nested { get; set; }
        }

        private class Target2
        {
            [Validate]
            public object Field;

            [Validate]
            public object Property2 { get; set; }
        }

        [Test]
        public void ValidateWithException()
        {
            var target = new Target
            {
                Field = new object(),
                Field2 = new object(),
                Property = new object(),
                Property2 = new object(),
                Nested = new Target2
                {
                    Field = new object(),
                    Property2 = new object()
                }
            };

            var target2 = new Target
            {
                Field = null,
                Field2 = null,
                Property = null,
                Property2 = null,
                Nested = new Target2
                {
                    Field = new object(),
                    Property2 = null
                }
            };

            Assert.DoesNotThrow(() => ValidateUtility.Validate(target, new Context()));

            try
            {
                ValidateUtility.Validate(target2, new Context());
                Assert.Fail("No exception found.");
            }
            catch (Exception exception)
            {
                Assert.IsInstanceOf<ValidateReportException>(exception);

                var reportException = (ValidateReportException)exception;

                Assert.AreEqual(5, reportException.Report.Results.Count);
                Assert.AreEqual("Field", reportException.Report.Results[0].Member.Name);
                Assert.AreEqual("Field2", reportException.Report.Results[1].Member.Name);
                Assert.AreEqual("Property", reportException.Report.Results[2].Member.Name);
                Assert.AreEqual("Property2", reportException.Report.Results[3].Member.Name);
                Assert.AreEqual("Property2", reportException.Report.Results[4].Member.Name);
                Assert.Pass(exception.Message);
            }
        }

        [Test]
        public void ValidateWithReport()
        {
            var target = new Target
            {
                Field = new object(),
                Field2 = new object(),
                Property = new object(),
                Property2 = new object(),
                Nested = new Target2
                {
                    Field = new object(),
                    Property2 = new object()
                }
            };

            var target2 = new Target
            {
                Field = null,
                Field2 = null,
                Property = null,
                Property2 = null,
                Nested = new Target2
                {
                    Field = new object(),
                    Property2 = null
                }
            };

            bool result = ValidateUtility.Validate(target, new Context(), out ValidateReport report);
            bool result2 = ValidateUtility.Validate(target2, new Context(), out ValidateReport report2);

            Assert.True(result);
            Assert.False(report.HasResults);
            Assert.False(result2);
            Assert.True(report2.HasResults);

            Assert.AreEqual(5, report2.Results.Count);
            Assert.AreEqual("Field", report2.Results[0].Member.Name);
            Assert.AreEqual("Field2", report2.Results[1].Member.Name);
            Assert.AreEqual("Property", report2.Results[2].Member.Name);
            Assert.AreEqual("Property2", report2.Results[3].Member.Name);
            Assert.AreEqual("Property2", report2.Results[4].Member.Name);
        }

        [Test]
        public void ValidateFields()
        {
            var target = new Target
            {
                Field = new object(),
                Field2 = new object(),
                Property = new object(),
                Property2 = new object(),
                Nested = new Target2
                {
                    Field = new object(),
                    Property2 = new object()
                }
            };

            var target2 = new Target
            {
                Field = null,
                Field2 = null,
                Property = null,
                Property2 = null,
                Nested = new Target2
                {
                    Field = null,
                    Property2 = null
                }
            };

            bool result = ValidateUtility.ValidateFields(target, new Context(), out ValidateReport report);
            bool result2 = ValidateUtility.ValidateFields(target2, new Context(), out ValidateReport report2);

            Assert.True(result);
            Assert.False(report.HasResults);
            Assert.False(result2);
            Assert.True(report2.HasResults);

            Assert.AreEqual(2, report2.Results.Count);
            Assert.AreEqual("Field", report2.Results[0].Member.Name);
            Assert.AreEqual("Field2", report2.Results[1].Member.Name);
        }

        [Test]
        public void ValidateProperties()
        {
            var target = new Target
            {
                Field = new object(),
                Field2 = new object(),
                Property = new object(),
                Property2 = new object(),
                Nested = new Target2
                {
                    Field = new object(),
                    Property2 = new object()
                }
            };

            var target2 = new Target
            {
                Field = null,
                Field2 = null,
                Property = null,
                Property2 = null,
                Nested = new Target2
                {
                    Field = null,
                    Property2 = null
                }
            };

            bool result = ValidateUtility.ValidateProperties(target, new Context(), out ValidateReport report);
            bool result2 = ValidateUtility.ValidateProperties(target2, new Context(), out ValidateReport report2);

            Assert.True(result);
            Assert.False(report.HasResults);
            Assert.False(result2);
            Assert.True(report2.HasResults);

            Assert.AreEqual(3, report2.Results.Count);
            Assert.AreEqual("Property", report2.Results[0].Member.Name);
            Assert.AreEqual("Property2", report2.Results[1].Member.Name);
            Assert.AreEqual("Property2", report2.Results[2].Member.Name);
        }
    }
}
