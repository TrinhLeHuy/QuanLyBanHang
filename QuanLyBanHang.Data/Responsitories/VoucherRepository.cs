using QuanLyBanHang.Data.DataContext;
using QuanLyBanHang.Data.Entities;
namespace QuanLyBanHang.Data.Repositories
{
    public class VoucherRepository
    {
        private readonly ApplicationDbContext _context;

        public VoucherRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Voucher> GetAll()
        {
            return _context.Vouchers.ToList();
        }

        public Voucher? GetById(int id)
        {
            return _context.Vouchers.Find(id);
        }

        public void Add(Voucher voucher)
        {
            _context.Vouchers.Add(voucher);
            _context.SaveChanges();
        }

        public void Update(Voucher voucher)
        {
            _context.Vouchers.Update(voucher);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var voucher = _context.Vouchers.Find(id);
            if (voucher != null)
            {
                _context.Vouchers.Remove(voucher);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Voucher> Search(string keyword)
        {
            return _context.Vouchers
                .Where(v => v.Code.Contains(keyword) || v.Description.Contains(keyword))
                .ToList();
        }

        public IEnumerable<Voucher> Filter(bool isActive)
        {
            return _context.Vouchers.Where(v => v.IsActive == isActive).ToList();
        }
    }
}
