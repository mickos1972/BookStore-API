using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;
using AutoMapper;
using BookStore_API.Data;
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
                return InternalError($"{ex.Message} - {ex.InnerException}");
            }
        }

        /// <summary>
        /// Get single author by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Author</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAuthor(int id)
        {
            try
            {
                var author = await _authRepo.FindById(id);

                if (author == null)
                {
                    return NotFound();
                }

                var response = _mapper.Map<AuthorDTO>(author);

                return Ok(author);

            }
            catch (Exception ex)
            {
                return InternalError($"{ex.Message} - {ex.InnerException}");
            }
        }

        /// <summary>
        /// Create a new author
        /// </summary>
        /// <param name="author"></param>
        /// <returns>201 created</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] AuthorCreateDTO authorDTO)
        {
            try
            {
                if (authorDTO == null || !ModelState.IsValid)
                    return BadRequest(ModelState);

                //Map from the dto to the entity
                var author = _mapper.Map<Author>(authorDTO);

                //Do an insert
                var isSuccess = await _authRepo.Create(author);

                if (!isSuccess)
                {
                    return InternalError($"Author creation failed");
                }

                return Created("Create", new { author });

            }
            catch (Exception ex)
            {
                return InternalError($"{ex.Message} - {ex.InnerException}");
            }
        }

        /// <summary>
        /// Update existing author
        /// </summary>
        /// <param name="id"></param>
        /// <param name="author"></param>
        /// <returns>200 Updated</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] AuthorUpdateDTO authorDTO)
        {
            try
            {
                if (id < 1 || authorDTO == null || !ModelState.IsValid || id != authorDTO.Id)
                    return BadRequest(ModelState);
               
                if (!await _authRepo.IsExists(id))
                    return NotFound();

                //Map from the dto to the entity
                var author = _mapper.Map<Author>(authorDTO);

                //Do an insert
                var isSuccess = await _authRepo.Update(author);

                if (!isSuccess)
                {
                    return InternalError($"Author update failed");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return InternalError($"Update Failed: {ex.Message} - {ex.InnerException}");
            }
        }

        /// <summary>
        /// Delete author
        /// </summary>
        /// <param name="id"></param>
        /// <returns>200 Deleted</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id < 1)
                    return BadRequest(ModelState);

                var author = await _authRepo.FindById(id);

                if (author == null)
                    return NotFound();

                var isSuccess = await _authRepo.Delete(author);

                if (!isSuccess)
                {
                    return InternalError($"Author update failed");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return InternalError($"Update Failed: {ex.Message} - {ex.InnerException}");
            }
        }

        private ObjectResult InternalError(string message)
        {
            _logger.LogError(message);
            return StatusCode(500, $"Somethings not right .. {message}");
        }
    }
}