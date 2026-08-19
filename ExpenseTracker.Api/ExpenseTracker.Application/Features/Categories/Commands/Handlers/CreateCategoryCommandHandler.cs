using AutoMapper;
using ExpenseTracker.Application.Features.Categories.Commands.Models;
using ExpenseTracker.Application.Interfaces.Common;
using ExpenseTracker.Application.Interfaces.IUnitOfWork;
using ExpenseTracker.Domain.Common;
using ExpenseTracker.Domain.Entities;
using MediatR;

namespace ExpenseTracker.Application.Features.Categories.Commands.Handlers;

public class CreateCategoryCommandHandler
    : IRequestHandler<CreateCategoryCommand, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public CreateCategoryCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<Result<int>> Handle(
        CreateCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        var exists =
            await _unitOfWork.Categories.AnyAsync(
                x =>
                    x.UserId == userId &&
                    x.Name.ToLower() == request.Name.ToLower() &&
                    x.Type == request.Type,
                cancellationToken);

        if (exists)
        {
            return Result<int>.Failure(
                "A category with this name already exists.",
                409);
        }

        var category =
            _mapper.Map<Category>(request);

        await _unitOfWork.Categories.AddAsync(
            category,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Result<int>.Success(category.Id);
    }
}