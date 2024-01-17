using AspDigitalMemoSlip.Domain;
using AutoMapper;
using DTOClassLibrary.DTO;
using DTOClassLibrary.DTO.Authentication;
using DTOClassLibrary.DTO.Consignee;
using DTOClassLibrary.DTO.Consigner;
using DTOClassLibrary.DTO.Memo;
using DTOClassLibrary.DTO.Notification;
using DTOClassLibrary.DTO.Product;
using DTOClassLibrary.DTO.ProductSale;
using System.Globalization;

namespace AspDigitalMemoSlip.Application
{

    public class Mappings : Profile
    {
        public Mappings()
        {
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Consignee, ConsigneeDTO>().ReverseMap();
            CreateMap<Consigner, ConsignerDTO>().ReverseMap();
            CreateMap<SalesConfirmation, SalesConfirmationDTO>();
            CreateMap<SalesConfirmation, SalesConfirmationDTO>()
            .ForMember(dest => dest.SoldProducts, opt => opt.MapFrom(src => src.ProductSales))
            .ReverseMap();
            CreateMap<ProductSale, ProductSaleDTO>().ReverseMap();
            CreateMap<Memo, MemoDTO>().ReverseMap();
            CreateMap<GenericNotification, NotificationDTO>();
            CreateMap<RegisterDTO, Consignee>()
            .ForMember(dest => dest.NationalRegistryExpirationDate, opt => opt.MapFrom(src => ParseDate(src.NationalRegistryExpirationDate)))
            .ForMember(dest => dest.InsuranceCoverage, opt => opt.MapFrom(src => double.Parse(src.InsuranceCoverage, CultureInfo.InvariantCulture)));
        }

        private DateTime ParseDate(string date)
        {
            DateTime dateValue;
            string[] formats = { "d-M-yyyy", "M/d/yyyy", "dd-MM-yyyy", "MM/dd/yyyy", "yyyy-MM-dd", "MM-dd-yyyy", "M-d-yyyy", "yyyy/M/d"};
            if (DateTime.TryParseExact(date, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue))
            {
                return dateValue;
            }
            else
            {
                throw new Exception("Invalid date format.");
            }
        }

    }



}
