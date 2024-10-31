using mvc_api.Model;

namespace mvc_api.Repository
{
    // Interface for the Product repository, defining methods for data operations
    public interface IProductRepository
    {
        // Inserts a new product into the database
        void InsertProduct(Product product);

        // Updates an existing product in the database
        void UpdateProduct(Product product);

        // Deletes a product from the database based on its ID
        void DeleteProduct(int productId);

        // Retrieves a specific product by its ID
        Product GetProductById(int Id);

        // Retrieves all products from the database
        IEnumerable<Product> GetProducts();
    }
}
