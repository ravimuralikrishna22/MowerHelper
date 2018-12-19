var sessionTimeout = "120"; setTimeout('RedirectToWelcomePage()', parseInt(sessionTimeout) * 60 * 1000);
function RedirectToWelcomePage() {
    alert("Session expired. You will be redirected to session expired page");
    window.location = "../";
}