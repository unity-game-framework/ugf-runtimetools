namespace UGF.RuntimeTools.Runtime.Objects
{
    public delegate bool ObjectRelativePredicate<in TArguments, in TValue>(TArguments arguments, TValue value) where TValue : class;
}
