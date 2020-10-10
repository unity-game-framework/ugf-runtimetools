namespace UGF.RuntimeTools.Runtime.Collections
{
    public delegate bool CollectionPredicate<in TValue, in TArgument>(TValue value, TArgument argument);
}
