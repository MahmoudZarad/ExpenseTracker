using AutoMapper;
using ExpenseTracker.Application.Features.Transactions.Commands.Models;
using ExpenseTracker.Application.Interfaces.Common;
using ExpenseTracker.Application.Interfaces.IUnitOfWork;
using ExpenseTracker.Domain.Common;
using ExpenseTracker.Domain.Entities;
using MediatR;

namespace ExpenseTracker.Application.Features.Transactions.Commands.Handlers;

public class CreateTransactionCommandHandler
    : IRequestHandler<CreateTransactionCommand, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly int UserId;

    public CreateTransactionCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        UserId = currentUserService.UserId;
    }

    public async Task<Result<int>> Handle(
        CreateTransactionCommand request,
        CancellationToken cancellationToken)
    {


        var category =
            await _unitOfWork.Categories.GetByAsync(
                x =>
                    x.Id == request.CategoryId &&
                    x.UserId == UserId,

                x => new
                {
                    x.Id,
                    x.Type
                },
                cancellationToken);

        if (category is null)
        {
            return Result<int>.Failure(
                "Category not found.",
                404);
        }

        if (category.Type != request.Type)
        {
            return Result<int>.Failure(
                "Transaction type must match category type.",
                400);
        }

        var transaction =
            _mapper.Map<Transaction>(request);

        transaction.UserId = UserId;

        await _unitOfWork.Transactions.AddAsync(
            transaction,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Result<int>.Success(
            transaction.Id);
    }
}