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
        }

        [Test]
        [TestCase(true, TestName = "All")]
        [TestCase(false, TestName = "Single")]
        public void ValidateWithException(bool all)
        {
            var target = new Target
            {
                Field = new object(),
                Field2 = new object(),
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

            Assert.DoesNotThrow(() => ValidateUtility.Validate(target, new Context(), all));

            try
            {
                ValidateUtility.Validate(target2, new Context(), all);
                Assert.Fail("No exception found.");
            }
            catch (Exception exception)
            {
                if (all)
                {
                    Assert.IsInstanceOf<ValidateReportException>(exception);

                    var reportException = (ValidateReportException)exception;

                    Assert.AreEqual(4, reportException.Report.Results.Count);
                    Assert.AreEqual("Field", reportException.Report.Results[0].Member.Name);
                    Assert.AreEqual("Field2", reportException.Report.Results[1].Member.Name);
                    Assert.AreEqual("Property", reportException.Report.Results[2].Member.Name);
                    Assert.AreEqual("Property2", reportException.Report.Results[3].Member.Name);
                }
                else
                {
                    Assert.IsInstanceOf<ValidateResultException>(exception);

                    var resultException = (ValidateResultException)exception;

                    Assert.AreEqual("Field", resultException.Result.Member.Name);
                }
            }
        }

        [Test]
        [TestCase(true, TestName = "All")]
        [TestCase(false, TestName = "Single")]
        public void ValidateWithReport(bool all)
        {
            var target = new Target
            {
                Field = new object(),
                Field2 = new object(),
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

            bool result = ValidateUtility.Validate(target, new Context(), out ValidateReport report, all);
            bool result2 = ValidateUtility.Validate(target2, new Context(), out ValidateReport report2, all);

            Assert.True(result);
            Assert.False(report.HasResults);
            Assert.False(result2);
            Assert.True(report2.HasResults);

            if (all)
            {
                Assert.AreEqual(4, report2.Results.Count);
                Assert.AreEqual("Field", report2.Results[0].Member.Name);
                Assert.AreEqual("Field2", report2.Results[1].Member.Name);
                Assert.AreEqual("Property", report2.Results[2].Member.Name);
                Assert.AreEqual("Property2", report2.Results[3].Member.Name);
            }
            else
            {
                Assert.AreEqual(1, report2.Results.Count);
                Assert.AreEqual("Field", report2.Results[0].Member.Name);
            }
        }

        [Test]
        [TestCase(true, TestName = "All")]
        [TestCase(false, TestName = "Single")]
        public void ValidateFields(bool all)
        {
            var target = new Target
            {
                Field = new object(),
                Field2 = new object(),
                Property = null,
                Property2 = null
            };

            var target2 = new Target
            {
                Field = null,
                Field2 = null,
                Property = null,
                Property2 = null
            };

            bool result = ValidateUtility.ValidateFields(target, new Context(), out ValidateReport report, all);
            bool result2 = ValidateUtility.ValidateFields(target2, new Context(), out ValidateReport report2, all);

            Assert.True(result);
            Assert.False(report.HasResults);
            Assert.False(result2);
            Assert.True(report2.HasResults);

            if (all)
            {
                Assert.AreEqual(2, report2.Results.Count);
                Assert.AreEqual("Field", report2.Results[0].Member.Name);
                Assert.AreEqual("Field2", report2.Results[1].Member.Name);
            }
            else
            {
                Assert.AreEqual(1, report2.Results.Count);
                Assert.AreEqual("Field", report2.Results[0].Member.Name);
            }
        }

        [Test]
        [TestCase(true, TestName = "All")]
        [TestCase(false, TestName = "Single")]
        public void ValidateProperties(bool all)
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

            bool result = ValidateUtility.ValidateProperties(target, new Context(), out ValidateReport report, all);
            bool result2 = ValidateUtility.ValidateProperties(target2, new Context(), out ValidateReport report2, all);

            Assert.True(result);
            Assert.False(report.HasResults);
            Assert.False(result2);
            Assert.True(report2.HasResults);

            if (all)
            {
                Assert.AreEqual(2, report2.Results.Count);
                Assert.AreEqual("Property", report2.Results[0].Member.Name);
                Assert.AreEqual("Property2", report2.Results[1].Member.Name);
            }
            else
            {
                Assert.AreEqual(1, report2.Results.Count);
                Assert.AreEqual("Property", report2.Results[0].Member.Name);
            }
        }
    }
}
