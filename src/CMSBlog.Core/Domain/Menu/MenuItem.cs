using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMSBlog.Core.Domain.Menu
{
    [Table("MenuItems")]
    public class MenuItem
    {
        // 1. KHÓA CHÍNH (PRIMARY KEY)
        [Key]
        public Guid Id { get; set; }

        // 2. THÔNG TIN CƠ BẢN
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } // Tên hiển thị trên menu (VD: "Tin tức Công nghệ")

        // 3. CẤP BẬC (HIERARCHY - MỐI QUAN HỆ ĐỆ QUY)
        public Guid? ParentId { get; set; } // ID của mục menu cha (Null nếu là cấp cao nhất)

        // 4. PHÂN LOẠI VÀ SẮP XẾP
        public int SortOrder { get; set; } // Thứ tự hiển thị
        public bool IsActive { get; set; } // Trạng thái kích hoạt/vô hiệu hóa

        [Required]
        [MaxLength(50)]
        // Phân loại vị trí menu (Main, Footer, Sidebar, v.v.)
        public string MenuGroup { get; set; } = "Main";

        // 5. LIÊN KẾT ĐỘNG (DYNAMIC LINKING)

        [Required]
        [MaxLength(50)]
        /* * Loại liên kết:
         * - "CustomLink": Liên kết ngoài hoặc URL cố định.
         * - "Category": Liên kết đến PostCategories.Id.
         * - "Post": Liên kết đến Posts.Id.
         * - "Series": Liên kết đến Series.Id.
        */
        public string LinkType { get; set; }

        // ID của thực thể được liên kết (Post, Category, Series)
        public Guid? EntityId { get; set; }

        [MaxLength(500)]
        // Chỉ dùng khi LinkType = "CustomLink"
        public string? CustomUrl { get; set; }

        // 6. THÔNG TIN BỔ SUNG
        // Có thể tùy chọn mở liên kết trong cửa sổ/tab mới
        public bool? OpenInNewTab { get; set; } = false;
    }
}
