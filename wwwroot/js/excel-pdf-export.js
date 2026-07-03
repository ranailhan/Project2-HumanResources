/**
 * HR Portal - Excel & PDF Export Utilities
 */

// Export HTML Table to Excel using SheetJS
function exportTableToExcel(tableId, filename = 'rapor') {
    const table = document.getElementById(tableId);
    if (!table) {
        console.error("Table not found: " + tableId);
        return;
    }

    // Clone the table to avoid modifying the original UI
    const clonedTable = table.cloneNode(true);
    
    // Remove Action (İşlemler) columns or print-hidden headers
    const rows = clonedTable.querySelectorAll('tr');
    rows.forEach(row => {
        const lastCell = row.lastElementChild;
        if (lastCell && (lastCell.tagName === 'TH' || lastCell.tagName === 'TD')) {
            // Check if it's the actions column
            if (lastCell.textContent.trim().toLowerCase().includes('işlemler') || lastCell.querySelector('a') || lastCell.innerHTML.includes('mdi-pencil') || lastCell.innerHTML.includes('fa-')) {
                lastCell.remove();
            }
        }
    });

    // Generate sheet from the cleaned cloned table
    const wb = XLSX.utils.table_to_book(clonedTable, { sheet: "Rapor Verisi" });
    XLSX.writeFile(wb, `${filename}_${new Date().toISOString().slice(0, 10)}.xlsx`);
}

// Export HTML Table to PDF using jsPDF & jsPDF-AutoTable
function exportTableToPDF(tableId, filename = 'rapor', title = 'Rapor') {
    const { jsPDF } = window.jspdf;
    const doc = new jsPDF('p', 'pt', 'a4');
    
    const table = document.getElementById(tableId);
    if (!table) {
        console.error("Table not found: " + tableId);
        return;
    }

    // Add document header
    doc.setFont("Helvetica", "bold");
    doc.setFontSize(18);
    doc.setTextColor(112, 40, 226); // Purple brand color
    doc.text("HR PORTAL", 40, 40);
    
    doc.setFontSize(14);
    doc.setTextColor(51, 51, 51);
    doc.text(title, 40, 65);
    
    doc.setFont("Helvetica", "normal");
    doc.setFontSize(9);
    doc.setTextColor(128, 128, 128);
    doc.text(`Tarih: ${new Date().toLocaleDateString('tr-TR')} ${new Date().toLocaleTimeString('tr-TR')}`, 40, 85);
    doc.line(40, 95, 555, 95); // Horizontal line

    // Clone the table to extract clean text data
    const clonedTable = table.cloneNode(true);
    const rows = Array.from(clonedTable.querySelectorAll('tr'));
    
    // Process headers
    const headerRow = rows[0];
    const headers = [];
    Array.from(headerRow.children).forEach(cell => {
        if (!cell.textContent.trim().toLowerCase().includes('işlemler')) {
            headers.push(cell.textContent.trim());
        }
    });

    // Process body rows
    const data = [];
    for (let i = 1; i < rows.length; i++) {
        const cells = Array.from(rows[i].children);
        const rowData = [];
        // Skip last cell if it's Actions
        const skipLast = cells[cells.length - 1] && (
            cells[cells.length - 1].textContent.trim().toLowerCase().includes('işlemler') || 
            cells[cells.length - 1].querySelector('a') || 
            cells[cells.length - 1].innerHTML.includes('mdi-') ||
            cells[cells.length - 1].innerHTML.includes('fa-')
        );
        
        const limit = skipLast ? cells.length - 1 : cells.length;
        for (let j = 0; j < limit; j++) {
            rowData.push(cells[j].textContent.trim().replace(/\s+/g, ' '));
        }
        if (rowData.length > 0) {
            data.push(rowData);
        }
    }

    // Generate the PDF table
    doc.autoTable({
        head: [headers],
        body: data,
        startY: 110,
        margin: { left: 40, right: 40 },
        theme: 'striped',
        headStyles: { fillDark: true, fillColor: [154, 85, 255] }, // Purple primary
        styles: { font: "Helvetica", fontSize: 9, cellPadding: 6 },
        didDrawPage: function (data) {
            // Footer
            const str = "Sayfa " + doc.internal.getNumberOfPages();
            doc.setFontSize(8);
            doc.setTextColor(150);
            doc.text(str, data.settings.margin.left, doc.internal.pageSize.height - 30);
        }
    });

    doc.save(`${filename}_${new Date().toISOString().slice(0, 10)}.pdf`);
}
