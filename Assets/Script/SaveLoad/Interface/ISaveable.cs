using System;
using System.Threading.Tasks;
namespace Systems.SaveLoad.Interface
{
    public interface ISaveable
    {
        Guid Id { get; set; }
    }
}