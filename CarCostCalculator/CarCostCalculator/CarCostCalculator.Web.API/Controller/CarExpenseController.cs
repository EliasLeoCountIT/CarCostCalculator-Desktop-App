using CarCostCalculator.Data.Contract;
using CarCostCalculator.Domain.Model.CarExpense;
using Microsoft.AspNetCore.Mvc;

namespace CarCostCalculator.Web.API.Controller;

[Route("api/[controller]")]
[ApiController]
public class CarExpenseController(ICarExpenseRepository repo) : ControllerBase
{
    #region Private Members

    private readonly ICarExpenseRepository _repo = repo;

    #endregion

    #region Public Methods

    // DELETE api/CarExpense/5
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            await _repo.Delete(id, cancellationToken);
            return Ok("Successfully deleted");
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    // GET: api/CarExpense
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CarExpenseChangeable>>> Get(
        CancellationToken cancellationToken = default)
    {
        var CarExpenses = await _repo.LoadAll(cancellationToken);
        return Ok(CarExpenses);
    }

    // GET api/CarExpense/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CarExpenseChangeable>> Get(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            var CarExpense = await _repo.LoadByPrimaryKey(id, cancellationToken);
            return Ok(CarExpense);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    // GET api/CarExpense/date/2025-12-06
    [HttpGet("date/{date}")]
    public async Task<ActionResult<CarExpenseChangeable>> GetByDate(DateOnly date, CancellationToken cancellationToken = default)
    {
        try
        {
            var carExpense = await _repo.LoadByDate(date, cancellationToken);
            return Ok(carExpense);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    // GET api/CarExpense/month/2025/12
    [HttpGet("month/{year}/{month}")]
    public async Task<ActionResult<IEnumerable<CarExpenseChangeable>>> GetByMonth(int year, int month, CancellationToken cancellationToken = default)
    {
        if (month < 1 || month > 12)
        {
            return BadRequest("Month must be between 1 and 12");
        }

        var carExpenses = await _repo.GetByMonth(year, month, cancellationToken);
        return Ok(carExpenses);
    }

    // GET api/CarExpense/range?startDate=2025-01-01&endDate=2025-03-31
    [HttpGet("range")]
    public async Task<ActionResult<IEnumerable<CarExpenseChangeable>>> GetByTimeRange(
        [FromQuery] DateOnly startDate,
        [FromQuery] DateOnly endDate,
        CancellationToken cancellationToken = default)
    {
        if (startDate > endDate)
        {
            return BadRequest("Start date must be before or equal to end date");
        }

        var carExpenses = await _repo.GetByTimeRange(startDate, endDate, cancellationToken);
        return Ok(carExpenses);
    }

    // GET api/CarExpense/year/2025
    [HttpGet("year/{year}")]
    public async Task<ActionResult<IEnumerable<CarExpenseChangeable>>> GetByYear(int year, CancellationToken cancellationToken = default)
    {
        var carExpenses = await _repo.GetByYear(year, cancellationToken);
        return Ok(carExpenses);
    }

    // POST api/CarExpense/ExcelFileImport
    [HttpPost("ExcelFileImport")]
    public async Task<ActionResult> ImportFromExcel(
        IFormFile file,
        CancellationToken cancellationToken = default)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded");
        }

        if (!file.FileName.EndsWith(".xlsx") && !file.FileName.EndsWith(".xls"))
        {
            return BadRequest("Only Excel files (.xlsx, .xls) are supported");
        }

        // Process Excel file here
        // You'll need a library like EPPlus or ClosedXML

        return Ok("File imported successfully");
    }

    // POST api/CarExpense
    [HttpPost]
    public async Task<ActionResult<CarExpenseChangeable>> Post(
        [FromBody] CarExpenseAddable CarExpenseAddable,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var CarExpense = await _repo.Add(CarExpenseAddable, cancellationToken);
            return CreatedAtAction(nameof(Get), new { id = CarExpense.Id }, CarExpense);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    // PUT api/CarExpense/5
    [HttpPut("{id}")]
    public async Task<ActionResult<CarExpenseChangeable>> Put(
        long id,
        [FromBody] CarExpenseChangeable CarExpenseChangeable,
        CancellationToken cancellationToken = default)
    {
        if (id != CarExpenseChangeable.Id)
        {
            return BadRequest("Id mismatch");
        }

        try
        {
            var updatedCarExpense = await _repo.Update(CarExpenseChangeable, cancellationToken);
            return Ok(updatedCarExpense);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    #endregion
}