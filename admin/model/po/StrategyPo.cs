using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapi.model.po;

public class StrategyPo : EntityPo
{
    
    public long userId { get; set; }

    [MaxLength(255)] public string userName { get; set; } = null!;

    public long sceneryId { get; set; }
    
    [Column(TypeName = "longtext")]
    [MaxLength(int.MaxValue)]
    public string html { get; set; } = null!;

    public int likeCount   { get; set; }= 0;
    
}