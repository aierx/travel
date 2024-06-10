using System.ComponentModel.DataAnnotations;

namespace webapi.model.po;

public class CommentPo : EntityPo
{
    public long userId { get; set; }

    [MaxLength(255)] public string userName { get; set; } = null!;

    public long sceneryId { get; set; }

    [MaxLength(1000)] public string comment { get; set; } = null!;
    
    [MaxLength(100)] public string? filter { get; set; } = null!;

}