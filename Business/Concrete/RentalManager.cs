﻿using Business.Abstract;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class RentalManager : IRentalService
    {
        IRentalDAL _irentalDal;

        public RentalManager(IRentalDAL irentalDal)
        {
            _irentalDal = irentalDal;
        }
        [ValidationAspect(typeof(RentalValidator))]
        public IResult Add(Rental rental)
        {
            if (CheckCar(rental.CarId).Success)
            {
                if(!CheckReturnDate(rental.CarId).Success)

                return new ErrorResult(Messages.NotAdded);
            }
            
            _irentalDal.Add(rental);
            return new SuccessResult();
        }

        public IResult CheckCar(int carId)
        {
            

            var result = _irentalDal.GetAll();
            foreach (var rental in result)
            {
                if (carId == rental.CarId)
                {
                    
                    return new SuccessResult();
                }
                
            }
            return new ErrorResult();

        }

        public IResult CheckReturnDate(int carId)
        {


            var result = _irentalDal.Get(c => c.CarId == carId);
             if (result.ReturnDate!=null)
                {
                   return new SuccessResult();
                }

            
            return new ErrorResult();
        }

        public IResult Delete(Rental rental)
        {
            _irentalDal.Delete(rental);
            return new SuccessResult();
        }

        public IDataResult<List<Rental>> GetAll()
        {
            return new SuccessDataResult<List<Rental>>(_irentalDal.GetAll());
        }

        public IDataResult<Rental> GetById(int id)
        {
            return new SuccessDataResult<Rental>(_irentalDal.Get(r=>r.Id==id));
        }

        public IResult Update(Rental rental)
        {
            _irentalDal.Update(rental);
            return new SuccessResult();
        }
    }
}