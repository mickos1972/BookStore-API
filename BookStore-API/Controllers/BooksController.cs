using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Policy;
using System.Threading.Tasks;
using AutoMapper;
using BookStore_API.Data;
using BookStore_API.Data.DTOs;
using BookStore_API.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookStore_API.Controllers
{
    /// <summary>
    /// Endpoint to interact with the books in the book store's database
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepo;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;

        public BooksController(IBookRepository bookRepo, IMapper mapper, ILoggerService logger)
        {
            _bookRepo = bookRepo;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get all Books
        /// </summary>
        /// <returns>List of books</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBooks()
        {
            var location = GetControllerActionNames();

            try
            {
                var books = await _bookRepo.FindAll();
                var response = _mapper.Map<List<BookDTO>>(books);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return InternalError($"{location}: {ex.Message} - {ex.InnerException}");
            }
        }

        /// <summary>
        /// Get book by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>found or not</returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBook(int id)
        {
            var location = GetControllerActionNames();

            try
            {
                var book = await _bookRepo.FindById(id);

                if (book == null)
                    return NotFound();

                var response = _mapper.Map<BookDTO>(book);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return InternalError($"{location}: {ex.Message} - {ex.InnerException}");
            }
        }

        /// <summary>
        /// Creates a new book
        /// </summary>
        /// <param name="bookDto"></param>
        /// <returns>Book Object</returns>
        [HttpPost("book:BookDTO")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] BookCreateDTO bookDto)
        {
            var location = GetControllerActionNames();

            try
            {
                if (bookDto == null || !ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var book = _mapper.Map<Book>(bookDto);
                var success = await _bookRepo.Create(book);

                if (!success)
                    return InternalError($"{location}: book not created");

                return Created("Create", new { book });

            }
            catch (Exception ex)
            {
                return InternalError($"{location}: {ex.Message} - {ex.InnerException}");
            }
        }

        /// <summary>
        /// Update book entity
        /// </summary>
        /// <param name="id"></param>
        /// <param name="BookUpdateDto"></param>
        /// <returns>200 OK</returns>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] BookUpdateDTO bookUpdateDto)
        {
            var location = GetControllerActionNames();

            try
            {
                if (bookUpdateDto == null || !ModelState.IsValid || id != bookUpdateDto.Id)
                {
                    return BadRequest(ModelState);
                }

                var isExists = await _bookRepo.IsExists(id);
                if (!isExists)
                    return NotFound();

                var book = _mapper.Map<Book>(bookUpdateDto);
                var success = await _bookRepo.Update(book);

                if (!success)
                    return InternalError($"{location}: book not created");

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalError($"{location}: {ex.Message} - {ex.InnerException}");
            }
        }

        /// <summary>
        /// Delete a book
        /// </summary>
        /// <param name="id"></param>
        /// <returns>OK</returns>        
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            var location = GetControllerActionNames();

            try
            {
                var isExists = await _bookRepo.IsExists(id);

                if (id < 1 || !isExists)
                    return NotFound();

                var book = await _bookRepo.FindById(id);
                var isSuccess = await _bookRepo.Delete(book);

                if (!isSuccess)
                    return InternalError($"{location}: book not deleted");

                return Ok();

            }
            catch (Exception ex)
            {
                return InternalError($"{location}: {ex.Message} - {ex.InnerException}");
            }
        }

        private string GetControllerActionNames()
        {
            var controller = ControllerContext.ActionDescriptor.ControllerName;
            var action = ControllerContext.ActionDescriptor.ActionName;

            return $"{controller} - {action}";
        }

        private ObjectResult InternalError(string message)
        {
            _logger.LogError(message);
            return StatusCode(500, $"Somethings not right .. {message}");
        }
    }
}
