using QuanLyBanHang.Data.DataContext;
using QuanLyBanHang.Data.Entities;


namespace QuanLyBanHang.Data.Repositories
{
    public class WarehouseRepository
    {
        private readonly ApplicationDbContext _context;

        public WarehouseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Lấy toàn bộ kho hàng
        public IEnumerable<Warehouse> GetAll()
        {
            return _context.Warehouses.ToList();
        }

        // Lấy kho theo ID
        public Warehouse GetById(int id)
        {
            return _context.Warehouses.FirstOrDefault(w => w.WarehouseId == id);
        }

        // Thêm kho mới
        public void Add(Warehouse warehouse)
        {
            _context.Warehouses.Add(warehouse);
            _context.SaveChanges();
        }

        // Cập nhật kho
        public void Update(Warehouse warehouse)
        {
            _context.Warehouses.Update(warehouse);
            _context.SaveChanges();
        }

        // Xóa kho
        public void Delete(int id)
        {
            var w = _context.Warehouses.FirstOrDefault(x => x.WarehouseId == id);
            if (w != null)
            {
                _context.Warehouses.Remove(w);
                _context.SaveChanges();
            }
        }
    }
}
