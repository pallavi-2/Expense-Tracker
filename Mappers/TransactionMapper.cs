using ExpenseTracker.Dtos;
using ExpenseTracker.Models;

namespace ExpenseTracker.Mappers
{
        public static class TransactionMapper
        {
            public static TransactionResponse ToTransactionDto(this Transaction transactionDto)
            {
                return new TransactionResponse
                {
                    TransactionId = transactionDto.TransactionId,
                    Type = transactionDto.Type,
                    Amount = transactionDto.Amount,
                    Date = transactionDto.Date,
                    Category = transactionDto.Category,
                    Description = transactionDto.Description,
                };
            }

            public static Transaction ToTransactionFromDto(this TransactionCreateDto transactionDto, int UserId)
            {
                return new Transaction
                {

                    Type = transactionDto.Type,
                    Amount = transactionDto.Amount,
                    Category = transactionDto.Category,
                    Description = transactionDto.Description,
                    UserId = UserId
                };
            }
        }
    }
