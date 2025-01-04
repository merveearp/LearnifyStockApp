using System;
using System.Data;
using Dapper;
using LearnifyStockApp.ViewModels;

namespace LearnifyStockApp.Models.Repositories;

public class CustomerRepository
{
    private readonly IDbConnection _dbConnection;

    public CustomerRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }
    public async Task<IEnumerable<CustomerViewModel>> GetAllAsync()
    {
        string query = "SELECT Id, Name, ContactName, Email, PhoneNumber, Address, City, Country FROM Customers";
        return await _dbConnection.QueryAsync<CustomerViewModel>(query);
    }
     public async Task<IEnumerable<CustomerViewModel>> GetAllAsync(bool isDeleted)
    {
        string query = "SELECT Id, Name, ContactName, Email, PhoneNumber, Address, City, Country FROM Customers WHERE IsDeleted=@IsDeleted";
        return await _dbConnection.QueryAsync<CustomerViewModel>(query, new { IsDeleted = isDeleted });
    }
     public async Task<CustomerViewModel?> GetByIdAsync(int id)
    {
        string query = "SELECT Id, Name, ContactName, Email, PhoneNumber, Address, City, Country, UpdatedAt FROM Customers WHERE Id=@Id";
        return await _dbConnection.QueryFirstOrDefaultAsync<CustomerViewModel?>(query, new { Id = id });
    }

    public async Task CreateAsync(CustomerCreateViewModel customer)
    {
        string query = "INSERT INTO Customers(Name,ContactName, Email, PhoneNumber, Address, City, Country) VALUES(@Name,@ContactName, @Email, @PhoneNumber, @Address, @City, @Country)";
        await _dbConnection.ExecuteAsync(query, customer);
    }
    public async Task UpdateAsync(CustomerUpdateViewModel customer)
    {
        string query = "UPDATE Customers SET Name=@Name, ContactName=@ContactName, Email=@Email, PhoneNumber=@PhoneNumber, Address=@Address, City=@City, Country=@Country, UpdatedAt=@UpdatedAt  WHERE Id=@Id";
        customer.UpdatedAt = DateTime.Now;
        await _dbConnection.ExecuteAsync(query, customer);
    }
    public async Task<bool> HardDeleteAsync(int id)
    {
        string queryGet = "SELECT Id, IsDeleted FROM Customers WHERE Id=@Id";
        var customer = await _dbConnection.QueryFirstOrDefaultAsync(queryGet, new { Id = id });

        string query = "DELETE FROM Customers WHERE Id=@Id";
        await _dbConnection.ExecuteAsync(query, new { Id = id });

        return customer.IsDeleted;
    }

    public async Task<bool> SoftDeleteAsync(int id)
    {
        string queryGet = "SELECT Id, IsDeleted FROM Customers WHERE Id=@Id";
        var customer = await _dbConnection.QueryFirstOrDefaultAsync(queryGet, new { Id = id });
        bool isDeleted = !customer.IsDeleted;
        string queryDelete = "UPDATE Customers SET IsDeleted=@IsDeleted, UpdatedAt=@UpdatedAt WHERE Id=@Id";
        await _dbConnection.ExecuteAsync(queryDelete, new { Id = id, IsDeleted = isDeleted, UpdatedAt = DateTime.Now });
        return !isDeleted;
    }
    public async Task<int> GetCustomersCountAsync()
    {
        int count = await _dbConnection.QueryFirstOrDefaultAsync<int>("SELECT COUNT(*) FROM Customers WHERE IsDeleted=@IsDeleted", new { IsDeleted = false });
        return count;
    }
    public async Task<IEnumerable<CustomerViewModel>> GetLastCustomersAsync(int quantity)
    {
        string query ="SELECT TOP(@Quantity)*FROM Customers WHERE IsDeleted=@IsDeleted ORDER BY Id DESC";
        var customers=await _dbConnection.QueryAsync<CustomerViewModel>(query,new {Quantity=quantity, IsDeleted=false});
        return customers;
    }




}
