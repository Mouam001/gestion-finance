using Business.Interfaces;
using Common.DAO;
using Common.DTO;
using Common.Requests;
using DataAccess.Implementations;
using Microsoft.EntityFrameworkCore;

public class TransactionService : ITransactionService
{
    private readonly AppDbContext _context;

    public TransactionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<TransactionDto> AddTransaction(int userId, CreateTransactionRequest request)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) throw new Exception("Utilisateur non trouvé");

        var transaction = new TransactionDao()
        {
            Type = request.Type,
            Category = request.Category, // ← ici
            Description = request.Description,
            Amount = request.Amount,
            CompletedDate = request.CompletedDate,
            PostedDate = request.PostedDate,
            UserId = userId,
            Currency = request.Currency
        };


        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();

        return new TransactionDto
        {
            Id = transaction.Id,
            UserId = userId,
            Type = Enum.TryParse<TransactionType>(transaction.Type, out var type) ? type : null,
            Category = Enum.TryParse<TransactionCategory>(transaction.Category, out var cat) ? cat : null,
            Description = transaction.Description,
            Amount = transaction.Amount,
            Currency = Enum.TryParse<CurrencyCode>(transaction.Currency, out var currency) ? currency : null,
            CompletedDate = transaction.CompletedDate,
            PostedDate = transaction.PostedDate
        };
    }

    public async Task<List<TransactionDto>> GetUserTransactions(int userId)
    {
        var list = await _context.Transactions
            .Where(t => t.UserId == userId)
            .ToListAsync();

        return list.Select(t => new TransactionDto
        {
            Id = t.Id,
            UserId = t.UserId,
            Type = Enum.TryParse<TransactionType>(t.Type, out var type) ? type : null,
            Category = Enum.TryParse<TransactionCategory>(t.Category, out var cat) ? cat : null,
            Description = t.Description,
            Amount = t.Amount,
            Currency = Enum.TryParse<CurrencyCode>(t.Currency, out var curr) ? curr : null,
            CompletedDate = t.CompletedDate,
            PostedDate = t.PostedDate
        }).ToList();
    }

    public async Task<bool> DeleteTransaction(int transactionId)
    {
        var tx = await _context.Transactions.FindAsync(transactionId);
        if (tx == null) return false;

        _context.Transactions.Remove(tx);
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<TransactionDto> UpdateTransaction(int transactionId, UpdateTransactionRequest request)
    {
        var transaction = await _context.Transactions.FindAsync(transactionId);
        if (transaction == null)
            return null;

        transaction.Description = request.Description;
        transaction.Amount = request.Amount;
        transaction.CompletedDate = request.Date;
        transaction.Type = request.Type;
        transaction.Currency = request.Currency;
        transaction.Category = request.Category;
        

        
        await _context.SaveChangesAsync();

        return new TransactionDto
        {
            Id = transaction.Id,
            Description = transaction.Description,
            Amount = transaction.Amount,
            CompletedDate = transaction.CompletedDate,
            Type = Enum.TryParse<TransactionType>(transaction.Type, out var type) ? type : null,
            Category = Enum.TryParse<TransactionCategory>(transaction.Category, out var cat) ? cat : null,
            PostedDate = transaction.PostedDate,
            UserId = transaction.UserId
        };
    }

}