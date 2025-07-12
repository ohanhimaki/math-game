function downloadFile(filename, text) {
    const blob = new Blob([text], { type: 'application/json' });
    const link = document.createElement("a");
    link.download = filename;
    link.href = URL.createObjectURL(blob);
    link.click();
}
