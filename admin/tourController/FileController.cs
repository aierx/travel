using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using webapi.model.po;
using webapi.util;
using MediaTypeHeaderValue = Microsoft.Net.Http.Headers.MediaTypeHeaderValue;

namespace webapi.Controllers;

[Route("file")]
public class FileController : Controller
{
    private AppDbContext _db;

    private string directory = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "myupfiles" +
                               Path.DirectorySeparatorChar;

    public FileController(AppDbContext db)
    {
        _db = db;
    }

    [HttpPost("upload")]
    public async Task<IResult> Upload([FromForm] IFormCollection formcollection)
    {
        string tag = "image/jpg";
        if (formcollection.Files.Count == 0)
        {
            return Results.BadRequest("文件不能为空");
        }

        List<ResourcePo> list = new List<ResourcePo>();
        var order = _db.Resource.Where(e => e.Tag == tag).OrderByDescending(e => e.Sort).AsEnumerable()
            .Select(e => e.Sort).FirstOrDefault(0);
        string filename = "";
        foreach (var file in formcollection.Files)
        {
            string contentType = file.ContentType;
            string fileOrginname = file.FileName; //新建文本文档.txt  原始的文件名称

            string fileExtention = Path.GetExtension(fileOrginname);
            string cdipath = Directory.GetCurrentDirectory();

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string fileupName = Guid.NewGuid().ToString("d") + fileExtention;
            string upfilePath = Path.Combine(directory, fileupName);
            if (!System.IO.File.Exists(upfilePath))
            {
                using var stream = System.IO.File.Create(upfilePath);
            }

            double count = await UpLoadFileStreamHelper.UploadWriteFileAsync(file.OpenReadStream(), upfilePath);

            var resourcePo = new ResourcePo();

            resourcePo.fileOrginname = fileOrginname;
            resourcePo.contentType = contentType;
            resourcePo.fileExtention = fileExtention;
            if (String.IsNullOrWhiteSpace(filename))
            {
                filename = fileupName;
            }

            resourcePo.fileupName = fileupName;
            resourcePo.Sort = ++order;
            resourcePo.Tag = tag;
            list.Add(resourcePo);
        }

        _db.Resource.AddRange(list);
        _db.SaveChanges();

        return Results.Ok(HttpContext.Request.GetDisplayUrl().Replace("upload", filename));
    }

    [HttpGet("{filename}")]
    public Task<FileContentResult> Download(string filename)
    {
        var resourcePo = _db.Resource.OrderByDescending(e => e.CreateTime)
            .FirstOrDefault(e => e.fileupName == filename);
        if (resourcePo == null)
        {
            throw new ArgumentException("文件不存在");
        }

        string path = directory + resourcePo.fileupName;
        var bytes = System.IO.File.ReadAllBytes(path);
        var actionresult = new FileContentResult(bytes, new MediaTypeHeaderValue(resourcePo.contentType));


        actionresult.EnableRangeProcessing = true;

        return Task.FromResult(actionresult);
    }
}