using HMS.Service.Domain.Abstractions.Entities;
using HMS.Service.Domain.Abstractions.Interfaces;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace HMS.Service.Data.Seeder
{
    public class HMSDbMigrator : IMigratable
    {
        private readonly HMSContext _context;
        private readonly ILogger _logger;
        private readonly TestDataModel _testDataModel;
        public HMSDbMigrator(HMSContext context,
                             ILogger<HMSDbMigrator> logger,
                             TestDataFileContents fileTestData)
        {
            _context = context;
            _logger = logger;

            if (fileTestData != null)
                _testDataModel = TestDataModel.Parse(fileTestData.Contents);
        }
        public int Priority => 1;

        public Type DbContextType => _context.GetType();

        public async Task ClearData()
        {
            try
            {
                if (_testDataModel.Beds.AsEnumerable().Any())
                    await _context.Beds.DeleteManyAsync(new BsonDocument());
                if (_testDataModel.Doctors.AsEnumerable().Any())
                    await _context.Doctors.DeleteManyAsync(new BsonDocument());
                if (_testDataModel.Patients.AsEnumerable().Any())
                    await _context.Patients.DeleteManyAsync(new BsonDocument());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(ClearData)} - threw an exception");
                throw;
            }
        }

        public Task EnsureCreated()
        {
            return Task.CompletedTask;
        }

        public Task Migrate()
        {
            return Task.CompletedTask;
        }

        public Task SeedConfigData(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public async Task SeedTestData(CancellationToken cancellationToken = default)
        {
            if (_testDataModel == null)
                return;

            try
            {
                await SeedInitialTestData(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(SeedTestData)} threw an exception");
                throw;
            }
        }

        private async Task SeedInitialTestData(CancellationToken cancellationToken)
        {
            var insertPatients = new List<PatientEntity>();

            if (_testDataModel.Patients.AsEnumerable().Any())
            {
                foreach (var item in _testDataModel.Patients)
                {
                    if (cancellationToken.IsCancellationRequested)
                        return;

                    var entity = await _context.Patients.Find(i => i.Id == item.Id).FirstOrDefaultAsync(cancellationToken);
                    if (entity is null)
                    {
                        item.CreatedAt = DateTime.UtcNow;
                        item.CreatedBy = nameof(HMSDbMigrator);
                        insertPatients.Add(item);
                        _logger.LogInformation("Inserted Patient: {name}", item.PatientName);
                    }
                }
            }
            if (insertPatients.Any())
            {
                await _context.Patients.InsertManyAsync(insertPatients, null, cancellationToken);
            }

            var insertDoctors = new List<DoctorEntity>();

            if (_testDataModel.Doctors.AsEnumerable().Any())
            {
                foreach (var item in _testDataModel.Doctors)
                {
                    if (cancellationToken.IsCancellationRequested)
                        return;

                    var entity = await _context.Doctors.Find(i => i.Id == item.Id).FirstOrDefaultAsync(cancellationToken);
                    if (entity is null)
                    {
                        item.CreatedAt = DateTime.UtcNow;
                        item.CreatedBy = nameof(HMSDbMigrator);
                        insertDoctors.Add(item);
                        _logger.LogInformation("Inserted Doctor: {DoctorName}", item.DoctorName);
                    }
                }
            }
            if (insertDoctors.Any())
            {
                await _context.Doctors.InsertManyAsync(insertDoctors, null, cancellationToken);
            }

            var insertBeds= new List<BedEntity>();

            if (_testDataModel.Beds.AsEnumerable().Any())
            {
                foreach (var item in _testDataModel.Beds)
                {
                    if (cancellationToken.IsCancellationRequested)
                        return;

                    var entity = await _context.Beds.Find(i => i.Id == item.Id).FirstOrDefaultAsync(cancellationToken);
                    if (entity is null)
                    {
                        item.CreatedAt = DateTime.UtcNow;
                        item.CreatedBy = nameof(HMSDbMigrator);
                        insertBeds.Add(item);
                        _logger.LogInformation("Inserted Beds: {BedNumber}", item.BedNumber);
                    }
                }
            }
            if (insertBeds.Any())
            {
                await _context.Beds.InsertManyAsync(insertBeds, null, cancellationToken);
            }
        }
    }
}
