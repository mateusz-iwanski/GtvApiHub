using GtvApiHub.WebApi.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtvApiHub.WebApi.FirebaseManagement
{
    public class FirestoreContentsDtoBuilder
    {
        private ContentsDto _contentsDto { get; set; }

        public ContentsDto Build(List<FirestoreItemDto> allApiItemDtos)
        {
            List<ItemCodePair> ItemCodePairs = new List<ItemCodePair>();

            foreach (var item in allApiItemDtos)
            {
                ItemCodePairs.Add(new ItemCodePair { Id = item.Id, ItemCode = item.ItemCode });
            }

            _contentsDto = new ContentsDto()
            {
                ItemCodePairs = ItemCodePairs
            };

            return _contentsDto;
        }
    }
}
