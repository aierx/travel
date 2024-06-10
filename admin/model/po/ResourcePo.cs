using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace webapi.model.po;

[Index("Sort")]
public class ResourcePo : EntityPo
{
    [MaxLength(255)] public string fileOrginname { get; set; } = null!;

    [MaxLength(255)] public string fileupName { get; set; } = null!;

    [MaxLength(255)] public string contentType { set; get; } = null!;

    [MaxLength(255)] public string fileExtention { set; get; } = null!;

    [MaxLength(255)] public string Tag { get; set; } = null!;

    public int? Sort { get; set; }
}