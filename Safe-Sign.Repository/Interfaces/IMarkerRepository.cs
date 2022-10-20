using Safe_Sign.DTO.Marker;

namespace Safe_Sign.Repository.Interfaces
{
    public interface IMarkerRepository
    {
        MarkerDTO GetMarkerById(ulong idMarker);

        IList<MarkerDTO> GetAllMarkers();

        MarkerDTO CreateMarker(MarkerDTO marker);

        MarkerDTO UpdateMarker(UpdateMarkerDTO marker);

        void DeleteMarker(ulong idMarker);

        void SwitchStatusMarker(ulong idMarker);
    }
}
