using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentApi.Models;
using StudentApi.Repositories;
using StudentApi.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentApi.Controllers
{
    [ApiController]
	[Route("api/[controller]")]
    [Authorize]
    public class StudentsController : ControllerBase
	{
		private readonly StudentRepository _studentRepository;

		public StudentsController(StudentRepository studentRepository)
		{
			_studentRepository = studentRepository;
		}

        /// <summary>
        /// Get all students
        /// </summary>
        /// <returns>List of students</returns>
        [HttpGet]
		public async Task<ActionResult<IEnumerable<Student>>> GetStudents([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
		{
			var students = await _studentRepository.GetAllAsync();
            var pagedStudents = students
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var totalRecords = students.Count();

            var response = new
            {
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Data = pagedStudents
            };

            return Ok(response);
        }

        /// <summary>
        /// Get a specific student by id
        /// </summary>
        /// <param name="id">Student id</param>
        /// <returns>Student object</returns>
        [HttpGet("{id}")]
		public async Task<ActionResult<Student>> GetStudent(int id)
		{
			var student = await _studentRepository.GetByIdAsync(id);
			if (student == null)
			{
				return NotFound();
			}
			return Ok(student);
		}

        /// <summary>
        /// Create a new student
        /// </summary>
        /// <param name="student">Student object</param>
        /// <returns>Newly created student object</returns>
        [HttpPost]
		public async Task<ActionResult<Student>> PostStudent(CreateStudentDto createStudentDto)
		{
            var student = new Student
            {
                Nome = createStudentDto.Nome,
                Idade = createStudentDto.Idade,
                Serie = createStudentDto.Serie,
                NotaMedia = createStudentDto.NotaMedia,
                Endereco = createStudentDto.Endereco,
                NomePai = createStudentDto.NomePai,
                NomeMae = createStudentDto.NomeMae,
                DataNascimento = createStudentDto.DataNascimento
            };

            var createdStudent = await _studentRepository.AddAsync(student);
			return CreatedAtAction(nameof(GetStudent), new { id = createdStudent.Id }, createdStudent);
		}

        /// <summary>
        /// Update an existing student partially
        /// </summary>
        /// <param name="id">Student id</param>
        /// <param name="studentUpdateDto">Updated student object with partial fields</param>
        /// <returns>No content</returns>
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchStudent(int id, StudentUpdateDto studentUpdateDto)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            if (studentUpdateDto.Nome != null)
            {
                student.Nome = studentUpdateDto.Nome;
            }
            if (studentUpdateDto.Idade.HasValue)
            {
                student.Idade = studentUpdateDto.Idade.Value;
            }
            if (studentUpdateDto.Serie.HasValue)
            {
                student.Serie = studentUpdateDto.Serie.Value;
            }
            if (studentUpdateDto.NotaMedia.HasValue)
            {
                student.NotaMedia = studentUpdateDto.NotaMedia.Value;
            }
            if (studentUpdateDto.Endereco != null)
            {
                student.Endereco = studentUpdateDto.Endereco;
            }
            if (studentUpdateDto.NomePai != null)
            {
                student.NomePai = studentUpdateDto.NomePai;
            }
            if (studentUpdateDto.NomeMae != null)
            {
                student.NomeMae = studentUpdateDto.NomeMae;
            }
            if (studentUpdateDto.DataNascimento.HasValue)
            {
                student.DataNascimento = studentUpdateDto.DataNascimento.Value;
            }

            await _studentRepository.UpdateAsync(student);
            return NoContent();
        }

        /// <summary>
        /// Delete a student
        /// </summary>
        /// <param name="id">Student id</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
		public async Task<IActionResult> DeleteStudent(int id)
		{
			await _studentRepository.DeleteAsync(id);
			return NoContent();
		}
	}
}