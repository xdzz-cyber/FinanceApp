using Application.Common.Dtos;
using MediatR;

namespace Application.Category.Queries.GetCategories;

public class GetCategories : IRequest<List<CategoryDto>>
{
    
}