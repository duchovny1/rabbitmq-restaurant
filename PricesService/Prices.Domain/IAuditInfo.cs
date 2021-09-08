using System;

namespace Prices.Domain
{
    public interface IAuditInfo
    {
        DateTime CreatedOn { get; set; }
        DateTime? ModifiedOn { get; set; }
    }
}
