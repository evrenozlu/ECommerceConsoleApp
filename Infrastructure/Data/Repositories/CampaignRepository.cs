using Domain.Campaigns;
using Domain.Orders;

namespace Infrastructure.Data.Repositories
{
    public class CampaignRepository : RepositoryBase<Campaign>
        , ICampaignRepository
    {
        public CampaignRepository(EFContext dbContext) : base(dbContext)
        {
        }
    }
}