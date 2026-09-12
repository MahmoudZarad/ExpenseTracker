using AutoMapper;
using ExpenseTracker.Application.Features.Categories.Commands.Models;
using ExpenseTracker.Application.Interfaces.Common;
using ExpenseTracker.Application.Interfaces.IUnitOfWork;
using ExpenseTracker.Domain.Common;
using MediatR;

namespace ExpenseTracker.Application.Features.Categories.Commands.Handlers;

public class UpdateCategoryCommandHandler
    : IRequestHandler<
        UpdateCategoryCommand,
        Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public UpdateCategoryCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<Result<bool>> Handle(
        UpdateCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var category =
            await _unitOfWork.Categories.FindAsync(
                x =>
                    x.Id == request.Id &&
                    x.UserId == _currentUserService.UserId,
                cancellationToken);

        if (category is null)
        {
            return Result<bool>.Failure(
                "Category not found.",
                404);
        }

        var exists =
            await _unitOfWork.Categories.AnyAsync(
                x =>
                    x.Id != request.Id &&
                    x.UserId == _currentUserService.UserId &&
                    x.Name.ToLower() == request.Name.ToLower() &&
                    x.Type == request.Type,
                cancellationToken);

        if (exists)
        {
            return Result<bool>.Failure(
                "A category with this name already exists.",
                409);
        }

        _mapper.Map(request, category);

        await _unitOfWork.Categories.UpdateAsync(category);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Result<bool>.Success(true);
    }
}