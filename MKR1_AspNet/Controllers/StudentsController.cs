using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MKR1_AspNet.DTOs;
using MKR1_AspNet.Entities;

namespace MKR1_AspNet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StudentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentReadDto>>> GetAll()
        {
            return await _context.Students
                .Include(s => s.Classroom)
                .Select(s => new StudentReadDto(s.Id, s.FullName, s.Age, s.Classroom!.Name))
                .ToListAsync();
        }

        // GET: api/students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentReadDto>> GetById(int id)
        {
            var student = await _context.Students
                .Include(s => s.Classroom)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null) return NotFound();

            return new StudentReadDto(student.Id, student.FullName, student.Age, student.Classroom!.Name);
        }

        // POST: api/students
        [HttpPost]
        public async Task<ActionResult> Create(StudentCreateDto dto)
        {
            var student = new Student
            {
                FullName = dto.FullName,
                Age = dto.Age,
                ClassroomId = dto.ClassroomId
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = student.Id }, dto);
        }

        // PUT: api/students/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, StudentCreateDto dto)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound();

            student.FullName = dto.FullName;
            student.Age = dto.Age;
            student.ClassroomId = dto.ClassroomId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound();

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
