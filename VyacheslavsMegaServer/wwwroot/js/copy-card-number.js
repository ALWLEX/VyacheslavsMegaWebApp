function copyCardNumber(event, cardNumber) {
    const copyLink = event.target;

    navigator.clipboard.writeText(cardNumber).then(function () {
        copyLink.textContent = "Скопировано!";
        setTimeout(function () {
            copyLink.textContent = "Скопировать номер";
        }, 3000);
    });
}