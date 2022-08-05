using System;
using System.Collections.Generic;
using NUnit.Framework;
using UGF.RuntimeTools.Runtime.Contexts;
using UGF.RuntimeTools.Runtime.Validation;

namespace UGF.RuntimeTools.Runtime.Tests.Validation
{
    public class TestValidateAttributes
    {
        private class Target
        {
            [Validate]
            public bool Bool { get; set; }

            [ValidateNot(false)]
            public bool Bool2 { get; set; }

            [ValidateMin(1)]
            public int Int { get; set; }

            [ValidateNot(0)]
            [ValidateNot(int.MinValue)]
            public long Long { get; set; }

            [ValidateMax(10.5F)]
            public float Float { get; set; }

            [ValidateRange(1D, 10D)]
            public double Double { get; set; }

            [ValidateMax(3)]
            public string String { get; set; }

            [ValidateRange(TypeCode.Boolean, TypeCode.Byte)]
            public TypeCode Code { get; set; }

            [ValidateRange(1, 3)]
            public string[] Array { get; set; }

            [ValidateOneOf("One", "Two")]
            public string Option { get; set; }

            [ValidateOneOf(TypeCode.Boolean, TypeCode.Byte)]
            public TypeCode Option2 { get; set; }

            [ValidateNotDefault]
            public Guid Guid { get; set; }

            [ValidateNotEmpty]
            public string String2 { get; set; }

            [ValidateNotEmpty]
            public object[] Array2 { get; set; }

            [ValidateNotEmpty]
            public List<object> Collection { get; set; }
        }

        [Test]
        public void ValidateTarget()
        {
            var target = new Target
            {
                Bool = false,
                Bool2 = true,
                Int = 2,
                Long = 1,
                Float = 10F,
                Double = 5D,
                String = "00",
                Code = TypeCode.SByte,
                Array = new[]
                {
                    "One"
                },
                Option = "One",
                Option2 = TypeCode.Boolean,
                Guid = Guid.NewGuid(),
                String2 = "0",
                Array2 = new object[] { "0" },
                Collection = new List<object>
                {
                    "0"
                }
            };

            var target2 = new Target
            {
                Bool = false,
                Bool2 = true,
                Int = 2,
                Long = 1,
                Float = 10F,
                Double = 5D,
                String = "00",
                Code = TypeCode.SByte,
                Array = new[]
                {
                    "One"
                },
                Option = "One",
                Option2 = TypeCode.Boolean,
                Guid = default,
                String2 = "",
                Array2 = Array.Empty<object>(),
                Collection = new List<object>()
            };

            Assert.DoesNotThrow(() => ValidateUtility.Validate(target, new Context()));
            Assert.Throws<ValidateReportException>(() => ValidateUtility.Validate(target2, new Context()));
        }

        [Test]
        public void Validate()
        {
            Assert.True(new ValidateAttribute().Validate(new object(), new Context()));
            Assert.True(new ValidateAttribute().Validate(new Context(), new Context()));
            Assert.False(new ValidateAttribute(typeof(Context)).Validate(new object(), new Context()));

            Assert.True(new ValidateRangeAttribute(0, 0).Validate(0, new Context()));
            Assert.True(new ValidateRangeAttribute(0, 5).Validate(1, new Context()));
            Assert.True(new ValidateRangeAttribute(-15L, 5L).Validate(-1L, new Context()));
            Assert.True(new ValidateRangeAttribute(45L, 5646L).Validate(4541L, new Context()));
            Assert.True(new ValidateRangeAttribute(0D, 5D).Validate(1D, new Context()));
            Assert.True(new ValidateRangeAttribute(-10D, 5D).Validate(-1D, new Context()));
            Assert.True(new ValidateRangeAttribute(-51353.5D, 1651065D).Validate(546D, new Context()));

            Assert.True(new ValidateRangeAttribute(0L, 5D).Validate(1, new Context()));
            Assert.True(new ValidateRangeAttribute(0L, 5.5D).Validate(1, new Context()));
            Assert.False(new ValidateRangeAttribute(0L, 5D).Validate(55.5D, new Context()));
            Assert.False(new ValidateRangeAttribute(0L, 5.5D).Validate(-10L, new Context()));

            Assert.True(new ValidateRangeAttribute(0L, 5.5D).Validate(new object[1], new Context()));
            Assert.True(new ValidateRangeAttribute(1, 4).Validate("000", new Context()));
            Assert.False(new ValidateRangeAttribute(0L, 5.49D).Validate(new object[6], new Context()));
            Assert.False(new ValidateRangeAttribute(1, 4).Validate("00000", new Context()));

            Assert.True(new ValidateRangeAttribute(0, 2).Validate(TypeCode.Empty, new Context()));
            Assert.True(new ValidateRangeAttribute(TypeCode.Empty, TypeCode.Char).Validate(TypeCode.Boolean, new Context()));
            Assert.False(new ValidateRangeAttribute(0, 2).Validate(TypeCode.Byte, new Context()));
            Assert.False(new ValidateRangeAttribute(TypeCode.Empty, TypeCode.Boolean).Validate(TypeCode.Char, new Context()));

            Assert.True(new ValidateMinAttribute(0).Validate(0, new Context()));
            Assert.True(new ValidateMinAttribute(10).Validate(15, new Context()));
            Assert.True(new ValidateMinAttribute(-10).Validate(-5, new Context()));
            Assert.False(new ValidateMinAttribute(10).Validate(5, new Context()));
            Assert.False(new ValidateMinAttribute(-10).Validate(-15, new Context()));

            Assert.True(new ValidateMinAttribute(5).Validate(new object[5], new Context()));
            Assert.True(new ValidateMinAttribute(2).Validate("000", new Context()));
            Assert.False(new ValidateMinAttribute(5).Validate(new object[2], new Context()));
            Assert.False(new ValidateMinAttribute(2).Validate("0", new Context()));

            Assert.True(new ValidateMinAttribute(2).Validate(TypeCode.Boolean, new Context()));
            Assert.True(new ValidateMinAttribute(TypeCode.Boolean).Validate(TypeCode.Char, new Context()));
            Assert.False(new ValidateMinAttribute(2).Validate(TypeCode.Empty, new Context()));
            Assert.False(new ValidateMinAttribute(TypeCode.Char).Validate(TypeCode.Empty, new Context()));

            Assert.True(new ValidateMaxAttribute(0).Validate(0, new Context()));
            Assert.True(new ValidateMaxAttribute(10).Validate(5, new Context()));
            Assert.True(new ValidateMaxAttribute(-10).Validate(-15, new Context()));
            Assert.False(new ValidateMaxAttribute(10).Validate(15, new Context()));
            Assert.False(new ValidateMaxAttribute(-10).Validate(-5, new Context()));

            Assert.True(new ValidateMaxAttribute(5).Validate(new object[5], new Context()));
            Assert.True(new ValidateMaxAttribute(3).Validate("000", new Context()));
            Assert.False(new ValidateMaxAttribute(5).Validate(new object[6], new Context()));
            Assert.False(new ValidateMaxAttribute(2).Validate("000", new Context()));

            Assert.True(new ValidateMaxAttribute(2).Validate(TypeCode.Empty, new Context()));
            Assert.True(new ValidateMaxAttribute(TypeCode.Char).Validate(TypeCode.Boolean, new Context()));
            Assert.False(new ValidateMaxAttribute(2).Validate(TypeCode.Char, new Context()));
            Assert.False(new ValidateMaxAttribute(TypeCode.Boolean).Validate(TypeCode.Char, new Context()));

            Assert.True(new ValidateMatchAttribute("^[0-9]*$").Validate(156, new Context()));
            Assert.True(new ValidateMatchAttribute(@"^\d*\.?\d*$").Validate(5.55F, new Context()));

            Assert.False(new ValidateNotAttribute(default(int)).Validate(default(int), new Context()));
            Assert.False(new ValidateNotAttribute(default(Guid)).Validate(Guid.Empty, new Context()));
            Assert.False(new ValidateNotAttribute(TypeCode.Empty).Validate(TypeCode.Empty, new Context()));

            Assert.True(new ValidateOneOfAttribute("One", "Two", "Three").Validate("One", new Context()));
            Assert.False(new ValidateOneOfAttribute("One", "Two", "Three").Validate("Test", new Context()));
        }
    }
}
