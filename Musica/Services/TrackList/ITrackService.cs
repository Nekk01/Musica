using System;
using System.Collections;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components.Forms;

namespace Musica;

public interface ITrackService
{
    Task<List<TrackModel>>GetAllRecords();
    Task<bool> UploadRecord(TrackModel model, IBrowserFile audioFile, IBrowserFile coverFile, string userId);
    Task<int> FindArtistByUserID(int userId);
}
