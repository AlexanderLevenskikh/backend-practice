using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using Ddd.Infrastructure;

namespace Ddd.Taxi.Domain
{
	// In real aplication it whould be the place where database is used to find driver by its Id.
	// But in this exercise it is just a mock to simulate database
	public class DriversRepository
	{
		public Driver GetDriver(int driverId)
		{
			if (driverId == 15)
			{
				var driverName = new PersonName("Drive", "Driverson");
				var car = new Car("Lada sedan", "Baklazhan", "A123BT 66");
				var driver = new Driver(driverId, driverName, car);

				return driver;
			}
			else
				throw new Exception("Unknown driver id " + driverId);
		}
	}

	public class TaxiApi : ITaxiApi<TaxiOrder>
	{
		private readonly DriversRepository driversRepo;
		private readonly Func<DateTime> currentTime;
		private int idCounter;
		public TaxiApi(DriversRepository driversRepo, Func<DateTime> currentTime)
		{
			this.driversRepo = driversRepo;
			this.currentTime = currentTime;
		}

		public TaxiOrder CreateOrderWithoutDestination(string firstName, string lastName, string street, string building)
		{
			var client = new PersonName(firstName, lastName);
			var start = new Address(street, building);
			
			return new TaxiOrder(
				idCounter++,
				client,
				start,
				null,
				null,
				null,
				currentTime(),
				null,
				null,
				null,
				null);
		}

		public void UpdateDestination(TaxiOrder order, string street, string building)
		{
			var destination = new Address(street, building);
			
			order.UpdateDestination(destination);
		}

		public void AssignDriver(TaxiOrder order, int driverId)
		{
			var driver = driversRepo.GetDriver(driverId);
			var driverAssignmentTime = currentTime();
			order.AssignDriver(driver, driverAssignmentTime);
		}

		public void UnassignDriver(TaxiOrder order)
		{
			order.UnassignDriver();
		}

		public string GetDriverFullInfo(TaxiOrder order)
		{
			return order.GetDriverFullInfo();
		}

		public string GetShortOrderInfo(TaxiOrder order)
		{
			return order.GetShortOrderInfo();
		}

		public void Cancel(TaxiOrder order)
		{
			order.Cancel(currentTime());
		}

		public void StartRide(TaxiOrder order)
		{
			order.StartRide(currentTime());
		}

		public void FinishRide(TaxiOrder order)
		{
			order.FinishRide(currentTime());
		}
	}

	public class Car : ValueType<Car>
	{
		public Car(
			string model,
			string color,
			string plateNumber)
		{
			Color = color;
			Model = model;
			PlateNumber = plateNumber;
		}
		
		public string Color { get; }
		public string Model { get; }
		public string PlateNumber { get; }
	}

	public class Driver : Entity<int>
	{
		public Driver(int id,
			PersonName driverName,
			Car car) : base(id)
		{
			Id = id;
			DriverName = driverName;
			Car = car;
		}
		
		public int Id { get; }
		public PersonName DriverName { get; }
		public Car Car { get; }
	}

	public class TaxiOrder : Entity<int>
	{
		public TaxiOrder(
			int id,
			PersonName clientName, 
			Address start, 
			Address destination, 
			Driver driver,
			TaxiOrderStatus? status,
			DateTime? creationTime,
			DateTime? driverAssignmentTime,
			DateTime? cancelTime,
			DateTime? startRideTime,
			DateTime? finishRideTime
		) : base(id)
		{
			Id = id;
			ClientName = clientName;
			Start = start;
			Destination = destination;
			Driver = driver;
			Status = status ?? TaxiOrderStatus.WaitingForDriver;
			CreationTime = creationTime ?? new DateTime();
			DriverAssignmentTime = driverAssignmentTime ?? new DateTime();
			CancelTime = cancelTime ?? new DateTime();
			StartRideTime = startRideTime ?? new DateTime();
			FinishRideTime = finishRideTime ?? new DateTime();
		}
		public void UpdateDestination(Address destination)
		{
			Destination = destination;
		}
		
		public void AssignDriver(Driver driver, DateTime driverAssignmentTime)
		{
			if (Driver != null)
			{
				throw new InvalidOperationException(driver.ToString());
			}
			
			Driver = driver;
			DriverAssignmentTime = driverAssignmentTime;
			Status = TaxiOrderStatus.WaitingCarArrival;
		}
		
		public void UnassignDriver()
		{
			if (Driver == null)
			{
				throw new InvalidOperationException(Status.ToString());
			}
			
			if (Status == TaxiOrderStatus.InProgress || Status == TaxiOrderStatus.Canceled ||
			    Status == TaxiOrderStatus.Finished)
			{
				throw new InvalidOperationException(Status.ToString());
			}
			
			Driver = null;
			Status = TaxiOrderStatus.WaitingForDriver;
		}

		public string GetDriverFullInfo()
		{
			if (Status == TaxiOrderStatus.WaitingForDriver) return null;
			
			return string.Join(" ",
				"Id: " + Driver?.Id,
				"DriverName: " + FormatName(Driver?.DriverName?.FirstName, Driver?.DriverName?.LastName),
				"Color: " + Driver?.Car?.Color,
				"CarModel: " + Driver?.Car?.Model,
				"PlateNumber: " + Driver?.Car?.PlateNumber);
		}

		public string GetShortOrderInfo()
		{
			return string.Join(" ",
				"OrderId: " + Id,
				"Status: " + Status,
				"Client: " + FormatName(ClientName?.FirstName, ClientName?.LastName),
				"Driver: " + FormatName(Driver?.DriverName?.FirstName, Driver?.DriverName?.LastName),
				"From: " + FormatAddress(Start?.Street, Start?.Building),
				"To: " + FormatAddress(Destination?.Street, Destination?.Building),
				"LastProgressTime: " + GetLastProgressTime(this).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
		}

		private DateTime GetLastProgressTime(TaxiOrder order)
		{
			if (order.Status == TaxiOrderStatus.WaitingForDriver) return order.CreationTime;
			if (order.Status == TaxiOrderStatus.WaitingCarArrival) return order.DriverAssignmentTime;
			if (order.Status == TaxiOrderStatus.InProgress) return order.StartRideTime;
			if (order.Status == TaxiOrderStatus.Finished) return order.FinishRideTime;
			if (order.Status == TaxiOrderStatus.Canceled) return order.CancelTime;
			throw new NotSupportedException(order.Status.ToString());
		}

		private string FormatName(string firstName, string lastName)
		{
			return string.Join(" ", new[] { firstName, lastName }.Where(n => n != null));
		}

		private string FormatAddress(string street, string building)
		{
			return string.Join(" ", new[] { street, building }.Where(n => n != null));
		}

		public void Cancel(DateTime cancelTime)
		{
			if (Status == TaxiOrderStatus.InProgress || Status == TaxiOrderStatus.Canceled ||
			    Status == TaxiOrderStatus.Finished)
			{
				throw new InvalidOperationException(Status.ToString());
			}
			
			Status = TaxiOrderStatus.Canceled;
			CancelTime = cancelTime;
		}

		public void StartRide(DateTime startRideTime)
		{
			if (Driver == null)
			{
				throw new InvalidOperationException(Status.ToString());
			}
			
			Status = TaxiOrderStatus.InProgress;
			StartRideTime = startRideTime;
		}

		public void FinishRide(DateTime finishRideTime)
		{
			if (Driver == null)
			{
				throw new InvalidOperationException(Status.ToString());
			}
			
			if (Status == TaxiOrderStatus.Finished || Status == TaxiOrderStatus.WaitingCarArrival ||
			    Status == TaxiOrderStatus.WaitingForDriver)
			{
				throw new InvalidOperationException(Status.ToString());
			}
			
			Status = TaxiOrderStatus.Finished;
			FinishRideTime = finishRideTime;
		}
		
		public int Id { get; private set; }
		public PersonName ClientName { get; private set; }
		public Address Start { get; private set; }
		public Address Destination { get; private set; }
		public Driver Driver { get; private set; }
		public TaxiOrderStatus Status { get; private set; }
		public DateTime CreationTime { get; private set; }
		public DateTime DriverAssignmentTime { get; private set; }
		public DateTime CancelTime { get; private set; }
		public DateTime StartRideTime { get; private set; }
		public DateTime FinishRideTime { get; private set; }
	}
}