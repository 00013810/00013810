using Microsoft.AspNetCore.Mvc;
using mvc_api.Model;
using mvc_api.Repository;
using System.Transactions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace mvc_api.Controllers
{
    // Defines the base route for this controller, making it accessible at /api/Category
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        // Private readonly field for the category repository, used to access data operations
        private readonly ICategoryRepository _categoryRepository;

        // Constructor injection for the repository dependency
        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        // GET: api/Category
        // Retrieves all categories from the repository
        [HttpGet]
        public IActionResult Get()
        {
            // Call repository method to get all categories
            var cate = _categoryRepository.GetCategories();
            // Return the list of categories with a 200 OK response
            return new OkObjectResult(cate);
        }

        // GET: api/Category/5
        // Retrieves a single category by ID
        [HttpGet("{id}", Name = "Get")]
        public IActionResult GetGetByID(int id)
        {
            // Call repository method to get a specific category by ID
            var cate = _categoryRepository.GetCategoryId(id);
            // Return the found category with a 200 OK response
            return new OkObjectResult(cate);
        }

        // POST: api/Category
        // Inserts a new category into the repository
        [HttpPost]
        public IActionResult Post([FromBody] Category cate)
        {
            // Using a transaction scope to ensure data consistency
            using (var scope = new TransactionScope())
            {
                // Insert the new category using the repository method
                _categoryRepository.Insertcategory(cate);
                // Complete the transaction
                scope.Complete();
                // Return a 201 Created response with the new category's URI and data
                return CreatedAtAction(nameof(Get), new { id = cate.ID }, cate);
            }
        }

        // PUT: api/Category/5
        // Updates an existing category by ID
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Category category)
        {
            if (category != null)
            {
                // Using a transaction scope to ensure data consistency
                using (var scope = new TransactionScope())
                {
                    // Update the category data in the repository
                    _categoryRepository.UpdateCategory(category);
                    // Complete the transaction
                    scope.Complete();
                    // Return a 200 OK response indicating the update was successful
                    return new OkResult();
                }
            }
            // Return a 204 No Content response if the category object was null
            return new NoContentResult();
        }

        // DELETE: api/Category/5
        // Deletes a category by ID
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            // Call repository method to delete the specified category
            _categoryRepository.DeleteCategory(id);
            // Return a 200 OK response indicating the deletion was successful
            return new OkResult();
        }
    }
}
