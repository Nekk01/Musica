using System;

namespace Musica
{
    public class SyncSelectedTrackService
    {
        public TrackModel track { get; set; }

        public Action<TrackModel> onIDChanged;
        public void ChangeID(TrackModel newTrack)
        {
            track = newTrack;
            onIDChanged.Invoke(track);
        }
    }
}
