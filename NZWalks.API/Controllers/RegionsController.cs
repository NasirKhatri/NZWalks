using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDBContext _dbContext;
        private readonly IRegionInterface _regionInterface;
        private readonly IMapper _mapper;
        public RegionsController(NZWalksDBContext dbContext, IRegionInterface iRegionInterface, IMapper iMapper)
        {
            _dbContext = dbContext;
            _regionInterface = iRegionInterface;
            _mapper = iMapper;
        }

        [HttpGet]
        [Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> getAll()
        {
            var regions = await _regionInterface.GetAllAsync();
            /*var regionsDTO = new List<RegionDTO>();
            foreach (var region in regions)
            {
                regionsDTO.Add(new RegionDTO()
                {
                    Id = region.Id,
                    Name = region.Name,
                    Code = region.Code,
                    ImageURL = region.ImageURL,
                });
            }*/
            var regionsDTO = _mapper.Map<List<RegionDTO>>(regions);
            return Ok(regionsDTO);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> getById([FromRoute] Guid id) 
        {
            //Find method can only be used with primary key
            //var region = _dbContext.Regions.Find(id);

            //First or default can be used with any field of data row 
            var region = await _regionInterface.GetById(id);

            if(region == null)
            {
                return NotFound();
            }
            else
            {
                /*var regionDTO = new RegionDTO
                {
                    Id = region.Id,
                    Code = region.Code,
                    ImageURL = region.ImageURL,
                    Name = region.Name,
                };*/
                return Ok(_mapper.Map<RegionDTO>(region));
            }
        }

        [HttpPost]
        [Authorize(Roles = "Writer")]
        //By using below ValidateModel annotation there is no need to explicitly check isValid and return bad request it will handle by Customer Validaor.
        //We have keep it here just for reference
        [ValidateModel]
        public async Task<IActionResult> addRegion([FromBody] AddRegionRequestDTO requestDTO)
        {
            if(ModelState.IsValid)
            {
                /*var regionDomainModel = new Region
            {
                Code = requestDTO.Code,
                Name = requestDTO.Name,
                ImageURL = requestDTO.ImageURL,
            };*/

                var regionDomainModel = _mapper.Map<Region>(requestDTO);

                regionDomainModel = await _regionInterface.Create(regionDomainModel);

                /*var regionDTO = new RegionDTO
                {
                    Id = regionDomainModel.Id,
                    Name = regionDomainModel.Name,
                    Code = regionDomainModel.Code,
                    ImageURL = regionDomainModel.ImageURL,
                };*/

                var regionDTO = _mapper.Map<RegionDTO>(regionDomainModel);

                return CreatedAtAction(nameof(getById), new { id = regionDomainModel.Id }, regionDomainModel);
            }
            else
            {
                return BadRequest();
            }
            
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> updateRegion([FromRoute] Guid id, [FromBody] UpdateRegionRequestDTO updateRegionRequestDTO)
        {
            var updateRegionDomainModel = new Region
            {
                Code = updateRegionRequestDTO.Code,
                Name = updateRegionRequestDTO.Name,
                ImageURL = updateRegionRequestDTO.ImageURL,
            };
            var regionDomainModel = await _regionInterface.Update(id, updateRegionDomainModel);
            if(regionDomainModel == null)
            {
                return NotFound();
            }

            else
            {
                var regionDTO = new RegionDTO
                {
                    Id = regionDomainModel.Id,
                    Name = regionDomainModel.Name,
                    Code = regionDomainModel.Code,
                    ImageURL = regionDomainModel.ImageURL,
                };

                return Ok(regionDTO);
            }
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> deleteRegion([FromRoute] Guid id)
        {
            var regionDomainModel = await _regionInterface.Delete(id);
            if(regionDomainModel == null)
            {
                return NotFound();
            }
            return Ok(regionDomainModel);
        }
    }
}
