const selectId = document.getElementById('reportId');
const textareaText = document.getElementById('reportText');
const inputSubject = document.getElementById('reportSubject');
const buttonSubmit = document.getElementById('submit');

if (Number(selectId.value) <= 0) buttonSubmit.setAttribute('disabled', 'disabled');

function updateTextarea() {
    const selectedId = Number(selectId.value);

    const selectedReport = Reports.find(r => r.Id === selectedId);

    if (selectedReport) {
        textareaText.value = selectedReport.ReportText;
        inputSubject.value = selectedReport.Subject;
        buttonSubmit.removeAttribute('disabled');
    } else {
        textareaText.value = '';
        inputSubject.value = '';
        buttonSubmit.setAttribute('disabled', 'disabled');
    }
}