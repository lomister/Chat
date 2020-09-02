function Validate() {
    var password = document.getElementById("PasswordField").value;
    var confirmPassword = document.getElementById("ConfirmPasswordField").value;

    if (password != confirmPassword) {
        alert("Passwords do not match.");

        return false;
    }

    return true;
}

$(document).ready(function() {
    $("#messageInput").emojioneArea({
        pickerPosition: "top"
    });
})