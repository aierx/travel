using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.model.po;

namespace webapi.tourController;

[Route("tour/comment")]
[Authorize]
public class CommentController
{
    private readonly AppDbContext _db;
    
    public CommentController(AppDbContext db)
    {
        _db = db;
    }
    
    [HttpGet("{sceneryId}")]
    [AllowAnonymous]
    public List<CommentPo> QueryBySceneryId( int sceneryId,[FromQuery]string filter)
    {
        if (filter==null)
        {
            return _db.comment.Where(e => e.sceneryId == sceneryId).ToList(); 
        }

        var split = filter.Split(",");
        return _db.comment.Where(e => e.sceneryId == sceneryId && split.Contains(e.filter)).ToList(); 
    }
    
    [HttpPost("save")]
    public BaseResult Save([FromBody]CommentPo commentPo)
    {
        _db.comment.Add(commentPo);
        _db.SaveChanges();
        return BaseResult.Ok("ok");
    }
}