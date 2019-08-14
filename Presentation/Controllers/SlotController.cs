using System;
using System.Collections.Generic;
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

        // GET: api/Slot
        [HttpGet]
        [Route("page/{id:int}")]
        public async Task<IEnumerable<SlotMinimumDTO>> Get(int id)
        {
            if (id < 1)
            {
                Response.StatusCode = 400;
                return null;
            }

            var slotPage = await _slotRepresentationService.GetPage(id, 10);
            if (!slotPage.Any())
            {
                Response.StatusCode = 404;
                return null;
            }

            return slotPage;
        }

        // GET: api/Slot/5
        [HttpGet("{id:int}", Name = "GetSlot")]
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
            catch (Exception e)
            {
                Response.StatusCode = 500;
                await Response.WriteAsync(e.Message);
            }
            return null;
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
            catch (Exception)
            {
                Response.StatusCode = 500;
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

        // PUT: api/Slot/5
        [Authorize]
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            string imageLink = await _slotManagementService.DeleteSlot(id, Convert.ToInt32(User.FindFirst("Id").Value));
            System.IO.File.Delete(imageLink.Replace("/", "\\"));
        }
    }
}
