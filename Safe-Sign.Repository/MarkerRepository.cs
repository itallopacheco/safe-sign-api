using Safe_Sign.DAL.Models;
using Safe_Sign.DTO.Marker;
using Safe_Sign.DAL.DAO.Base;
using Safe_Sign.Repository.Interfaces;

namespace Safe_Sign.Repository
{
    public class MarkerRepository : IMarkerRepository
    {
        private readonly IDAO<Marker> _markerDAO;

        public MarkerRepository(IDAO<Marker> markerDAO)
        {
            _markerDAO = markerDAO;
        }

        public MarkerDTO GetMarkerById(ulong idMarker)
        {
            Marker marker = _markerDAO.GetById(idMarker);

            if (marker == null) throw new NullReferenceException();

            else
            {
                MarkerDTO newDTO = new()
                {
                    Id = marker.Id,
                    IsActive = marker.IsActive,
                    IdTheme = marker.IdTheme,
                };

                return newDTO;
            }
        }

        public IList<MarkerDTO> GetAllMarkers()
        {
            IEnumerable<Marker> markers = _markerDAO.GetAll();

            if (markers is null) throw new NullReferenceException();

            else
            {
                List<MarkerDTO> markersDTO = new();

                foreach (Marker marker in markers)
                {
                    MarkerDTO newDTO = new()
                    {
                        Id = marker.Id,
                        IsActive = marker.IsActive,
                        IdTheme = marker.IdTheme
                    };

                    markersDTO.Add(newDTO);
                }

                return markersDTO;
            }
        }

        public MarkerDTO CreateMarker(MarkerDTO marker)
        {
            Marker? registred = _markerDAO.GetAll().FirstOrDefault(m => m.IdTheme == marker.IdTheme);

            if (registred == null)
            {
                Marker newEntity = new() 
                {
                    IsActive = marker.IsActive,
                    IdTheme = marker.IdTheme
                };

                Marker newMarker = _markerDAO.Create(newEntity);

                MarkerDTO newDTO = new() 
                {
                    Id = newMarker.Id,
                    IsActive = newMarker.IsActive,
                    IdTheme = newMarker.IdTheme
                };

                return newDTO;
            }

            else throw new Exception();
        }

        public MarkerDTO UpdateMarker(UpdateMarkerDTO marker)
        {
            Marker oldMarker = _markerDAO.GetById(marker.Id);

            if (oldMarker is null) throw new NullReferenceException();

            else
            {
                oldMarker.IdTheme = !(oldMarker.IdTheme.Equals(marker.IdTheme)) ? marker.IdTheme : oldMarker.IdTheme;

                _markerDAO.Update(oldMarker);

                MarkerDTO newDTO = new()
                {
                    Id = oldMarker.Id,
                    IsActive = oldMarker.IsActive,
                    IdTheme = oldMarker.IdTheme
                };

                return newDTO;
            }
        }

        public void SwitchStatusMarker(ulong idMarker)
        {
            Marker marker = _markerDAO.GetById(idMarker);

            if (marker == null) throw new NullReferenceException();

            marker.IsActive = !marker.IsActive;

            _markerDAO.Update(marker);
        }

        public void DeleteMarker(ulong idMarker)
        {
            Marker marker = _markerDAO.GetById(idMarker);

            if (marker == null) throw new NullReferenceException();

            else _markerDAO.Delete(marker);
        }
    }
}
