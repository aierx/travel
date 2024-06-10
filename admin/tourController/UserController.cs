using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.model.po;
using webapi.model.vo;
using webapi.util;

namespace webapi.tourController;

[Route("tour/user")]
[Authorize]
public class UserController : Controller
{
    private readonly AppDbContext _db;

    private static readonly IHashPassword HashPassword = new Sha512HashPassword();

    public UserController(AppDbContext db)
    {
        _db = db;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public BaseResult Login([FromBody] LoginUserVo loginUserVo)
    {
        UserPo? po = _db.User.FirstOrDefault(e => e.name == loginUserVo.phone || e.phone == loginUserVo.phone);
        if (po == null || !HashPassword.Validate(loginUserVo.Password, po.passwd!))
        {
            return BaseResult.Error("账号或密码错误");
        }

        var identity = new ClaimsIdentity("Basic");
        var claim = new Claim("userContext", "user");
        claim.Properties.Add(new KeyValuePair<string, string>("userId", po.Id.ToString()));
        identity.AddClaim(claim);
        if (po.isAdmin)
        {
            identity.AddClaim(new Claim(ClaimsIdentity.DefaultRoleClaimType, "admin"));
        }

        HttpContext.SignInAsync(new ClaimsPrincipal(identity));
        return BaseResult.Ok(po);
    }

    [HttpPost("logout")]
    public BaseResult Login()
    {
        HttpContext.SignOutAsync();
        return BaseResult.Ok("ok");
    }

    [HttpPost("save")]
    [AllowAnonymous]
    public BaseResult Save([FromBody] UserPo userPo)
    {
        if (userPo.Id != 0)
        {
            var originName = _db.User.Where(e => e.Id == userPo.Id).Select(e=>e.name).FirstOrDefault();
            if (originName != userPo.name && _db.User.FirstOrDefault(e => e.name == userPo.name) != null)
            {
                return BaseResult.Error("用户已存在");
            }
            _db.Update(userPo);
        }
        else
        {
            var user = _db.User.FirstOrDefault(e => e.name == userPo.name);
            if (user != null)
            {
                return BaseResult.Error("用户已存在");
            }

            UserPo po = _db.User.OrderByDescending(e => e.Id).ToList().FirstOrDefault(new UserPo { Id = 0 });
            var po1 = po.Id + 1000 + 1;
            userPo.userNo = "U" + po1;
            var salt = HashPassword.GenerateSalt();
            var passwd = HashPassword.Generate(salt, userPo.passwd!);
            userPo.passwd = passwd;
            _db.Add(userPo);
        }

        _db.SaveChanges();
        return BaseResult.Ok("注册成功");
    }

    [HttpPost("list")]
    [Authorize(Roles = "admin")]
    public List<UserPo> List()
    {
        var pos = _db.User.ToList();
        return pos;
    }

    [HttpPost("modify")]
    [Authorize(Roles = "admin")]
    public BaseResult Modify([FromBody] UserPo userPo)
    {
        var isExit = _db.User.FirstOrDefault(e => e.Id == userPo.Id);
        if (isExit == null)
        {
            return BaseResult.Error("用户不存在");
        }

        isExit.name = userPo.name;
        isExit.aliasName = userPo.aliasName;
        isExit.phone = userPo.phone;
        isExit.email = userPo.email;
        isExit.address = userPo.address;
        isExit.balance = userPo.balance;
        isExit.isAdmin = userPo.isAdmin;
        _db.User.Update(isExit);
        _db.SaveChanges();
        return BaseResult.Ok("修改成功");
    }

    [HttpPost("modifyPassword")]
    [Authorize(Roles = "admin")]
    public BaseResult ModifyPassword([FromBody] UserPo userPo)
    {
        var isExit = _db.User.FirstOrDefault(e => e.Id == userPo.Id);
        if (isExit == null)
        {
            return BaseResult.Error("用户不存在");
        }

        var salt = HashPassword.GenerateSalt();
        string hashPassWord = HashPassword.Generate(salt, userPo.newPasswd!);
        isExit.passwd = hashPassWord;
        _db.User.Update(isExit);
        _db.SaveChanges();
        return BaseResult.Ok("修改成功");
    }

    [HttpPost("reversalAdmin")]
    [Authorize(Roles = "admin")]
    public BaseResult ReversalAdmin([FromBody] UserPo userPo)
    {
        var po = _db.User.FirstOrDefault(e => e.Id == userPo.Id)!;
        po.isAdmin = !po.isAdmin;

        _db.User.Update(po);
        _db.SaveChanges();
        return BaseResult.Ok("ok");
    }


    [HttpPost("modifyByUser")]
    public BaseResult ModifyByUser([FromBody] UserPo userPo)
    {
        var id = 0L;
        try
        {
            var claim = HttpContext.User.Claims.FirstOrDefault(e => e.Type == "userContext");
            id = long.Parse(claim!.Properties["userId"]);
        }
        catch (Exception)
        {
            return BaseResult.Error("请登入");
        }

        var isExit = _db.User.FirstOrDefault(e => e.Id == id);
        if (isExit == null)
        {
            return BaseResult.Ok("用户不存在");
        }

        isExit.name = userPo.name;
        isExit.sex = userPo.sex;
        isExit.birthDate = userPo.birthDate;
        isExit.aliasName = userPo.aliasName;
        isExit.phone = userPo.phone;
        isExit.email = userPo.email;
        isExit.address = userPo.address;
        _db.User.Update(isExit);
        _db.SaveChanges();
        return BaseResult.Ok("修改成功");
    }

    [HttpPost("modifyPasswordByUser")]
    public BaseResult ModifyPasswordByUser([FromBody] UserPo userPo)
    {
        var id = 0L;
        try
        {
            var claim = HttpContext.User.Claims.FirstOrDefault(e => e.Type == "userContext");
            id = long.Parse(claim!.Properties["userId"]);
        }
        catch (Exception)
        {
            return BaseResult.Error("请登入");
        }

        var isExit = _db.User.FirstOrDefault(e => e.Id == id);
        if (isExit == null)
        {
            return BaseResult.Error("用户不存在");
        }

        if (!HashPassword.Validate(userPo.passwd!, isExit.passwd!))
        {
            return BaseResult.Error("密码不正确");
        }

        var salt = HashPassword.GenerateSalt();
        string hashPassWord = HashPassword.Generate(salt, userPo.newPasswd!);
        isExit.passwd = hashPassWord;
        _db.User.Update(isExit);
        _db.SaveChanges();
        return BaseResult.Ok("修改成功");
    }


    [HttpGet("info")]
    public BaseResult QueryUserById()
    {
        var claim = HttpContext.User.Claims.FirstOrDefault(e => e.Type == "userContext");
        var id = long.Parse(claim!.Properties["userId"]);

        var user = _db.User.FirstOrDefault(e => e.Id == id);
        return BaseResult.Ok(user!);
    }
    
    [HttpPost("charge")]
    public BaseResult Charge([FromBody] ChargeInfo chargeInfo)
    {
        var claim = HttpContext.User.Claims.FirstOrDefault(e => e.Type == "userContext");
        var id = long.Parse(claim!.Properties["userId"]);
        var user = _db.User.FirstOrDefault(e => e.Id == id);
        user!.balance += chargeInfo.count;
        _db.User.Update(user);
        _db.SaveChanges();
        return BaseResult.Ok("充值成功");
    }
}