using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.model.po;

namespace webapi.tourController;

[Route("tour/strategy")]
[Authorize]
public class StrategyController : Controller
{
    private readonly AppDbContext _db;

    public StrategyController(AppDbContext db)
    {
        _db = db;
    }

    [HttpPost("submit")]
    public BaseResult submit([FromBody] StrategyPo strategyPo)
    {
        _db.strategy.Add(strategyPo);
        _db.SaveChanges();
        return BaseResult.Ok("ok");
    }

    [HttpGet("{sceneryId}")]
    [AllowAnonymous]
    public BaseResult query(long sceneryId)
    {
        var pos = _db.strategy.Where(e => e.sceneryId == sceneryId);
        return BaseResult.Ok(pos);
    }

    [HttpGet("like/{sceneryId}")]
    [AllowAnonymous]
    public BaseResult findMeLike(long sceneryId)
    {
        var claim = HttpContext.User.Claims.FirstOrDefault(e => e.Type == "userContext");
        if (claim == null)
        {
            return BaseResult.Ok(new List<LikePo>());
        }

        var id = long.Parse(claim.Properties["userId"]);

        var likePos = _db.like.Where(e => e.sceneryId == sceneryId && e.userId == id);
        return BaseResult.Ok(likePos);
    }

    [HttpPost("like")]
    public BaseResult put([FromBody] LikePo likeVo)
    {
        var strategyPo = _db.strategy.FirstOrDefault(e => e.Id == likeVo.strategyId)!;
        var likePo = _db.like.FirstOrDefault(e => e.sceneryId == likeVo.sceneryId && e.userId == likeVo.userId);
        if (likePo == null)
        {
            strategyPo.likeCount++;
            _db.Add(likeVo);
        }
        else
        {
            strategyPo.likeCount--;
            _db.Remove(likePo);
        }

        _db.SaveChanges();
        return BaseResult.Ok("ok");
    }
}