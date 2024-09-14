
window.addEventListener('scroll', function() {
    var header = document.querySelector('.headder-top');
    // Kiểm tra vị trí cuộn
    if (window.scrollY > 0) {
        // Nếu người dùng đã cuộn trang, thêm lớp 'shrink' vào phần header
        header.classList.add('shrink');
    } else {
        // Nếu người dùng cuộn về đầu trang, loại bỏ lớp 'shrink' khỏi phần header
        header.classList.remove('shrink');
    }
});
