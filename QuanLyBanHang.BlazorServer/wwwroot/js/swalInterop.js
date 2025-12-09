window.SwalInterop = {
    ShowSuccess: function (message) {
        Swal.fire({
            icon: 'success',
            title: 'Thành công',
            text: message
        });
    },

    ShowError: function (message) {
        Swal.fire({
            icon: 'error',
            title: 'Lỗi',
            text: message
        });
    },

    ShowConfirm: function (message) {
        return Swal.fire({
            title: 'Bạn chắc chắn?',
            text: message,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Đồng ý',
            cancelButtonText: 'Hủy'
        }).then(result => result.isConfirmed);
    }
};
