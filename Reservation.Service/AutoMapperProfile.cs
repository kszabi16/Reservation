using AutoMapper;
using Reservation.DataContext.Dtos;
using Reservation.DataContext.Entities;

namespace Reservation.Service.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDto>();

            CreateMap<CreateUserDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.Properties, opt => opt.Ignore())
                .ForMember(dest => dest.Bookings, opt => opt.Ignore())
                .ForMember(dest => dest.Comments, opt => opt.Ignore())
                .ForMember(dest => dest.Favorites, opt => opt.Ignore())
                .ForMember(dest => dest.Likes, opt => opt.Ignore())
                .ForMember(dest => dest.Deleted, opt => opt.Ignore())
                .ForMember(dest => dest.PhoneNumber, opt => opt.Ignore())
                .ForMember(dest => dest.Location, opt => opt.Ignore())
                .ForMember(dest => dest.Bio, opt => opt.Ignore())
                .ForMember(dest => dest.AvatarUrl, opt => opt.Ignore());

            CreateMap<UpdateUserProfileDto, User>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Property, PropertyDto>()
                .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src =>
                    src.Ratings != null && src.Ratings.Any() ? src.Ratings.Average(r => r.Score) : 0))
                .ForMember(dest => dest.ReviewCount, opt => opt.MapFrom(src =>
                    src.Ratings != null ? src.Ratings.Count : 0)) 
                .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src =>
                    src.Images != null ? src.Images.Select(i => i.ImageUrl).ToList() : new List<string>()))
                .ForMember(dest => dest.Amenities, opt => opt.MapFrom(src =>
                    src.PropertyAmenities != null ? src.PropertyAmenities.Select(pa => pa.Amenity.Name).ToList() : new List<string>()));

            CreateMap<CreatePropertyDto, Property>()
                .ForMember(dest => dest.Host, opt => opt.Ignore())
                .ForMember(dest => dest.Bookings, opt => opt.Ignore())
                .ForMember(dest => dest.Comments, opt => opt.Ignore())
                .ForMember(dest => dest.Favorites, opt => opt.Ignore())
                .ForMember(dest => dest.Likes, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Deleted, opt => opt.Ignore());

            CreateMap<Booking, BookingDto>()
                .ForMember(dest => dest.PropertyTitle, opt => opt.MapFrom(src => src.Property.Title));
            CreateMap<CreateBookingDto, Booking>()
                .ForMember(dest => dest.Property, opt => opt.Ignore())
                .ForMember(dest => dest.Guest, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Deleted, opt => opt.Ignore());

            CreateMap<Comment, CommentDto>()
                .ForMember(dest => dest.UserAvatarUrl, opt => opt.MapFrom(src => src.User.AvatarUrl));
            CreateMap<CreateCommentDto, Comment>()
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Property, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Deleted, opt => opt.Ignore());

            CreateMap<Favorite, FavoriteDto>();
            CreateMap<CreateFavoriteDto, Favorite>()
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Property, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Deleted, opt => opt.Ignore());

            CreateMap<Like, LikeDto>();
            CreateMap<CreateLikeDto, Like>()
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Property, opt => opt.Ignore())
                .ForMember(dest => dest.Comment, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Deleted, opt => opt.Ignore());

            CreateMap<HostRequest, HostRequestDto>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username))
                .ForMember(dest => dest.PropertyTitle, opt => opt.MapFrom(src => src.Property.Title))
                .ForMember(dest => dest.ApprovedAt, opt => opt.MapFrom(src => src.ApprovedAt));

            CreateMap<CreateHostRequestDto, HostRequest>();

            CreateMap<Rating, RatingDto>();
            CreateMap<CreateRatingDto, Rating>()
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Property, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Deleted, opt => opt.Ignore());

            CreateMap<SystemLog, SystemLogDto>();

            
        }

    }
}
