using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using Project.Interfaces;
using Project.Models;
using Project.Services.Properties;

namespace Project.Services
{
    public class TicketRepository : BaseRepository, ITicketRepository
    {
        private readonly IUserRepository _userRepository;

        public TicketRepository(IUnitOfWork unitOfWork, IUserRepository userRepository) : base(unitOfWork)
        {
            _userRepository = userRepository;
        }

        public Ticket GetTicket(long ticketId)
        {
            try
            {
                return GetDbSet<Ticket>().FirstOrDefault(s => s.TicketId == ticketId);
            }
            catch (InvalidOperationException e)
            {
                throw new BusinessServiceException(Resources.UnableToGetTicketExceptionMessage, e);
            }
        }

        public Ticket GetTicket(long userId, long ticketId)
        {
            try
            {
                return GetDbSet<Ticket>().FirstOrDefault(s => s.User.UserId == userId && s.TicketId == ticketId);
            }
            catch (InvalidOperationException e)
            {
                throw new BusinessServiceException(Resources.UnableToGetTicketExceptionMessage, e);
            }
        }

        public Ticket GetTicketLocal(long? ticketId)
        {
            if (ticketId == null) return null;

            try
            {
                return GetDbSet<Ticket>().FirstOrDefault(s => s.TicketId == ticketId && 
                                                            s.Language == (CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "ru" ? 1 : 2));
            }
            catch (InvalidOperationException e)
            {
                throw new BusinessServiceException(Resources.UnableToGetTicketExceptionMessage, e);
            }
        }

        public ReadOnlyCollection<Ticket> GetUserTickets(long userId, bool paged)
        {
            try
            {
                var tickets = GetDbSet<Ticket>().Where(s => s.User.UserId == userId).OrderByDescending(s => s.TicketId);

                if (paged)
                    tickets = TakePageItems(tickets);

                return new ReadOnlyCollection<Ticket>(tickets.ToList());
            }
            catch (InvalidOperationException e)
            {
                throw new BusinessServiceException(Resources.UnableToGetTicketsExceptionMessage, e);
            }
        }

        public ReadOnlyCollection<Ticket> GetUserTicketsLocal(long? userId, bool paged)
        {
            if (userId == null) return null;

            try
            {
                var tickets = GetDbSet<Ticket>().Where(s => s.User.UserId == userId &&
                                                        s.Language == (CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "ru" ? 1 : 2))
                                            .OrderByDescending(s => s.TicketId);
                if (paged)
                    tickets = TakePageItems(tickets);

                return new ReadOnlyCollection<Ticket>(tickets.ToList());
            }
            catch (InvalidOperationException e)
            {
                throw new BusinessServiceException(Resources.UnableToGetTicketsExceptionMessage, e);
            }
        }

        public ReadOnlyCollection<Ticket> GetAllTickets(long mainAdminId, bool paged)
        {
            try
            {
                var tickets = GetDbSet<Ticket>().Where(s => s.User == null || s.User.UserId != mainAdminId).OrderByDescending(s => s.TicketId);

                if (paged)
                    tickets = TakePageItems(tickets);

                return new ReadOnlyCollection<Ticket>(tickets.ToList());
            }
            catch (InvalidOperationException e)
            {
                throw new BusinessServiceException(Resources.UnableToGetTicketsExceptionMessage, e);
            }
        }

        public ReadOnlyCollection<Ticket> GetAllTicketsLocal(long mainAdminId, bool paged)
        {
            try
            {
                var tickets = GetDbSet<Ticket>().Where(s => (s.User == null || s.User.UserId != mainAdminId) &&
                                                        (s.Language == (CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "ru" ? 1 : 2)))
                                            .OrderByDescending(s => s.TicketId);
                if (paged)
                    tickets = TakePageItems(tickets);

                return new ReadOnlyCollection<Ticket>(tickets.ToList());
            }
            catch (InvalidOperationException e)
            {
                throw new BusinessServiceException(Resources.UnableToGetTicketsExceptionMessage, e);
            }
        }

        public ReadOnlyCollection<Ticket> GetTicketsByDeletedUsers(bool paged)
        {
            try
            {
                var tickets = GetDbSet<Ticket>().Where(s => s.User == null)
                                            .OrderByDescending(s => s.TicketId);
                if (paged)
                    tickets = TakePageItems(tickets);

                return new ReadOnlyCollection<Ticket>(tickets.ToList());
            }
            catch (InvalidOperationException e)
            {
                throw new BusinessServiceException(Resources.UnableToGetTicketsExceptionMessage, e);
            }
        }

        public long CountUserTickets(long userId)
        {
            try
            {
                return GetDbSet<Ticket>().LongCount(s => s.User.UserId == userId);
            }
            catch (InvalidOperationException e)
            {
                throw new BusinessServiceException(Resources.UnableToCountTicketsExceptionMessage, e);
            }
        }

        public long CountTicketsByDeletedUsers()
        {
            try
            {
                return GetDbSet<Ticket>().LongCount(s => s.User == null);
            }
            catch (InvalidOperationException e)
            {
                throw new BusinessServiceException(Resources.UnableToCountTicketsExceptionMessage, e);
            }
        }

        public long CountAllTickets(long mainAdminId)
        {
            try
            {
                return GetDbSet<Ticket>().LongCount(s => s.User == null || s.User.UserId != mainAdminId);
            }
            catch (InvalidOperationException e)
            {
                throw new BusinessServiceException(Resources.UnableToCountTicketsExceptionMessage, e);
            }
        }

        public void CreateTicket(long userId, Ticket newTicket)
        {
            try
            {
                User user = _userRepository.GetUser(userId);
                if (user == null) return;

                newTicket.User = user;
                newTicket.Country = (newTicket.Country != null) ? newTicket.Country.Trim() : null;
                newTicket.City = (newTicket.City != null) ? newTicket.City.Trim() : null;
                newTicket.PlaceDescription = newTicket.PlaceDescription.Replace("\r\n", " ").Trim();
                newTicket.LookDescription = newTicket.LookDescription.Replace("\r\n", " ").Trim();
                newTicket.AdditionalNote = (newTicket.AdditionalNote != null) ? newTicket.AdditionalNote.Replace("\r\n", " ").Trim() : null;
                newTicket.Firstname = (newTicket.Firstname != null) ? newTicket.Firstname.Trim() : null;
                newTicket.Lastname = (newTicket.Lastname != null) ? newTicket.Lastname.Trim() : null;
                newTicket.Language = (short) (CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "ru" ? 1 : 2);
                
                GetDbSet<Ticket>().Add(newTicket);
                UnitOfWork.SaveChanges();
            }
            catch (InvalidOperationException e)
            {
                throw new BusinessServiceException(Resources.UnableToCreateTicketExceptionMessage, e);
            }
        }

        public void UpdateTicket(Ticket updatedTicket)
        {
            try
            {
                var ticket = GetTicket(updatedTicket.TicketId);
                if (ticket == null) return;

                updatedTicket.Country = (updatedTicket.Country != null) ? updatedTicket.Country.Trim() : null;
                updatedTicket.City = (updatedTicket.City != null) ? updatedTicket.City.Trim() : null;
                updatedTicket.PlaceDescription = updatedTicket.PlaceDescription.Replace("\r\n", " ").Trim();
                updatedTicket.LookDescription = updatedTicket.LookDescription.Replace("\r\n", " ").Trim();
                updatedTicket.AdditionalNote = (updatedTicket.AdditionalNote != null) ? updatedTicket.AdditionalNote.Replace("\r\n", " ").Trim() : null;
                updatedTicket.Firstname = (updatedTicket.Firstname != null) ? updatedTicket.Firstname.Trim() : null;
                updatedTicket.Lastname = (updatedTicket.Lastname != null) ? updatedTicket.Lastname.Trim() : null;

                SetEntityState(ticket, EntityState.Modified);
                UnitOfWork.SaveChanges();
            }
            catch (InvalidOperationException e)
            {
                throw new BusinessServiceException(Resources.UnableToUpdateTicketExceptionMessage, e);
            }
        }

        public void UpdateTicket(long userId, Ticket updatedTicket)
        {
            try
            {
                var ticket = GetTicket(userId, updatedTicket.TicketId);
                if (ticket == null) return;

                updatedTicket.Country = (updatedTicket.Country != null) ? updatedTicket.Country.Trim() : null;
                updatedTicket.City = (updatedTicket.City != null) ? updatedTicket.City.Trim() : null;
                updatedTicket.PlaceDescription = updatedTicket.PlaceDescription.Replace("\r\n", " ").Trim();
                updatedTicket.LookDescription = updatedTicket.LookDescription.Replace("\r\n", " ").Trim();
                updatedTicket.AdditionalNote = (updatedTicket.AdditionalNote != null) ? updatedTicket.AdditionalNote.Replace("\r\n", " ").Trim() : null;
                updatedTicket.Firstname = (updatedTicket.Firstname != null) ? updatedTicket.Firstname.Trim() : null;
                updatedTicket.Lastname = (updatedTicket.Lastname != null) ? updatedTicket.Lastname.Trim() : null;

                SetEntityState(ticket, EntityState.Modified);
                UnitOfWork.SaveChanges();
            }
            catch (InvalidOperationException e)
            {
                throw new BusinessServiceException(Resources.UnableToUpdateTicketExceptionMessage, e);
            }
        }

        public void DeleteTicket(long ticketId)
        {
            try
            {
                var ticket = GetTicket(ticketId);
                if (ticket == null) return;

                GetDbSet<Ticket>().Remove(ticket);
                UnitOfWork.SaveChanges();
            }
            catch (InvalidOperationException e)
            {
                throw new BusinessServiceException(Resources.UnableToDeleteTicketExceptionMessage, e);
            }
        }

        public void DeleteTicket(long userId, long ticketId)
        {
            try
            {
                var ticket = GetTicket(userId, ticketId);
                if (ticket == null) return;

                GetDbSet<Ticket>().Remove(ticket);
                UnitOfWork.SaveChanges();
            }
            catch (InvalidOperationException e)
            {
                throw new BusinessServiceException(Resources.UnableToDeleteTicketExceptionMessage, e);
            }
        }

        public void DeleteTicketsByDeletedUsers()
        {
            try
            {
                var tickets = GetTicketsByDeletedUsers(false);

                foreach (var ticket in tickets)
                    GetDbSet<Ticket>().Remove(ticket);

                UnitOfWork.SaveChanges();
            }
            catch (InvalidOperationException e)
            {
                throw new BusinessServiceException(Resources.UnableToDeleteTicketExceptionMessage, e);
            }
        }
    }
}
