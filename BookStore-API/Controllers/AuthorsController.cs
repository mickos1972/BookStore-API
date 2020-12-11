using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookStore_API.Data.DTOs;
using BookStore_API.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore_API.Controllers
{
    /// <summary>
    /// Endpoint to interact with the authors in the book store's database
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorRepository _authRepo;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;

        public AuthorsController(IAuthorRepository authoRepo, ILoggerService logger, IMapper mapper)
        {
            _authRepo = authoRepo;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all authors
        /// </summary>
        /// <returns>List of Authors</returns>
        [HttpGet]
        public async Task<IActionResult> GetAuthors()
        {
            try
            {
                var authors = await _authRepo.FindAll();
                var response = _mapper.Map<List<AuthorDTO>>(authors);

                return Ok(response);
            }
            catch (Exception ex)
            {
                //_logger.LogDebug(ex.Message);

                return StatusCode(500, $"Somethings not right .. {ex.Message}");
            }            
        }
    }
}