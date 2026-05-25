using BenchmarkDotNet.Attributes;
using LMS.Domain.Entities;
using LMS.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace LMS.Benchmarks
{
    [MemoryDiagnoser]
    public class BookFetchBenchmarks
    {
        private ApplicationDbContext _context;

        [GlobalSetup]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer("Data Source=.;Initial Catalog=LibraryManagement;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False")
                .Options;

            _context = new ApplicationDbContext(options);

            _context.Books.Take(1).ToList();
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            _context?.Dispose();
        }

        [Benchmark(Baseline = true)]
        public async Task<List<Book>> GetWithTracking()
        {
            return await _context.Books
                .ToListAsync();
        }

        [Benchmark]
        public async Task<List<Book>> GetWithAsNoTracking()
        {
            return await _context.Books
                .AsNoTracking()
                .ToListAsync();
        }

    }
}
