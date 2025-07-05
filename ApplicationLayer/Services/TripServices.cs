using ApplicationLayer.Contracts.Services;
using ApplicationLayer.Contracts.UnitToWork;
using ApplicationLayer.DTOs;
using ApplicationLayer.DTOs.Trip;
using ApplicationLayer.DTOs.TripDtos;
using ApplicationLayer.Helper;
using ApplicationLayer.Models;
using ApplicationLayer.QueryParams;
using ApplicationLayer.Specifications.TripSpecifictions;
using DomainLayer.Entities;
using Microsoft.AspNetCore.Http;
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
            var Specs = new GetAllTripsWithSpecs(Params, isAdmin);
            var Trips = await unitOfWork.Repository<Trip>().GetAllWithSpecification(Specs)
                .Select(trip => new TripDTOResponse
                {
                    Id = trip.Id,
                    Name = trip.Name,
                    Duration = trip.DurationInMinutes,
                    Price = trip.Price,
                    IsAvailable = isAdmin ? trip.IsAvailable : null,
                    DateTime = trip.DateTime,
                    Category = trip.Category.Name,
                    Rating = trip.TripReviews.Any() ? trip.TripReviews.Average(r => r.Rating) : 0,
                    IsBestSeller = trip.IsBestSeller,
                    Reviews = trip.TripReviews.Count(),
                    MainImageURL = URLResolver.BuildFileUrl(trip.TripImages.Where(i => i.IsMainImage).Select(img => img.ImageURL).FirstOrDefault())
                })
                .ToListAsync();

            var CountSpecs = new CountAllTripsWithSpecs(Params , isAdmin);
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
                    Duration = T.DurationInMinutes,
                    Description = T.Description,
                    Price = T.Price,
                    IsAvailable = T.IsAvailable,
                    DateTime = T.DateTime,
                    Rating = T.TripReviews.Any() ? T.TripReviews.Average(r => r.Rating) : 0,
                    IsBestSeller = T.IsBestSeller,
                    CategoryId = T.CategoryId,
                    MeetingPoint = T.MeetingPoint,
                    Activities = T.Activities.Select(ta => ta.Activity.Name).ToList(),
                    Languages = T.TripLanguages.Select(tl => tl.Language.Name).ToList(),
                    Includes = T.TripIncludes.Where(ti => ti.IsIncluded).Select(ti => ti.Includes.Name).ToList(),
                    NotIncludes = T.TripIncludes.Where(ti => !ti.IsIncluded).Select(ti => ti.Includes.Name).ToList(),
                    MainImage = T.TripImages.Where(i => i.IsMainImage).Select( i => new ImageDTO { Id = i.Id, ImageURL = URLResolver.BuildFileUrl(i.ImageURL) }).FirstOrDefault(),
                    Images = T.TripImages.Where(i => i.IsMainImage == false).Select(img => new ImageDTO {Id = img.Id, ImageURL = URLResolver.BuildFileUrl(img.ImageURL) }).ToList(),
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


            var Trip = new Trip()
            {
                Name = TripDto.Name,
                DurationInMinutes = TripDto.Duration,
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

            using var transaction = await unitOfWork.BeginTransactionAsync();

            try
            {
                #region Activities
                var activities = await unitOfWork.Repository<Activity>()
                                        .GetAll()
                                        .Where(a => TripDto.Activities.Contains(a.Id))
                                        .ToListAsync();

                Trip.Activities.AddRange(
                  activities.Select(a => new TripActivities { ActivityId = a.Id }));
                #endregion

                #region Languages
                var languages = await unitOfWork.Repository<Language>()
                                        .GetAll()
                                        .Where(l => TripDto.Languages.Contains(l.Id))
                                        .ToListAsync();

                Trip.TripLanguages.AddRange(
                    languages.Select(l => new TripLanguages { LanguageId = l.Id }));
                #endregion

                #region Includes
                var Includes = await unitOfWork.Repository<Includes>()
                                                .GetAll()
                                                .Where(i => TripDto.Includes.Contains(i.Id))
                                                .ToListAsync();

                Trip.TripIncludes.AddRange(
                    Includes.Select(i => new TripIncludes { IncludesId = i.Id, IsIncluded = true }));
                #endregion

                #region NotIncludes
                Includes = await unitOfWork.Repository<Includes>()
                                                .GetAll()
                                                .Where(i => TripDto.NotIncludes.Contains(i.Id))
                                                .ToListAsync();

                Trip.TripIncludes.AddRange(
                    Includes.Select(i => new TripIncludes { IncludesId = i.Id, IsIncluded = false }));
                #endregion

                string MainImageUrl = await FileHandler.SaveFileAsync("TripImages", TripDto.MainImage);

                if (MainImageUrl is null)
                    throw new Exception("Main Image cannot be null");

                Trip.TripImages.Add(new TripImages()
                {
                    ImageURL = MainImageUrl,
                    IsMainImage = true
                });

                await unitOfWork.Repository<Trip>().AddAsync(Trip);

                var result = await unitOfWork.CompleteAsync();
                if (!result)
                    throw new Exception("An Error Ocurred.");

                await transaction.CommitAsync();

                return APIResponse<string>.SuccessResponse(201, null, "Trip added successfully.");
            }
            catch (Exception ex)
            {
                FileHandler.DeleteFile(Trip.TripImages.Find(i => i.IsMainImage)?.ImageURL);

                await transaction.RollbackAsync();
                return APIResponse<string>.FailureResponse(500, null, "Failed To Add Trip.");
            }
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

            var trip = await unitOfWork.Repository<Trip>().GetByIdAsync(Id);
            if (trip == null)
                return APIResponse<string>.FailureResponse(404, null, "Trip Not found.");

            using var transaction = await unitOfWork.BeginTransactionAsync();
            
            trip.Name = TripDto.Name;
            trip.Description = TripDto.Description;
            trip.DurationInMinutes = TripDto.Duration;
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
            trip.TripIncludes.Clear();

            var includes = await unitOfWork.Repository<Includes>().GetAll()
                .Where(i => TripDto.Includes.Contains(i.Id))
                .ToListAsync();

            trip.TripIncludes.AddRange(includes.Select(ti => new TripIncludes { IncludesId = ti.Id, IsIncluded = true }));

            includes.Clear();

            includes = await unitOfWork.Repository<Includes>().GetAll()
                .Where(i => TripDto.NotIncludes.Contains(i.Id))
                .ToListAsync();

            trip.TripIncludes.AddRange(includes.Select(ti => new TripIncludes { IncludesId = ti.Id, IsIncluded = false }));
            #endregion

            FileHandler.DeleteFile(trip.TripImages.Where(i => i.IsMainImage).Select(i => i.ImageURL).FirstOrDefault());

            trip.TripImages.Add(new TripImages()
            {
                ImageURL = await FileHandler.SaveFileAsync("TripImages", TripDto.MainImage),
                IsMainImage = true
            });
            
            unitOfWork.Repository<Trip>().Update(trip);
            result = await unitOfWork.CompleteAsync();

            if (!result)
            {
                await transaction.RollbackAsync();
                return APIResponse<string>.FailureResponse(500, null, "Failed to Update trip.");
            }

            await transaction.CommitAsync();
            return APIResponse<string>.SuccessResponse(200, null, "Trip Updated Successfully.");
        }

        public async Task<APIResponse<string>> AddImagesToTrip(int tripId, List<IFormFile> files)
        {
            var trip = await unitOfWork.Repository<Trip>().GetByIdAsync(tripId);
            if (trip == null)
                return APIResponse<string>.FailureResponse(404, null, "Trip not found.");

            var tripImagesIds = trip.TripImages.Select(i => i.Id).ToList();

            using var transaction = await unitOfWork.BeginTransactionAsync();
            try
            {
                foreach (var file in files)
                {
                    if (file != null)
                    {
                        var imageUrl = await FileHandler.SaveFileAsync("TripImages", file);
                        if (imageUrl is not null)
                        {
                            trip.TripImages.Add(new TripImages { ImageURL = imageUrl, IsMainImage = false });
                        }
                    }
                }

                var result = await unitOfWork.CompleteAsync();
                if (!result)
                    throw new Exception("An Error Ocurred.");

                await transaction.CommitAsync();
                return APIResponse<string>.SuccessResponse(201, null, "Images added successfully.");
            }
            catch (Exception ex)
            {
                foreach (var image in trip.TripImages)
                {
                    if (!tripImagesIds.Contains(image.Id)) 
                    FileHandler.DeleteFile(image.ImageURL);
                }

                await transaction.RollbackAsync();
                return APIResponse<string>.FailureResponse(500, null, "Failed to add images.");
            }
        }
    
        public async Task<APIResponse<string>> DeleteImagesFromTrip(int tripId, List<int> imageIds)
        {
            var trip = await unitOfWork.Repository<Trip>().GetByIdAsync(tripId);
            if (trip == null)
                return APIResponse<string>.FailureResponse(404, null, "Trip not found.");

            var TripImagesURLs = new List<string>();

            using var transaction = await unitOfWork.BeginTransactionAsync();
            try
            {
                foreach (var imageId in imageIds)
                {
                    var image = await unitOfWork.Repository<TripImages>().GetByIdAsync(imageId);
                    if (image != null)
                    {
                        TripImagesURLs.Add(image.ImageURL);
                        trip.TripImages.Remove(image);
                    }
                }

                var result = await unitOfWork.CompleteAsync();
                if (!result)
                    throw new Exception("An Error Ocurred.");

                foreach (var imageUrl in TripImagesURLs)
                { 
                    if (imageUrl != null)
                    {
                        FileHandler.DeleteFile(imageUrl);
                    }
                }

                await transaction.CommitAsync();
                return APIResponse<string>.SuccessResponse(200, null, "Images deleted successfully.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return APIResponse<string>.FailureResponse(500, null, "Failed to delete images.");
            }
        }
    }
}
