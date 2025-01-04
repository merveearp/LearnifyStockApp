using System;
using System.Data;
using Dapper;
using LearnifyStockApp.Models.Entities;
using LearnifyStockApp.ViewModels;

namespace LearnifyStockApp.Models.Repositories;

public class CategoryRepository
{
    private readonly IDbConnection _dbConnection;
    public CategoryRepository(IDbConnection dbConnection)
    {
        _dbConnection=dbConnection;
        
    }
    public async Task<IEnumerable<CategoryViewModel>>GetAllAsync()
    {
        string query="SELECT Id,Name FROM Categories ";
        return await _dbConnection.QueryAsync<CategoryViewModel>(query);
    }
    public async Task<IEnumerable<CategoryViewModel>>GetAllAsync(bool isDeleted)
    {
        string query="SELECT Id,Name FROM Categories WHERE IsDeleted=@IsDeleted ";
        return await _dbConnection.QueryAsync<CategoryViewModel>(query,new {IsDeleted=isDeleted});
    }
    public async Task<Category?> GetByIdAsync(int id)
    {
        string query= "SELECT*FROM Categories WHERE Id=@Id ";
        return await _dbConnection.QueryFirstOrDefaultAsync<Category?>(query,new {Id=id});

    }
    public async Task CreateAsync(CategoryCreateViewModel category)
    {
        string query="INSERT INTO Categories(Name,Description) VALUES(@Name,@Description)";
        await _dbConnection.ExecuteAsync(query ,category);

    }
    public async Task UpdateAsync(CategoryUpdateViewModel category)
    {
        string query ="UPDATE Categories SET Name=@Name, Description=@Description, UpdatedAt=@UpdatedAt WHERE Id=@Id";
        category.UpdatedAt=DateTime.Now;
        await _dbConnection.ExecuteAsync(query ,category); 
    }
    public async Task<bool> HardDeleteAsync(int id)
    {
        string queryGet="SELECT * FROM Categories WHERE Id=@Id";
        var category=await _dbConnection.QueryFirstOrDefaultAsync(queryGet,new{Id=id});
        string query="DELETE FROM Categories WHERE Id=@Id";
        await _dbConnection.ExecuteAsync(query , new {Id=id});

        return category.IsDeleted;
    }
    public async Task<bool> SoftDeleteAsync(int id)
    {
        string queryGet ="SELECT * FROM Categories WHERE Id=@Id";
        var category = await _dbConnection.QueryFirstOrDefaultAsync(queryGet,new{Id=id});
        bool isDeleted = !category.IsDeleted;
        string queryDelete="UPDATE Categories SET IsDeleted=@IsDeleted,UpdatedAt=@UpdatedAt WHERE Id=@Id";
        await _dbConnection.ExecuteAsync(queryDelete,new {Id=id,IsDeleted=isDeleted,UpdatedAt=DateTime.Now});
        return !isDeleted;
    }
    public async Task<(string CategoryName,decimal TotalSales)> GetTopSellingCategoryAsync(bool asc=true)
    {

        var value =asc ? "ASC":"DESC";
        var query =$@"
    SELECT TOP(1)
        c.Name AS CategoryName,
    SUM(s.Quantity *s.UnitPrice) AS TotalSales
    FROM Categories c JOIN Products p 
        ON c.Id=p.CategoryId JOIN Sales s 
            ON p.Id=s.ProductId
    GROUP BY c.Name
    ORDER BY SUM(s.Quantity *s.UnitPrice) {value}
        ";
        return await _dbConnection.QueryFirstOrDefaultAsync<(string CategoryName,decimal TotalSales)>(query);

    }
}
