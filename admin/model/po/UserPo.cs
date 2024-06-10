using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace webapi.model.po;

public class UserPo : EntityPo
{
    [MaxLength(255)] public string? userNo { get; set; } = null!;
    [MaxLength(255)] public string? name { get; set; } = null!;
    [MaxLength(255)] public string? aliasName { get; set; } = null!;
    [MaxLength(255)] public string? sex { get; set; }= null!;
    [MaxLength(255)] public string? birthDate { get; set; } = null!;
    [MaxLength(255)] public string? phone { get; set; } = null!;
    [MaxLength(255)] public string? email { get; set; } = null!;
    [MaxLength(255)] public string? address { get; set; } = null!;
    public double? balance { get; set; } = 0;
    public bool isAdmin { get; set; }
    [MaxLength(255)] public string? passwd { set; get; } = null!;
    [NotMapped]
    [MaxLength(255)] public string? newPasswd { set; get; } = null!;
    [JsonIgnore]
    [NotMapped]
    public byte[]? salt { get; set; } = null!;
}