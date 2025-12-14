using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyBanHang.Data.Migrations
{
    /// <inheritdoc />
    public partial class TriggerCartItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DELIMITER $$
                CREATE TRIGGER update_cart_item_status
                AFTER UPDATE ON products
                FOR EACH ROW
                BEGIN
                    IF NEW.Stock = 0 AND OLD.Stock > 0 THEN
                        UPDATE cart_item
                        SET status = 'outstock'
                        WHERE product_id = NEW.Id AND status = 'instock';
                    ELSEIF NEW.Stock > 0 AND OLD.Stock = 0 THEN
                        UPDATE cart_item
                        SET status = 'instock'
                        WHERE product_id = NEW.Id AND status = 'outstock';
                    END IF;
                END$$
                DELIMITER ;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS update_cart_item_status;");
        }
    }
}
