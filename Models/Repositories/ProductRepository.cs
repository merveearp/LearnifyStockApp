using System;
using System.Data;
using Dapper;
using LearnifyStockApp.ViewModels;

namespace LearnifyStockApp.Models.Repositories;

public class ProductRepository
{
    private readonly IDbConnection _dbConnection;

    public ProductRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }
    public async Task<IEnumerable<ProductViewModel>>GetAllAsync()
    {
        string query=@"
        SELECT
        p.Id ,
        p.IsDeleted,
        p.Name,
        p.Description,
        c.Name AS CategoryName,
        s.Name AS SupplierName,
        p.StockQuantity,
        p.MinimumStockLevel,
        p.Price
        
        FROM 
        Products p JOIN Categories c 
            ON p.CategoryId=c.Id JOIN Suppliers s 
                ON p.SupplierId=s.Id
        
        ";
        return await _dbConnection.QueryAsync<ProductViewModel>(query);
    }   
    public async Task<IEnumerable<ProductViewModel>>GetAllAsync(bool isDeleted)
    {
        string query=@"
        SELECT
        p.Id ,
        p.IsDeleted,
        p.Name,
        p.Description,
        c.Name AS CategoryName,
        s.Name AS SupplierName,
        p.StockQuantity,
        p.MinimumStockLevel,
        p.Price
        
        FROM 
        Products p JOIN Categories c 
            ON p.CategoryId=c.Id JOIN Suppliers s 
                ON p.SupplierId=s.Id
        WHERE p.IsDeleted=@IsDeleted  
        ";
        return await _dbConnection.QueryAsync<ProductViewModel>(query,new{IsDeleted=isDeleted});
    }
    public async Task<ProductViewModel?> GetByIdAsync(int id)
    {
        string query=@"
        SELECT
        p.Id ,
        p.IsDeleted,
        p.Name,
        p.Description,
        c.Name AS CategoryName,
        s.Name AS SupplierName,
        p.StockQuantity,
        p.MinimumStockLevel,
        p.Price,
        p.UpdatedAt
        FROM 
        Products p JOIN Categories c 
            ON p.CategoryId=c.Id JOIN Suppliers s 
                ON p.SupplierId=s.Id 
        WHERE p.Id=@Id 
        ";
        return await _dbConnection.QueryFirstOrDefaultAsync<ProductViewModel>(query , new {Id=id});
    }
    public async Task CreateAsync(ProductCreateViewModel productCreateViewModel)
    {
        string query=@"
        INSERT INTO Products
        (Name,Description,CategoryId,SupplierId,StockQuantity,MinimumStockLevel,Price)
        VALUES
        (@Name,@Description,@CategoryId,@SupplierId,@StockQuantity,@MinimumStockLevel,@Price)
   
        ";
        await _dbConnection.ExecuteAsync(query,productCreateViewModel);
    }
    public async Task UpdateAsync(ProductUpdateViewModel productUpdateViewModel)
    {
        string query=@"
        UPDATE Products
        SET 
        Name=@Name,
        Description=@Description,
        CategoryId=@CategoryId,
        SupplierId=@SupplierId,
        StockQuantity=@StockQuantity,
        MinimumStockLevel=@MinimumStockLevel,
        Price=@Price,
        UpdatedAt=@UpdatedAt
        WHERE Id=@Id
        ";
        productUpdateViewModel.UpdatedAt=DateTime.Now;
        await _dbConnection.ExecuteAsync(query ,productUpdateViewModel);

    }
    public async Task<bool> HardDeleteAsync(int id)
    {
        string queryGet="SELECT * FROM Products WHERE Id=@Id";
        var product=await _dbConnection.QueryFirstOrDefaultAsync(queryGet ,new{Id=id});

        string query =" DELETE FROM Products WHERE Id=@Id";
        await _dbConnection.ExecuteAsync(query,new {Id=id});
        return product.IsDeleted;

    }
    public async Task<bool> SoftDeleteAsync(int id)
    {
        string queryGet ="SELECT * FROM Products WHERE Id=@Id";
        var product = await _dbConnection.QueryFirstOrDefaultAsync(queryGet,new{Id=id});
        bool isDeleted = !product.IsDeleted;
        string queryDelete="UPDATE Products SET IsDeleted=@IsDeleted,UpdatedAt=@UpdatedAt WHERE Id=@Id";
        await _dbConnection.ExecuteAsync(queryDelete,new {Id=id,IsDeleted=isDeleted,UpdatedAt=DateTime.Now});
        return !isDeleted;
    }
    public async Task<int> GetProductsCountAsync()
    {
        int count =await _dbConnection.QueryFirstOrDefaultAsync<int>("SELECT COUNT(*) FROM Products WHERE IsDeleted=@IsDeleted",new {IsDeleted=false});
        return count;
    }
    public async Task<IEnumerable<ProductViewModel>> GetLastProductAsync(int quantity)
    {
        string query = $@"
            SELECT 
                TOP(@Quantity)
                p.Id,
                p.IsDeleted,
                p.Name,
                p.Description,
                c.Name AS CategoryName,
                s.Name AS SupplierName,
                p.StockQuantity,
                p.MinimumStockLevel,
                p.Price
            FROM Products p JOIN Categories c 
                ON p.CategoryId=c.Id JOIN Suppliers s
                    ON p.SupplierId=s.Id
            WHERE p.IsDeleted=@IsDeleted
            ORDER BY p.Id DESC
        ";
        var products =await _dbConnection.QueryAsync<ProductViewModel>(query,new{IsDeleted=false,Quantity=quantity});
        return products;
    }
    }
    


