using ExpenseTracker.Domain.Common;
using ExpenseTracker.Domain.Enums;
using MediatR;

namespace ExpenseTracker.Application.Features.Transactions.Commands.Models;

public record UpdateTransactionCommand(
    int Id,
    string Title,
    string Description,
    decimal Amount,
    TransactionType Type,
    DateTime Date,
    int CategoryId
) : IRequest<Result<bool>>;
