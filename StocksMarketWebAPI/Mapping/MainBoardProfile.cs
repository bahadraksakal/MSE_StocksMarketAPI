using AutoMapper;
using StocksMarketWebAPI.DTOs.MainBoardDTOs;
using StockMarketEntitiesLibrary.Entities;

namespace StocksMarketWebAPI.Mapping
{
    public class MainBoardProfile:Profile
    {
        public MainBoardProfile()
        {
            CreateMap<MainBoard, MainBoardDTO>();
            CreateMap<MainBoardDTO, MainBoard>();
        }
    }
}
