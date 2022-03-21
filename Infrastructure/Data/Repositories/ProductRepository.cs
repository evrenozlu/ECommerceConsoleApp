using Domain.Products;

namespace Infrastructure.Data.Repositories
{
    public class ProductRepository : RepositoryBase<Product>
        , IProductRepository
    {
        public ProductRepository(EFContext dbContext) : base(dbContext)
        {
        }
    }
}
