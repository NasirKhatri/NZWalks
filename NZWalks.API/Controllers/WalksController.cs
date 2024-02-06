using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IWalkInterface _iWalkInterface;
        public WalksController(IMapper mapper, IWalkInterface iWalkInterface)
        {
            _mapper = mapper;
            _iWalkInterface = iWalkInterface;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDTO addWalkRequestDTO)
        {
            var walkDomainModel = _mapper.Map<Walk>(addWalkRequestDTO);
            var newWalk = await _iWalkInterface.CreateAsync(walkDomainModel);

            return Ok(_mapper.Map<WalkDTO>(walkDomainModel));

        }

        //api/Walks?filterOn=value1&filterQuery=value2&sortBy=value3&isAscending=boolvalue
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? filterOn, 
            [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy,
            [FromQuery] bool? isAscending,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 1000)
        {
            var walksDomainModel = await _iWalkInterface.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true, pageNumber, pageSize);

            return Ok(_mapper.Map<List<WalkDTO>>(walksDomainModel));
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walk = await _iWalkInterface.GetByIdAsync(id);
            if(walk == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<WalkDTO>(walk));
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalkRequestDTO updateWalkRequestDTO)
        {
            var walkDomainModel = _mapper.Map<Walk>(updateWalkRequestDTO);
            walkDomainModel = await _iWalkInterface.UpdateAsync(id, walkDomainModel);
            if(walkDomainModel == null) { return NotFound(); }
            return Ok(_mapper.Map<WalkDTO>(walkDomainModel));

        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deletedWalkDomainModel = _iWalkInterface.DeleteAsync(id);
            if(deletedWalkDomainModel == null) { return NotFound(); }
            return Ok(_mapper.Map<WalkDTO>(deletedWalkDomainModel));
        }
    }
}
