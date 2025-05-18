using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using RestaurantApp.Data;
using RestaurantApp.Models;

namespace RestaurantApp.Repositories
{
    public class CategoryRepository
    {
        public void AddCategory(string name)
        {
            SqlParameter[] parameters = { new SqlParameter("@Name", name) };
            DatabaseHelper.ExecuteNonQuery("AddCategory", parameters);
        }

        public List<Category> GetCategories()
        {
            DataTable table = DatabaseHelper.ExecuteQuery("GetCategories");
            return table.AsEnumerable().Select(row => new Category
            {
                CategoryID = row.Field<int>("CategoryID"),
                Name = row.Field<string>("Name")
            }).ToList();
        }

        public void UpdateCategory(int id, string name)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@CategoryID", id),
                new SqlParameter("@Name", name)
            };
            DatabaseHelper.ExecuteNonQuery("UpdateCategory", parameters);
        }

        public void DeleteCategory(int id)
        {
            SqlParameter[] parameters = { new SqlParameter("@CategoryID", id) };
            DatabaseHelper.ExecuteNonQuery("DeleteCategory", parameters);
        }
    }
}