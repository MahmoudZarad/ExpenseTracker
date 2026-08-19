using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Domain.Common;
using MediatR;

namespace ExpenseTracker.Application.Features.Transactions.Queries.Models;

public record GetTransactionByIdQuery(
    int Id) : IRequest<Result<TransactionDto>>;
