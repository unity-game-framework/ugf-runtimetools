namespace UGF.RuntimeTools.Runtime.Contexts
{
    public delegate bool ContextPredicate<in TArguments, in TValue>(TArguments arguments, TValue value);
}
