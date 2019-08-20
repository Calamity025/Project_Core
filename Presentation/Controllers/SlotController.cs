using System;
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

        [HttpGet]
        [Route("page/{id:int}&{itemsOnPage}")]
        public async Task<Page> GetPage(int id, int itemsOnPage)
        {
            if (id <= 0 || itemsOnPage <= 0)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Page cannot be 0 or lower");
                return null;
            }
            return await _slotRepresentationService.GetPage(id, itemsOnPage);
        }

        [HttpGet("{id:int}")]
        public async Task<SlotFullDTO> GetSlot(int id)
        {
            if (id <= 0)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Slot id cannot be 0 or lower");
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
        public async Task<decimal?> GetSlotPrice(int id)
        {
            if (id <= 0)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Slot id cannot be 0 or lower");
                return null;
            }
            try
            {
                var res = await _slotRepresentationService.GetSlotPrice(id);
                Response.StatusCode = 200;

                return res;
            }
            catch
            {
                return null;
            }
        }

        [Authorize]
        [HttpGet("{id}/userBet")]
        public async Task<decimal?> GetUserBet(int id)
        {
            if (id <= 0)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Slot id cannot be 0 or lower");
                return null;
            }
            var userId = Convert.ToInt32(User.Claims.First(x => x.Type == "Id").Value);
            try
            {
                var res = await _slotRepresentationService.GetUserBet(id, userId);
                Response.StatusCode = 200;

                return res;
            }
            catch
            {
                return null;
            }
        }

        [Authorize]
        [HttpPost]
        public async Task Post([FromBody] SlotCreationModel newSlot)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = 400;
                await WriteErrors();
                return;
            }
            var id = Convert.ToInt32(User.Claims.First(x => x.Type == "Id").Value);
            try
            {
                int slotId = await _slotManagementService.CreateSlot(id, _mapper.Map<SlotCreationDTO>(newSlot));
                Response.StatusCode = 201;
                await Response.WriteAsync(slotId.ToString());
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                await Response.WriteAsync(e.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("image/{id}")]
        public async Task PostImage(IFormFile file, int id)
        {
            if (id <= 0 || file == null)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Slot id cannot be 0 or lower \n No file");
                return;
            }
            try
            {
                if (!Directory.Exists(Directory.GetCurrentDirectory() + "\\wwwroot\\SlotImages\\"))
                {
                    Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\wwwroot\\SlotImages\\");
                }
                using (FileStream filestream = System.IO.File.Create(
                    Directory.GetCurrentDirectory() + "\\wwwroot\\SlotImages\\" + $"_{id}{Path.GetExtension(file.FileName)}"))
                {
                    file.CopyTo(filestream);
                    filestream.Flush();
                    await _slotManagementService.AddImageLink(id,
                        $"/SlotImages/_{id}{Path.GetExtension(file.FileName)}");
                }
                Response.StatusCode = 201;
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
            if (id <= 0 || page <= 0 || itemsOnPage <= 0)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Page cannot be 0 or lower");
                return null;
            }
            var slots = await _slotRepresentationService.GetByCategory(id, page, itemsOnPage);
            Response.StatusCode = 200;
            return slots;
        }

        [HttpPost("byTags/{page}&{itemsOnPage}")]
        public async Task<Page> GetSlotsByTags(int page, int itemsOnPage, [FromBody]int[] ids)
        {
            if (page <= 0 || itemsOnPage <= 0)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Page cannot be 0 or lower");
                return null;
            }
            var slots = await _slotRepresentationService.GetByTags(ids, page, itemsOnPage);
            Response.StatusCode = 200;
            return slots;
        }

        [HttpPost("byName/{page}&{itemsOnPage}")]
        public async Task<Page> GetSlotsByName(int page, int itemsOnPage, [FromBody]string name)
        {
            if (page <= 0 || itemsOnPage <= 0)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Page cannot be 0 or lower");
                return null;
            }
            var slots = await _slotRepresentationService.GetByName(name, page, itemsOnPage);
            Response.StatusCode = 200;
            return slots;
        }

        [Authorize]
        [HttpPut("makeBet/{id}")]
        public async Task MakeBet(int id, [FromBody]decimal bet)
        {
            if (id <= 0 || bet <= 0)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Slot id or bet cannot be 0 or lower");
                return;
            }
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
            if (id <= 0)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Slot id cannot be 0 or lower");
                return;
            }
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

        [Authorize]
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] SlotUpdateModel slotInfo)
        {
            if (id <= 0 || !ModelState.IsValid)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Slot id cannot be 0 or lower");
                await WriteErrors();
                return;
            }
            try
            {
                var info = _mapper.Map<SlotUpdateDTO>(slotInfo);
                await _slotManagementService.UpdateGeneralInfo(id, info);
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
            if (id <= 0 || file == null)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Slot id cannot be 0 or lower \n No file");
                return;
            }
            try
            {
                var slot = await _slotRepresentationService.GetSlot(id);
                string link = slot.ImageLink;
                link = Directory.GetCurrentDirectory() + "\\wwwroot" + link;
                System.IO.File.Delete(link.Replace("/", "\\"));
                await PostImage(file, id);
                Response.StatusCode = 204;
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
            if (id > 0 && Enum.Parse(typeof(Status.SlotStatus), status) is Status.SlotStatus slotStatus)
            {
                try
                {
                    await _slotManagementService.UpdateStatus(id, slotStatus);
                    Response.StatusCode = 204;
                }
                catch
                {
                    Response.StatusCode = 500;
                }
            }
            else
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Slot id cannot be 0 or lower \n No matching status found");
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            if (id <= 0)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Slot id cannot be 0 or lower");
                return;
            }
            var userId = Convert.ToInt32(User.FindFirst("Id").Value);
            try
            {
                string imageLink = await _slotManagementService.DeleteSlot(id, userId);
                if (imageLink != null)
                {
                    imageLink = Directory.GetCurrentDirectory() + "\\wwwroot" + imageLink;
                    imageLink = imageLink.Replace("/", "\\");
                    System.IO.File.Delete(imageLink);
                }
            }
            catch
            {
                Response.StatusCode = 500;
            }
        }

        private async Task WriteErrors()
        {
            foreach (var modelStateValue in ModelState.Values)
            {
                foreach (var error in modelStateValue.Errors)
                {
                    await Response.WriteAsync(error.ErrorMessage);
                }
            }
        }
    }
}
