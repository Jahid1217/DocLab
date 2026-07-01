// Custom JS for admin appointments page
document.addEventListener('DOMContentLoaded', function () {
    // Example: highlight row on click
    const rows = document.querySelectorAll('.admin-appointments-table tbody tr');
    rows.forEach(row => {
        row.addEventListener('click', function () {
            rows.forEach(r => r.classList.remove('table-active'));
            this.classList.add('table-active');
        });
    });
});

$(document).ready(function () {
    let currentPage = 1;
    const itemsPerPage = 10;

    function getStatus(bookingDate) {
        // bookingDate is in "yyyy-MM-dd" format
        const today = new Date();
        const date = new Date(bookingDate);
        today.setHours(0, 0, 0, 0);
        date.setHours(0, 0, 0, 0);
        return date < today ? "Completed" : "Upcoming";
    }

    function filterAppointments() {
        const searchTerm = $('#appointmentSearch').val().toLowerCase();
        const filterStatus = $('#filterStatus').val();
        const filterDoctorType = $('#filterDoctorType').val();

        let $rows = $('#appointmentsTbody tr').not('.no-appointments');
        let filteredRows = $rows.filter(function () {
            const $row = $(this);
            const patient = $row.data('patient').toLowerCase();
            const doctor = $row.data('doctor').toLowerCase();
            const specialty = $row.data('specialty');
            const status = $row.data('status');

            const matchesSearch = patient.includes(searchTerm) || doctor.includes(searchTerm);
            const matchesStatus = filterStatus ? status === filterStatus : true;
            const matchesSpecialty = filterDoctorType ? specialty === filterDoctorType : true;

            return matchesSearch && matchesStatus && matchesSpecialty;
        });

        $rows.hide();
        filteredRows.show();

        // Pagination
        const totalCount = filteredRows.length;
        const totalPages = Math.max(1, Math.ceil(totalCount / itemsPerPage));
        if (currentPage > totalPages) currentPage = totalPages;

        filteredRows.hide();
        filteredRows.slice((currentPage - 1) * itemsPerPage, currentPage * itemsPerPage).show();

        $('#showingCount').text(filteredRows.slice((currentPage - 1) * itemsPerPage, currentPage * itemsPerPage).length);
        $('#totalCount').text(totalCount);
        $('#pageInfo').text(`Page ${totalCount === 0 ? 0 : currentPage} of ${totalPages}`);

        $('#prevPage').prop('disabled', currentPage === 1);
        $('#nextPage').prop('disabled', currentPage === totalPages || totalCount === 0);

        // Show/hide "No appointments found" row
        if (totalCount === 0) {
            $('.no-appointments').show();
        } else {
            $('.no-appointments').hide();
        }
    }

    $('#appointmentSearch').on('input', function () {
        currentPage = 1;
        filterAppointments();
    });

    $('#clearSearch').click(function () {
        $('#appointmentSearch').val('');
        currentPage = 1;
        filterAppointments();
    });

    $('#filterStatus').change(function () {
        currentPage = 1;
        filterAppointments();
    });

    $('#filterDoctorType').change(function () {
        currentPage = 1;
        filterAppointments();
    });

    $('#prevPage').click(function () {
        if (currentPage > 1) {
            currentPage--;
            filterAppointments();
        }
    });

    $('#nextPage').click(function () {
        const totalCount = $('#appointmentsTbody tr:visible').not('.no-appointments').length;
        const totalPages = Math.max(1, Math.ceil(totalCount / itemsPerPage));
        if (currentPage < totalPages) {
            currentPage++;
            filterAppointments();
        }
    });

    // Initial call
    filterAppointments();
});