using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BLL;
using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Presentation.Models;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SlotController : ControllerBase
    {
        private readonly ISlotRepresentationService _slotRepresentationService;
        private readonly ISlotManagementService _slotManagementService;
        private readonly IMapper _mapper;

        public SlotController(ISlotRepresentationService slotRepresentationService, ISlotManagementService slotManagementService, IMapper mapper)
        {
            _slotRepresentationService = slotRepresentationService;
            _slotManagementService = slotManagementService;
            _mapper = mapper;
        }

        // GET: api/Slot
        [HttpGet]
        [Route("page/{id:int}&{itemsOnPage}")]
        public async Task<Page> GetPage(int id, int itemsOnPage)
        {
            if (id < 1)
            {
                Response.StatusCode = 400;
                return null;
            }

            var slotPage = await _slotRepresentationService.GetPage(id, itemsOnPage);

            return slotPage;
        }

        // GET: api/Slot/5
        [HttpGet("{id:int}")]
        public async Task<SlotFullDTO> GetSlot(int id)
        {
            if (id < 1)
            {
                Response.StatusCode = 400;
                return null;
            }

            try
            {
                return await _slotRepresentationService.GetSlot(id);
            }
            catch (NotFoundException)
            {
                Response.StatusCode = 404;
            }
            catch (Exception)
            {
                Response.StatusCode = 500;
            }

            return null;
        }

        [HttpGet("{id}/price")]
        public async Task<decimal> GetSlotPrice(int id)
        {
            return await _slotRepresentationService.GetSlotPrice(id);
        }

        [Authorize]
        [HttpGet("{id}/userBet")]
        public async Task GetUserBet(int id)
        {
            var userId = Convert.ToInt32(User.Claims.First(x => x.Type == "Id").Value);
            var res = await _slotRepresentationService.GetUserBet(id, userId);
            if (res == -1)
            {
                return;
            }

            Response.StatusCode = 200;
            await Response.WriteAsync(JsonConvert.SerializeObject(res,
                new JsonSerializerSettings {Formatting = Formatting.Indented}));
        }

        // POST: api/Slot
        [Authorize]
        [HttpPost]
        public async Task Post([FromBody] SlotCreationModel newSlot)
        {
            var id = Convert.ToInt32(User.Claims.First(x => x.Type == "Id").Value);
            try
            {
                int slotId = await _slotManagementService.CreateSlot(id, _mapper.Map<SlotCreationDTO>(newSlot));
                Response.StatusCode = 200;
                await Response.WriteAsync(slotId.ToString());
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                await Response.WriteAsync(e.Message);
            }
        }

        // POST: api/Slot
        [Authorize]
        [HttpPost]
        [Route("image/{id}")]
        public async Task Post(IFormFile file, int id)
        {
            try
            {
                if (!Directory.Exists(Directory.GetCurrentDirectory() + "\\wwwroot\\SlotImages\\"))
                {
                    Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\wwwroot\\SlotImages\\");
                }

                using (FileStream filestream = System.IO.File.Create(
                    Directory.GetCurrentDirectory() + "\\wwwroot\\SlotImages\\" +
                    $"{Path.GetFileNameWithoutExtension(file.FileName)}_{id}{Path.GetExtension(file.FileName)}"))
                {
                    file.CopyTo(filestream);
                    filestream.Flush();
                    await _slotManagementService.AddImageLink(id,
                        $"/SlotImages/{Path.GetFileNameWithoutExtension(file.FileName)}_{id}{Path.GetExtension(file.FileName)}");
                }

                Response.StatusCode = 200;
                await Response.WriteAsync(id.ToString());
            }
            catch (NotFoundException)
            {
                Response.StatusCode = 404;
            }
            catch (Exception)
            {
                Response.StatusCode = 500;
            }

        }

        [HttpPost("byCategory/{page}&{itemsOnPage}")]
        public async Task<Page> GetSlotsByCategory(int page, int itemsOnPage, [FromBody]int id)
        {
            if (id < 0)
            {
                Response.StatusCode = 400;
                return null;
            }

            var slots = await _slotRepresentationService.GetByCategory(id, page, itemsOnPage);

            Response.StatusCode = 200;
            return slots;
        }

        [HttpPost("byTags/{page}&{itemsOnPage}")]
        public async Task<Page> GetSlotsByTags(int page, int itemsOnPage, [FromBody]int[] ids)
        {
            var slots = await _slotRepresentationService.GetByTags(ids, page, itemsOnPage);

            Response.StatusCode = 200;
            return slots;
        }

        [HttpPost("byName/{page}&{itemsOnPage}")]
        public async Task<Page> GetSlotsByName(int page, int itemsOnPage, [FromBody]string name)
        {
            var slots = await _slotRepresentationService.GetByName(name, page, itemsOnPage);

            Response.StatusCode = 200;
            return slots;
        }

        [Authorize]
        [HttpPut("makeBet/{id}")]
        public async Task MakeBet(int id, [FromBody]decimal bet)
        {
            var userId = Convert.ToInt32(User.Claims.First(x => x.Type == "Id").Value);
            try
            {
                await _slotManagementService.MakeBet(id, userId, bet);
                Response.StatusCode = 204;
            }
            catch
            {
                Response.StatusCode = 500;
            }
        }

        [Authorize]
        [HttpDelete("undoBets/{id}")]
        public async Task UndoBet(int id)
        {
            var userId = Convert.ToInt32(User.Claims.First(x => x.Type == "Id").Value);
            try
            {
                await _slotManagementService.UndoBet(id, userId);
                Response.StatusCode = 204;
            }
            catch
            {
                Response.StatusCode = 500;
            }
        }

        // PUT: api/Slot/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] SlotUpdateDTO slotInfo)
        {
            try
            {
                await _slotManagementService.UpdateGeneralInfo(id, slotInfo);
                Response.StatusCode = 204;
            }
            catch
            {
                Response.StatusCode = 500;
            }
        }

        [Authorize]
        [HttpPut("{id}/image")]
        public async Task UpdateImage(int id, IFormFile file)
        {
            try
            {
                if (!Directory.Exists(Directory.GetCurrentDirectory() + "\\wwwroot\\SlotImages\\"))
                {
                    Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\wwwroot\\SlotImages\\");
                }

                using (FileStream filestream = System.IO.File.Create(
                    Directory.GetCurrentDirectory() + "\\wwwroot\\SlotImages\\" +
                    $"{Path.GetFileNameWithoutExtension(file.FileName)}_{id}{Path.GetExtension(file.FileName)}"))
                {
                    var slot = await _slotRepresentationService.GetSlot(id);
                    string link = slot.ImageLink;
                    link = Directory.GetCurrentDirectory() + "\\wwwroot" + link;
                    System.IO.File.Delete(link.Replace("/", "\\"));
                    file.CopyTo(filestream);
                    filestream.Flush();
                    await _slotManagementService.AddImageLink(id,
                        $"/SlotImages/{Path.GetFileNameWithoutExtension(file.FileName)}_{id}{Path.GetExtension(file.FileName)}");
                }

                Response.StatusCode = 200;
                await Response.WriteAsync(id.ToString());
            }
            catch (NotFoundException)
            {
                Response.StatusCode = 404;
            }
            catch (Exception)
            {
                Response.StatusCode = 500;
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}/status")]
        public async Task UpdateStatus(int id, [FromBody] string status)
        {
            try
            {
                await _slotManagementService.UpdateStatus(id, (Status) Enum.Parse(typeof(Status), status));
                Response.StatusCode = 204;
            }
            catch
            {
                Response.StatusCode = 500;
            }
        }

        // DELETE: api/ApiWithActions/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            string imageLink = await _slotManagementService.DeleteSlot(id, Convert.ToInt32(User.FindFirst("Id").Value));
            if (imageLink != null)
            {
                imageLink = Directory.GetCurrentDirectory() + "\\wwwroot" + imageLink;
                System.IO.File.Delete(imageLink.Replace("/", "\\"));
            }
        }
    }
}
