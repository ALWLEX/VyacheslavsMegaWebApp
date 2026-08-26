function confirmRemove(event) {
    if (!confirm("Подтвердите удаление")) {
        event.preventDefault();
    }
}