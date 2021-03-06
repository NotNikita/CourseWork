﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comics.Domain;
using Comics.DAL;
using Comics.Services.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Comics.Services.Entity
{
    public class CollectionRepository : ICollectionRepository
    {
        private ComicsDbContext db;
        private ICollRepository<Collection> collRepository;
        
        public CollectionRepository(ComicsDbContext _context, ICollRepository<Collection> _collRepository)
        {
            db = _context;
            collRepository = _collRepository;
        }

        public IEnumerable<Collection> GetAllCollections()
        {
            return collRepository.GetAll();
        }
        public IEnumerable<Collection> MyCollections(User user)
        {
            return db.Collections.Where(x => x.User.Id == user.Id).ToList();
        }
        public Collection GetCollectionById(int? id)
        {
            return db.Collections.FirstOrDefault(coll => coll.Id == id);
        }

        public void AddCollectionDB(Collection coll)
        {
            collRepository.Insert(coll);
        }
        public void UpdateCollection(Collection coll)
        {
            collRepository.Update(coll);
        }
        public void DeleteCollection(Collection coll)
        {
            collRepository.Delete(coll);
        }

        public IEnumerable<Collection> GetUserCollections(User user)
        {
            return db.Collections.Where(c => c.User.Id == user.Id).ToList();
        }
        public IEnumerable<BaseItem> GetCollectionItems(int? id)
        {
            return db.BaseItems.Where(c => c.CollectionId == id).ToList();
        }
    }
}