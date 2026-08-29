const sentAtElement = document.getElementById('code-sent-at');
const resendText = document.getElementById('resend-code');
const resendTimer = document.getElementById('resend-timer');

const sentAt = new Date(sentAtElement.value);
const resendAvailableAt = new Date(sentAt.getTime() + 3 * 60 * 1000);

function updateTimer() {
    const remaining = resendAvailableAt - new Date();

    if (remaining <= 0) {
        resendText.innerHTML =
            'Новый код можно будет запросить повторно ' +
            '<a href="/Account/ConfirmEmail">по ссылке</a>';

        return false;
    }

    const totalSeconds = Math.ceil(remaining / 1000);

    const minutes = Math.floor(totalSeconds / 60);
    const seconds = totalSeconds % 60;

    resendText.textContent =
        `Новый код можно будет запросить повторно через ` +
        `${minutes}:${String(seconds).padStart(2, '0')}`;

    return true;
}

updateTimer();

const timer = setInterval(() => {
    if (!updateTimer()) {
        clearInterval(timer);
    }
}, 1000);