using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.model.po;

namespace webapi.tourController;

[Route("tour/scenery")]
[Authorize]
public class SceneryController
{
    private readonly AppDbContext _db;
    
    public SceneryController(AppDbContext db)
    {
        _db = db;
    }

    [HttpPost("save")]
    [Authorize(Roles = "admin")]
    public BaseResult Save([FromBody] SceneryPo sceneryPo)
    {
        if (sceneryPo.Id==0&&_db.scenery.FirstOrDefault(e => e.name == sceneryPo.name)!=null)
        {
            return BaseResult.Error("已存在");
        }
        try
        {
            if (sceneryPo.Id == 0 )
            {
                sceneryPo.status = false;
                if (sceneryPo.product == null)
                {
                    sceneryPo.product = new List<ProductPo>();
                }
                _db.scenery.Add(sceneryPo);
            }
            else
            {
                _db.scenery.Update(sceneryPo);
            }
            _db.SaveChanges();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BaseResult.Error(e.Message);
        }

        return BaseResult.Ok("ok");
    }

    [HttpPost("list")]
    [AllowAnonymous]
    public List<SceneryPo> List([FromBody] SceneryPo sceneryPo)
    {
        var citys = sceneryPo.attr1?.Split(";")!;
        var data = (from a in _db.scenery
            where citys == null || citys.Contains(a.city)
            where sceneryPo.attr2 == null || (sceneryPo.attr2 == "hotel" ? a.isHotel : !a.isHotel)
            where sceneryPo.attr3 == null || a.status
            select new { scenery = a, commentCount = _db.comment.Count(e => a.Id == e.sceneryId) }).ToList();
        return data.Select(e =>
        {
            e.scenery.commentCount = e.commentCount;
            return e.scenery;
        }).ToList();
    }

    [HttpPost("reversalStatus")]
    [Authorize(Roles = "admin")]
    public BaseResult ReversalStatus([FromBody] SceneryPo sceneryPo)
    {
        var po = _db.scenery.FirstOrDefault(e => e.Id == sceneryPo.Id)!;
        po.status = !po.status;

        _db.scenery.Update(po);
        _db.SaveChanges();
        return BaseResult.Ok("ok");
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public SceneryPo QueryById(int id)
    {
        var po = _db.scenery.FirstOrDefault(e => e.Id == id);
        if (po == null)
        {
            return new SceneryPo();
        }
        return po;
    }


    [HttpPost("delete")]
    [Authorize(Roles = "admin")]
    public BaseResult Delete([FromBody] List<long> id)
    {
        var po = _db.scenery.Where(e => id.Contains(e.Id)).ToList();
        _db.scenery.RemoveRange(po);
        _db.SaveChanges();
        return BaseResult.Ok("ok");
    }
}