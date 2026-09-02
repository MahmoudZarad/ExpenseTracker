using MediatR;
using ExpenseTracker.Domain.Common;

namespace ExpenseTracker.Application.Features.Transactions.Commands.Models;

public record DeleteTransactionCommand(
    int Id) : IRequest<Result<bool>>;
