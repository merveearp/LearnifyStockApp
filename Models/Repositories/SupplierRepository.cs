using System;
using System.Data;
using Dapper;
using LearnifyStockApp.ViewModels;

namespace LearnifyStockApp.Models.Repositories;

public class SupplierRepository
{
    private readonly IDbConnection _dbConnection;

    public SupplierRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }
    public async Task<IEnumerable<SupplierViewModel>> GetAllAsync()
    {
        string query="SELECT Id, Name,ContactName,Email,PhoneNumber,Address,City,Country FROM Suppliers";
        return await _dbConnection.QueryAsync<SupplierViewModel>(query);
    }
    public async Task<IEnumerable<SupplierViewModel>> GetAllAsync(bool isDeleted)
    {
        string query="SELECT Id, Name,ContactName,Email,PhoneNumber,Address,City,Country FROM Suppliers WHERE IsDeleted=@IsDeleted";
        return await _dbConnection.QueryAsync<SupplierViewModel>(query,new{IsDeleted=isDeleted});
    }
     public async Task<SupplierViewModel?> GetByIdAsync(int id)
    {
        string query = "SELECT Id, Name, ContactName, Email, PhoneNumber, Address, City, Country, UpdatedAt FROM Suppliers WHERE Id=@Id";
        return await _dbConnection.QueryFirstOrDefaultAsync<SupplierViewModel?>(query, new { Id = id });
    }
    public async Task CreateAsync(SupplierCreateViewModel supplier)
    {
        string query = "INSERT INTO Suppliers(Name,ContactName, Email, PhoneNumber, Address, City, Country) VALUES(@Name,@ContactName, @Email, @PhoneNumber, @Address, @City, @Country)";
        await _dbConnection.ExecuteAsync(query, supplier);
    }
    public async Task UpdateAsync(SupplierUpdateViewModel supplier)
    {
        string query = "UPDATE Suppliers SET Name=@Name, ContactName=@ContactName, Email=@Email, PhoneNumber=@PhoneNumber, Address=@Address, City=@City, Country=@Country, UpdatedAt=@UpdatedAt WHERE Id=@Id";
        supplier.UpdatedAt = DateTime.Now;
        await _dbConnection.ExecuteAsync(query, supplier);
    }
    public async Task<bool> HardDeleteAsync(int id)
    {
        string queryGet = "SELECT Id, IsDeleted FROM Suppliers WHERE Id=@Id";
        var supplier = await _dbConnection.QueryFirstOrDefaultAsync(queryGet, new { Id = id });

        string query = "DELETE FROM Suppliers WHERE Id=@Id";
        await _dbConnection.ExecuteAsync(query, new { Id = id });

        return supplier.IsDeleted;
    }

    public async Task<bool> SoftDeleteAsync(int id)
    {
        string queryGet = "SELECT Id, IsDeleted FROM Suppliers WHERE Id=@Id";
        var supplier = await _dbConnection.QueryFirstOrDefaultAsync(queryGet, new { Id = id });
        bool isDeleted = !supplier.IsDeleted;
        string queryDelete = "UPDATE Suppliers SET IsDeleted=@IsDeleted, UpdatedAt=@UpdatedAt WHERE Id=@Id";
        await _dbConnection.ExecuteAsync(queryDelete, new { Id = id, IsDeleted = isDeleted, UpdatedAt = DateTime.Now });
        return !isDeleted;
    }
     public async Task<int> GetSuppliersCountAsync()
    {
        int count = await _dbConnection.QueryFirstOrDefaultAsync<int>("SELECT COUNT(*) FROM Suppliers ");
        return count;
    }
    public async Task<int> GetSuppliersCountAsync(bool isDeleted)
    {

        int count = await _dbConnection.QueryFirstOrDefaultAsync<int>("SELECT COUNT(*) FROM Suppliers WHERE IsDeleted=@IsDeleted", new { IsDeleted = isDeleted });
        return count;
    }









    


}
