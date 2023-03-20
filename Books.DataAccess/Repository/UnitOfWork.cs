﻿using Books.DataAccess.Data;
using Books.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public ICategoryRepository CategoryRepository { get; private set; }
        public ICoverTypeRepository CoverTypeRepository { get; private set; }
        public IProductRepository ProductRepository { get; private set; }
        public ICompanyRepository CompanyRepository { get; private set; }
        public IApplicationUserRepository ApplicationUserRepository { get; private set; }
        public IShoppingCartRepository ShoppingCartRepository { get; private set; }
		public IOrderDetailRepository OrderDetailRepository { get; private set; }
		public IOrderHeaderRepository OrderHeaderRepository { get; private set; }


		private ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            CategoryRepository = new CategoryRepository(_db);
            CoverTypeRepository = new CoverTypeRepository(_db);
            ProductRepository = new ProductRepository(_db);
            CompanyRepository = new CompanyRepository(_db);
            ShoppingCartRepository = new ShoppingCartRepository(_db);
            ApplicationUserRepository = new ApplicationUserRepository(_db);
			OrderDetailRepository=new OrderDetailRepository(_db);
			OrderHeaderRepository=new OrderHeaderRepository(_db);
		}

       

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
