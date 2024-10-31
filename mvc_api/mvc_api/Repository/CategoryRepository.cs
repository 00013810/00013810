using Microsoft.EntityFrameworkCore;
using mvc_api.DBContexts;
using mvc_api.Model;

namespace mvc_api.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        // Private readonly field for the product context to interact with the database
        private readonly ProductContext _productContext;

        // Constructor to inject the product context dependency
        public CategoryRepository(ProductContext productContext)
        {
            _productContext = productContext;
        }

        // Deletes a category and all associated products based on the provided category ID
        public void DeleteCategory(int categoryId)
        {
            // Find the category to delete by ID
            var category = _productContext.Categories.Find(categoryId);
            if (category != null)
            {
                // Find all products associated with this category
                var productsToDelete = _productContext.Products
                                                      .Where(p => p.ProductCategory.ID == categoryId)
                                                      .ToList();

                // Remove all related products from the context
                _productContext.Products.RemoveRange(productsToDelete);

                // Remove the category itself from the context
                _productContext.Categories.Remove(category);
                Save(); // Save changes to ensure deletion is persisted
            }
        }

        // Retrieves all categories from the database
        public IEnumerable<Category> GetCategories()
        {
            return _productContext.Categories.ToList(); // Returns all categories as a list
        }

        // Retrieves a specific category by its ID
        public Category GetCategoryId(int Id)
        {
            var c = _productContext.Categories.Find(Id); // Finds category by ID
            return c;
        }

        // Inserts a new category into the database
        public void Insertcategory(Category category)
        {
            _productContext.Categories.Add(category); // Adds category to the context
            Save(); // Saves changes to persist the new category
        }

        // Updates an existing category in the database
        public void UpdateCategory(Category category)
        {
            // Retrieve the original category without tracking to avoid conflicts during updates
            var originalCategory = _productContext.Categories
                                                  .AsNoTracking()
                                                  .FirstOrDefault(c => c.ID == category.ID);

            if (originalCategory != null)
            {
                // Check if the ID of the category has changed
                bool isIdChanged = originalCategory.ID != category.ID;

                if (isIdChanged)
                {
                    // Find all products associated with the original category
                    var productsToUpdate = _productContext.Products
                                                          .Where(p => p.ProductCategory.ID == originalCategory.ID)
                                                          .ToList();

                    // Remove the original category to prevent duplicate IDs
                    _productContext.Categories.Remove(originalCategory);

                    // Update each product to reference the modified category
                    foreach (var product in productsToUpdate)
                    {
                        product.ProductCategory = category; // Assign updated category reference
                    }

                    // Add the updated category with the new ID to the context
                    _productContext.Categories.Add(category);
                }
                else
                {
                    // If ID hasn't changed, update only fields that differ (not the ID)
                    originalCategory.Name = category.Name ?? originalCategory.Name;
                    originalCategory.Description = category.Description ?? originalCategory.Description;
                    // Set modified state to ensure only necessary fields are updated
                    _productContext.Entry(originalCategory).State = EntityState.Modified;
                }

                // Save changes to persist updates in the database
                Save();
            }
        }

        // Saves all changes to the database context
        public void Save()
        {
            _productContext.SaveChanges(); // Commits all changes made in the context
        }
    }
}
