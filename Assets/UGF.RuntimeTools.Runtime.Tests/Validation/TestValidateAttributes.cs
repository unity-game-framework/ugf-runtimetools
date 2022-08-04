using System;
using NUnit.Framework;
using UGF.RuntimeTools.Runtime.Contexts;
using UGF.RuntimeTools.Runtime.Validation;

namespace UGF.RuntimeTools.Runtime.Tests.Validation
{
    public class TestValidateAttributes
    {
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
