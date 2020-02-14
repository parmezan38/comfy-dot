using IdServer.Data;
using IdServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdServer.Controllers
{
    public class RoomController : IRoomController
    {
        private readonly ApplicationDbContext _db;

        public RoomController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<RoomDisplayable>> Get(string id)
        {
            try
            {
                var rooms = await _db.Rooms.ToListAsync();

                List<RoomDisplayable> result = rooms.Select(room => new RoomDisplayable()
                {
                    Id = room.Id,
                    Name = room.Name,
                    Capacity = room.Capacity,
                    Owned = id.Equals(room.UserId)
                }).ToList();

                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task Create(Room room)
        {
            try
            {
                var result = await _db.Rooms.AddAsync(room);
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task Delete(int id)
        {
            try
            {
                var room = await _db.Rooms.FirstOrDefaultAsync(it => it.Id == id);
                _db.Remove(room);
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
