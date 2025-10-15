using AutoMapper;
using Reservation.DataContext.Dtos;
using Reservation.DataContext.Entities;

namespace Reservation.Service.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // User mappings
            CreateMap<User, UserDto>();
            CreateMap<CreateUserDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password)) // majd hash-elni kell
                .ForMember(dest => dest.Properties, opt => opt.Ignore())
                .ForMember(dest => dest.Bookings, opt => opt.Ignore())
                .ForMember(dest => dest.Comments, opt => opt.Ignore())
                .ForMember(dest => dest.Favorites, opt => opt.Ignore())
                .ForMember(dest => dest.Likes, opt => opt.Ignore())
                .ForMember(dest => dest.Deleted, opt => opt.Ignore());

            // Property mappings
            CreateMap<Property, PropertyDto>();
            CreateMap<CreatePropertyDto, Property>()
                .ForMember(dest => dest.Host, opt => opt.Ignore())
                .ForMember(dest => dest.Bookings, opt => opt.Ignore())
                .ForMember(dest => dest.Comments, opt => opt.Ignore())
                .ForMember(dest => dest.Favorites, opt => opt.Ignore())
                .ForMember(dest => dest.Likes, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Deleted, opt => opt.Ignore());

            // Booking mappings
            CreateMap<Booking, BookingDto>();
            CreateMap<CreateBookingDto, Booking>()
                .ForMember(dest => dest.Property, opt => opt.Ignore())
                .ForMember(dest => dest.Guest, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Deleted, opt => opt.Ignore());

            // Comment mappings
            CreateMap<Comment, CommentDto>();
            CreateMap<CreateCommentDto, Comment>()
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Property, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Deleted, opt => opt.Ignore());

            // Favorite mappings
            CreateMap<Favorite, FavoriteDto>();
            CreateMap<CreateFavoriteDto, Favorite>()
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Property, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Deleted, opt => opt.Ignore());

            // Like mappings
            CreateMap<Like, LikeDto>();
            CreateMap<CreateLikeDto, Like>()
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Property, opt => opt.Ignore())
                .ForMember(dest => dest.Comment, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Deleted, opt => opt.Ignore());
        }
    }
}
