using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.model.po;

namespace webapi.model.vo;

[Route("tour/properties")]
[Authorize]
public class PropertiesController :Controller
{
    private readonly AppDbContext _db;

    public PropertiesController(AppDbContext db)
    {
        _db = db;
    }

    [Route("save")]
    [Authorize(Roles = "admin")]
    public BaseResult Save([FromBody] PropertiesPo po)
    {
        _db.properties.Add(po);
        _db.SaveChanges();
        return BaseResult.Ok("ok");
    }

    [Route("query/{name}")]
    [AllowAnonymous]
    public BaseResult Query(string name)
    {
        
        var po = _db.properties.OrderByDescending(e => e.CreateTime).FirstOrDefault(e => e.name == name);
        if (po == null)
        {
            
            return BaseResult.Error("不存在") ;
        }
        return BaseResult.Ok(po.value);
    }
}