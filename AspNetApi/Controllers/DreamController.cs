using Microsoft.AspNetCore.Mvc;
using DreamComeTrueApi.Models;
using EntityFramework.Exceptions.Common;

namespace DreamComeTrueApi.Controllers;

[ApiController]
[Route("[controller]")]
public class DreamController : ControllerBase
{
    private readonly DreamComeTrueContext _dbContext;
    public DreamController(DreamComeTrueContext dbContext)
    {
       _dbContext = dbContext;
    }

    [HttpGet("GetAll")]
    public IActionResult GetAll()
    { 
        var result = _dbContext.Dreams.ToList();
       return Ok(result);
    }
    [HttpGet("GetById/{id:guid}")]
    public IActionResult GetById(Guid id)
    { 
        var result = _dbContext.Dreams.FirstOrDefault(d=>d.Id == id);
        return Ok(result);
    }

    [HttpPost]
    public IActionResult Create(string dreamName)
    {
        try
        {
            var dream = new Dream();
            dream.Id = Guid.NewGuid();
            dream.Name = null;//dreamName;
            dream.Goals = new List<Goal>();
            _dbContext.Dreams.Add(dream);
            _dbContext.SaveChanges();
            return Ok(true);
        }
        catch (CannotInsertNullException e)
        {
            return BadRequest(e);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }

    [HttpDelete("remove/{id:guid}")]
    public IActionResult Remove(Guid id)
    { 
        var result = _dbContext.Dreams.FirstOrDefault(d=>d.Id == id);
        if (result != null)
        {
            _dbContext.Remove(result);
            _dbContext.SaveChanges();
            return Ok(true);
        }

        return Ok(false);
    }
}
