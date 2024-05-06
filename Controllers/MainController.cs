using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MainController : ControllerBase
    {

        private readonly AppDBContext _dbContext;

        public MainController(AppDBContext dBContext)
        {
            _dbContext = dBContext;
        }

        [HttpPost]
        [Route("InsertEmployee")]
        public async Task<IActionResult> PostEmployee(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (!EmployeeExists(employee.IdEmployee))
                {
                    _dbContext.Employee.Add(employee);
                    await _dbContext.SaveChangesAsync();
                    return CreatedAtAction(nameof(GetEmployee), new { id = employee.IdEmployee }, employee);
                }
                else
                {
                    return BadRequest("The employee id already exists");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetEmployeeId")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            try
            {
                var employee = await _dbContext.Employee.FindAsync(id);

                if (employee == null)
                {
                    return NotFound();
                }

                return Ok(employee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetAllEmployee")]
        public async Task<IActionResult> GetAllEmployee()
        {
            try
            {
                var Employee = await _dbContext.Employee.ToListAsync();

                if (Employee == null || Employee.Count == 0)
                {
                    return NotFound();
                }

                return Ok(Employee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("UpdateEmployee")]
        public async Task<IActionResult> UpdateEmployee(int id, Employee employee)
        {
            if (id != employee.IdEmployee)
            {
                return BadRequest("Employee ID mismatch");
            }

            try
            {
                _dbContext.Entry(employee).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                return Ok(employee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private bool EmployeeExists(int id)
        {
            return _dbContext.Employee.Any(e => e.IdEmployee == id);
        }

        [HttpDelete]
        [Route("DeleteEmployee")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                var employee = await _dbContext.Employee.FindAsync(id);

                if (employee == null)
                {
                    return BadRequest("Employee id does not exist");
                }

                _dbContext.Employee.Remove(employee);
                await _dbContext.SaveChangesAsync();

                return Ok(true);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
