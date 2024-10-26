using System;

namespace Systems.SaveLoad.Interface
{
    public interface IBind<TData> where TData : ISaveable
    {
        Guid Id { get; set; }
        void Bind(TData data);
    }
}