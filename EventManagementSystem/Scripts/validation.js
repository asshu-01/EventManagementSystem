function validateBooking() {
    var seatsInput = document.querySelector("input[id$='txtSeats']");
    if (!seatsInput) {
        return true;
    }

    var seats = seatsInput.value;

    if (seats == "" || seats <= 0) {
        alert("Enter valid seats");
        return false;
    }
    return true;
}