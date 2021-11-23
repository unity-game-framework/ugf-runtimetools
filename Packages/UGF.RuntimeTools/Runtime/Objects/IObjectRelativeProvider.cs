using UGF.RuntimeTools.Runtime.Providers;

namespace UGF.RuntimeTools.Runtime.Objects
{
    public interface IObjectRelativeProvider : IProvider<object, IObjectRelativesCollection>
    {
        void Connect(object first, object second);
        bool Disconnect(object first, object second);
        void AddChild(object parent, object child);
        bool RemoveChild(object parent, object child);
    }
}
