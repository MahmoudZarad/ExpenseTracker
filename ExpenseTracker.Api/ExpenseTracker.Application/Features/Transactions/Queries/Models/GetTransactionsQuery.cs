using ExpenseTracker.Application.Common.Patterns;
using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Application.Interfaces.Common;
using ExpenseTracker.Domain.Common;
using MediatR;

namespace ExpenseTracker.Application.Features.Transactions.Queries.Models;

public record GetTransactionsQuery(TransactionQueryParams Params
) : IRequest<Result<PaginatedResult<TransactionDto>>>,
    IHasPagination<TransactionQueryParams>;