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
    public class SongRepository : BaseRepository, ISongRepository
    {
        private readonly IUserRepository _userRepository;

        public SongRepository(IUnitOfWork unitOfWork, IUserRepository userRepository) : base(unitOfWork)
        {
            _userRepository = userRepository;
        }

        public Song GetSong(long songId)
        {
            try
            {
                return GetDbSet<Song>().FirstOrDefault(s => s.SongId == songId);
            }
            catch (InvalidOperationException e)
            {
                throw new BusinessServiceException(Resources.UnableToGetSongExceptionMessage, e);
            }
        }

        public Song GetSong(long userId, long songId)
        {
            try
            {
                return GetDbSet<Song>().FirstOrDefault(s => s.User.UserId == userId && s.SongId == songId);
            }
            catch (InvalidOperationException e)
            {
                throw new BusinessServiceException(Resources.UnableToGetSongExceptionMessage, e);
            }
        }

        public Song GetSongLocal(long? songId)
        {
            if (songId == null) return null;

            try
            {
                return GetDbSet<Song>().FirstOrDefault(s => s.SongId == songId && 
                                                            s.Language == (CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "ru" ? 1 : 2));
            }
            catch (InvalidOperationException e)
            {
                throw new BusinessServiceException(Resources.UnableToGetSongExceptionMessage, e);
            }
        }

        public ReadOnlyCollection<Song> GetUserSongs(long userId, bool paged)
        {
            try
            {
                var songs = GetDbSet<Song>().Where(s => s.User.UserId == userId).OrderByDescending(s => s.SongId);

                if (paged)
                    songs = TakePageItems(songs);

                return new ReadOnlyCollection<Song>(songs.ToList());
            }
            catch (InvalidOperationException e)
            {
                throw new BusinessServiceException(Resources.UnableToGetSongsExceptionMessage, e);
            }
        }

        public ReadOnlyCollection<Song> GetUserSongsLocal(long? userId, bool paged)
        {
            if (userId == null) return null;

            try
            {
                var songs = GetDbSet<Song>().Where(s => s.User.UserId == userId &&
                                                        s.Language == (CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "ru" ? 1 : 2))
                                            .OrderByDescending(s => s.SongId);
                if (paged)
                    songs = TakePageItems(songs);

                return new ReadOnlyCollection<Song>(songs.ToList());
            }
            catch (InvalidOperationException e)
            {
                throw new BusinessServiceException(Resources.UnableToGetSongsExceptionMessage, e);
            }
        }

        public ReadOnlyCollection<Song> GetAllSongs(long mainAdminId, bool paged)
        {
            try
            {
                var songs = GetDbSet<Song>().Where(s => s.User == null || s.User.UserId != mainAdminId).OrderByDescending(s => s.SongId);

                if (paged)
                    songs = TakePageItems(songs);

                return new ReadOnlyCollection<Song>(songs.ToList());
            }
            catch (InvalidOperationException e)
            {
                throw new BusinessServiceException(Resources.UnableToGetSongsExceptionMessage, e);
            }
        }

        public ReadOnlyCollection<Song> GetAllSongsLocal(long mainAdminId, bool paged)
        {
            try
            {
                var songs = GetDbSet<Song>().Where(s => (s.User == null || s.User.UserId != mainAdminId) &&
                                                        (s.Language == (CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "ru" ? 1 : 2)))
                                            .OrderByDescending(s => s.SongId);
                if (paged)
                    songs = TakePageItems(songs);

                return new ReadOnlyCollection<Song>(songs.ToList());
            }
            catch (InvalidOperationException e)
            {
                throw new BusinessServiceException(Resources.UnableToGetSongsExceptionMessage, e);
            }
        }

        public ReadOnlyCollection<Song> GetSongsByDeletedUsers(bool paged)
        {
            try
            {
                var songs = GetDbSet<Song>().Where(s => s.User == null)
                                            .OrderByDescending(s => s.SongId);
                if (paged)
                    songs = TakePageItems(songs);

                return new ReadOnlyCollection<Song>(songs.ToList());
            }
            catch (InvalidOperationException e)
            {
                throw new BusinessServiceException(Resources.UnableToGetSongsExceptionMessage, e);
            }
        }

        public long CountUserSongs(long userId)
        {
            try
            {
                return GetDbSet<Song>().LongCount(s => s.User.UserId == userId);
            }
            catch (InvalidOperationException e)
            {
                throw new BusinessServiceException(Resources.UnableToCountSongsExceptionMessage, e);
            }
        }

        public long CountSongsByDeletedUsers()
        {
            try
            {
                return GetDbSet<Song>().LongCount(s => s.User == null);
            }
            catch (InvalidOperationException e)
            {
                throw new BusinessServiceException(Resources.UnableToCountSongsExceptionMessage, e);
            }
        }

        public long CountAllSongs(long mainAdminId)
        {
            try
            {
                return GetDbSet<Song>().LongCount(s => s.User == null || s.User.UserId != mainAdminId);
            }
            catch (InvalidOperationException e)
            {
                throw new BusinessServiceException(Resources.UnableToCountSongsExceptionMessage, e);
            }
        }

        public void CreateSong(long userId, Song newSong)
        {
            try
            {
                User user = _userRepository.GetUser(userId);
                if (user == null) return;

                newSong.User = user;
                newSong.Title = newSong.Title.Trim();
                newSong.Singer = newSong.Singer.Trim();
                newSong.Quote = newSong.Quote.Replace("\r\n", " ").Trim();
                newSong.Translation = (newSong.Translation != null) ? newSong.Translation.Replace("\r\n", " ").Trim() : null;
                newSong.Language = (short) (CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "ru" ? 1 : 2);
                
                GetDbSet<Song>().Add(newSong);
                UnitOfWork.SaveChanges();
            }
            catch (InvalidOperationException e)
            {
                throw new BusinessServiceException(Resources.UnableToCreateSongExceptionMessage, e);
            }
        }

        public void UpdateSong(Song updatedSong)
        {
            try
            {
                var song = GetSong(updatedSong.SongId);
                if (song == null) return;

                song.Title = updatedSong.Title.Trim();
                song.Singer = updatedSong.Singer.Trim();
                song.Quote = updatedSong.Quote.Trim();
                song.Translation = (updatedSong.Translation != null) ? updatedSong.Translation.Trim() : null;

                SetEntityState(song, EntityState.Modified);
                UnitOfWork.SaveChanges();
            }
            catch (InvalidOperationException e)
            {
                throw new BusinessServiceException(Resources.UnableToUpdateSongExceptionMessage, e);
            }
        }

        public void UpdateSong(long userId, Song updatedSong)
        {
            try
            {
                var song = GetSong(userId, updatedSong.SongId);
                if (song == null) return;

                song.Title = updatedSong.Title.Trim();
                song.Singer = updatedSong.Singer.Trim();
                song.Quote = updatedSong.Quote.Trim();
                song.Translation = (updatedSong.Translation != null) ? updatedSong.Translation.Trim() : null;

                SetEntityState(song, EntityState.Modified);
                UnitOfWork.SaveChanges();
            }
            catch (InvalidOperationException e)
            {
                throw new BusinessServiceException(Resources.UnableToUpdateSongExceptionMessage, e);
            }
        }

        public void DeleteSong(long songId)
        {
            try
            {
                var song = GetSong(songId);
                if (song == null) return;

                GetDbSet<Song>().Remove(song);
                UnitOfWork.SaveChanges();
            }
            catch (InvalidOperationException e)
            {
                throw new BusinessServiceException(Resources.UnableToDeleteSongExceptionMessage, e);
            }
        }

        public void DeleteSong(long userId, long songId)
        {
            try
            {
                var song = GetSong(userId, songId);
                if (song == null) return;

                GetDbSet<Song>().Remove(song);
                UnitOfWork.SaveChanges();
            }
            catch (InvalidOperationException e)
            {
                throw new BusinessServiceException(Resources.UnableToDeleteSongExceptionMessage, e);
            }
        }

        public void DeleteSongsByDeletedUsers()
        {
            try
            {
                var songs = GetSongsByDeletedUsers(false);

                foreach (var song in songs)
                    GetDbSet<Song>().Remove(song);

                UnitOfWork.SaveChanges();
            }
            catch (InvalidOperationException e)
            {
                throw new BusinessServiceException(Resources.UnableToDeleteSongExceptionMessage, e);
            }
        }
    }
}
