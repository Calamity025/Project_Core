using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BLL;
using BLL.DTO;
using BLL.Interfaces;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Presentation.Models;

namespace Presentation.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SlotController : ControllerBase
    {
        private readonly ISlotRepresentationService _slotRepresentation;
        private readonly ISlotManagementService _slotManagement;
        private readonly IMapper _mapper;

        public SlotController(ISlotRepresentationService slotRepresentation, ISlotManagementService slotManagement, IMapper mapper)
        {
            _slotRepresentation = slotRepresentation;
            _slotManagement = slotManagement;
            _mapper = mapper;
        }

        // GET: api/Slot
        [HttpGet]
        [Route("page/{id:int}")]
        public async Task<IEnumerable<SlotMinimumDTO>> Get(int id)
        {
            return await _slotRepresentation.GetPage(id, 10);
        }

        // GET: api/Slot/5
        [HttpGet("{id:int}", Name = "GetSlot")]
        public async Task<SlotFullDTO> GetSlot(int id)
        {
            return await _slotRepresentation.GetSlot(id);
        }

        // POST: api/Slot
        [HttpPost]
        public async Task Post([FromBody] SlotCreationModel newSlot)
        {
            int slotId = await _slotManagement.CreateSlot(newSlot.UserId, _mapper.Map<SlotCreationDTO>(newSlot));
            Response.StatusCode = 200;
            await Response.WriteAsync(slotId.ToString());
        }

        // POST: api/Slot
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
                using (FileStream filestream = System.IO.File.Create(Directory.GetCurrentDirectory() + "\\wwwroot\\SlotImages\\" + $"{Path.GetFileNameWithoutExtension(file.FileName)}_{id}{Path.GetExtension(file.FileName)}"))
                {
                    file.CopyTo(filestream);
                    filestream.Flush();
                    await _slotManagement.AddImageLink(id,
                        $"/SlotImages/{Path.GetFileNameWithoutExtension(file.FileName)}_{id}{Path.GetExtension(file.FileName)}");
                }

                Response.StatusCode = 200;
                await Response.WriteAsync(id.ToString());
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        // PUT: api/Slot/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _slotManagement.DeleteSlot(id);
        }
    }
}
