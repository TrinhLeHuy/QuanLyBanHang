using QuanLyBanHang.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyBanHang.Data.DTO
{
    public class CheckoutEventAgrs
    {
        public List<int> SelectedCartItemIds { get; set; } = [];
        public int? VoucherId { get; set; }
    }
}
