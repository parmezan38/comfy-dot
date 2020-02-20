using IdServer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdServer.Controllers
{
    public interface IRoomController
    {
        Task<IEnumerable<RoomDisplayable>> Get(string id);
        Task Create(Room room);
        Task Delete(int id, string userId);
    }
}
