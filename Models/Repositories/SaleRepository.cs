using System;
using System.Data;
using Dapper;
using LearnifyStockApp.ViewModels;

namespace LearnifyStockApp.Models.Repositories;

public class SaleRepository
{
    private IDbConnection _dbConnection;

    public SaleRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<IEnumerable<SaleViewModel>> GetAllAsync()
    {
        string query=@"
        SELECT
        s.Id ,
        s.IsDeleted,
        p.Name AS ProductName,
        c.Name AS CustomerName,
        s.Quantity,
        s.UnitPrice,
        s.SaleDate        
        FROM 
        Sales s JOIN Customers c 
            ON s.CustomerId=c.Id JOIN Products p 
                ON s.ProductId=p.Id
        
        ";
        return await _dbConnection.QueryAsync<SaleViewModel>(query);


    }

    public async Task<IEnumerable<SaleViewModel>> GetAllAsync(bool isDeleted)
    {
        string query=@"
        SELECT
        s.Id ,
        s.IsDeleted,
        p.Name AS ProductName,
        c.Name AS CustomerName,
        s.Quantity,
        s.UnitPrice,
        s.SaleDate        
        FROM 
        Sales s JOIN Customers c 
            ON s.CustomerId=c.Id JOIN Products p 
                ON s.ProductId=p.Id
        WHERE s.IsDeleted=@IsDeleted  
             
        ";
        return await _dbConnection.QueryAsync<SaleViewModel>(query,new{IsDeleted=isDeleted});
    }

    public async Task<SaleViewModel?> GetByIdAsync(int id)
    {
        string query=@"
        SELECT
        s.Id ,
        s.IsDeleted,
        p.Name AS ProductName,
        c.Name AS CustomerName,
        s.Quantity,
        s.UnitPrice,
        s.SaleDate ,
        s.UpdatedAt       
        FROM 
        Sales s JOIN Customers c 
            ON s.CustomerId=c.Id JOIN Products p 
                ON s.ProductId=p.Id
        WHERE s.Id=@Id
        "; 
        return await _dbConnection.QueryFirstOrDefaultAsync<SaleViewModel>(query,new{Id=id});     
        
    }
    public async Task<bool> CreateAsync(SaleCreateViewModel saleCreateViewModel)
    {
        string query=@"
        SELECT
        p.StockQuantity
        FROM Products p WHERE Id=@Id
        ";
        int stockQuantity=await _dbConnection.QueryFirstOrDefaultAsync<int>(query,new {Id=saleCreateViewModel.ProductId});
        if(stockQuantity>=saleCreateViewModel.Quantity)
        {     
        query=@"
        INSERT INTO Sales
        (ProductId,CustomerId,Quantity,UnitPrice,SaleDate)
        VALUES
        (@ProductId,@CustomerId,@Quantity,@UnitPrice,@SaleDate)
        ";
        await _dbConnection.ExecuteAsync(query,saleCreateViewModel);
        return true;
        }
        return false;
    }
    public async Task UpdateAsync(SaleUpdateViewModel saleUpdateViewModel)
    {
        string query=@"
        UPDATE Sales
        SET 
        ProductId=@ProductId,
        CustomerId=@CustomerId,
        Quantity=@Quantity,
        UnitPrice=@UnitPrice,
        SaleDate=@SaleDate,
        UpdatedAt=@UpdatedAt
        WHERE Id=@Id
        ";
        saleUpdateViewModel.UpdatedAt=DateTime.Now; 
        await _dbConnection.ExecuteAsync(query ,saleUpdateViewModel);

    }
    public async Task<bool> HardDeleteAsync(int id)
    {
        string queryGet="SELECT * FROM Sales WHERE Id=@Id";
        var sale=await _dbConnection.QueryFirstOrDefaultAsync(queryGet ,new{Id=id});

        string query =" DELETE FROM Sales WHERE Id=@Id";
        await _dbConnection.ExecuteAsync(query,new {Id=id});
        return sale.IsDeleted;

    }
    public async Task<bool> SoftDeleteAsync(int id)
    {
        string queryGet ="SELECT * FROM Sales WHERE Id=@Id";
        var sale = await _dbConnection.QueryFirstOrDefaultAsync(queryGet,new{Id=id});
        bool isDeleted = !sale.IsDeleted;
        string queryDelete="UPDATE Sales SET IsDeleted=@IsDeleted,UpdatedAt=@UpdatedAt WHERE Id=@Id";
        await _dbConnection.ExecuteAsync(queryDelete,new {Id=id,IsDeleted=isDeleted,UpdatedAt=DateTime.Now});
        return !isDeleted;
    }

}
