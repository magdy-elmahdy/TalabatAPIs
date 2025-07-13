using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Repository.Data
{
    public class StoreContextSeed
    {
        public async static Task SeedAsync(StoreContext _dbContext) {
            // Brands Seed
            if (!_dbContext.ProductBrands.Any()) {
                var brandsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/brands.json");
                var Brnads = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                if (Brnads?.Count()>0)
                {
                    foreach (var item in Brnads)
                    {
                        _dbContext.Set<ProductBrand>().Add(item);
                        //_dbContext.ProductBrands.Add(item);

                    }
                }
                await _dbContext.SaveChangesAsync();
            }

            // Categories Seed
            if (_dbContext.Set<ProductCategory>().Count()==0)
            {
                var CategoriesData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/categories.json");
                var Categories = JsonSerializer.Deserialize<List<ProductCategory>>(CategoriesData);
                if (Categories?.Count() > 0)
                {
                    foreach (var item in Categories)
                    {
                        _dbContext.Set<ProductCategory>().Add(item);
                    }
                }
                await _dbContext.SaveChangesAsync();
            }

            // Products Seed
            if (_dbContext.Set<Product>().Count() == 0)
            {
                var ProductsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/products.json");
                var Products = JsonSerializer.Deserialize<List<Product>>(ProductsData);
                if (Products?.Count() > 0)
                {
                    foreach (var item in Products)
                    {
                        _dbContext.Set<Product>().Add(item);
                    }
                }
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
