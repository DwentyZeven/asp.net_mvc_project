using System.Collections.ObjectModel;
using Project.Models;

namespace Project.Interfaces
{
    public interface ITicketRepository
    {
        Ticket GetTicket(long ticketId);

        Ticket GetTicket(long userId, long ticketId);

        Ticket GetTicketLocal(long? ticketId);

        ReadOnlyCollection<Ticket> GetUserTickets(long userId, bool paged);

        ReadOnlyCollection<Ticket> GetUserTicketsLocal(long? userId, bool paged);

        ReadOnlyCollection<Ticket> GetAllTickets(long mainAdminId, bool paged);

        ReadOnlyCollection<Ticket> GetAllTicketsLocal(long mainAdminId, bool paged);

        ReadOnlyCollection<Ticket> GetTicketsByDeletedUsers(bool paged);

        long CountUserTickets(long userId);

        long CountTicketsByDeletedUsers();

        long CountAllTickets(long mainAdminId);

        void CreateTicket(long userId, Ticket newTicket);

        void UpdateTicket(Ticket updatedTicket);

        void UpdateTicket(long userId, Ticket updatedTicket);

        void DeleteTicket(long ticketId);
        
        void DeleteTicket(long userId, long ticketId);

        void DeleteTicketsByDeletedUsers();

        void SetPaginationParams(int pageIndex, int pageSize);
    }
}
