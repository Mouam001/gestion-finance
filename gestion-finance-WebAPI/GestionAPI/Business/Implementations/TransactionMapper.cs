using Common.DTO;
using Common.DAO; 
using System;

public static class TransactionMapper
{
    public static TransactionDao ToDao(TransactionDto dto, int userId)
    {
        return new TransactionDao
        {
            UserId = userId,
            Type = dto.Type?.ToString(),
            Category = dto.Category?.ToString(),
            Description = dto.Description,
            Amount = dto.Amount,
            Currency = dto.Currency?.ToString(),
            PostedDate = dto.PostedDate,
            CompletedDate = dto.CompletedDate
        };
    }

    public static TransactionDto FromDao(TransactionDao dao)
    {
        return new TransactionDto
        {
            Type = Enum.TryParse<TransactionType>(dao.Type, out var type) ? type : null,
            Category = Enum.TryParse<TransactionCategory>(dao.Category, out var cat) ? cat : null,
            Description = dao.Description,
            Amount = dao.Amount,
            Currency = Enum.TryParse<CurrencyCode>(dao.Currency, out var currency) ? currency : null,
            PostedDate = dao.PostedDate,
            CompletedDate = dao.CompletedDate
        };
    }
}