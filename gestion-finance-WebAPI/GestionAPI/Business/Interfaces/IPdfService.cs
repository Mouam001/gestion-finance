using Common.DTO;

namespace Business.Interfaces
{

    public interface IPdfService
    {
        byte[] GenerateObpPdf(List<TransactionDto> transactions, string bankId);
    }
}