using System.Collections.ObjectModel;
using Project.Models;

namespace Project.Interfaces
{
    public interface ISongRepository
    {
        Song GetSong(long songId);

        Song GetSong(long userId, long songId);

        Song GetSongLocal(long? songId);

        ReadOnlyCollection<Song> GetUserSongs(long userId, bool paged);

        ReadOnlyCollection<Song> GetUserSongsLocal(long? userId, bool paged);

        ReadOnlyCollection<Song> GetAllSongs(long mainAdminId, bool paged);

        ReadOnlyCollection<Song> GetAllSongsLocal(long mainAdminId, bool paged);

        ReadOnlyCollection<Song> GetSongsByDeletedUsers(bool paged);

        long CountUserSongs(long userId);

        long CountSongsByDeletedUsers();

        long CountAllSongs(long mainAdminId);

        void CreateSong(long userId, Song newSong);

        void UpdateSong(Song updatedSong);

        void UpdateSong(long userId, Song updatedSong);

        void DeleteSong(long songId);
        
        void DeleteSong(long userId, long songId);

        void DeleteSongsByDeletedUsers();

        void SetPaginationParams(int pageIndex, int pageSize);
    }
}
