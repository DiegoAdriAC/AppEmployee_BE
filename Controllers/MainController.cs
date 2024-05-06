using AppEmployee.Models;
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

        /* ---------- Users ---------- */
        private bool UserExists(string userName)
        {
            return _dbContext.User.Any(e => e.UserName == userName);
        }

        [HttpPost]
        [Route("InsertUser")]
        public async Task<IActionResult> PostUser([FromBody] User newUser)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (!UserExists(newUser.UserName))
                {
                    _dbContext.User.Add(newUser);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    return BadRequest("The user already exists");
                }

                return Ok(true);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("authentication")]
        public async Task<IActionResult> Authentication( string userName, string pasword)
        {
            try
            {
                var us = await _dbContext.User.FirstOrDefaultAsync(u => u.UserName == userName && u.Pasword == pasword);

                if (us == null)
                {
                    return NotFound();
                }

                return Ok(true);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        
        [HttpGet]
        [Route("GetUser")]
        public async Task<IActionResult> GetUser( string userName)
        {
            try
            {
                var us = await _dbContext.User.FirstOrDefaultAsync(u => u.UserName == userName);
                us.Pasword = "";
                if (us == null)
                {
                    return NotFound();
                }

                return Ok(us);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetAllUser")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _dbContext.User.ToListAsync();
                users.ForEach(u => u.Pasword = ""  );
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //[HttpPut]
        //[Route("UpdateUser")]
        //public async Task<IActionResult> UpdateUser(string userName, [FromBody] User user)
        //{
        //    if (userName != user.UserName)
        //    {
        //        return BadRequest("User mismatch");
        //    }

        //    try
        //    {
        //        if (UserExists(user.UserName))
        //        {
        //            _dbContext.Entry(user).State = EntityState.Modified;
        //            await _dbContext.SaveChangesAsync();
        //            return Ok(user);
        //        }
        //        else
        //        {
        //            return BadRequest("The user does not exists");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}

        [HttpDelete]
        [Route("DeleteUser")]
        public async Task<IActionResult> DeleteUser(string userName)
        {
            try
            {
                var user = await _dbContext.User.FindAsync(userName);

                if (user == null)
                {
                    return BadRequest("user does not exist");
                }

                _dbContext.User.Remove(user);
                await _dbContext.SaveChangesAsync();

                return Ok(true);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /* ---------- Employees ---------- */
        private bool EmployeeExists(int id)
        {
            return _dbContext.Employee.Any(e => e.IdEmployee == id);
        }

        [HttpPost]
        [Route("InsertEmployee")]
        public async Task<IActionResult> PostEmployee([FromBody] Employee employee)
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
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] Employee employee)
        {
            if (id != employee.IdEmployee)
            {
                return BadRequest("Employee ID mismatch");
            }

            try
            {
                if (EmployeeExists(employee.IdEmployee))
                {
                    _dbContext.Entry(employee).State = EntityState.Modified;
                    await _dbContext.SaveChangesAsync();
                    return Ok(employee);
                }
                else
                {
                    return BadRequest("The employee id does not exists");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
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
