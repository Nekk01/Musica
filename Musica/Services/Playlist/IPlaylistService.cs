using System;
using System.Collections;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Musica.Components.Pages;

namespace Musica;

public interface IPlaylistService
{
    Task<List<TrackModel>>GetAllTracksFromPlaylist(int playlistID);
    Task<List<PlaylistModel>>GetAllRecords(int user_id);
    Task<bool> CreatePlaylist(string name, int owner_id);
    Task<bool> RemovePlaylist(int id);
    Task<bool> AddTrackToPlaylist(int playlistId, int trackId);
    Task<bool> RemoveTrackFromPlaylist(int playlistId, int trackId);
}
