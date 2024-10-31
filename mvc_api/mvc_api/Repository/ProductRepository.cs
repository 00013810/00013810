using Microsoft.EntityFrameworkCore;
using mvc_api.DBContexts;
using mvc_api.Model;

namespace mvc_api.Repository
{
    // Repository class for managing Product entities.
    public class ProductRepository : IProductRepository
    {
        private readonly ProductContext _dbContext; // Database context for accessing Product data

        // Constructor to initialize the ProductContext
        public ProductRepository(ProductContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Deletes a product from the database based on the product ID
        public void DeleteProduct(int productId)
        {
            // Find the product using the provided product ID
            var product = _dbContext.Products.Find(productId);
            // Remove the product from the context
            _dbContext.Products.Remove(product);
            // Save changes to the database
            Save();
        }

        // Retrieves a product by its ID, including its associated category
        public Product GetProductById(int productId)
        {
            // Find the product by ID
            var prod = _dbContext.Products.Find(productId);
            // Load the associated ProductCategory for the found product
            _dbContext.Entry(prod).Reference(s => s.ProductCategory).Load();
            return prod; // Return the found product
        }

        // Retrieves all products from the database, including their associated categories
        public IEnumerable<Product> GetProducts()
        {
            // Include ProductCategory in the results and return as a list
            return _dbContext.Products.Include(s => s.ProductCategory).ToList();
        }

        // Inserts a new product into the database
        public void InsertProduct(Product product)
        {
            // Check if the category already exists in the database
            var existingCategory = _dbContext.Categories
                                    .FirstOrDefault(c => c.Name == product.ProductCategory.Name);

            if (existingCategory != null)
            {
                // If the category exists, assign it to the product
                product.ProductCategory = existingCategory;
            }
            else
            {
                // Otherwise, add the new category along with the product
                _dbContext.Categories.Add(product.ProductCategory);
            }

            // Add the new product to the context
            _dbContext.Products.Add(product);
            // Save changes to the database
            Save();
        }

        // Saves all changes made in the context to the database
        public void Save()
        {
            _dbContext.SaveChanges();
        }

        // Updates an existing product in the database
        public void UpdateProduct(Product product)
        {
            // Find the existing product in the database by ID, including its ProductCategory
            var existingProduct = _dbContext.Products
                .Include(p => p.ProductCategory)
                .FirstOrDefault(p => p.ID == product.ID);

            if (existingProduct != null)
            {
                // Check if the category has changed
                bool categoryChanged = existingProduct.ProductCategory.ID != product.ProductCategory.ID;

                // Update the properties of the existing product
                existingProduct.Name = product.Name;
                existingProduct.Description = product.Description;
                existingProduct.Price = product.Price; // Assuming Price is a property

                // Handle category update if necessary
                if (categoryChanged)
                {
                    // Find the existing category by ID
                    var existingCategory = _dbContext.Categories.Find(product.ProductCategory.ID);

                    if (existingCategory != null)
                    {
                        // Assign the existing category to the product
                        existingProduct.ProductCategory = existingCategory;
                    }
                    else
                    {
                        // Handle case where the provided category ID doesn't exist
                        throw new ArgumentException("Provided category ID not found.");
                    }
                }

                // Mark the existing product as modified
                _dbContext.Entry(existingProduct).State = EntityState.Modified;
            }
            else
            {
                // Handle case where the product to update is not found
                throw new ArgumentException("Product with provided ID not found.");
            }

            // Save changes to the database
            Save();
        }
    }
}
