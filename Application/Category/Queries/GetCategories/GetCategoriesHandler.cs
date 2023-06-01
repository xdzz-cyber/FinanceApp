using Application.Common.Dtos;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Category.Queries.GetCategories;

public class GetCategoriesHandler : IRequestHandler<Category.Queries.GetCategories.GetCategories, List<CategoryDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCategoriesHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public Task<List<CategoryDto>> Handle(Category.Queries.GetCategories.GetCategories request, CancellationToken cancellationToken)
    {
        var categories = _context.Categories
            .AsNoTracking()
            .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
            .ToList();
        
        return Task.FromResult(categories);
    }
}