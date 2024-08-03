using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentApi.Models;
using StudentApi.Repositories;
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

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
		{
			var students = await _studentRepository.GetAllAsync();
			return Ok(students);
		}

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

		[HttpPost]
		public async Task<ActionResult<Student>> PostStudent(Student student)
		{
			var createdStudent = await _studentRepository.AddAsync(student);
			return CreatedAtAction(nameof(GetStudent), new { id = createdStudent.Id }, createdStudent);
		}

		[HttpPut]
		public async Task<IActionResult> PutStudent(int id, Student student)
		{
			if (id != student.Id)
			{
				return BadRequest();
			}
			await _studentRepository.UpdateAsync(student);
			return NoContent();
		}

		[HttpDelete]
		public async Task<IActionResult> DeleteStudent(int id)
		{
			await _studentRepository.DeleteAsync(id);
			return NoContent();
		}
	}
}