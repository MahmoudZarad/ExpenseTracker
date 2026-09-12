using AutoMapper;
using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Application.Features.Categories.Queries.Models;
using ExpenseTracker.Application.Interfaces.IUnitOfWork;
using ExpenseTracker.Domain.Common;
using MediatR;

namespace ExpenseTracker.Application.Features.Categories.Queries.Handlers;

public class GetCategoryByIdQueryHandler
    : IRequestHandler<
        GetCategoryByIdQuery,
        Result<CategoryDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCategoryByIdQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<CategoryDto>> Handle(
        GetCategoryByIdQuery request,
        CancellationToken cancellationToken)
    {
        var category =
            await _unitOfWork.Categories.GetByAsync<CategoryDto>(
                x =>
                    x.Id == request.Id &&
                    x.UserId == request.UserId,

                _mapper.ConfigurationProvider,

                cancellationToken);

        if (category is null)
        {
            return Result<CategoryDto>.Failure(
                "Category not found.",
                404);
        }

        return Result<CategoryDto>.Success(category);
    }
}