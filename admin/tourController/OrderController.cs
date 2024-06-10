using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.model.po;

namespace webapi.tourController;

[Route("tour/order")]
[Authorize]
public class OrderController
{
    private readonly AppDbContext _db;

    public OrderController(AppDbContext db)
    {
        _db = db;
    }

    [HttpPost("pay")]
    public BaseResult SubmitPay([FromBody] OrderPo orderPo)
    {
        var user = _db.User.FirstOrDefault(e => e.Id == orderPo.userId);
        if (user.balance==null|| orderPo.price > user.balance)
        {
            return BaseResult.Error("余额不足");
        }

        user.balance -= orderPo.price;
        _db.User.Update(user);
        _db.order.Add(orderPo);
        _db.SaveChanges();
        return BaseResult.Ok("成功");
    }

    [HttpPost("query")]
    public List<OrderPo> QueryOrder([FromBody] OrderPo orderPo)
    {
        List<OrderPo> orderPos = _db.order
            .Where(e => orderPo.productId == 0 || e.productId == orderPo.productId)
            .Where(e => orderPo.userId == 0 || e.userId == orderPo.userId)
            .ToList();
        return orderPos;
    }

    [HttpPost("update")]
    public BaseResult Update([FromBody] OrderPo orderPo)
    {
        OrderPo? po = _db.order.FirstOrDefault(e => e.Id == orderPo.Id);
        if (po == null)
        {
            return BaseResult.Error("不存在");
        }
        po.status = orderPo.status;
        UserPo? user = _db.User.FirstOrDefault(e => e.Id == po.userId);
        user!.balance += po.price;
        _db.User.Update(user);
        _db.SaveChanges();
        return BaseResult.Ok("ok");
    }
}