using ApplicationLayer.Contracts.Services;
using ApplicationLayer.Contracts.UnitToWork;
using ApplicationLayer.DTOs.Trip;
using ApplicationLayer.DTOs.TripDtos;
using ApplicationLayer.Helper;
using ApplicationLayer.Models;
using ApplicationLayer.QueryParams;
using ApplicationLayer.Specifications.TripSpecifictions;
using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Services
{
    public class TripServices : ITripServices
    {
        private readonly IUnitOfWork unitOfWork;

        public TripServices(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<APIResponse<Pagination<TripDTOResponse>>> GetAll(TripSpecParams Params, bool isAdmin)
        {
            var Specs = new GetAllTripsWithSpecs(Params);
            var Trips = await unitOfWork.Repository<Trip>().GetAllWithSpecification(Specs)
                .Select(trip => new TripDTOResponse
                {
                    Id = trip.Id,
                    Name = trip.Name,
                    Duration = trip.Duration,
                    Price = trip.Price,
                    IsAvailable = isAdmin ? trip.IsAvailable : null,
                    DateTime = trip.DateTime,
                    Category = trip.Category.Name,
                    Rating = trip.TripReviews.Average(r => r.Rating),
                    IsBestSeller = trip.IsBestSeller,
                    Reviews = trip.TripReviews.Count(),
                    MainImageURL = URLResolver.BuildFileUrl(trip.TripImages.Where(i => i.IsMainImage).Select(img => img.ImageURL).FirstOrDefault())
                })
                .ToListAsync();

            var CountSpecs = new CountAllTripsWithSpecs(Params);
            var Count = await unitOfWork.Repository<Trip>().GetCountWithSpecs(CountSpecs);

            var Pagination = new Pagination<TripDTOResponse>(Params.PageNumber, Params.PageSize, Count, Trips);

            return APIResponse<Pagination<TripDTOResponse>>.SuccessResponse(200, Pagination, "Trips retrieved successfully.");
        }

        public async Task<APIResponse<TripToBeReturnedDTO>> GetById(int Id)
        {
            var trip = await unitOfWork.Repository<Trip>().GetAll().Where(t => t.Id == Id)
                .Select(T => new TripToBeReturnedDTO()
                {
                    Name = T.Name,
                    Duration = T.Duration,
                    Description = T.Description,
                    Price = T.Price,
                    IsAvailable = T.IsAvailable,
                    DateTime = T.DateTime,
                    Rating = T.TripReviews.Average(r => r.Rating),
                    IsBestSeller = T.IsBestSeller,
                    CategoryId = T.CategoryId,
                    MeetingPoint = T.MeetingPoint,
                    Activities = T.Activities.Select(ta => ta.Activity.Name).ToList(),
                    Languages = T.TripLanguages.Select(tl => tl.Language.Name).ToList(),
                    Includes = T.TripIncludes.Where(ti => ti.IsIncluded).Select(ti => ti.Includes.Name).ToList(),
                    NotIncludes = T.TripIncludes.Where(ti => !ti.IsIncluded).Select(ti => ti.Includes.Name).ToList(),
                    MainImage = URLResolver.BuildFileUrl(T.TripImages.Where(i => i.IsMainImage).Select(img => img.ImageURL).FirstOrDefault()),
                    Images = T.TripImages.Select(img => URLResolver.BuildFileUrl(img.ImageURL)).ToList(),
                }).FirstOrDefaultAsync();

            return trip is not null 
                ? APIResponse<TripToBeReturnedDTO>.SuccessResponse(200, trip, "Trip retrieved successfully.")
                : APIResponse<TripToBeReturnedDTO>.FailureResponse(404, null, "Trip not found.");
        }

        public async Task<APIResponse<string>> Add(TripToBeAddedDTO TripDto)
        {
            var TripExists = unitOfWork.Repository<Trip>().GetAll().Any(t => t.Name == TripDto.Name);

            if (TripExists)
                return APIResponse<string>.FailureResponse(400, null, "Trip with the same name already exists.");

            bool result = false;

            using (var transaction = await unitOfWork.BeginTransactionAsync())
            {
                var Trip = new Trip()
                {
                    Name = TripDto.Name,
                    Duration = TripDto.Duration,
                    Description = TripDto.Description,
                    Price = TripDto.Price,
                    IsAvailable = TripDto.IsAvailable,
                    DateTime = TripDto.DateTime,
                    CategoryId = TripDto.CategoryId,
                    MeetingPoint = new MeetingPoint
                    {
                        Address = TripDto.MeetingPointAddress,
                        URL = TripDto.MeetingPointURL
                    }
                };

                foreach (var activityId in TripDto.Activities)
                {
                    var activity = await unitOfWork.Repository<Activity>().GetByIdAsync(activityId);
                    if (activity != null)
                        Trip.Activities.Add(new TripActivities { Activity = activity });
                }

                foreach (var languageId in TripDto.Languages)
                {
                    var language = await unitOfWork.Repository<Language>().GetByIdAsync(languageId);
                    if (language != null)
                        Trip.TripLanguages.Add(new TripLanguages { Language = language });
                }

                foreach (var includeId in TripDto.Includes)
                {
                    var include = await unitOfWork.Repository<Includes>().GetByIdAsync(includeId);
                    if (include != null)
                        Trip.TripIncludes.Add(new TripIncludes { Includes = include, IsIncluded = true });
                }

                foreach (var notIncludeId in TripDto.NotIncludes)
                {
                    var notInclude = await unitOfWork.Repository<Includes>().GetByIdAsync(notIncludeId);
                    if (notInclude != null)
                        Trip.TripIncludes.Add(new TripIncludes { Includes = notInclude, IsIncluded = false });
                }

                foreach (var image in TripDto.Images)
                {
                    if (image != null)
                    {
                        var imageUrl = await FileHandler.SaveFileAsync("TripImages", image);
                        if (imageUrl is not null)
                            Trip.TripImages.Add(new TripImages { ImageURL = imageUrl, IsMainImage = false });
                    }
                }

                Trip.TripImages.Add(new TripImages()
                {
                    ImageURL = await FileHandler.SaveFileAsync("TripImages", TripDto.MainImage),
                    IsMainImage = true
                });


                await unitOfWork.Repository<Trip>().AddAsync(Trip);
                result = await unitOfWork.CompleteAsync();
            }

            if (!result)
                return APIResponse<string>.FailureResponse(500, null, "Failed to add trip.");
            
            return APIResponse<string>.SuccessResponse(201, null, "Trip added successfully.");

        }

        public async Task<APIResponse<string>> Delete(int Id)
        {
            var Trip = await unitOfWork.Repository<Trip>().GetByIdAsync(Id);
            if (Trip == null)
                return APIResponse<string>.FailureResponse(500, null, "Trip not found.");

            Trip.IsDeleted = true;
            unitOfWork.Repository<Trip>().Update(Trip);
            var result = await unitOfWork.CompleteAsync();

            if (!result)
                return APIResponse<string>.FailureResponse(500, null, "An error ocurred.");
            return APIResponse<string>.SuccessResponse(200, null, "Trip deleted successfully.");
        }

        public async Task<APIResponse<string>> Update(int Id, TripToBeUpdatedDTO TripDto)
        {
            bool result = false;
            using (var transaction = await unitOfWork.BeginTransactionAsync())
            {
                var trip = await unitOfWork.Repository<Trip>().GetByIdAsync(Id);
                if (trip == null)
                    return APIResponse<string>.FailureResponse(404, null, "Trip Not found.");

                trip.Name = TripDto.Name;
                trip.Description = TripDto.Description;
                trip.Duration = TripDto.Duration;
                trip.Price = TripDto.Price;
                trip.IsAvailable = TripDto.IsAvailable;
                trip.DateTime = TripDto.DateTime;
                trip.MeetingPoint.Address = TripDto.MeetingPointAddress;
                trip.MeetingPoint.URL = TripDto.MeetingPointURL;
                trip.CategoryId = TripDto.CategoryId;

                #region Activity
                //foreach (var tripActivity in trip.Activities)
                //{
                //    unitOfWork.Repository<TripActivities>().Delete(tripActivity);
                //}
                trip.Activities.Clear(); // Buld deletion 

                foreach (var activityId in TripDto.Activities)
                {
                    var activity = await unitOfWork.Repository<Activity>().GetByIdAsync(activityId);
                    if (activity != null)
                        trip.Activities.Add(new TripActivities { Activity = activity });
                }
                #endregion

                #region Languages
                //foreach (var tripLanguage in trip.TripLanguages)
                //{
                //    unitOfWork.Repository<TripLanguages>().Delete(tripLanguage);
                //}
                trip.TripLanguages.Clear();

                foreach (var languageId in TripDto.Languages)
                {
                    var language = await unitOfWork.Repository<Language>().GetByIdAsync(languageId);
                    if (language != null)
                        trip.TripLanguages.Add(new TripLanguages { Language = language });
                }
                #endregion

                #region Includes
                //foreach(var tripInclude in trip.TripIncludes)
                //{
                //    unitOfWork.Repository<TripIncludes>().Delete(tripInclude);
                //}
                trip.TripIncludes.Clear();

                foreach (var includeId in TripDto.Includes)
                {
                    var include = await unitOfWork.Repository<Includes>().GetByIdAsync(includeId);
                    if (include != null)
                        trip.TripIncludes.Add(new TripIncludes { Includes = include, IsIncluded = true });
                }
                #endregion

                #region NotInclude
                foreach (var NotIncludeId in TripDto.Includes)
                {
                    var include = await unitOfWork.Repository<Includes>().GetByIdAsync(NotIncludeId);
                    if (include != null)
                        trip.TripIncludes.Add(new TripIncludes { Includes = include, IsIncluded = false });
                }
                #endregion

                #region Images
                foreach (var image in trip.TripImages)
                {
                    FileHandler.DeleteFile(image.ImageURL);
                }

                trip.TripImages.Clear();

                foreach (var image in TripDto.Images)
                {
                    string url = await FileHandler.SaveFileAsync("TripImages", image);
                    if (url != null)
                    {
                        trip.TripImages.Add(new TripImages { ImageURL = url, IsMainImage = false });
                    }
                }
                #endregion

                trip.TripImages.Add(new TripImages()
                {
                    ImageURL = await FileHandler.SaveFileAsync("TripImages", TripDto.MainImage),
                    IsMainImage = true
                });

                unitOfWork.Repository<Trip>().Update(trip);
                result = await unitOfWork.CompleteAsync();
            }

            if (!result)
                return APIResponse<string>.FailureResponse(500, null, "An Error Occured.");
            return APIResponse<string>.SuccessResponse(200, null, "Trip Updated Successfully.");
        }
    }
}
