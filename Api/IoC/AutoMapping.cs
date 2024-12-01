using Api.Models;
using AutoMapper;
using Data.Collection;
using Data.Models;

namespace Api.IoC
{
  public class AutoMapping : Profile
  {
    public AutoMapping()
    {
      CreateMap<Item, ItemModel>();
      CreateMap<ItemModel, Item>();
      CreateMap<Category, CategoryModel>();

      CreateMap<AddItemModel, ItemModel>();
    }
  }
    
}
