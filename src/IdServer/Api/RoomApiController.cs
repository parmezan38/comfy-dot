using IdServer.Hubs;
using IdServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System.Security.Claims;

namespace IdServer.Controllers
{
    [ApiController]
    [Route("api/rooms")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class RoomApiController : ControllerBase
    {
        private readonly IRoomController _roomController;
        private readonly IHubContext<ChatHub> _chatHub;

        public RoomApiController(IRoomController roomController, IHubContext<ChatHub> chatHub)
        {
            _roomController = roomController;
            _chatHub = chatHub;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var rooms = await _roomController.Get(id);
                return new JsonResult(rooms);
            }
            catch
            {
                return new JsonResult("Something went wrong with fetching the rooms.");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody]Room room)
        {
            try
            {
                await _roomController.Create(room);
                // TODO: Move to separate chat hub manager
                await _chatHub.Clients.All.SendAsync("RefreshRooms");
                return new JsonResult($"{room.Name} created.");
            }
            catch
            {
                return new JsonResult($"Something went wrong with creating {room.Name}.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _roomController.Delete(id);
                // TODO: Move to separate chat hub manager
                await _chatHub.Clients.All.SendAsync("RefreshRooms");
                return new JsonResult("Room deleted.");
            }
            catch
            {
                return new JsonResult("Something went wrong with deleting the room.");
            }
        }

        [HttpGet("/{id}")]
        public async Task<ActionResult> Join(int id)
        {
            try
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                await _chatHub.Groups.AddToGroupAsync(userId, id.ToString());
                return new JsonResult("Joined a room.");
            }
            catch
            {
                return new JsonResult("Something went wrong with joining the room.");
            }
        }
    }
}
