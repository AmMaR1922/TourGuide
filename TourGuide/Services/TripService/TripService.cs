using ApplicationLayer.Contracts.Repositories;
using ApplicationLayer.Contracts.UnitToWork;
using ApplicationLayer.DTOs.TripDtos;
using ApplicationLayer.Models;
using ApplicationLayer.Models.SpecificationParameters;
using DomainLayer.Entities;

namespace TourGuide.Services.TripService
{
    public class TripService : ITripRepository
    {
        public IUnitOfWork UnitOfWork { get; }
        public TripService(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }


        public async Task<APIResponse<AddTripDto>> AddTrip(AddTripDto tripDto)
        {
            if (tripDto == null)
            {
                return APIResponse<AddTripDto>.FailureResponse(new List<string> { "Trip data cannot be null." }, "Failed to add trip.");
            }
            var tripRepository = UnitOfWork.Repository<DomainLayer.Entities.Trip>();
            bool tripExists = tripRepository.GetAll()
                .Any(t => t.Name.ToLower() == tripDto.Name.ToLower() && !t.IsDeleted);
            if (tripExists)
            {
                return APIResponse<AddTripDto>.FailureResponse(new List<string> { "Trip with this name already exists." }, "Failed to add trip.");
            }
            var activities = tripDto.Activities?.Select(a => new TripActivities { Activity = new Activity { Name = a } }).ToList() ?? new List<TripActivities>();
            var trip = new DomainLayer.Entities.Trip
            {
                Name = tripDto.Name,
                Description = tripDto.Description,
                CategoryId = tripDto.CategoryId,
                Price = tripDto.Price,
                Duration = tripDto.Duration,
                DateTime = tripDto.DateTime,
                IsAvailable = tripDto.IsAvailable,
                MeetingPoint = new MeetingPoint
                {
                    Address = tripDto.MeetingPointAddress,
                    URL = tripDto.MeetingPointURL
                },
                Activities = activities,
                TripLanguages = tripDto.Languages?
         .Select(l => new TripLanguages
         {
             Language = new Language
             {
                 Name = l,
                 Code = l // Ensure Code is set to a non-null value
             }
         }).ToList() ?? new List<TripLanguages>(),
                TripIncludes = tripDto.Includes?.Select(i => new TripIncludes { Includes = new Includes { Name = i } }).ToList() ?? new List<TripIncludes>(),
                TripImages = tripDto.ImageUrls?.Select(url => new TripImages { ImageURL = url }).ToList() ?? new List<TripImages>()
            };


            await tripRepository.AddAsync(trip);
            var saveResult = await UnitOfWork.CompleteAsync();
            if (!saveResult)
            {
                return APIResponse<AddTripDto>.FailureResponse(new List<string> { "Failed to save changes to the database." }, "Failed to add trip.");
            }
            return APIResponse<AddTripDto>.SuccessResponse(tripDto, "Trip added successfully.");


        }

        public async Task<APIResponse<List<TripDtoResponse>>> GetAllTrips(TripSpecParams Params)
        {
            var tripRepository = UnitOfWork.Repository<DomainLayer.Entities.Trip>();
            var spec = new Specification.GetAllTripSpecification(Params);
            var trips = tripRepository.GetAllWithSpecification(spec).Where(t => !t.IsDeleted).ToList();
            if (trips == null || !trips.Any())
            {
                return APIResponse<List<TripDtoResponse>>.FailureResponse(new List<string> { "No trips found." }, "Failed to retrieve trips.");
            }
            var tripDtos = trips.Select(t => new TripDtoResponse
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                CategoryId = t.CategoryId,
                Price = t.Price,
                Duration = t.Duration,
                DateTime = t.DateTime,
                IsAvailable = t.IsAvailable,
                MeetingPointAddress = t.MeetingPoint.Address,
                MeetingPointURL = t.MeetingPoint.URL,
                Activities = t.Activities.Select(a => a.Activity.Name).ToList(),
                Languages = t.TripLanguages.Select(l => l.Language.Name).ToList(),
                Includes = t.TripIncludes.Select(i => i.Includes.Name).ToList(),
                ImageUrls = t.TripImages.Select(img => img.ImageURL).ToList()
            }).ToList();
            return APIResponse<List<TripDtoResponse>>.SuccessResponse(tripDtos, "Trips retrieved successfully.");

        }

        public async Task<APIResponse<TripDtoResponse>> GetTripById(int Id)
        {

            var tripRepository = UnitOfWork.Repository<Trip>();
            var trip = await tripRepository.GetByIdAsync(Id);
            if (trip == null || trip.IsDeleted)
            {
                return APIResponse<TripDtoResponse>.FailureResponse(new List<string> { "Trip not found." }, "Failed to retrieve trip.");
            }
            var tripDto = new TripDtoResponse
            {
                Id = trip.Id,
                Name = trip.Name,
                Description = trip.Description,
                CategoryId = trip.CategoryId,
                Price = trip.Price,
                Duration = trip.Duration,
                DateTime = trip.DateTime,
                IsAvailable = trip.IsAvailable,
                MeetingPointAddress = trip.MeetingPoint.Address,
                MeetingPointURL = trip.MeetingPoint.URL,
                Activities = trip.Activities.Select(a => a.Activity.Name).ToList(),
                Languages = trip.TripLanguages.Select(l => l.Language.Name).ToList(),
                Includes = trip.TripIncludes.Select(i => i.Includes.Name).ToList(),
                ImageUrls = trip.TripImages.Select(img => img.ImageURL).ToList()
            };
            return APIResponse<TripDtoResponse>.SuccessResponse(tripDto, "Trip retrieved successfully.");


        }

        public async Task<APIResponse<UpdateTripDto>> UpdateTrip(UpdateTripDto tripDto, int Id)
        {
            if (tripDto == null)
            {
                return APIResponse<UpdateTripDto>.FailureResponse(new List<string> { "Trip data cannot be null." }, "Failed to update trip.");
            }
            var tripRepository = UnitOfWork.Repository<DomainLayer.Entities.Trip>();
            var trip = await tripRepository.GetByIdAsync(Id);
            if (trip == null || trip.IsDeleted)
            {
                return APIResponse<UpdateTripDto>.FailureResponse(new List<string> { "Trip not found." }, "Failed to update trip.");
            }
            bool tripExists = tripRepository.GetAll()
                .Any(t => t.Name.ToLower() == tripDto.Name.ToLower() && t.Id != Id && !t.IsDeleted);
            if (tripExists)
            {
                return APIResponse<UpdateTripDto>.FailureResponse(new List<string> { "Trip with this name already exists." }, "Failed to update trip.");
            }
            var activities = tripDto.Activities?.Select(a => new TripActivities { Activity = new Activity { Name = a } }).ToList() ?? new List<TripActivities>();
            trip.Name = tripDto.Name;
            trip.Description = tripDto.Description;
            trip.CategoryId = tripDto.CategoryId;
            trip.Price = tripDto.Price;
            trip.Duration = tripDto.Duration;
            trip.DateTime = tripDto.DateTime;
            trip.IsAvailable = tripDto.IsAvailable;
            trip.MeetingPoint.Address = tripDto.MeetingPointAddress;
            trip.MeetingPoint.URL = tripDto.MeetingPointURL;
            trip.Activities = activities;
            // Update languages
            if (tripDto.Languages != null)
            {
                var languages = new List<TripLanguages>();
                foreach (var lang in tripDto.Languages)
                {
                    languages.Add(new TripLanguages
                    {
                        Language = new Language
                        {
                            Name = lang,
                            Code = lang // Ensure Code is set to a non-null value
                        }
                    });
                }
                trip.TripLanguages = languages;
            }
            else
            {
                // If no languages provided, clear the existing ones
                trip.TripLanguages.Clear();
            }
            // Update includes
            if (tripDto.Includes != null)
            {
                var includes = new List<TripIncludes>();
                foreach (var inc in tripDto.Includes)
                {
                    includes.Add(new TripIncludes { Includes = new Includes { Name = inc } });
                }
                trip.TripIncludes = includes;
            }
            else
            {
                // If no includes provided, clear the existing ones
                trip.TripIncludes.Clear();
            }
            // Update images
            if (tripDto.ImageUrls != null)
            {
                var images = new List<TripImages>();
                foreach (var url in tripDto.ImageUrls)
                {
                    images.Add(new TripImages { ImageURL = url });
                }
                trip.TripImages = images;
            }
            else
            {
                // If no images provided, clear the existing ones
                trip.TripImages.Clear();
            }
            tripRepository.Update(trip);
            var saveResult = await UnitOfWork.CompleteAsync();
            if (!saveResult)
            {
                return APIResponse<UpdateTripDto>.FailureResponse(new List<string> { "Failed to save changes to the database." }, "Failed to update trip.");
            }
            return APIResponse<UpdateTripDto>.SuccessResponse(tripDto, "Trip updated successfully.");
        }
        public async Task<APIResponse<TripDtoResponse>> DeleteTrip(int Id)
        {
            var tripRepository = UnitOfWork.Repository<Trip>();
            var trip = await tripRepository.GetByIdAsync(Id);
            if (trip == null || trip.IsDeleted)
            {
                return APIResponse<TripDtoResponse>.FailureResponse(new List<string> { "Trip not found." }, "Failed to delete trip.");
            }
            trip.IsDeleted = true;

            tripRepository.Update(trip); 
            var saveResult = await UnitOfWork.CompleteAsync();
            if (!saveResult)
            {
                return APIResponse<TripDtoResponse>.FailureResponse(new List<string> { "Failed to save changes to the database." }, "Failed to delete trip.");
            }
            return APIResponse<TripDtoResponse>.SuccessResponse(new TripDtoResponse
            {
                Id = trip.Id,
                Name = trip.Name,
                Description = trip.Description,
                CategoryId = trip.CategoryId,
                Price = trip.Price,
                Duration = trip.Duration,
                DateTime = trip.DateTime,
                IsAvailable = trip.IsAvailable,
                MeetingPointAddress = trip.MeetingPoint.Address,
                MeetingPointURL = trip.MeetingPoint.URL
            }, "Trip deleted successfully.");
        }

         

        public async Task<APIResponse<List<TripDtoResponse>>> GetTripsByCategoryId(int categoryId)
        {

            var tripRepository = UnitOfWork.Repository<Trip>();
            var trips = tripRepository.GetAll()
                .Where(t => t.CategoryId == categoryId && !t.IsDeleted)
                .ToList();
            if (trips == null || !trips.Any())
            {
                return APIResponse<List<TripDtoResponse>>.FailureResponse(new List<string> { "No trips found for this category." }, "Failed to retrieve trips by category.");
            }
            var tripDtos = trips.Select(t => new TripDtoResponse
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                CategoryId = t.CategoryId,
                Price = t.Price,
                Duration = t.Duration,
                DateTime = t.DateTime,
                IsAvailable = t.IsAvailable,
                MeetingPointAddress = t.MeetingPoint.Address,
                MeetingPointURL = t.MeetingPoint.URL,
                Activities = t.Activities.Select(a => a.Activity.Name).ToList(),
                Languages = t.TripLanguages.Select(l => l.Language.Name).ToList(),
                Includes = t.TripIncludes.Select(i => i.Includes.Name).ToList(),
                ImageUrls = t.TripImages.Select(img => img.ImageURL).ToList()
            }).ToList();
            return APIResponse<List<TripDtoResponse>>.SuccessResponse(tripDtos, "Trips retrieved successfully by category.");


        }

        public async Task<APIResponse<List<TripDtoResponse>>> GetTripsByCategoryName(string categoryName)
        {

            var tripRepository = UnitOfWork.Repository<Trip>();
            var trips = tripRepository.GetAll()
                .Where(t => t.Category.Name.ToLower() == categoryName.ToLower() && !t.IsDeleted)
                .ToList();
            if (trips == null || !trips.Any())
            {
                return APIResponse<List<TripDtoResponse>>.FailureResponse(new List<string> { "No trips found for this category name." }, "Failed to retrieve trips by category name.");
            }
            var tripDtos = trips.Select(t => new TripDtoResponse
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                CategoryId = t.CategoryId,
                Price = t.Price,
                Duration = t.Duration,
                DateTime = t.DateTime,
                IsAvailable = t.IsAvailable,
                MeetingPointAddress = t.MeetingPoint.Address,
                MeetingPointURL = t.MeetingPoint.URL,
                Activities = t.Activities.Select(a => a.Activity.Name).ToList(),
                Languages = t.TripLanguages.Select(l => l.Language.Name).ToList(),
                Includes = t.TripIncludes.Select(i => i.Includes.Name).ToList(),
                ImageUrls = t.TripImages.Select(img => img.ImageURL).ToList()
            }).ToList();
            return APIResponse<List<TripDtoResponse>>.SuccessResponse(tripDtos, "Trips retrieved successfully by category name.");


        }

        public Task<APIResponse<List<TripDtoResponse>>> GetTripsByRating(double rating)
        {
            var tripRepository = UnitOfWork.Repository<Trip>();
            var trips = tripRepository.GetAll()
                .Where(t => t.Rating >= rating && !t.IsDeleted)
                .ToList();
            if (trips == null || !trips.Any())
            {
                return Task.FromResult(APIResponse<List<TripDtoResponse>>.FailureResponse(new List<string> { "No trips found with the specified rating." }, "Failed to retrieve trips by rating."));
            }
            var tripDtos = trips.Select(t => new TripDtoResponse
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                CategoryId = t.CategoryId,
                Price = t.Price,
                Duration = t.Duration,
                DateTime = t.DateTime,
                IsAvailable = t.IsAvailable,
                MeetingPointAddress = t.MeetingPoint.Address,
                MeetingPointURL = t.MeetingPoint.URL,
                Activities = t.Activities.Select(a => a.Activity.Name).ToList(),
                Languages = t.TripLanguages.Select(l => l.Language.Name).ToList(),
                Includes = t.TripIncludes.Select(i => i.Includes.Name).ToList(),
                ImageUrls = t.TripImages.Select(img => img.ImageURL).ToList()
            }).ToList();
            return Task.FromResult(APIResponse<List<TripDtoResponse>>.SuccessResponse(tripDtos, "Trips retrieved successfully by rating."));

        }
    }
}
