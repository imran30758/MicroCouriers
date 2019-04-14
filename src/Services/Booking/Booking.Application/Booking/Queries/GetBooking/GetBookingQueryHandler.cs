﻿using Booking.Application.Booking.Queries.DTO;
using Booking.Domain.AggregatesModel.BookingAggregate;
using Booking.Domain.Booking;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Booking.Application.Booking.Queries.GetBooking
{
    public class GetBookingQueryHandler : IRequestHandler<GetBookingQuery, BookingOrderDTO>
    {
        private readonly IBookingRespository _context;

        public GetBookingQueryHandler(IBookingRespository context)
        {
            _context = context;
        }

        public async Task<BookingOrderDTO> Handle(GetBookingQuery request, CancellationToken cancellationToken)
        {
            BookingOrder bookingObj = await _context.FindByIdAsync(request.BookingId);

            if (bookingObj == null)
            {
                return null;
            }

            //Get the Booking
            var bookingDTO = new BookingOrderDTO
            {
                BookingOrderId = bookingObj.BookingOrderId,
                CustomerId = bookingObj.CustomerID,
                Origin = bookingObj.Origin,
                Destination = bookingObj.Destination                
            };

            //Get the Booking Details
            ICollection<BookingOrderDetailDTO> listBoookingDetails = new List<BookingOrderDetailDTO>();
            foreach (BookingOrderDetail bookdetails in bookingObj.BookingDetails)
            {
                var bookingDetailsObj = new BookingOrderDetailDTO
                {
                    Price = bookdetails.Price,
                    Description = bookdetails.PackageDescription,
                    PackageType = bookdetails.PackageType
                };

                listBoookingDetails.Add(bookingDetailsObj);
            }

            //Fill the Main booking with Details
            bookingDTO.BookingDetails = listBoookingDetails;

            //Return Complete Object
            return bookingDTO;
        }
    }
}
