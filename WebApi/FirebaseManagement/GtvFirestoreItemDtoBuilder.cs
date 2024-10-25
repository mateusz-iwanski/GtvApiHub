using GtvApiHub.WebApi.DTOs;
using GtvApiHub.WebApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtvApiHub.WebApi.FirebaseManagement
{
    public class GtvFirestoreItemDtoBuilder
    {
        private FirestoreItemDto _firestoreItemDto { get; set; }

        public FirestoreItemDto Build(
            string itemCode,
            List<ItemDto> allApiItemDtos,
            List<PriceDto> allApiPriceDtos,
            List<StockDto> allApiStockDtos,
            List<AttributeDto> allApiAttributeDtos,
            List<CategoryTreeDto> allApiCategoryTreeDtos,
            List<PackageTypeDto> allApiPackageTypeDtos,
            List<AlternativeItemDto> allApiAlternativeItemDtos
            )
        {
            List<ItemDto> items = allApiItemDtos.Where(x => x.ItemCode == itemCode).ToList();
            List<PriceDto> prices = allApiPriceDtos.Where(x => x.ItemCode == itemCode).ToList();
            List<StockDto> stocks = allApiStockDtos.Where(x => x.ItemCode == itemCode).ToList();
            List<AttributeDto> attributes = allApiAttributeDtos.Where(x => x.ItemCode == itemCode).ToList();
            List<CategoryTreeDto> categoryTrees = allApiCategoryTreeDtos.Where(x => x.ItemCode == itemCode).ToList();
            List<PackageTypeDto> packageTypes = allApiPackageTypeDtos.Where(x => x.ItemCode == itemCode).ToList();
            List<AlternativeItemDto> alternateItems = allApiAlternativeItemDtos.Where(x => x.ItemCode == itemCode).ToList();

            _firestoreItemDto = new FirestoreItemDto()
            {
                Item = items,
                Price = prices.FirstOrDefault(),
                Stocks = stocks,
                Attributes = attributes,
                CategoryTrees = categoryTrees,
                PackageTypes = packageTypes,
                ItemCode = itemCode,
                AlternateItems = alternateItems
            };

            return _firestoreItemDto;
        }

        public FirestoreItemDto Get() => _firestoreItemDto;
    }
}
