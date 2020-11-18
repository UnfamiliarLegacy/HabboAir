using System.Threading.Tasks;
using Sulakore.Network.Protocol;

namespace Sulakore.Network
{
    public interface IHNode
    {
        Task<int> SendPacketAsync(HPacket packet);
    }
}