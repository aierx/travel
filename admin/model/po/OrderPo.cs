using System.ComponentModel.DataAnnotations;

namespace webapi.model.po;

public class OrderPo : EntityPo
{
    public long userId { get; set; } = 0;
    public long productId { get; set; } = 0;
    [MaxLength(255)] public string name { get; set; } = null!;
    public double? price { get; set; } = null;
    [MaxLength(255)] public string validDate { get; set; } = null!;
    [MaxLength(255)] public string status { get; set; } = null!;
    [MaxLength(255)] public string aliasName { set; get; } = null!;
    public int count { get; set; } = 0;

    [MaxLength(255)] public string type { get; set; } = null!;
}