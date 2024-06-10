using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace webapi.model.po;

public class SceneryPo : EntityPo
{
    [MaxLength(255)] public string? name { get; set; } = null!;

    [MaxLength(255)] public string? type { get; set; } = null!;

    [MaxLength(255)] public string? grade { get; set; } = null!;
    [MaxLength(255)] public string? isCharge { get; set; } = null!;
    [MaxLength(255)] public string? phone { get; set; } = null!;
    [MaxLength(255)] public string? email { get; set; } = null!;

    [MaxLength(255)] public string? summary { get; set; } = null!;
    [MaxLength(255)] public string? province { get; set; } = null!;
    [MaxLength(255)] public string? city { get; set; } = null!;
    [MaxLength(255)] public string? area { get; set; } = null!;

    [MaxLength(255)] public string? address { get; set; } = null!;

    public List<ProductPo>? product { get; set; } =new();
    [MaxLength(800)] public List<ImagePo>? imageList { get; set; } = new();

    public bool status { get; set; }

    [MaxLength(255)] public string score { get; set; } = null!;

    [Column(TypeName = "longtext")]
    [MaxLength(int.MaxValue)]
    public string html { get; set; } = null!;

    public bool isHotel { get; set; } = false;

    [NotMapped] public int commentCount { get; set; } = 0;
    [NotMapped] [JsonIgnore] public string attr1 { get; set; } = null!;
    [NotMapped] [JsonIgnore] public string attr2 { get; set; } = null!;
    [NotMapped] [JsonIgnore] public string attr3 { get; set; } = null!;
}