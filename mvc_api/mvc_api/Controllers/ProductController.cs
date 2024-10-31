using Microsoft.AspNetCore.Mvc;
using mvc_api.Model;
using mvc_api.Repository;
using System.Transactions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace mvc_api.Controllers
{
    // Sets up the base route for this controller, accessible at /api/Product
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        // Private readonly field for the product repository, used for data operations
        private readonly IProductRepository _productRepository;

        // Constructor to inject the repository dependency
        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // GET: api/Product
        // Retrieves all products from the repository
        [HttpGet]
        public IActionResult Get()
        {
            // Calls repository method to get all products
            var products = _productRepository.GetProducts();
            // Returns the list of products with a 200 OK response
            return new OkObjectResult(products);
        }

        // GET: api/Product/5
        // Retrieves a single product by ID
        [HttpGet("{id}", Name = "GetProducts")]
        public IActionResult GetByID(int id)
        {
            // Calls repository method to get a specific product by ID
            var product = _productRepository.GetProductById(id);
            // Returns the found product with a 200 OK response
            return new OkObjectResult(product);
        }

        // POST: api/Product
        // Inserts a new product into the repository
        [HttpPost]
        public IActionResult Post([FromBody] Product product)
        {
            // Using a transaction scope to ensure data consistency during insertion
            using (var scope = new TransactionScope())
            {
                // Inserts the new product using the repository method
                _productRepository.InsertProduct(product);
                // Completes the transaction
                scope.Complete();
                // Returns a 201 Created response with the new product's URI and data
                return CreatedAtAction(nameof(Get), new { id = product.ID }, product);
            }
        }

        // PUT: api/Product/5
        // Updates an existing product by ID
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Product product)
        {
            if (product != null)
            {
                // Using a transaction scope to ensure data consistency during update
                using (var scope = new TransactionScope())
                {
                    // Updates the product data in the repository
                    _productRepository.UpdateProduct(product);
                    // Completes the transaction
                    scope.Complete();
                    // Returns a 200 OK response indicating the update was successful
                    return new OkResult();
                }
            }
            // Returns a 204 No Content response if the product object was null
            return new NoContentResult();
        }

        // DELETE: api/Product/5
        // Deletes a product by ID
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            // Calls repository method to delete the specified product
            _productRepository.DeleteProduct(id);
            // Returns a 200 OK response indicating the deletion was successful
            return new OkResult();
        }
    }
}
