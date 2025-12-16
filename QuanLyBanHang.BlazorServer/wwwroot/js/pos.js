// POS Keyboard Shortcuts
window.posKeyboardShortcuts = {
    init: function (dotNetHelper) {
        document.addEventListener('keydown', function (e) {
            // F1: Focus vào ô tìm kiếm
            if (e.key === 'F1') {
                e.preventDefault();
                var input = document.querySelector('input[placeholder*="F1"]');
                if (input) {
                    input.focus();
                    input.select();
                }
            }
            // F12: Thanh toán
            if (e.key === 'F12') {
                e.preventDefault();
                var btn = document.querySelector('button:contains("Thanh Toán")');
                if (!btn) {
                    // Try alternative selector
                    btn = Array.from(document.querySelectorAll('button')).find(b => 
                        b.textContent.includes('Thanh Toán') && !b.disabled
                    );
                }
                if (btn && !btn.disabled) {
                    btn.click();
                }
            }
            // Esc: Hủy giỏ hàng
            if (e.key === 'Escape') {
                e.preventDefault();
                if (confirm('Bạn có chắc muốn hủy đơn hàng này?')) {
                    dotNetHelper.invokeMethodAsync('ClearCartFromJS');
                }
            }
        });
    },
    dispose: function () {
        // Cleanup if needed
    }
};

