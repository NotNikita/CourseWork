using Comics.Domain;
using System.Collections.Generic;

namespace Comics.Services.Abstract
{
    public interface ICollectionRepository
    {
        public IEnumerable<Collection> GetAllCollections();
        public IEnumerable<Collection> MyCollections(User user);
        public Collection GetCollectionById(int? id);

        public void AddCollectionDB(Collection coll);
        public void UpdateCollection(Collection coll);
        public void DeleteCollection(Collection coll);

        public IEnumerable<Collection> GetUserCollections(User user);
        public IEnumerable<BaseItem> GetCollectionItems(int? id);
    }
}
