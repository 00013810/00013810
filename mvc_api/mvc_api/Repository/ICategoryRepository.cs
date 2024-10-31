using mvc_api.Model;

namespace mvc_api.Repository
{
    public interface ICategoryRepository
    {
        void Insertcategory(Category category); // Method to insert a new Category object into the repository
        void UpdateCategory(Category category); // Method to update an existing Category object in the repository
        void DeleteCategory(int CategoryID); // Method to delete a Category from the repository by its ID
        Category GetCategoryId(int Id); // Method to retrieve a single Category by its ID
        IEnumerable<Category> GetCategories(); // Method to retrieve a list of all Category objects
    }
}
