document.addEventListener('DOMContentLoaded', function () {
    const expenseSelect = document.querySelector('#expense-select');
    const selectedValue = document.querySelector('#select-value').value;
    if (expenseSelect)
    {
        expenseSelect.value = selectedValue;
    }
});